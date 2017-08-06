using ApprovalTests;
using ApprovalTests.Reporters;
using WebAPI.Service;
using Xunit;

namespace WebAPI.Tests.Acceptance.ToXml
{
    public class ConvertFromResource_Tests
    {
        [Fact, UseReporter(typeof(DiffReporter))]
        public void ConvertFromXml()
        {
            var resourceValue = Samples.ResourceEdmx.InitialMigration;
            var resourceEdmx = ResourceEdmx.Create(resourceValue);
            var xml = ConvertToXml.Convert(resourceEdmx);
            Approvals.Verify(xml.Value);
        }
    }
}
