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
        private readonly IObjectFactory _objectFactory;
        private readonly IDefaultValueProvider _defaultValueProvider;
        private readonly IEntityGenerationConfig _config;
        private readonly IDictionary<Type, int> _recursionDepths = new Dictionary<Type, int>();

        public EntityGenerator(
            ITypeMetadataCache typeMetadataCache,
            IObjectFactory objectFactory,
            IDefaultValueProvider defaultValueProvider,
            IEntityGenerationConfig config)
        {
            _typeMetadataCache = typeMetadataCache;
            _objectFactory = objectFactory;
            _defaultValueProvider = defaultValueProvider;
            _config = config;
        }

        public T CreateFake<T>()
        {
            return (T)CreateFake(typeof(T));
        }

        public object CreateFake(Type type)
        {
            if (_defaultValueProvider.HasDefaultValue(type))
            {
                return _defaultValueProvider.GetDefaultValue(type);
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

            object instance = _objectFactory.CreateInstance(type);

            if (instance is IDictionary dictionary && type.IsGenericType)
            {
                Type[] genericArgs = type.GetGenericArguments();
                Type keyType = genericArgs[0];
                Type valueType = genericArgs[1];
                MethodInfo addMethod = type.GetMethod("Add");

                int count = _config.RandomizeListSize ? new Random().Next(_config.MinListSize, _config.MaxListSize + 1) : _config.MaxListSize;
                for (int i = 0; i < count; i++)
                {
                    object key = CreateFake(keyType);
                    object value = CreateFake(valueType);
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
                    object item = CreateFake(elementType);
                    addMethod.Invoke(list, new object[] { item });
                }

                _recursionDepths[type]--;
                return instance;
            }

            PropertyInfo[] properties = _typeMetadataCache.GetProperties(type);
            foreach (var prop in properties)
            {
                if (!prop.CanWrite || prop.GetIndexParameters().Length > 0)
                    continue;

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
