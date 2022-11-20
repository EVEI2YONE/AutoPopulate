namespace FakeTests.Tests
{
    public class NullablePrimitiveTest : TestBase
    {
        [Test]
        public void NullablePrimitives_Test_1()
        {
            var response = (NullablePrimitive)generator.CreateFake(typeof(NullablePrimitive));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
        
        [Test]
        public void NullablePrimitives_Test_2()
        {
            var response = generator.CreateFake<NullablePrimitive>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
