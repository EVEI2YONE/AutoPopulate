using AutoPopulate_Attribute;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace AutoPopulate_Generator
{
    public class AutoPopulate
    {
        #region Variables
        public static Dictionary<Type, Delegate> DefaultValues { get; set; } = new Dictionary<Type, Delegate>()
        {
            { typeof(string), () => "_" },
            { typeof(bool), () => true },
            { typeof(Int16), () => (Int16)1 },
            { typeof(int), () => 1 },
            { typeof(uint), () => 1u },
            { typeof(long), () => 1l },
            { typeof(ulong), () => 1ul },
            { typeof(decimal), () => 1m },
            { typeof(double), () => 1.0d },
            { typeof(float), () => 1.0f },
            { typeof(char), () => '_' },
            { typeof(byte), () => (byte)('_') },
            { typeof(DateTime), () => DateTime.Now },
            { typeof(object), () => "object" }
        };
        public static int RecursiveLimit = 1;
        private static Dictionary<Type, int> RecursiveOccurrences { get; set; } = new Dictionary<Type, int>();
        private static Stack<Type> Stack = new Stack<Type>();
        #endregion
        #region Enums
        private static Random random = new Random();
        public static int CollectionLimit = 0;
        public static int CollectionStart = 0;
        public static RandomizationType RandomizationBehavior = RandomizationType.Fixed;
        public enum RandomizationType
        {
            Fixed,
            Range
        }
        #endregion
        public T CreateFake<T>() where T : new()
            => (T)GenerateFake(typeof(T));

        public object CreateFake(Type type)
            => GenerateFake(type);

        #region Main Generation Logic
        public virtual dynamic? GenerateFake(dynamic? o)
        {
            if (o == null)
                return null;

            bool isType = (o is Type);
            Type currentType = (isType) ? (Type)ExtractNullableType(o) : ExtractNullableType(((object)o).GetType());
            if (HasDefaultValue(currentType))
            {
                if (HasDelegate(currentType))
                    o = ((Delegate)DefaultValues[currentType].DynamicInvoke()).DynamicInvoke();
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
                        ((IList)o).Add(GenerateFake(nestedType));
                }
                else if (IsGenericCollection(currentType))
                {
                    //((ICollection)o)
                }
                else if (IsGenericDictionary(currentType))
                {
                    Type keyType = currentType.GetGenericArguments()[0];
                    Type valueType = currentType.GetGenericArguments()[1];
                    for (int i = start; i <= end; i++)
                        ((IDictionary)o).Add(GenerateFake(keyType), GenerateFake(valueType));
                }
                else
                {
                    Stack.Push(currentType); //preps for recursion
                    foreach (var prop in currentType.GetProperties())
                    {
                        if (!HasCustomValue(prop, out object value))
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
        private Type ResolveInterface(Type type)
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

        private bool IsGenericList(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(IList<>);

        private bool IsGenericDictionary(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Dictionary<,>);

        private bool IsRecursiveType(Type propType)
            => Stack.Contains(propType);

        private bool HasDefaultValue(Type propType)
            => DefaultValues.ContainsKey(propType);

        private Type ExtractNullableType(Type propType)
        {
            Type nullableType = Nullable.GetUnderlyingType(propType);
            return nullableType ?? propType;
        }

        private void IncrementType(Type propType)
        {
            if (!RecursiveOccurrences.TryAdd(propType, 1))
                RecursiveOccurrences[propType] = RecursiveOccurrences.GetValueOrDefault(propType) + 1;
        }

        private bool RecursiveLimitReached(Type propType)
            => RecursiveOccurrences[propType] > RecursiveLimit;

        private void DecrementType(Type propType)
            => RecursiveOccurrences[propType] = RecursiveOccurrences[propType] - 1;

        private int GetRandomGenerationLimit()
            => RandomizationBehavior == RandomizationType.Range ? random.Next(CollectionStart, CollectionLimit) : CollectionLimit;

        public static void SetRandomizationRange(int start, int end)
        {
            CollectionStart = start;
            CollectionLimit = end;
        }

        public bool HasCustomValue(PropertyInfo propInfo, out dynamic value)
        {
            var attribute = propInfo.GetCustomAttribute<AutoPopulateAttribute>();
            if (attribute != null)
            {
                if (attribute.Value != null && attribute.Value.GetType().IsArray)
                    value = ((object[])attribute.Value)[random.Next(0, ((object[])attribute.Value).Length)];
                else
                    value = attribute.Value;
            }
            else
                value = null;
            return attribute != null;
        }

        public bool HasDelegate(Type propType)
        {
            var value = DefaultValues[propType].DynamicInvoke();
            if (value is Delegate)
                return true;
            return false;
        }
        #endregion
    }
}