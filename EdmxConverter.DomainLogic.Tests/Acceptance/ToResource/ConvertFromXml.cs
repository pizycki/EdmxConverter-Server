using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Tests.Properties;
using Xunit;

namespace EdmxConverter.DomainLogic.Tests.Acceptance.ToResource
{
    public class ConvertFromXml
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [UseApprovalSubdirectory("Approved")]
        public void convert_plain_xml()
        {
            var xmlValue = Resources.SampleXmlEdmx;
            var xmlEdmx = new XmlEdmx(xmlValue);
            ConvertToResource.FromXml(xmlEdmx)
                .Match(res => res.ToString(), () => "")
                .Verify();
        }
    }
}
