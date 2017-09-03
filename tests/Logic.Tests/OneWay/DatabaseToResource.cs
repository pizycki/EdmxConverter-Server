using EdmxConverter.Logic.Tests.Properties;
using EdmxConverter.Schema;
using LanguageExt;
using Shouldly;
using Xunit;

namespace EdmxConverter.Logic.Tests.OneWay
{
    public class ConvertFromDatabaseToResource
    {
        [Fact(DisplayName = "DB -> Resx")]
        public void convert_from_database_to_resource() =>
            Prelude.Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ToResource.FromDatabaseModel)
                .IfSome(edmx => edmx.ToString().ShouldBe(Resources.SampleResourceEdmx));
    }
}