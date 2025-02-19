using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Interfaces
{
    /// <summary>
    /// Defines configuration options for entity generation.
    /// </summary>
    public interface IEntityGenerationConfig
    {
        public double ObjectNullableChance { get; set; }
        public double PrimitiveNullableChance { get; set; }

        // List Generation
        public int MinListSize { get; }
        public int MaxListSize { get; }
        public bool RandomizeListSize { get; }

        // Recursive Object Handling
        public int MaxRecursionDepth { get; }
        public RecursionReferenceBehavior ReferenceBehavior { get; }
        public Dictionary<Type, Func<object>> TypeInterceptorValueProviders { get; }
        public Dictionary<Attribute, IAttributeHandler> AttributeHandlers { get; }
    }
}
