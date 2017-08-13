using System.Xml.Linq;

namespace EdmxConverter.DomainLogic.Service
{
    public sealed class GZipBinary : Edmx
    {
        public byte[] Value { get; }

        public GZipBinary(byte[] value) : base(EdmxTypeEnum.GzipBinary) => Value = value;
    }

    public sealed class XmlEdmx : Edmx
    {
        public XDocument Value { get; }

        public XmlEdmx(string plainXml)
            : base(EdmxTypeEnum.Xml) =>
            Value = XDocument.Parse(plainXml);

        public override string ToString() => Value.ToString(SaveOptions.OmitDuplicateNamespaces);

    }

    public sealed class ResourceEdmx : Edmx
    {
        public string Value { get; }

        public ResourceEdmx(string value)
            : base(EdmxTypeEnum.Resource) =>
            Value = value;

        public override string ToString() => Value;
    }

    public sealed class DatabaseEdmx : Edmx
    {
        public string Value { get; }

        public DatabaseEdmx(string value)
            : base(EdmxTypeEnum.Database) =>
            Value = value;

        public override string ToString() => Value;
    }

    public abstract class Edmx
    {
        public EdmxTypeEnum Type { get; }

        protected Edmx(EdmxTypeEnum type)
        {
            Type = type;
        }
    }
}
