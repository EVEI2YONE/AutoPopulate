using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FakeTests.Classes
{
    public class DelegateCallback : ITestableObject
    {
        public string value { get; set; }
        public string Test()
        {
            return "Callback value";
        }

        public bool ItemsSuccessfullyPopulated(int? depth = 1)
        {
            if (value != "Callback value") return false;
            return true;
        }
    }
}
