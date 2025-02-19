using AutoPopulate.Core;
using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests.Tests
{
    /// <summary>
    /// Base class for setting up shared test instances.
    /// </summary>
    [TestFixture]
    public abstract class TestBase
    {
        protected IEntityGenerator EntityGenerator;
        protected IEntityGenerationConfig Config;

        protected IEntityGenerator EntityGeneratorOrig;
        protected IEntityGenerationConfig ConfigOrig;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            TestableObjectExtensions.DefaultValues = _defaultValues;
        }

        [SetUp]
        public void Setup()
        {
            SetupNew();
            SetupOrig();
        }

        private void SetupNew()
        {
            Config = new EntityGenerationConfig
            {
                MinListSize = 2,
                MaxListSize = 5,
                RandomizeListSize = true,
                MaxRecursionDepth = 3,
                TypeInterceptorValueProviders = _defaultValues,
                AttributeHandlers = new Dictionary<Attribute, IAttributeHandler>(),
                OptionChances = new Dictionary<GenerationOption, double>()
                {
                    { GenerationOption.NullableObjectChance, 0 },
                    { GenerationOption.NullablePrimitiveChance, 0 },
                    { GenerationOption.RecursionExistingReferenceChance, 0 },
                }
            };

            EntityGenerator = new EntityGenerator(config: Config);
        }

        private void SetupOrig()
        {
            ConfigOrig = new EntityGenerationConfig
            {
                MinListSize = 1,
                MaxListSize = 1,
                RandomizeListSize = false,
                MaxRecursionDepth = 3,
                TypeInterceptorValueProviders = _defaultValues,
                AttributeHandlers = new Dictionary<Attribute, IAttributeHandler>(),
                OptionChances = new Dictionary<GenerationOption, double>()
                {
                    { GenerationOption.NullableObjectChance, 0 },
                    { GenerationOption.NullablePrimitiveChance, 0 },
                    { GenerationOption.RecursionExistingReferenceChance, 0 },
                }
            };

            EntityGeneratorOrig = new EntityGenerator(config: ConfigOrig);
        }

        private static Dictionary<Type, Func<object>>  _defaultValues = new Dictionary<Type, Func<object>>()
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
            { typeof(object), () => "object" }
        };
    }
}
