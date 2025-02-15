namespace FakeTests.Tests
{
    public class ListNullablesTest : TestBase
    {
        [Test]
        public void ListNullables_Test1()
        {
            var response = (ListNullables)generator.CreateFake(typeof(ListNullables));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListNullables_Test2()
        {
            var response = generator.CreateFake<ListNullables>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
