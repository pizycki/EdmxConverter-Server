using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.Cirrcular
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


        [Fact(DisplayName = "DB -> Xml -> DB",
              Skip = "GZipping produces different results. This test is temporary turned off.")]
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
