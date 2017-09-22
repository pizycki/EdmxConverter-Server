using CSharpFunctionalExtensions;
using EdmxConv.Schema;
using EdmxConv.Schema.DTO;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.Validation
{
    public class ConvertParamsValidationTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void should_not_validate_params_with_empty_string_edmx(string edmx) =>
            With(new ConvertParams { Edmx = edmx })
                .OnSuccess(@params => ConvertParamsValidationModule.Validate(@params))
                .OnBoth(result =>
                {
                    result.IsFailure.ShouldBeTrue();
                    return result;
                });

        [Theory]
        [InlineData(EdmxTypeEnum.Database)]
        [InlineData(EdmxTypeEnum.Xml)]
        [InlineData(EdmxTypeEnum.Resource)]
        public void should_not_validate_params_with_equal_source_and_target_types(EdmxTypeEnum sourceAndTarget) =>
            With(new ConvertParams
            {
                Edmx = "sample edmx",
                Source = sourceAndTarget,
                Target = sourceAndTarget
            }).OnSuccess(@params => ConvertParamsValidationModule.Validate(@params))
              .OnBoth(result =>
              {
                  result.IsFailure.ShouldBeTrue();
                  return result;
              });
    }
}
