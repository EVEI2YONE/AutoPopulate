using AutoPopulate.AttributeHandlers;
using AutoPopulate.Attributes;
using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace AutoPopulate.Core
{
    /// <summary>
    /// Main class responsible for entity generation with configurable options.
    /// </summary>
    public class EntityGenerator : IEntityGenerator
    {
        private readonly ITypeMetadataCache _typeMetadataCache;
        private readonly IEntityValueProvider _entityValueProvider;
        private readonly IEntityGenerationConfig _config;
        private readonly IDictionary<Type, int> _recursionDepths = new Dictionary<Type, int>();

        public EntityGenerator(
            ITypeMetadataCache typeMetadataCache,
            IEntityValueProvider entityValueProvider,
            IEntityGenerationConfig config)
        {
            _typeMetadataCache = typeMetadataCache;
            _entityValueProvider = entityValueProvider;
            _config = config;
            RegisterAutoPopulateAttributeHandlers();
        }

        private void RegisterAutoPopulateAttributeHandlers()
        {
            _entityValueProvider.RegisterAttributeHandler<AutoPopulateAttribute>(new AutoPopulateValueHandler());
        }

        public T CreateFake<T>()
        {
            return (T)CreateFake(typeof(T));
        }

        public object CreateFake(Type type)
        {
            if (_entityValueProvider.HasDefaultValue(type))
            {
                return _entityValueProvider.GetDefaultValue(type);
            }

            if (!_recursionDepths.ContainsKey(type))
            {
                _recursionDepths[type] = 0;
            }

            if (_recursionDepths[type] > _config.MaxRecursionDepth)
            {
                return null;
            }

            _recursionDepths[type]++;

            object instance = _entityValueProvider.CreateInstance(type);

            // Process class-level attributes
            foreach (var attr in type.GetCustomAttributes())
            {
                object? modifiedInstance = _entityValueProvider.HandleAttribute(attr, instance);
                if (modifiedInstance != null)
                {
                    return modifiedInstance; // Short-circuit if an attribute provides a replacement
                }
            }

            PropertyInfo[] properties = _typeMetadataCache.GetProperties(type);
            foreach (var prop in properties)
            {
                if (!prop.CanWrite || prop.GetIndexParameters().Length > 0 || prop.GetCustomAttribute<AutoPopulateIgnoreAttribute>() != null)
                    continue;

                object? attributeValue = _entityValueProvider.HandleAttributes(prop, instance);
                if (attributeValue != null)
                {
                    prop.SetValue(instance, attributeValue);
                    continue; // Short-circuit if an attribute supplies a valid value
                }

                foreach (var attr in prop.GetCustomAttributes())
                {
                    object? modifiedValue = _entityValueProvider.HandleAttribute(attr, instance);
                    if (modifiedValue != null)
                    {
                        prop.SetValue(instance, modifiedValue);
                        continue; // Short-circuit if attribute provides a replacement value
                    }
                }

                object value = CreateFake(prop.PropertyType);

                if (value == null && prop.PropertyType.IsValueType && Nullable.GetUnderlyingType(prop.PropertyType) == null)
                    continue;

                if (value != null && !prop.PropertyType.IsAssignableFrom(value.GetType()))
                {
                    try
                    {
                        value = Convert.ChangeType(value, prop.PropertyType);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
                prop.SetValue(instance, value);
            }

            _recursionDepths[type]--;
            return instance;
        }
    }
}
