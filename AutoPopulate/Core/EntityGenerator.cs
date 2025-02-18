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
        private readonly ICollectionGenerator _collectionGenerator;
        private readonly IDefaultValueProvider _defaultValueProvider;
        private readonly IEntityGenerationConfig _config;

        public EntityGenerator(
            ITypeMetadataCache typeMetadataCache,
            IObjectFactory objectFactory,
            ICollectionGenerator collectionGenerator,
            IDefaultValueProvider defaultValueProvider,
            IEntityGenerationConfig config)
        {
            _typeMetadataCache = typeMetadataCache;
            _objectFactory = objectFactory;
            _collectionGenerator = collectionGenerator;
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
            PropertyInfo[] properties = _typeMetadataCache.GetProperties(type);

            foreach (var prop in properties)
            {
                if (!prop.CanWrite)
                    continue;

                object value = CreateFake(prop.PropertyType);
                prop.SetValue(instance, value);
            }

            return instance;
        }
    }
}
