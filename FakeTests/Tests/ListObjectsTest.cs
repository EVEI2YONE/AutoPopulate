namespace FakeTests.Tests
{
    public class ListObjectsTest : TestBase
    {
        [Test]
        public void ListObjects_Test1()
        {
            var response = (ListObjects)generator.CreateFake(typeof(ListObjects));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
        
        [Test]
        public void ListObjects_Test2()
        {
            var response = generator.CreateFake<ListObjects>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}