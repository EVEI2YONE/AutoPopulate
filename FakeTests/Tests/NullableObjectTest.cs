namespace FakeTests.Tests
{
    public class NullableObjectTest : TestBase
    {
        [Test]
        public void NullableObject_Test1()
        {
            var response = (NullableObject)generator.CreateFake(typeof(NullableObject));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void NullableObject_Test2()
        {
            var response = generator.CreateFake<NullableObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
