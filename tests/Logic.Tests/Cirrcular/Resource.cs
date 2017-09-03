using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.Cirrcular
{
    public class Resource
    {
        [Fact(DisplayName = "")]
        public void convert_resource_to_xml_back_and_forth() =>
            Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ToXml.FromResource)
                .Bind(ToDatabase.FromXml)
                .IfSome(edmx => edmx.ToString()
                    .ShouldBe(Resources.SampleResourceEdmx));

        [Fact]
        public void convert_resource_to_database_back_and_forth() =>
            Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ToDatabase.FromResource)
                .Bind(ToResource.FromDatabaseModel)
                .IfSome(edmx => edmx.ToString()
                                    .ShouldBe(Resources.SampleResourceEdmx));

    }
}
