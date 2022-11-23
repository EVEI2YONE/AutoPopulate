namespace FakeTests.Tests
{
    public class ListListsTest : TestBase
    {
        [TearDown]
        public void CleanUp()
        {
            AutoPopulate.SetRandomizationRange(1, 1);
            AutoPopulate.RandomizationBehavior = AutoPopulate.RandomizationType.Fixed;
        }

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

        [Test]
        public void ListList_Test3()
        {
            AutoPopulate.CollectionLimit = 2;
            AutoPopulate.RandomizationBehavior = AutoPopulate.RandomizationType.Fixed;
            var response = generator.CreateFake<ListLists>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
        
        [Test]
        public void ListList_Test4()
        {
            AutoPopulate.SetRandomizationRange(1, 4);
            AutoPopulate.RandomizationBehavior = AutoPopulate.RandomizationType.Range;
            var response = generator.CreateFake<ListLists>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
