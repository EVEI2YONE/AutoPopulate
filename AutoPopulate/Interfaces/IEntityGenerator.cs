using System;
using System.Collections.Generic;
using System.Text;
using static AutoPopulate.Core.EntityGenerator;

namespace AutoPopulate.Interfaces
{
    public interface IEntityGenerator
    {
        public T CreateFake<T>();
        public object CreateFake(Type type);
    }
}
