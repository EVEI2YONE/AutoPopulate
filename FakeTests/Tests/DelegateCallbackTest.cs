using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests.Tests
{
    internal class DelegateCallbackTest : TestBase
    {
        [Test]
        public void DelegateTest()
        {
            var callback = new DelegateCallback().Test;
            var orig = generator.DefaultValues[typeof(string)];
            generator.DefaultValues[typeof(string)] = callback;

            var response = generator.CreateFake<DelegateCallback>();

            Assert.That(response.ItemsSuccessfullyPopulated(), Is.True);

            generator.DefaultValues[typeof(string)] = orig;
        }
    }
}
