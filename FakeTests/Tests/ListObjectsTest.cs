namespace FakeTests.Tests
{
    public class ListObjectsTest : TestBase
    {
        [Test]
        public void ListObjects_Test1()
        {
            var response = (ListObjects)generator.CreateFake(typeof(ListObjects));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
        
        [Test]
        public void ListObjects_Test2()
        {
            var response = generator.CreateFake<ListObjects>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}