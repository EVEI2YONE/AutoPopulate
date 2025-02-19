using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPopulate.Attributes
{

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class AutoPopulateAttribute : Attribute
    {
        private object[] _values { get; set; }
        public object[] Values { get { return _values; } }
        public AutoPopulateAttribute(params object[] values) 
        {
            _values = values;
        }
    }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class AutoPopulateIgnoreAttribute : Attribute
    {
    }
}
