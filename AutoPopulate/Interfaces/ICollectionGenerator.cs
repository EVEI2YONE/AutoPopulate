using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Interfaces
{
    /// <summary>
    /// Generates lists, dictionaries, and other collections.
    /// </summary>
    public interface ICollectionGenerator
    {
        object GenerateCollection(Type collectionType, Func<Type, object> elementGenerator);
    }
}
