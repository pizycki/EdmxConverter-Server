using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.OneWay
{
    public class ConvertFromXmlToDatabase
    {
        [Fact(DisplayName = "Xml -> DB")]
        public void convert_xml_to_database() =>
            With(Resources.SampleXmlEdmx)
                .OnSuccess(sample => sample.ToXmlEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleDatabaseEdmx));
    }
}