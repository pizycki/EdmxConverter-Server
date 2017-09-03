using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class DatabaseToXml
    {
        [Fact(DisplayName = "DB -> Xml")]
        public void convert_from_database_to_xml() =>
            Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ToXml.FromDatabaseModel)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleXmlEdmx));
    }
}