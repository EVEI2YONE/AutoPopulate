using AutoPopulate.Core;
using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests
{
    public static partial class TestableObjectExtensions
    {
        /// <summary>
        /// Validates if a primitive list follows the expected default values.
        /// </summary>
        public static bool ValidPrimitiveList<T>(this IEntityGenerationConfig config, List<T> list) where T : struct
        {
            if (list == null || !list.Any()) return false;
            return config.CustomPrimitiveGenerators.ContainsKey(typeof(T)) &&
                   list.All(x => config.CustomPrimitiveGenerators[typeof(T)]().Equals(x));
        }

        /// <summary>
        /// Validates if a nullable primitive list follows the expected default values.
        /// </summary>
        public static bool ValidNullablePrimitiveList<T>(this IEntityGenerationConfig config, List<Nullable<T>> list) where T : struct
        {
            if (list == null || !list.Any() || !config.CustomPrimitiveGenerators.ContainsKey(typeof(T))) return false;
            return list.All(x => x.HasValue && config.CustomPrimitiveGenerators[typeof(T)]().Equals(x.Value));
        }

        /// <summary>
        /// Validates if a list follows expected default values or testable object rules.
        /// </summary>
        public static bool ValidList<T>(this IEntityGenerationConfig config, IEnumerable<T> list)
        {
            if (list == null || !list.Any()) return false;

            return !list.Any(x =>
            {
                if (config.CustomPrimitiveGenerators.ContainsKey(typeof(T)))
                    return list.Any(y => !config.CustomPrimitiveGenerators[typeof(T)]().Equals(y));
                else if (x is ITestableObject testable)
                    return list.Any(y => !testable.ItemsSuccessfullyPopulated());
                return false;
            });
        }

        /// <summary>
        /// Validates if a dictionary follows expected default values or testable object rules.
        /// </summary>
        public static bool ValidDictionary<K, V>(this IEntityGenerationConfig config, Dictionary<K, V> dict) where K : notnull where V : class
        {
            if (dict == null || !dict.Any()) return false;
            return !dict.Values.OfType<ITestableObject>().Any(obj => !obj.ItemsSuccessfullyPopulated());
        }
    }

    public static partial class TestableObjectExtensions
    {
        public static Dictionary<Type, Func<object>> DefaultValues { get; set; } = new Dictionary<Type, Func<object>>()
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

        public static bool EqualsTo<T>(this Nullable<T> nullable, T other) where T : struct
            => nullable.HasValue && nullable.Value.Equals(other);
        public static bool NotEqualTo<T>(this Nullable<T> nullable, T other) where T : struct
            => !nullable.HasValue || !nullable.Value.Equals(other);
        public static bool ValidPrimitiveList<T>(this List<T> list) where T : struct
            => list != null && list.Any() && (DefaultValues[typeof(T)].DynamicInvoke()!).Equals(list[0]);
        public static bool ValidNullablePrimitiveList<T>(this List<Nullable<T>> list) where T : struct
            => list?.FirstOrDefault() != null && list.Any() && list[0].HasValue && (DefaultValues[typeof(T)].DynamicInvoke()!).Equals(list[0]!.Value);
        public static bool ValidList<T>(this IEnumerable<T> list)
        {
            if (list == null || !list.Any())
                return false;

            //any fails returns true, so use ! to negate
            return !list.Where(x =>
            {
                //return true if any scenarios fail
                if (DefaultValues?.ContainsKey(typeof(T)) ?? false)
                    return list.Where(x => !DefaultValues[typeof(T)].DynamicInvoke()!.Equals(x)).Any();
                else if (x is ITestableObject)
                    return list.Where(x => !((ITestableObject)x!).ItemsSuccessfullyPopulated()).Any();
                return false;
            }).Any();
        }
        public static bool ValidDictionary<K, V>(this Dictionary<K, V> dict) where K : notnull where V : class
        {
            if (dict == null || !dict.Any()) return false;
            return !dict.Keys.Where(key => dict[key] is ITestableObject).Where(x => !((ITestableObject)x!).ItemsSuccessfullyPopulated()).Any();
        }
    }
}
