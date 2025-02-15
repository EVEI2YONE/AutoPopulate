namespace FakeTests.Tests
{
    public class ListListsTest : TestBase
    {
        [TearDown]
        public void CleanUp()
        {
            generator.SetRandomizationRange(1, 1);
            generator.RandomizationBehavior = AutoPopulate.RandomizationType.Fixed;
        }

        [Test]
        public void ListList_Test1()
        {
            var response = (ListLists?) generator.CreateFake(typeof(ListLists));

            Assert.That(response?.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListList_Test2()
        {
            var response = generator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListList_Test3()
        {
            generator.CollectionLimit = 2;
            generator.RandomizationBehavior = AutoPopulate.RandomizationType.Fixed;
            var response = generator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
        
        [Test]
        public void ListList_Test4()
        {
            generator.SetRandomizationRange(1, 4);
            generator.RandomizationBehavior = AutoPopulate.RandomizationType.Range;
            var response = generator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
