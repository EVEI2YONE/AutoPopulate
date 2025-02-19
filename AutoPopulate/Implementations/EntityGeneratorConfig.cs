using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Default implementation of IEntityGenerationConfig.
    /// </summary>
    public class EntityGenerationConfig : IEntityGenerationConfig
    {
        public double ObjectNullableChance { get; set; }
        public double PrimitiveNullableChance { get; set; }

        public int MinListSize { get; set; } = 1;
        public int MaxListSize { get; set; } = 5;
        public bool RandomizeListSize { get; set; } = true;

        public int MaxRecursionDepth { get; set; } = 3;
        public RecursionReferenceBehavior ReferenceBehavior { get; set; } = RecursionReferenceBehavior.NewInstance;
        public Dictionary<Type, Func<object>> TypeInterceptorValueProviders { get; set; } = new();
        public Dictionary<Attribute, IAttributeHandler> AttributeHandlers { get; set; } = new();
    }
}
