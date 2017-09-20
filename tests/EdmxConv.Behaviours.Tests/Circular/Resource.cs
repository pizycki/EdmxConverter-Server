using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.Circular
{
    public class Resource
    {
        [Fact(DisplayName = "Resx -> Xml -> Resx")]
        public void convert_resource_to_xml_back_and_forth() =>
            With(Resources.SampleResourceEdmx)
                .OnSuccess(edmx => edmx.ToResourceEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .OnSuccess(edmx => edmx.GetAs<XmlEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToResource(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleResourceEdmx));

        [Fact(DisplayName = "Resx -> DB -> Resx")]
        public void convert_resource_to_database_back_and_forth() =>
            With(Resources.SampleResourceEdmx)
                .OnSuccess(edmx => edmx.ToResourceEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .OnSuccess(edmx => edmx.GetAs<DatabaseEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToResource(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleResourceEdmx));

    }
}
