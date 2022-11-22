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

namespace FakeDbContext
{
    public class AutoPopulate
    {
        #region Variables
        public static Dictionary<Type, object> DefaultValues { get; set; } = new Dictionary<Type, object>()
        {
            { typeof(string), "_" },
            { typeof(bool), true },
            { typeof(Int16), (Int16)1 },
            { typeof(int), 1 },
            { typeof(uint), 1u },
            { typeof(long), 1l },
            { typeof(ulong), 1ul },
            { typeof(decimal), 1m },
            { typeof(double), 1.0d },
            { typeof(float), 1.0f },
            { typeof(char), '_' },
            { typeof(byte), (byte)('_') },
            { typeof(DateTime), DateTime.Now },
            //{ typeof(List<object>), null },
            { typeof(object), "object" }
        };
        public static Stack<Type> Stack = new Stack<Type>();
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
                o = DefaultValues[currentType];
            }
            else if (IsRecursiveType(currentType))
            {
                o = Activator.CreateInstance(currentType);
                foreach (var prop in currentType.GetProperties())
                {
                    if (IsRecursiveType(prop.PropertyType))
                        continue;
                    prop.SetValue(o, GenerateFake(prop.PropertyType), null);
                }
            }
            else
            {
                o = (isType) ? Activator.CreateInstance(currentType) : o;
                if (IsGenericCollection(currentType))
                {
                    Type nestedType = currentType.GetGenericArguments()[0];
                    ((IList)o).Add(GenerateFake(nestedType));
                }
                else if (IsGenericDictionary(currentType))
                {
                    Type keyType = currentType.GetGenericArguments()[0];
                    Type valueType = currentType.GetGenericArguments()[1];
                    ((IDictionary)o).Add(GenerateFake(keyType), GenerateFake(valueType));
                }
                else
                {
                    Stack.Push(currentType);
                    foreach (var prop in currentType.GetProperties())
                    {
                        prop.SetValue(o, GenerateFake(prop.PropertyType), null);
                    }
                    Stack.Pop();
                }
            }
            return o;
        }
        #endregion

        #region Generic Helper Methods
        private bool IsGenericCollection(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>);

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
        #endregion
    }
}