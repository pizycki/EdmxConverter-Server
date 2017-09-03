using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class ResourceToXml
    {
        [Fact(DisplayName = "Resx -> Xml")]
        public void should_convert() =>
            Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ToXml.FromResource)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleXmlEdmx));
    }
}