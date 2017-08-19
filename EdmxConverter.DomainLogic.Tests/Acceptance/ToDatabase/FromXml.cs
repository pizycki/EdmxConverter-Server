using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Tests.Properties;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Tests.Acceptance.ToDatabase
{
    public class FromXml
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [UseApprovalSubdirectory("Approved")]
        public void convert_from_plain_xml() =>
            Some(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .Bind(ConvertToDatabase.FromXml)
                .Match(edmx => edmx.ToString(), () => None.ToString())
                .Verify();
    }
}