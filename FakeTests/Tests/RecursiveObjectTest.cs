using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;

namespace FakeTests.Tests
{
    [TestFixture]
    public class RecursiveObjectTest : TestBase
    {
        private class SimpleRecursiveObject2
        {
            public SimpleRecursiveObject2? Child { get; set; }
        }

        [Test]
        public void Should_Generate_Recursive_Object()
        {
            SimpleRecursiveObject2 result = EntityGenerator.CreateFake<SimpleRecursiveObject2>();
            Assert.That(result, Is.Not.Null);
            if (Config.MaxRecursionDepth > 0)
            {
                Assert.That(result.Child, Is.Not.Null);
            }
        }

        [Test]
        public void Should_Limit_Recursion_Depth()
        {
            (Config as EntityGenerationConfig).MaxRecursionDepth = 2;
            SimpleRecursiveObject2 result = EntityGenerator.CreateFake<SimpleRecursiveObject2>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Child, Is.True);
            if (Config.MaxRecursionDepth == 2)
            {
                Assert.That(result.Child.Child, Is.Not.Null);
            }
        }

        [Test]
        public void RecursiveObject_Test1()
        {
            var response = (RecursiveObject)EntityGeneratorOrig.CreateFake(typeof(RecursiveObject));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void RecursiveObject_Test2()
        {
            var response = EntityGeneratorOrig.CreateFake<RecursiveObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void RecursiveObject_Test3()
        {
            (Config as EntityGenerationConfig).MaxRecursionDepth = 2;
            var response = EntityGeneratorOrig.CreateFake<RecursiveObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
            (Config as EntityGenerationConfig).MaxRecursionDepth = 1;
        }
    }
}
