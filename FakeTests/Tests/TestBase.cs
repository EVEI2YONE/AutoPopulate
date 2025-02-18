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
            // This can be used for any global setup if needed
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
                AllowNullObjects = false,
                AllowNullPrimitives = false,
                MaxRecursionDepth = 3,
                ReferenceBehavior = RecursionReferenceBehavior.NewInstance
            };

            var typeMetadataCache = new TypeMetadataCache();
            var objectFactory = new ObjectFactory(Config);
            var defaultValueProvider = new AutoPopulate.Implementations.DefaultValueProvider(Config);

            EntityGenerator = new EntityGenerator(typeMetadataCache, objectFactory, defaultValueProvider, Config);
        }

        private void SetupOrig()
        {
            ConfigOrig = new EntityGenerationConfig
            {
                MinListSize = 1,
                MaxListSize = 1,
                RandomizeListSize = false,
                AllowNullObjects = false,
                AllowNullPrimitives = false,
                MaxRecursionDepth = 3,
                ReferenceBehavior = RecursionReferenceBehavior.NewInstance
            };

            var typeMetadataCache = new TypeMetadataCache();
            var objectFactory = new ObjectFactory(ConfigOrig);
            var defaultValueProvider = new AutoPopulate.Implementations.DefaultValueProvider(ConfigOrig);

            EntityGeneratorOrig = new EntityGenerator(typeMetadataCache, objectFactory, defaultValueProvider, ConfigOrig);
        }
    }
}
