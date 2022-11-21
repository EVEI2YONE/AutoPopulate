using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Reflection;

namespace FakeDbContext
{
    public class AutoPopulate
    {
        #region constants
        public readonly string _ = string.Empty;
        public readonly DateTime now = DateTime.Now;
        public readonly decimal d = 0m;
        public readonly long l = 0;

        public static Dictionary<Type, object> DefaultValues 
        { 
            get { return _defaultValues; }
            set { _defaultValues = value; } 
        }

        private static Dictionary<Type, object> _defaultValues = new Dictionary<Type, object>()
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
        #endregion

        public T CreateFake<T>() where T : new()
            => (T)GenerateFake(typeof(T));

        public object CreateFake(Type type)
            => GenerateFake(type);

        Stack<Type> stack = new Stack<Type>();
        #region Main Generation Logic
        private dynamic? GenerateFake(dynamic? o)
        {
            if (o == null)
                return null;
            bool isType = (o is Type);
            Type currentType = (isType) ? (Type)ExtractNullableType(o) : ExtractNullableType(((object)o).GetType());
            if (HasDefaultValue(currentType))
            {
                o = DefaultValues[currentType];
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
                    foreach (var prop in currentType.GetProperties())
                    {
                        prop.SetValue(o, GenerateFake(prop.PropertyType), null);
                    }
                }
            }
            return o;
        }
        #endregion

        #region
        private bool IsGenericCollection(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>);

        private bool IsGenericDictionary(Type propType)
            => propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Dictionary<,>);

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