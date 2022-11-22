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
    }
}
