using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.Interfaces
{
    /// <summary>
    /// Caches reflection metadata to improve performance.
    /// </summary>
    public interface ITypeMetadataCache
    {
        PropertyInfo[] GetProperties(Type type);
    }
}
