using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace AutoPopulate
{
    public class EntityGenerator : IEntityGenerator
    {
        #region Variables
        public Dictionary<Type, Func<object>> DefaultValues { get; private set; } = new Dictionary<Type, Func<object>>()
        {
            { typeof(string), () => "_" },
            { typeof(bool), () => true },
            { typeof(short), () => (short)1 },
            { typeof(int), () => 1 },
            { typeof(uint), () => 1u },
            { typeof(long), () => 1L },
            { typeof(ulong), () => 1ul },
            { typeof(decimal), () => 1m },
            { typeof(double), () => 1.0d },
            { typeof(float), () => 1.0f },
            { typeof(char), () => '_' },
            { typeof(byte), () => (byte)('_') },
            { typeof(sbyte), () => (sbyte)1 },
            { typeof(DateTime), () => DateTime.Now },
            { typeof(object), () => "object" }
        };
        public int RecursiveLimit { get; set; } = 1;
        private Dictionary<Type, int> RecursiveOccurrences { get; set; } = new Dictionary<Type, int>();
        private Stack<Type> Stack = new Stack<Type>();
        #endregion
        #region Enums
        private Random random = new Random();
        public int CollectionLimit { get; set; } = 0;
        public int CollectionStart { get; set; } = 0;
        public RandomizationType RandomizationBehavior { get; set; } = RandomizationType.Fixed;
        public enum RandomizationType
        {
            Fixed,
            Range
        }
        #endregion
        public T CreateFake<T>() where T : class, new()
            => GenerateFake(typeof(T))! as T;

        public object? CreateFake(Type type)
            => GenerateFake(type);

        #region Main Generation Logic
        private object GenerateFake(object o)
        {
            if (o == null)
                return null;

            bool isType = false;
            Type currentType;
            if(o is Type oType)
            {
                isType = true;
                currentType = ExtractNullableType(oType);
            }
            else
            {
                currentType = ExtractNullableType((o).GetType());
            }
            if (HasDefaultValue(currentType))
            {
                if (HasDelegate(currentType))
                    o = ((Delegate)DefaultValues[currentType].DynamicInvoke()!).DynamicInvoke();
                else
                    o = DefaultValues[currentType].DynamicInvoke();
            }
            else if (IsRecursiveType(currentType))
            {
                o = Activator.CreateInstance(currentType);
                IncrementType(currentType);
                foreach (var prop in currentType.GetProperties())
                {
                    if (IsRecursiveType(prop.PropertyType) && RecursiveLimitReached(prop.PropertyType))
                        continue;
                    prop.SetValue(o, GenerateFake(prop.PropertyType), null);
                }
                DecrementType(currentType);
            }
            else
            {
                int start = 0;
                int end = GetRandomGenerationLimit();
                o = (isType) ? currentType.IsInterface ? Activator.CreateInstance(ResolveInterface(currentType)) : Activator.CreateInstance(currentType) : o;
                IncrementType(currentType); //preps to monitor recursion depth
                if (IsGenericList(currentType))
                {
                    Type nestedType = currentType.GetGenericArguments()[0];
                    for (int i = start; i < end; i++)
                        ((IList)o).Add(GenerateFake(nestedType)!);
                }
                else if (IsGenericCollection(currentType))
                {
                    //((ICollection)o)
                }
                else if (IsGenericDictionary(currentType))
                {
                    Type keyType = currentType.GetGenericArguments()[0];
                    Type valueType = currentType.GetGenericArguments()[1];
                    for (int i = start; i < end; i++)
                        ((IDictionary)o).Add(GenerateFake(keyType)!, GenerateFake(valueType)!);
                }
                else
                {
                    Stack.Push(currentType); //preps for recursion
                    foreach (var prop in currentType.GetProperties())
                    {
                        if (prop.SetMethod == null)
                            continue;

                        if (!HasCustomValue(prop, out object? value))
                            prop.SetValue(o, GenerateFake(prop.PropertyType), null);
                        else
                            prop.SetValue(o, value, null);
                    }
                    Stack.Pop();
                }
                DecrementType(currentType);
            }
            return o;
        }
        #endregion

        #region Generic Helper Methods
        private Type? ResolveInterface(Type type)
        {
            var genericArguments = type.GetGenericArguments();
            if (type != typeof(string) && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return typeof(List<>).MakeGenericType(genericArguments[0]);
            else if (type.GetGenericTypeDefinition() == typeof(IDictionary<,>))
                return typeof(Dictionary<,>).MakeGenericType(genericArguments[0], genericArguments[1]);
            else if (type.GetGenericTypeDefinition() == typeof(ICollection<>))
                return typeof(HashSet<>).MakeGenericType(genericArguments[0]);
            return null;
        }

        private bool IsGenericCollection(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(ICollection<>);

        private static List<Type> genericLists = new List<Type>() { typeof(IList<>), typeof(List<>) };
        private bool IsGenericList(Type propType)
            => propType.IsGenericType && genericLists.Contains(propType.GetGenericTypeDefinition());

        private bool IsGenericDictionary(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Dictionary<,>);

        private bool IsRecursiveType(Type propType)
            => Stack.Contains(propType);

        private bool HasDefaultValue(Type propType)
            => DefaultValues.ContainsKey(propType);

        private Type ExtractNullableType(Type propType)
        {
            Type? nullableType = Nullable.GetUnderlyingType(propType);
            return nullableType ?? propType;
        }

        private void IncrementType(Type propType)
        {
            if (!TryAdd(RecursiveOccurrences, propType, 1))
                RecursiveOccurrences[propType] = RecursiveOccurrences[propType] + 1;
        }

        private bool RecursiveLimitReached(Type propType)
            => RecursiveOccurrences[propType] > RecursiveLimit;

        private void DecrementType(Type propType)
            => RecursiveOccurrences[propType] = RecursiveOccurrences[propType] - 1;

        private int GetRandomGenerationLimit()
            => RandomizationBehavior == RandomizationType.Range ? random.Next(CollectionStart, CollectionLimit) : CollectionLimit;

        public void SetListRandomRange(int start, int end)
        {
            CollectionStart = start;
            CollectionLimit = end;
        }

        private bool HasCustomValue(PropertyInfo propInfo, out dynamic? value)
        {
            var attribute = propInfo.GetCustomAttribute<AutoPopulateAttribute>();
            value = null;
            if (attribute != null)
            {
                if (attribute.Values != null && attribute.Values.Any())
                    value = attribute.Values[random.Next(0, attribute.Values.Length)];
            }
            return attribute != null;
        }

        private bool HasDelegate(Type propType)
        {
            var value = DefaultValues[propType].DynamicInvoke();
            if (value is Delegate)
                return true;
            return false;
        }
        #endregion

        private bool TryAdd<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary == null)
            {
                return false;
            }

            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
                return true;
            }

            return false;
        }
    }
}
