using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using LanguageExt;
using Shouldly;
using Xunit;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class ResourceToXml
    {
        [Fact]
        public void should_convert() =>
            Prelude.Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ToXml.FromResource)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleXmlEdmx));
    }
}