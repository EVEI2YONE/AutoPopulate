using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Interfaces
{
    /// <summary>
    /// Provides default values for basic types and custom overrides.
    /// </summary>
    public interface IDefaultValueProvider
    {
        bool HasDefaultValue(Type type);
        object GetDefaultValue(Type type);
    }
}
