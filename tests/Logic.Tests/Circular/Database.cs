using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.Circular
{
    public class Database
    {
        [Fact(DisplayName = "DB -> Resx -> Db")]
        public void convert_database_to_resource_back_and_forth() =>
            Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ToResource.FromDatabaseModel)
                .Bind(ToDatabase.FromResource)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleDatabaseEdmx));


        [Fact(DisplayName = "DB -> Xml -> DB")]
        public void convert_database_to_xml_back_and_forth() =>
            Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ToXml.FromDatabaseModel)
                .Bind(ToDatabase.FromXml)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleDatabaseEdmx));
    }
}
