namespace FakeTests.Tests
{
    [TestFixture]
    public class PrimitivesTest : TestBase
    {

        [Test]
        public void Should_Generate_Integer()
        {
            int result = EntityGenerator.CreateFake<int>();
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Should_Generate_Boolean()
        {
            bool result = EntityGenerator.CreateFake<bool>();
            Assert.That(result, Is.EqualTo(false));
        }

        [Test]
        public void Primitive_Test_1()
        {
            var response = (Primitives) EntityGenerator.CreateFake(typeof(Primitives));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void Primitive_Test_2()
        {
            var response = EntityGenerator.CreateFake<Primitives>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
