namespace FakeTests.Tests
{
    [TestFixture]
    public class PrimitivesTest : TestBase
    {

        [Test]
        public void Should_Generate_Integer()
        {
            int result = EntityGenerator.CreateFake<int>();
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Should_Generate_Boolean()
        {
            bool result = EntityGenerator.CreateFake<bool>();
            Assert.That(result, Is.EqualTo(true));
        }

        [Test]
        public void Primitive_Test_1()
        {
            var response = (Primitives)EntityGeneratorOrig.CreateFake(typeof(Primitives));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void Primitive_Test_2()
        {
            var response = EntityGeneratorOrig.CreateFake<Primitives>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
