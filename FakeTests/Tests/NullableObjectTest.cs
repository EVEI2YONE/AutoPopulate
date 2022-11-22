namespace FakeTests.Tests
{
    public class NullableObjectTest : TestBase
    {
        [Test]
        public void NullableObject_Test1()
        {
            var response = (NullableObject)generator.CreateFake(typeof(NullableObject));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void NullableObject_Test2()
        {
            var response = generator.CreateFake<NullableObject>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
