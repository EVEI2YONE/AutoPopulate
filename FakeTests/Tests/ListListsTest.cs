namespace FakeTests.Tests
{
    public class ListListsTest : TestBase
    {
        [Test]
        public void ListList_Test1()
        {
            var response = (ListLists) generator.CreateFake(typeof(ListLists));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void ListList_Test2()
        {
            var response = generator.CreateFake<ListLists>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
