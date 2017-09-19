﻿using CSharpFunctionalExtensions;
using EdmxConv.Behaviours.Tests.Properties;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using Shouldly;
using Xunit;

namespace EdmxConv.Behaviours.Tests.Circular
{
    public class Xml
    {
        [Fact(DisplayName = "Xml -> Resx -> Xml")]
        public void convert_xml_to_resource_back_and_forth() =>
            FlowHelpers.With(Resources.SampleXmlEdmx)
                .Map(sample => new XmlEdmx(sample))
                .OnSuccess(edmx => ConvertModule.ConvertToResource(edmx))
                .OnSuccess(edmx => edmx.GetAs<ResourceEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleXmlEdmx));

        [Fact(DisplayName = "Xml -> DB -> Xml")]
        public void convert_xml_to_database_back_and_forth() =>
            FlowHelpers.With(Resources.SampleXmlEdmx)
                .Map(sample => sample.ToXmlEdmx())
                .OnSuccess(edmx => ConvertModule.ConvertToDatabase(edmx))
                .OnSuccess(edmx => edmx.GetAs<DatabaseEdmx>())
                .OnSuccess(edmx => ConvertModule.ConvertToXml(edmx))
                .Map(edmx => edmx.ToString())
                .OnSuccess(edmx => edmx.ShouldBe(Resources.SampleXmlEdmx));

    }
}