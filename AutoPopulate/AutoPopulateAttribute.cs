using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

namespace AutoPopulate_Attribute
{
    public class AutoPopulateBaseAttribute : Attribute { }
    
    public class AutoPopulateAttribute : AutoPopulateBaseAttribute
    {
        public object Value { get; set; }
    }

    public class AutoPopulateCallbackAttribute : AutoPopulateBaseAttribute
    {

    }

    public class AutoPopulateDependencyResolverAttribute : AutoPopulateBaseAttribute
    {
        
    }
}
