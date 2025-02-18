namespace FakeTests.Tests
{
    [TestFixture]
    public class ListPrimitivesTest : TestBase
    {
        [Test]
        public void Should_Generate_List_Of_Integers()
        {
            List<int> result = EntityGenerator.CreateFake<List<int>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
        }

        [Test]
        public void Should_Generate_List_Of_Booleans()
        {
            List<bool> result = EntityGenerator.CreateFake<List<bool>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
        }

        [Test]
        public void ListPrimitives_Test1()
        {
            var response = (ListPrimitives)EntityGeneratorOrig.CreateFake(typeof(ListPrimitives));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        

        [Test]
        public void ListPrimitives_Test2()
        {
            var response = EntityGeneratorOrig.CreateFake<ListPrimitives>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
