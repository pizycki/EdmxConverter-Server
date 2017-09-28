using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Modules;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.OneWay
{
    public class ConvertFromDatabaseToResource
    {
        [Fact(DisplayName = "DB -> Resx")]
        public void convert_from_database_to_resource() =>
            With(Resources.SampleDatabaseEdmx)
                .Map(sample => sample.ToHex())
                .Map(sample => sample.ToDatabaseEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToResource(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleResourceEdmx));
    }
}
