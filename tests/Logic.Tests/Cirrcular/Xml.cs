using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.Cirrcular
{
    public class Xml
    {
        [Fact(DisplayName = "Xml -> Resx -> Xml")]
        public void convert_xml_to_resource_back_and_forth() =>
            Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ToResource.FromXml)
                .Bind(ToXml.FromResource)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleXmlEdmx));

        [Fact(DisplayName = "Xml -> DB -> Xml")]
        public void convert_xml_to_database_back_and_forth() =>
            Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ToDatabase.FromXml)
                .Bind(ToXml.FromDatabaseModel)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleXmlEdmx));

    }
}
