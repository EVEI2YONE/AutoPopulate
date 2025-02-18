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
            return instance;
        }
    }
}
