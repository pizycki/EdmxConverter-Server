using EdmxConverter.Core;
using EdmxConverter.DomainLogic.Service;
using EdmxConverter.DomainLogic.Tests.Properties;
using Shouldly;
using Xunit;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Tests.EdmxTypes
{
    /// <summary>
    /// All EDMX types should return the string-value they're wrapping.
    /// </summary>
    public class ToStringOverrideTests
    {
        [Fact]
        public void ResourceEdmx_ToString_returns_wrapped_value()
        {
            Some(Resources.SampleResourceEdmx)
                .Map(smpl => (smpl, new ResourceEdmx(smpl).ToString()))
                .Map(Tuples.AreValuesEqual)
                .ShouldBe(true);
        }

        [Fact]
        public void XmlEdmx_ToString_returns_wrapped_value()
        {
            Some(Resources.SampleXmlEdmx)
                .Map(smpl => (smpl, new XmlEdmx(smpl).ToString()))
                .Map(Tuples.AreValuesEqual)
                .ShouldBe(true);
        }

        [Fact]
        public void DatabaseEdmx_ToString_returns_wrapped_value()
        {
            Some(Resources.SampleDatabaseEdmx)
                .Map(smpl => (smpl, new DatabaseEdmx(smpl).ToString()))
                .Map(Tuples.AreValuesEqual)
                .ShouldBe(true);
        }
    }
}
