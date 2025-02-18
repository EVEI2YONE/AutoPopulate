using AutoPopulate.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Provides default values for basic types and custom overrides with configurability.
    /// </summary>
    public class DefaultValueProvider : IDefaultValueProvider
    {
        private readonly IEntityGenerationConfig _config;

        private readonly Dictionary<Type, Func<object>> _defaultValues = new()
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
            { typeof(object), () => "object" },
        };



        public DefaultValueProvider(IEntityGenerationConfig config)
        {
            _config = config;
        }

        public bool HasDefaultValue(Type type)
        {
            return _config.CustomPrimitiveGenerators.ContainsKey(type) ||
                   _defaultValues.ContainsKey(type) ||
                   (Nullable.GetUnderlyingType(type) is Type underlyingType && _defaultValues.ContainsKey(underlyingType));
        }

        public object GetDefaultValue(Type type)
        {
            if (_config.CustomPrimitiveGenerators.TryGetValue(type, out var generator))
                return generator();

            Func<object> value;
            if (Nullable.GetUnderlyingType(type) is Type underlyingType)
            {
                return _defaultValues.TryGetValue(underlyingType, out value) ? value() : Activator.CreateInstance(underlyingType)!;
            }

            return _defaultValues.TryGetValue(type, out value) ? value() : Activator.CreateInstance(type)!;
        }
    }
}
