using AutoPopulate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests
{
    [TestFixture]
    public class TestBase
    {
        protected EntityGenerator generator;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            generator = new EntityGenerator();
            generator.CollectionLimit = 1;
        }
    }
}
