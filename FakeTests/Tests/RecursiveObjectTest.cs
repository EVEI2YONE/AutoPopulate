namespace FakeTests.Tests
{
    public class RecursiveObjectTest : TestBase
    {
        [Test]
        public void RecursiveObject_Test1()
        {
            var response = (RecursiveObject) generator.CreateFake(typeof(RecursiveObject));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
