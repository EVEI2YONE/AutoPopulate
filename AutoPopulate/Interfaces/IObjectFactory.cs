using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Interfaces
{
    /// <summary>
    /// Handles object instantiation and default value assignment.
    /// </summary>
    public interface IObjectFactory
    {
        object CreateInstance(Type type);
        T CreateInstance<T>() where T : class, new();
    }
}
