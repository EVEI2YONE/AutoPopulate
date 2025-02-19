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

        public EntityGenerator()
        {
            _typeMetadataCache = new TypeMetadataCache();
            _entityValueProvider = new EntityValueProvider();
            _config = _entityValueProvider.Config;
            SetUp();
        }

        public EntityGenerator(IEntityGenerationConfig config)
        {
            _typeMetadataCache = new TypeMetadataCache();
            _entityValueProvider = new EntityValueProvider(config);
            _config = config;
            SetUp();
        }

        private void SetUp()
        {
            foreach (var kvp in _config.TypeInterceptorValueProviders)
            {
                _entityValueProvider.RegisterTypeInterceptor(kvp.Key, kvp.Value);
            }

            _entityValueProvider.RegisterAttributeHandler<AutoPopulateAttribute>(new AutoPopulateValueHandler());
            var valueProviderType = _entityValueProvider.GetType();
            var methodInfo = valueProviderType.GetMethod("RegisterAttributeHandler");

            foreach (var kvp in _config.AttributeHandlers)
            {
                var genericMethod = methodInfo.MakeGenericMethod(kvp.Key.GetType());
                genericMethod.Invoke(_entityValueProvider, new object[] { kvp.Value });
            }
        }

        public T CreateFake<T>()
        {
            return (T) CreateFakeInternal(typeof(T), false);
        }

        public object CreateFake(Type type)
        {
            return CreateFakeInternal(type, false);
        }
        

        private object CreateFakeInternal(Type type, bool isIndex)
        {
            if (_entityValueProvider.HasDefaultValue(type))
            {
                return _entityValueProvider.GetDefaultValue(type, isIndex);
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

            if (instance == null)
                return null;

            // Process class-level attributes
            foreach (var attr in type.GetCustomAttributes())
            {
                object? modifiedInstance = _entityValueProvider.HandleAttribute(attr, instance);
                if (modifiedInstance != null)
                {
                    return modifiedInstance; // Short-circuit if an attribute provides a replacement
                }
            }

            if (instance is IDictionary dictionary && type.IsGenericType)
            {
                Type[] genericArgs = type.GetGenericArguments();
                Type keyType = genericArgs[0];
                Type valueType = genericArgs[1];
                MethodInfo addMethod = type.GetMethod("Add");

                int count = _config.RandomizeListSize ? new Random().Next(_config.MinListSize, _config.MaxListSize + 1) : _config.MaxListSize;
                for (int i = 0; i < count; i++)
                {
                    object key = CreateFakeInternal(keyType, true);
                    object value = CreateFakeInternal(valueType, false);
                    addMethod.Invoke(dictionary, new object[] { key, value });
                }

                _recursionDepths[type]--;
                return instance;
            }
            else if (instance is IList list && type.IsGenericType)
            {
                Type elementType = type.GetGenericArguments()[0];
                MethodInfo addMethod = type.GetMethod("Add");

                int count = _config.RandomizeListSize ? new Random().Next(_config.MinListSize, _config.MaxListSize + 1) : _config.MaxListSize;
                for (int i = 0; i < count; i++)
                {
                    object item = CreateFakeInternal(elementType, false);
                    addMethod.Invoke(list, new object[] { item });
                }

                _recursionDepths[type]--;
                return instance;
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

                object value = CreateFakeInternal(prop.PropertyType, false);

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
