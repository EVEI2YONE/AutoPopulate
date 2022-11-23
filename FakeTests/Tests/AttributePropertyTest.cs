namespace FakeTests.Tests
{
    public class AttributePropertyTest : TestBase
    {
        [Test]
        public void AttributeProperty_Test1()
        {
            var response = generator.CreateFake<AttributeProperty>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
