using System;
using System.Collections.Generic;
using System.Text;
using static AutoPopulate.Core.EntityGenerator;

namespace AutoPopulate
{
    public interface IEntityGenerator
    {
        public T CreateFake<T>() where T : class, new();
        public object CreateFake(Type type);
    }
}
