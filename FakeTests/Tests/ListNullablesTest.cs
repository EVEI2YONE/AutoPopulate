namespace FakeTests.Tests
{
    public class ListNullablesTest : TestBase
    {
        [Test]
        public void ListNullables_Test1()
        {
            var response = (ListNullables)generator.CreateFake(typeof(ListNullables));

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }

        [Test]
        public void ListNullables_Test2()
        {
            var response = generator.CreateFake<ListNullables>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
