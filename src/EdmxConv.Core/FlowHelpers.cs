using CSharpFunctionalExtensions;

namespace EdmxConv.Core
{
    public static class FlowHelpers
    {
        public static Result<K> With<K>(K target) => Result.Ok(target); // Consider null check

        //public static Result Try<T, K, E>(this Result<T> result, Func<T, K> func, string error = "")
        //    where E : Exception where K : class
        //{
        //    if (result.IsFailure)
        //        return result;

        //    try
        //    {
        //        var funcResult = func(result.Value);

        //        switch (funcResult)
        //        {
        //            case Result r:
        //                return r;
        //        }

        //        return funcResult;
        //    }
        //    catch (E ex)
        //    {
        //        return Result.Fail(error ?? ex.Message);
        //    }
        //}
    }
}