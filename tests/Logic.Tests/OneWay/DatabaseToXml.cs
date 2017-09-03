using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using LanguageExt;
using Shouldly;
using Xunit;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class DatabaseToXml
    {
        [Fact]
        public void should_convert() =>
            Prelude.Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ToXml.FromDatabaseModel)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleXmlEdmx));
    }
}