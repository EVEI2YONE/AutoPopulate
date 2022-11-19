using System.Collections;
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

        public readonly Dictionary<Type, object> defaultValues = new Dictionary<Type, object>()
        {
            { typeof(string), "_" },
            { typeof(bool), false },
            { typeof(int), 1 },
            { typeof(long), 1 },
            { typeof(decimal), 1m },
            { typeof(char), '_' },
            { typeof(byte), (byte)('_') },
            { typeof(double), 0 },
            { typeof(DateTime), DateTime.Now },
            { typeof(List<>), null },
        };
        #endregion

        public T? CreateFake<T>() where T : new()
            => GenerateFake(new T());

        public object? CreateFake(Type T)
            => GenerateFake(Activator.CreateInstance(T));

        Stack<Type> stack = new Stack<Type>();
        #region Main Generation Logic
        private dynamic? GenerateFake(dynamic o)
        {
            if (o == null)
                return null;
            Type currentType = ((object)o).GetType();
            var props = currentType.GetProperties();
            Type propertyType;
            bool nullable;
            foreach (var prop in props)
            {
                propertyType = ExtractNullableType(prop.PropertyType, out nullable);
                if (IsGenericList(propertyType))
                {
                    Type itemType = propertyType.GetGenericArguments()[0];

                    object instance = Activator.CreateInstance(propertyType);
                    // List<T> implements the non-generic IList interface
                    IList list = (IList)instance;

                    object item;
                    if (defaultValues.ContainsKey(itemType))
                        item = defaultValues[itemType];
                    else
                        item = CreateFake(itemType);
                    list.Add(item); //whatever you need to add
                    prop.SetValue(o, list, null);
                }
                else if (defaultValues.ContainsKey(propertyType))
                {
                    if (!TrySetPrimitiveValue(o, prop, propertyType, nullable))
                    {
                        try
                        {
                            //primitive is nullable
                            prop.SetValue(o, null);
                        }
                        catch (Exception)
                        {
                            //primitive has no set method
                        }
                    }
                }
                else
                {
                    bool pushed = false;
                    try
                    {
                        if (stack.Contains(prop.PropertyType))
                            prop.SetValue(o, Activator.CreateInstance(prop.PropertyType));
                        else
                        {
                            stack.Push(prop.PropertyType);
                            pushed = true;
                            TrySetObjectValue(o, prop);
                        }
                    }
                    catch (Exception)
                    {
                        //object is nullable
                    }
                    if (pushed)
                        stack.Pop();
                }
            }
            return o;
        }


        #endregion

        #region Generic Helper Function
        private void TrySetObjectValue(dynamic o, PropertyInfo prop)
        {
            Type t = prop.PropertyType;
            prop.SetValue(o, GenerateFake(Activator.CreateInstance(t)));
        }

        private bool TrySetPrimitiveValue(dynamic o, PropertyInfo prop, Type type, bool nullable)
        {
            try
            {
                var val = defaultValues[type];
                //if (nullable)
                //  val = ;
                prop.SetValue(o, val);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private Type ExtractNullableType(Type propType, out bool nullable)
        {
            Type nullableType = Nullable.GetUnderlyingType(propType);
            nullable = nullableType != null;
            propType = nullableType ?? propType;
            return propType;
        }

        private bool IsGenericList(Type propType)
        {
            return propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(List<>);
        }
        #endregion
    }
}