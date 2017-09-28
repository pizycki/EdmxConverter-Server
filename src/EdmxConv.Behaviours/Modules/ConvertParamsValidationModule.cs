using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema.DTO;

namespace EdmxConv.Behaviours.Modules
{
    public class ConvertParamsValidationModule
    {
        public static Result<ConvertParams> Validate(ConvertParams payload) =>
            FlowHelpers.With(payload)
                .Ensure(x => !string.IsNullOrWhiteSpace(x.Edmx), "Source EDMX should not be empty.")
                .Ensure(x => x.Source != payload.Target, "Source and target types are the same.");
    }
}