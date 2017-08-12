using System.Xml.Linq;

namespace EdmxConverter.DomainLogic.Service
{
    public sealed class GZipBinary : Edmx
    {
        public byte[] Value { get; }
        public EdmxTypeEnum Type { get; } = EdmxTypeEnum.GzipBinary;

        public GZipBinary(byte[] value) => Value = value;
    }

    public sealed class XmlEdmx : Edmx
    {
        public XDocument Value { get; }
        public EdmxTypeEnum Type { get; } = EdmxTypeEnum.Xml;

        public XmlEdmx(string plainXml)
        {
            Value = XDocument.Parse(plainXml);
        }

        public XmlEdmx(XDocument document)
        {
            Value = document;
        }

        private XDocument Validate(XDocument doc)
        {
            // TODO validation

            return doc;
        }

        public override string ToString() => Value.ToString(SaveOptions.OmitDuplicateNamespaces);

    }

    public sealed class ResourceEdmx : Edmx
    {
        public string Value { get; }
        public EdmxTypeEnum Type { get; } = EdmxTypeEnum.Resource;

        public ResourceEdmx(string value) => Value = value;

        public override string ToString() => Value;
    }

    public sealed class DatabaseEdmx : Edmx
    {
        public string Value { get; }
        public EdmxTypeEnum Type { get; } = EdmxTypeEnum.Xml;

        public DatabaseEdmx(string value) => Value = value;

        public override string ToString() => Value;
    }

    public abstract class Edmx
    {
        public EdmxTypeEnum Type { get; }
    }
}
