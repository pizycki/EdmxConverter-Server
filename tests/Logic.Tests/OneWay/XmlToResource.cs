using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class XmlToResource
    {
        [Fact(DisplayName = "Xml -> Resx",
              Skip = "GZipping produces different results. This test is temporary turned off.")]
        public void convert_xml_to_resource() =>
            Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ToResource.FromXml)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleResourceEdmx));
    }
}