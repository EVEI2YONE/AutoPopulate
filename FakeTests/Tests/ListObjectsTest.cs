namespace FakeTests.Tests
{
    public class ListObjectsTest : TestBase
    {
        public void ListObjects_Test1()
        {
            var response = (ListObjects)generator.CreateFake(typeof(ListObjects));

            Assert.IsNotNull(response);
        }
    }
}