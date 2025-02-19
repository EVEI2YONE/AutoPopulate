namespace FakeTests.Tests
{
    [TestFixture]
    public class ListObjectsTest : TestBase
    {
        public class SampleObject
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Test]
        public void Should_Generate_List_Of_Objects()
        {
            List<SampleObject> result = EntityGenerator.CreateFake<List<SampleObject>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            Assert.That(Config.ValidList(result), Is.True);
        }

        [Test]
        public void ListObjects_Test1()
        {
            var response = (ListObjects)EntityGeneratorOrig.CreateFake(typeof(ListObjects));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
        
        [Test]
        public void ListObjects_Test2()
        {
            var response = EntityGeneratorOrig.CreateFake<ListObjects>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}