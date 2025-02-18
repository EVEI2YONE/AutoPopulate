using AutoPopulate.Core;
using AutoPopulate.Implementations;

namespace FakeTests.Tests
{
    [TestFixture]
    public class ListListsTest : TestBase
    {
        [Test]
        public void Should_Generate_List_Of_Integer_Lists()
        {
            List<List<int>> result = EntityGenerator.CreateFake<List<List<int>>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            Assert.That(Config.ValidList(result), Is.True);
        }

        [Test]
        public void Should_Generate_List_Of_Object_Lists()
        {
            List<List<ListObjectsTest.SampleObject>> result = EntityGenerator.CreateFake<List<List<ListObjectsTest.SampleObject>>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));

            foreach (var innerList in result)
            {
                Assert.That(innerList, Is.Not.Null);
                Assert.That(innerList, Is.Not.Empty);
                Assert.That(innerList.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
                Assert.That(innerList.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            }
        }

        [TearDown]
        public void CleanUp()
        {
            (ConfigOrig as EntityGenerationConfig).MinListSize = 1;
            (ConfigOrig as EntityGenerationConfig).MaxListSize = 1;
            (ConfigOrig as EntityGenerationConfig).RandomizeListSize = false;
        }

        [Test]
        public void ListList_Test1()
        {
            var response = (ListLists?)EntityGenerator.CreateFake(typeof(ListLists));

            Assert.That(response?.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListList_Test2()
        {
            var response = EntityGenerator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListList_Test3()
        {
            (ConfigOrig as EntityGenerationConfig).MaxListSize = 2;
            (ConfigOrig as EntityGenerationConfig).RandomizeListSize = false;
            var response = EntityGenerator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListList_Test4()
        {
            (ConfigOrig as EntityGenerationConfig).MinListSize = 1;
            (ConfigOrig as EntityGenerationConfig).MaxListSize = 4;
            (ConfigOrig as EntityGenerationConfig).RandomizeListSize = true;
            var response = EntityGenerator.CreateFake<ListLists>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
