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
        // Primitive Value Handling
        bool AllowNullPrimitives { get; }
        bool AllowNullObjects { get; }
        Dictionary<Type, Func<object>> CustomPrimitiveGenerators { get; }

        // List Generation
        int MinListSize { get; }
        int MaxListSize { get; }
        bool RandomizeListSize { get; }

        // Recursive Object Handling
        int MaxRecursionDepth { get; }
        RecursionReferenceBehavior ReferenceBehavior { get; }

        // Attribute & Type Interception
        bool UseAutoPopulateAttributes { get; }
        bool UseTypeInterceptors { get; }

        // Performance & Debugging
        bool EnableParallelProcessing { get; }
        bool EnableDebugLogging { get; }
    }
}
