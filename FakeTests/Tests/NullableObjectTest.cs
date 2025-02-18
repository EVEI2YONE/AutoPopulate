namespace FakeTests.Tests
{
    [TestFixture]
    public class NullableObjectTest : TestBase
    {
        [Test]
        public void NullableObject_Test1()
        {
            var response = (NullableObject)EntityGeneratorOrig.CreateFake(typeof(NullableObject));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void NullableObject_Test2()
        {
            var response = EntityGeneratorOrig.CreateFake<NullableObject>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
