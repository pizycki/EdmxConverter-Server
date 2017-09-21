using System;
using CSharpFunctionalExtensions;

namespace EdmxConv.Core
{
    public static class OnSuccessTryExtensions
    {
        public static Result<K> OnSuccessTry<T, K, E>(this Result<T> result, Func<T, Result<K>> func, string error = "")
            where E : Exception
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            try
            {
                return func(result.Value);
            }
            catch (E e)
            {
                return Result.Fail<K>(error == string.Empty ? e.Message : error);
            }
        }

        public static Result<K> OnSuccessTry<T, K, E>(this Result<T> result, Func<T, K> func, string error = "")
            where E : Exception
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            try
            {
                return Result.Ok(func(result.Value));
            }
            catch (E e)
            {
                return Result.Fail<K>(error == string.Empty ? e.Message : error);
            }
        }
    }
}