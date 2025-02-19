using AutoPopulate.Implementations;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.Interfaces
{
    public interface IAttributeHandler
    {
        public object Handle(PropertyInfo property, object instance, EntityValueProvider provider);
    }
}
