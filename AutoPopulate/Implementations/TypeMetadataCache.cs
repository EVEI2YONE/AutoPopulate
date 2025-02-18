using AutoPopulate.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Caches reflection metadata to improve performance.
    /// </summary>
    public class TypeMetadataCache : ITypeMetadataCache
    {
        private readonly ConcurrentDictionary<Type, PropertyInfo[]> _cache = new();

        public PropertyInfo[] GetProperties(Type type)
        {
            return _cache.GetOrAdd(type, t => t.GetProperties(BindingFlags.Public | BindingFlags.Instance));
        }
    }
}
