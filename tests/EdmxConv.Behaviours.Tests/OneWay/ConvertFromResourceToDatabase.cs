using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Modules;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.OneWay
{
    public class ConvertFromResourceToDatabase
    {
        [Fact(DisplayName = "Resx -> DB")]
        public void convert_from_resource_to_database() =>
            With(Resources.SampleResourceEdmx)
                .OnSuccess(sample => sample.ToResourceEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleDatabaseEdmx));
    }
}