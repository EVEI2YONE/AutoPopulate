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
            if (Config.OptionChances.TryGetValue(GenerationOption.NullablePrimitiveChance, out var chance) && chance > 0.5)
            {
                Assert.That(result, Contains.Value(null));
            }
        }

        [Test]
        public void Should_Generate_All_Nullable_Doubles()
        {
            Config.OptionChances[GenerationOption.NullablePrimitiveChance] = 1.0;
            List<double?> result = EntityGenerator.CreateFake<List<double?>>();
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);
            Assert.That(result.Count, Is.GreaterThanOrEqualTo(Config.MinListSize));
            Assert.That(result.Count, Is.LessThanOrEqualTo(Config.MaxListSize));
            Assert.That(result, Has.All.Null);
        }

        [Test]
        public void ListNullables_Test1()
        {
            var response = (ListNullables)EntityGeneratorOrig.CreateFake(typeof(ListNullables));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }

        [Test]
        public void ListNullables_Test2()
        {
            var response = EntityGeneratorOrig.CreateFake<ListNullables>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
