using CSharpFunctionalExtensions;

namespace EdmxConv.Schema.Extensions
{
    public static class EdmxExtensions
    {
        public static Result<T> GetAs<T>(this Edmx @base) where T : Edmx =>
            @base is T target
                ? Result.Ok(target)
                : Result.Fail<T>($"Given EDMX is not an instance of {nameof(T)}.");
    }
}
