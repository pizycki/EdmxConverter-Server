using LanguageExt;

namespace EdmxConverter.Core
{
    public static class OptionExtensions
    {
        public static Result<T> ToResult<T>(this Option<T> option, string errorMessage)
        {
            return option.Match(Some: Result.Ok,
                                None: () => Result.Fail<T>(errorMessage));
        }
    }
}
