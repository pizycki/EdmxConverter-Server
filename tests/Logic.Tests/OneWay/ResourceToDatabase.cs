using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class ResourceToDatabase
    {
        [Fact(DisplayName = "Resx -> DB")]
        public void convert_from_resource_to_database() =>
            Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ToDatabase.FromResource)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleDatabaseEdmx));
    }
}
