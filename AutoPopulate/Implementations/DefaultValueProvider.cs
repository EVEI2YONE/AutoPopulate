using AutoPopulate.Interfaces;
using System;
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

        private readonly Dictionary<Type, object> _defaultValues = new()
        {
            { typeof(string), "" },
            { typeof(int), 0 },
            { typeof(bool), false },
            { typeof(double), 0.0 },
            { typeof(float), 0.0f },
            { typeof(long), 0L },
            { typeof(DateTime), DateTime.MinValue }
        };

        public DefaultValueProvider(IEntityGenerationConfig config)
        {
            _config = config;
        }

        public bool HasDefaultValue(Type type)
        {
            return _config.CustomPrimitiveGenerators.ContainsKey(type) || _defaultValues.ContainsKey(type);
        }

        public object GetDefaultValue(Type type)
        {
            if (_config.CustomPrimitiveGenerators.TryGetValue(type, out var generator))
                return generator();

            return _defaultValues.TryGetValue(type, out var value) ? value : Activator.CreateInstance(type)!;
        }
    }
}
