namespace FakeTests.Tests
{
    [TestFixture]
    public class ListNullablesTest : TestBase
    {
        [Test]
        public void Should_Generate_List_Of_Nullable_Integers()
        {
            List<int?> result = EntityGenerator.CreateFake<List<int?>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            Assert.That(Config.ValidNullablePrimitiveList(result), Is.True);
        }

        [Test]
        public void Should_Generate_List_Of_Nullable_Doubles()
        {
            List<double?> result = EntityGenerator.CreateFake<List<double?>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            if (Config.AllowNullPrimitives)
            {
                Assert.That(result, Contains.Value(null));
            }
        }

        [Test]
        public void ListNullables_Test1()
        {
            var response = (ListNullables)EntityGenerator.CreateFake(typeof(ListNullables));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListNullables_Test2()
        {
            var response = EntityGenerator.CreateFake<ListNullables>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
