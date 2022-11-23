using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace FakeTests.Tests
{
    public class AttributePropertyTest : TestBase
    {
        [Test]
        public void AttributePropertyExists_Test()
        {
            Type attributeProperty = typeof(AttributeProperty);

            var classProps = attributeProperty.GetProperties();
            var memberProps = classProps.Where(x => x.GetCustomAttributes(true).Any());
            var attributeExists = memberProps.Where(x => x.DeclaringType == typeof(AttributeProperty)).Any();
            Assert.IsTrue(attributeExists);
        }

        [Test]
        public void AttributeProperty_Test1()
        {
            var response = generator.CreateFake<AttributeProperty>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
