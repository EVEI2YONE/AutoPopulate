namespace FakeTests.Tests
{
    public class PrimitivesTest : TestBase
    {

        [Test]
        public void Should_Generate_Integer()
        {
            int result = EntityGenerator.CreateFake<int>();
            Assert.Equals(0, result);
        }

        [Test]
        public void Should_Generate_Boolean()
        {
            bool result = EntityGenerator.CreateFake<bool>();
            Assert.Equals(false, result);
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
