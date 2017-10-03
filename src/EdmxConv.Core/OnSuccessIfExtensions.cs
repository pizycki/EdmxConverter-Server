using System;
using CSharpFunctionalExtensions;

namespace EdmxConv.Core
{
    public static class OnSuccessIfExtensions
    {
        public static Result<T> OnSuccessIf<T>(this Result<T> result, Func<T, bool> condition, Func<T, Result<T>> func ) =>
            result.IsFailure ? Result.Fail<T>(result.Error)
            : condition(result.Value) ? func(result.Value)
            : result;
    }
}
