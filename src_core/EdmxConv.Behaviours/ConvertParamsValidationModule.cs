using CSharpFunctionalExtensions;
using EdmxConv.Schema.DTO;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public class ConvertParamsValidationModule
    {
        public static Result<ConvertParams> Validate(ConvertParams payload) =>
            With(payload)
                .Ensure(x => !string.IsNullOrWhiteSpace(x.Edmx), "Source EDMX should not be empty.")
                .Ensure(x => x.Source != payload.Target, "Source and target types are the same.");
    }
}