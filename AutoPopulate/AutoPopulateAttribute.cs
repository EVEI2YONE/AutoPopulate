using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace AutoPopulate_Attribute
{
    [AttributeUsage(AttributeTargets.All, Inherited = true, AllowMultiple = true)]
    public class AutoPopulateBaseAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = true)]
    public class AutoPopulateAttribute : AutoPopulateBaseAttribute
    {
        private object[] _values { get; set; }
        public object[] Values { get { return _values; } }
        public AutoPopulateAttribute(params object[] values) 
        {
            this._values = values;
        }
    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AutoPopulateCallbackAttribute : AutoPopulateBaseAttribute
    {

    }

    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class AutoPopulateDependencyResolverAttribute : AutoPopulateBaseAttribute
    {
        
    }
}
