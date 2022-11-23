namespace FakeTests.Tests
{
    public class ComplexObjectTest : TestBase
    {
        [Test]
        public void ComplexObject_Test1()
        {
            var response = (ComplexObject)generator.CreateFake(typeof(ComplexObject));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void ComplexObject_Test2()
        {
            var response = generator.CreateFake<ComplexObject>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
