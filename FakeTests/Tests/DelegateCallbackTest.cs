using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests.Tests
{
    [TestFixture]
    internal class DelegateCallbackTest : TestBase
    {
        [Test]
        public void DelegateTest()
        {
            var callback = new DelegateCallback().Test;
            var orig = TestableObjectExtensions.DefaultValues[typeof(string)];
            TestableObjectExtensions.DefaultValues[typeof(string)] = callback;

            var response = EntityGenerator.CreateFake<DelegateCallback>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);

            TestableObjectExtensions.DefaultValues[typeof(string)] = orig;
        }
    }
}
