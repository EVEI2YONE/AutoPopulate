using AutoPopulate.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Centralized provider for default values, object instantiation, and custom type interception.
    /// </summary>
    public class EntityValueProvider : IEntityValueProvider
    {
        private readonly IEntityGenerationConfig _config;
        private readonly Dictionary<Type, IAttributeHandler> _attributeHandlers = new();
        private static Random _random = new Random();
        private const double _epsilon = 0.0000001;

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

        public EntityValueProvider(IEntityGenerationConfig config)
        {
            _config = config ?? new EntityGenerationConfig()
            {
                PrimitiveNullableChance = 0.5,
                ObjectNullableChance = 0.5,
                CustomPrimitiveGenerators = new Dictionary<Type, Func<object>>(),
                MaxRecursionDepth = 10,
                MinListSize = 1,
                MaxListSize = 10,
                RandomizeListSize = true,
                ReferenceBehavior = RecursionReferenceBehavior.NewInstance,
            };
        }

        private bool GenerateNullable(double nullableChance, Type type)
        {
            return nullableChance > _epsilon && nullableChance >= _random.NextDouble() + _epsilon;
        }

        public void RegisterAttributeHandler<T>(IAttributeHandler handler) where T : Attribute
        {
            _attributeHandlers[typeof(T)] = handler;
        }

        public bool HasDefaultValue(Type type)
        {
            return _config.CustomPrimitiveGenerators.ContainsKey(type) ||
                   _defaultValues.ContainsKey(type) ||
                   (Nullable.GetUnderlyingType(type) is Type underlyingType && _defaultValues.ContainsKey(underlyingType));
        }

        public object GetDefaultValue(Type type, bool isIndex)
        {
            if (_config.CustomPrimitiveGenerators.TryGetValue(type, out var generator))
                return generator();

            Func<object> value;
            if (Nullable.GetUnderlyingType(type) is Type underlyingType)
            {
                if (!isIndex && GenerateNullable(_config.PrimitiveNullableChance, type))
                    return null;
                return _defaultValues.TryGetValue(underlyingType, out value) ? value() : Activator.CreateInstance(underlyingType)!;
            }

            return _defaultValues.TryGetValue(type, out value) ? value() : Activator.CreateInstance(type)!;
        }

        public object CreateInstance(Type type)
        {
            if (type.IsClass && GenerateNullable(_config.ObjectNullableChance, type))
                return null;

            if (type.IsGenericType)
            {
                Type genericTypeDef = type.GetGenericTypeDefinition();
                Type[] genericArgs = type.GetGenericArguments();

                // Handle Dictionary<TKey, TValue>
                if (genericTypeDef == typeof(Dictionary<,>) && genericArgs.Length == 2)
                {
                    return Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(genericArgs));
                }

                // Handle List<T> or other generic collections
                if (typeof(ICollection<>).IsAssignableFrom(genericTypeDef) && genericArgs.Length == 1)
                {
                    return Activator.CreateInstance(type);
                }
            }

            if (typeof(ICollection).IsAssignableFrom(type))
            {
                return Activator.CreateInstance(type)!;
            }

            return Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Cannot create an instance of {type.FullName}");
        }

        public object? HandleAttributes(PropertyInfo property, object instance)
        {
            foreach (var attr in property.GetCustomAttributes())
            {
                if (_attributeHandlers.TryGetValue(attr.GetType(), out var handler))
                {
                    return handler.Handle(property, instance, this);
                }
            }
            return null;
        }

        public object? HandleAttribute(Attribute attribute, object instance)
        {
            if (_attributeHandlers.TryGetValue(attribute.GetType(), out var handler))
            {
                return handler.Handle(null, instance, this); // Pass null for PropertyInfo since it's at class level
            }
            return null;
        }
    }
}
