using AutoPopulate.Attributes;
using AutoPopulate.Implementations;
using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AutoPopulate.AttributeHandlers
{
    public class AutoPopulateValueHandler : IAttributeHandler
    {
        public object Handle(PropertyInfo property, object instance, EntityValueProvider provider)
        {
            var attr = property.GetCustomAttribute<AutoPopulateAttribute>();
            return attr?.Values?.Length > 0 ? attr.Values[0] : provider.GetDefaultValue(property.PropertyType, false);
        }
    }
}
