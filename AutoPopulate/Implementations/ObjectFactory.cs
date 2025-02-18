using AutoPopulate.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Handles object instantiation, including collection initialization.
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
    }
}
