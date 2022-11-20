using FakeDbContext;
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
        protected AutoPopulate generator;
        [OneTimeSetUp] public void OneTimeSetUp()
        {
            generator = new AutoPopulate();
        }
    }
    /*
     * Todo:
     *  nullable object
     *  list object
     *  list lists
     *  complex object
     *  recursive object
     *  attribute property
     *  customer setter
     */
}
