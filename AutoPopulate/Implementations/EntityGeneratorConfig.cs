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
        public bool AllowNullPrimitives { get; set; } = false;
        public bool AllowNullObjects { get; set; } = false;
        public Dictionary<Type, Func<object>> CustomPrimitiveGenerators { get; set; } = new();

        public int MinListSize { get; set; } = 1;
        public int MaxListSize { get; set; } = 5;
        public bool RandomizeListSize { get; set; } = true;

        public int MaxRecursionDepth { get; set; } = 3;
        public RecursionReferenceBehavior ReferenceBehavior { get; set; } = RecursionReferenceBehavior.NewInstance;

        public bool UseAutoPopulateAttributes { get; set; } = true;
        public bool UseTypeInterceptors { get; set; } = true;

        public bool EnableParallelProcessing { get; set; } = false;
        public bool EnableDebugLogging { get; set; } = false;
    }
}
