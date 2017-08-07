﻿using ApprovalTests;
using ApprovalTests.Namers;
using ApprovalTests.Reporters;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Tests.Properties;
using Xunit;

namespace EdmxConverter.DomainLogic.Tests.Acceptance.ToXml
{
    public class ConvertFromResource_Tests
    {
        [Fact]
        [UseReporter(typeof(DiffReporter))]
        [UseApprovalSubdirectory("Approved")]
        public void convert_from_plain_resource()
        {
            var resourceValue = Resources.SampleResourceEdmx;
            var resourceEdmx = ResourceEdmx.Create(resourceValue);
            var xml = ConvertToXml.Convert(resourceEdmx);
            Approvals.Verify(xml.Value);
        }
    }
}
