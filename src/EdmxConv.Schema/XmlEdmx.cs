using System.Xml.Linq;

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

        public override string ToString() => Value.ToString();
    }
}