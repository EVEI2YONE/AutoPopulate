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
        public IEntityGenerationConfig Config => _config;
        private readonly Dictionary<Type, IAttributeHandler> _attributeHandlers = new();
        private static Random _random = new Random();
        private const double _epsilon = 0.0000001;

        public EntityValueProvider() : this(null) { }

        public EntityValueProvider(IEntityGenerationConfig config)
        {
            _config = config ?? new EntityGenerationConfig();
        }

        private bool GenerateNullable(GenerationOption option, Type type)
        {
            _config.OptionChances.TryGetValue(option, out var nullableChance);
            return nullableChance > _epsilon && nullableChance >= _random.NextDouble() + _epsilon;
        }

        public void RegisterAttributeHandler<T>(IAttributeHandler handler) where T : Attribute
        {
            _attributeHandlers[typeof(T)] = handler;
        }

        public bool HasDefaultValue(Type type)
        {
            return _config.TypeInterceptorValueProviders.ContainsKey(type) ||
                   (Nullable.GetUnderlyingType(type) is Type underlyingType && _config.TypeInterceptorValueProviders.ContainsKey(underlyingType));
        }

        public object GetDefaultValue(Type type, bool isIndex)
        {
            if (_config.TypeInterceptorValueProviders.TryGetValue(type, out var generator))
                return generator();

            Func<object> value;
            if (Nullable.GetUnderlyingType(type) is Type underlyingType)
            {
                if (!isIndex && GenerateNullable(GenerationOption.NullablePrimitiveChance, type))
                    return null;
                return _config.TypeInterceptorValueProviders.TryGetValue(underlyingType, out value) ? value() : Activator.CreateInstance(underlyingType)!;
            }

            return _config.TypeInterceptorValueProviders.TryGetValue(type, out value) ? value() : Activator.CreateInstance(type)!;
        }

        public object CreateInstance(Type type)
        {
            if (type.IsClass && GenerateNullable(GenerationOption.NullableObjectChance, type))
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
