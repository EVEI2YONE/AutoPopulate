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
            var orig = AutoPopulate.DefaultValues[typeof(string)];
            AutoPopulate.DefaultValues[typeof(string)] = callback;

            var response = generator.CreateFake<DelegateCallback>();

            Assert.IsTrue(response.ItemsSuccessfullyPopulated());

            AutoPopulate.DefaultValues[typeof(string)] = orig;
        }
    }
}
