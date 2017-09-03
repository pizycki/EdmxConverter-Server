using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using LanguageExt;
using Shouldly;
using Xunit;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class XmlToDatabase
    {
        [Fact]
        public void should_convert() =>
            Prelude.Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ToDatabase.FromXml)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleDatabaseEdmx));
    }
}