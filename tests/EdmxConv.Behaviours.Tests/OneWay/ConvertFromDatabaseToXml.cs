using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.OneWay
{
    public class ConvertFromDatabaseToXml
    {
        [Fact(DisplayName = "DB -> Xml")]
        public void convert_from_database_to_xml() =>
            With(Resources.SampleDatabaseEdmx)
                .Map(sample => sample.ToHex())
                .Map(sample => sample.ToDatabaseEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleXmlEdmx));
    }
}