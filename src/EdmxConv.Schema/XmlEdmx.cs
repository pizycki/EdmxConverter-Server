using System.Xml;
using System.Xml.Linq;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Schema
{
    public sealed class XmlEdmx : Edmx
    {
        public XDocument Value { get; }

        public XmlEdmx(string plainXml)
            : base(EdmxTypeEnum.Xml) =>
            Value = XDocument.Parse(plainXml);

        public XmlEdmx(XDocument document)
            : base(EdmxTypeEnum.Xml) =>
            Value = document;

        public static Result<XmlEdmx> Create(string plainXml) =>
            With(plainXml)
                .OnSuccessTry<string, XmlEdmx, XmlException>(xml => new XmlEdmx(xml), "Invalid XML.");

        public override string ToString() => Value.ToString();
    }
}