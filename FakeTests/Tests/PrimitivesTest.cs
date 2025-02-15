namespace FakeTests.Tests
{
    public class PrimitivesTest : TestBase
    {
        [Test]
        public void Primitive_Test_1()
        {
            var response = (Primitives)generator.CreateFake(typeof(Primitives));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
        
        [Test]
        public void Primitive_Test_2()
        {
            var response = generator.CreateFake<Primitives>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
