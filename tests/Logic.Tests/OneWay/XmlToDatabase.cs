using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class XmlToDatabase
    {
        [Fact(DisplayName = "Xml -> DB",
              Skip = "GZipping produces different results. This test is temporary turned off.")]
        public void convert_xml_to_database() =>
            Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ToDatabase.FromXml)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleDatabaseEdmx));
    }
}