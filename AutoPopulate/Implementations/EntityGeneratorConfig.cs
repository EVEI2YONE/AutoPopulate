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
        public int MinListSize { get; set; } = 1;
        public int MaxListSize { get; set; } = 5;
        public bool RandomizeListSize { get; set; } = true;

        public int MaxRecursionDepth { get; set; } = 3;
        public Dictionary<Type, Func<object>> TypeInterceptorValueProviders { get; set; } = new()
        {
            { typeof(string), () => "_" },
            { typeof(bool), () => true },
            { typeof(short), () => (short)1 },
            { typeof(int), () => 1 },
            { typeof(uint), () => 1u },
            { typeof(long), () => 1L },
            { typeof(ulong), () => 1ul },
            { typeof(decimal), () => 1m },
            { typeof(double), () => 1.0d },
            { typeof(float), () => 1.0f },
            { typeof(char), () => '_' },
            { typeof(byte), () => (byte)('_') },
            { typeof(sbyte), () => (sbyte)1 },
            { typeof(DateTime), () => DateTime.Now },
            { typeof(object), () => "object" },
        };
        public Dictionary<Attribute, IAttributeHandler> AttributeHandlers { get; set; } = new();
        public Dictionary<GenerationOption, double> OptionChances { get; set; } =
        new Dictionary<GenerationOption, double>
        {
            { GenerationOption.NullablePrimitiveChance, 0.1 },
            { GenerationOption.NullableObjectChance, 0.1 },
            { GenerationOption.RecursionExistingReferenceChance, 0.25 }
        };
    }
}
