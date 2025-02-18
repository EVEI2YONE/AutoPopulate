using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Handles object instantiation and default value assignment.
    /// </summary>
    public class ObjectFactory : IObjectFactory
    {
        private readonly IEntityGenerationConfig _config;

        public ObjectFactory(IEntityGenerationConfig config)
        {
            _config = config;
        }

        public object CreateInstance(Type type)
        {
            if (_config.AllowNullObjects && type.IsClass)
                return null;

            return Activator.CreateInstance(type) ?? throw new InvalidOperationException($"Cannot create an instance of {type.FullName}");
        }

        public T CreateInstance<T>() where T : class, new()
        {
            return _config.AllowNullObjects ? null : new T();
        }
    }
}
