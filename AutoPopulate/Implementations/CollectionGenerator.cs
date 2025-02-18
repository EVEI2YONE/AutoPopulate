using AutoPopulate.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoPopulate.Implementations
{
    /// <summary>
    /// Generates lists, dictionaries, and other collections with configurable options.
    /// </summary>
    public class CollectionGenerator : ICollectionGenerator
    {
        private readonly IEntityGenerationConfig _config;

        public CollectionGenerator(IEntityGenerationConfig config)
        {
            _config = config;
        }

        public object GenerateCollection(Type collectionType, Func<Type, object> elementGenerator)
        {
            if (collectionType.IsGenericType)
            {
                Type genericTypeDef = collectionType.GetGenericTypeDefinition();
                Type[] genericArgs = collectionType.GetGenericArguments();
                int listSize = _config.RandomizeListSize ? new Random().Next(_config.MinListSize, _config.MaxListSize + 1) : _config.MaxListSize;

                if (genericTypeDef == typeof(List<>))
                {
                    var list = (IList<object>)Activator.CreateInstance(typeof(List<>).MakeGenericType(genericArgs[0]))!;
                    for (int i = 0; i < listSize; i++)
                    {
                        list.Add(elementGenerator(genericArgs[0]));
                    }
                    return list;
                }
            }
            throw new NotSupportedException($"Collection type {collectionType.FullName} is not supported");
        }
    }
}
