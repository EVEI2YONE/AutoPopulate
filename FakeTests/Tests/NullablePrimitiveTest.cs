namespace FakeTests.Tests
{
    public class NullablePrimitiveTest : TestBase
    {
        [Test]
        public void Should_Generate_Nullable_Integer()
        {
            int? result = EntityGenerator.CreateFake<int?>();
            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void NullablePrimitives_Test_1()
        {
            var response = (NullablePrimitive) EntityGenerator.CreateFake(typeof(NullablePrimitive));

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
        
        [Test]
        public void NullablePrimitives_Test_2()
        {
            var response = EntityGenerator.CreateFake<NullablePrimitive>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
