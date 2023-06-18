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
        private static Dictionary<Type, Delegate> _defaultValues = AutoPopulate.DefaultValues;
        public static bool EqualsTo<T>(this Nullable<T> nullable, T other) where T : struct
            => nullable.HasValue && nullable.Value.Equals(other);
        public static bool NotEqualTo<T>(this Nullable<T> nullable, T other) where T : struct
            => !nullable.HasValue || !nullable.Value.Equals(other);
        public static bool ValidPrimitiveList<T>(this List<T> list) where T : struct
            => list != null && list.Any() && (_defaultValues[typeof(T)].DynamicInvoke()).Equals(list[0]);
        public static bool ValidNullablePrimitiveList<T>(this List<Nullable<T>> list) where T : struct
            => list != null && list.Any() && list[0].HasValue && (_defaultValues[typeof(T)].DynamicInvoke()).Equals(list[0].Value);
        public static bool ValidList<T>(this IEnumerable<T> list)
        {
            if (list == null || !list.Any())
                return false;

            //any fails returns true, so use ! to negate
            return !list.Where(x =>
            {
                //return true if any scenarios fail
                if (_defaultValues.ContainsKey(typeof(T)))
                    return list.Where(x => !_defaultValues[typeof(T)].DynamicInvoke().Equals(x)).Any();
                else if (x is ITestableObject)
                    return list.Where(x => !((ITestableObject)x).ItemsSuccessfullyPopulated()).Any();
                return false;
            }).Any();
        }
        public static bool ValidDictionary<K, V>(this Dictionary<K, V> dict) where V : class
        {
            if (dict == null || !dict.Any()) return false;
            return !dict.Keys.Where(key => dict[key] is ITestableObject).Where(x => !((ITestableObject)x).ItemsSuccessfullyPopulated()).Any();
        }
    }
}
