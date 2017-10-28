using System;
using CSharpFunctionalExtensions;

namespace EdmxConv.Core
{
    public static class OnSuccessTryExtensions
    {
        #region T -> K (E)

        public static Result<K> OnSuccessTry<T, K, E>(this Result<T> result, Func<T, Result<K>> func, string error1 = "")
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
                return Result.Fail<K>(error1 == string.Empty ? e.Message : error1);
            }
        }

        public static Result<K> OnSuccessTry<T, K, E>(this Result<T> result, Func<T, K> func, string error1 = "")
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
                return Result.Fail<K>(error1 == string.Empty ? e.Message : error1);
            }
        }

        #endregion

        #region T -> K (E1, E2)

        public static Result<K> OnSuccessTry<T, K, E1, E2>(this Result<T> result, Func<T, Result<K>> func, string error1 = "", string error2 = "")
            where E1 : Exception
            where E2 : Exception
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            try
            {
                return func(result.Value);
            }
            catch (E1 e)
            {
                return Result.Fail<K>(error1 == string.Empty ? e.Message : error1);
            }
            catch (E2 e)
            {
                return Result.Fail<K>(
                    error2 != string.Empty ? error2
                    : error1 != string.Empty ? error1
                    : e.Message);
            }
        }

        public static Result<K> OnSuccessTry<T, K, E1, E2>(this Result<T> result, Func<T, K> func, string error1 = "", string error2 = "")
            where E1 : Exception
            where E2 : Exception
        {
            if (result.IsFailure)
                return Result.Fail<K>(result.Error);

            try
            {
                return Result.Ok(func(result.Value));
            }
            catch (E1 e)
            {
                return Result.Fail<K>(error1 == string.Empty ? e.Message : error1);
            }
            catch (E2 e)
            {
                return Result.Fail<K>(
                    error2 != string.Empty ? error2
                    : error1 != string.Empty ? error1
                    : e.Message);
            }
        }

        #endregion
    }
}