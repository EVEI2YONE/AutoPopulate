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
        // List Generation
        public int MinListSize { get; set; }
        public int MaxListSize { get; set; }
        public bool RandomizeListSize { get; set; }

        // Recursive Object Handling
        public int MaxRecursionDepth { get; set; }
        public Dictionary<Type, Func<object>> TypeInterceptorValueProviders { get; set; }
        public Dictionary<Attribute, IAttributeHandler> AttributeHandlers { get; set; }
        public Dictionary<GenerationOption, double> OptionChances { get; set; }
    }
}
