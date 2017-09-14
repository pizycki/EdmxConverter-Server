using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace EdmxConv.Core
{
    public static class DictionaryExtensions
    {
        public static Maybe<V> GetMaybe<K, V>(this IReadOnlyDictionary<K, V> dict, K key) where V : class
        {
            try
            {
                return Maybe<V>.From(dict[key]);
            }
            catch (ArgumentOutOfRangeException)
            {
                return new Maybe<V>();
            }
        }
    }
}