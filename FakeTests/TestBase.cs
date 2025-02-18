using AutoPopulate.Core;
using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests
{
    /// <summary>
    /// Base class for setting up shared test instances.
    /// </summary>
    [TestFixture]
    public abstract class TestBase
    {
        protected IEntityGenerator EntityGenerator;
        protected IEntityGenerationConfig Config;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // This can be used for any global setup if needed
        }

        [SetUp]
        public void Setup()
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
            var collectionGenerator = new CollectionGenerator(Config);
            var defaultValueProvider = new AutoPopulate.Implementations.DefaultValueProvider(Config);

            EntityGenerator = new EntityGenerator(typeMetadataCache, objectFactory, collectionGenerator, defaultValueProvider, Config);
        }
    }
}
