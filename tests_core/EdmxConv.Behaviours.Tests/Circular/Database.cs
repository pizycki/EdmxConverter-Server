using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours.Tests.Circular
{
    public class Database
    {
        [Fact(DisplayName = "DB -> Resx -> Db")]
        public void convert_database_to_resource_back_and_forth() =>
            With(Resources.SampleDatabaseEdmx)
                .Map(edmx => edmx.ToHex())
                .Map(edmx => edmx.ToDatabaseEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToResource(edmx))
                .OnSuccess(edmx => edmx.GetAs<ResourceEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleDatabaseEdmx));


        [Fact(DisplayName = "DB -> Xml -> DB")]
        public void convert_database_to_xml_back_and_forth() =>
            With(Resources.SampleDatabaseEdmx)
                .Map(edmx => edmx.ToHex())
                .Map(edmx => edmx.ToDatabaseEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .OnSuccess(edmx => edmx.GetAs<XmlEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleDatabaseEdmx));
    }
}
