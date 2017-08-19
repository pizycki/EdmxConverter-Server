using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using EdmxConverter.DomainLogic.DataTypes;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Tests.Properties;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Tests.Acceptance.ToDatabase
{
    public class FromResource
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [UseApprovalSubdirectory("Approved")]
        public void convert_from_plain_resource() =>
            Some(Resources.SampleResourceEdmx)
                .Map(sample => new ResourceEdmx(sample))
                .Bind(ConvertToDatabase.FromResource)
                .Match(edmx => edmx.ToString(), () => None.ToString())
                .Verify();
    }
}