using System;
using System.Collections.Generic;
using System.Text;
using static AutoPopulate.EntityGenerator;

namespace AutoPopulate
{
    public interface IEntityGenerator
    {
        public Dictionary<Type, Func<object>> DefaultValues { get; }
        public int RecursiveLimit { get; set; }
        public int CollectionLimit { get; set; }
        public int CollectionStart { get; set; }
        public RandomizationType RandomizationBehavior { get; set; }
        public T CreateFake<T>() where T : class, new();
        public object? CreateFake(Type type);
        public void SetListRandomRange(int start, int end);
    }
}
