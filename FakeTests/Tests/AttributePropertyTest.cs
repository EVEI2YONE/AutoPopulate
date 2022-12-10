using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace FakeTests.Tests
{
    public class AttributePropertyTest : TestBase
    {
        [Test]
        public void AttributePropertyExists_Test()
        {
            Type attributeProperty = typeof(AttributeProperty);
            Type customAttribute = typeof(AutoPopulateAttribute);

            var classProps = attributeProperty.GetProperties();
            bool exists = false;
            foreach(var prop in classProps)
            {
                var customattributes =  prop.GetCustomAttributes(customAttribute, true).ToList();
                if(customattributes.Any())
                    exists = true;
            }
            Assert.IsTrue(exists);
        }

        [Test]
        public void AttributeProperty_Test1()
        {
            var response = generator.CreateFake<AttributeProperty>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());
        }
    }
}
