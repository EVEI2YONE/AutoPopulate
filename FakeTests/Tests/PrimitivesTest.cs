namespace FakeTests.Tests
{
    public class PrimitivesTest : TestBase
    {
        [Test]
        public void Primitive_Test_1()
        {
            var response = (Primitives)generator.CreateFake(typeof(Primitives));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
        
        [Test]
        public void Primitive_Test_2()
        {
            var response = generator.CreateFake<Primitives>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
