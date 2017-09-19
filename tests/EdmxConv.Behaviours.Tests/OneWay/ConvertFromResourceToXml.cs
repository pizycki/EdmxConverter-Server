using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.OneWay
{
    public class ConvertFromResourceToXml
    {
        [Fact(DisplayName = "Resx -> Xml")]
        public void convert_from_resource_to_xml() =>
            With(Resources.SampleResourceEdmx)
                .Map(sample => sample.ToResourceEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleXmlEdmx));
    }
}