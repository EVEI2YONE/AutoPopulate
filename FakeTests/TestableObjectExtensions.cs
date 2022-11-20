using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests
{
    public static class TestableObjectExtensions
    {
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
            { typeof(List<>), null },
        };
        public static bool EqualsTo<T>(this Nullable<T> nullable, T other) where T : struct
            => nullable.HasValue && nullable.Value.Equals(other);
        public static bool NotEqualTo<T>(this Nullable<T> nullable, T other) where T : struct
            => !nullable.HasValue || !nullable.Value.Equals(other);
        public static bool ValidPrimitiveList<T>(this List<T> list) where T : struct
            => list != null && list.Any() && (_defaultValues[typeof(T)]).Equals(list[0]);
        public static bool ValidNullablePrimitiveList<T>(this List<Nullable<T>> list) where T : struct
            => list != null && list.Any() && list[0].HasValue && (_defaultValues[typeof(T)]).Equals(list[0].Value);
        public static bool ValidList<T>(this List<T> list) where T : class
        {
            if (list == null || !list.Any())
                return false;

            if (list[0] is ITestableObject)
            {
                return list.Where(x => ((ITestableObject)x).ItemsSuccessfullyPopulated()).Any();
            }
            else if (_defaultValues.ContainsKey(typeof(T)))
                return list.Where(x => _defaultValues[typeof(T)].Equals(list[0])).Any();
            else
                return true;
        }
    }
}
