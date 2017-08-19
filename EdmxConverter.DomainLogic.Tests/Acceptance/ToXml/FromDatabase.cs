using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using EdmxConverter.DomainLogic.DataTypes;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Structs;
using EdmxConverter.DomainLogic.Tests.Properties;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Tests.Acceptance.ToXml
{
    public class FromDatabase
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [UseApprovalSubdirectory("Approved")]
        public void convert_from_plain_database() =>
            Some(Resources.SampleDatabaseEdmx)
                .Map(sample => new Hex(sample))
                .Map(sample => new DatabaseEdmx(sample))
                .Bind(ConvertToXml.FromDatabaseModel)
                .Match(edmx => edmx.ToString(), () => None.ToString())
                .Verify();

    }
}
