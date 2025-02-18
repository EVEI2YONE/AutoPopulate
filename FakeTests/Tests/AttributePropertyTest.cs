using AutoPopulate.Core;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;

namespace FakeTests.Tests
{
    [TestFixture]
    public class AttributePropertyTest : TestBase
    {
        private class SampleAttributeObject
        {
            [AutoPopulate("TestValue")]
            public string Name { get; set; }
        }

        [Test]
        public void Should_Populate_Using_AutoPopulateAttribute()
        {
            SampleAttributeObject result = EntityGenerator.CreateFake<SampleAttributeObject>();
            Assert.That(result, Is.Not.Null);
            if (Config.UseAutoPopulateAttributes)
            {
                Assert.That(result.Name, Is.EqualTo("TestValue"));
            }
        }

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
            Assert.That(exists, Is.True);
        }

        [Test]
        public void AttributeProperty_Test1()
        {
            var response = EntityGeneratorOrig.CreateFake<AttributeProperty>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);
        }
    }
}
