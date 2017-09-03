using System.Xml.Linq;

namespace EdmxConverter.Schema
{
    public sealed class GZipBinary : Edmx
    {
        public ByteArray ByteArray { get; }

        public GZipBinary(byte[] value) : base(EdmxTypeEnum.GzipBinary)
        {
            ByteArray = new ByteArray(value);
        }
    }

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

    public sealed class ResourceEdmx : Edmx
    {
        /// <summary>
        /// BASE64 value
        /// </summary>
        /// <example>H4sIAAAAAAAEAM1X227bOBB9X2D/g ... XQ0D7gMAAA=</example>
        public string Value { get; }

        public ResourceEdmx(string value)
            : base(EdmxTypeEnum.Resource) =>
            Value = value;

        public override string ToString() => Value;
    }

    public sealed class DatabaseEdmx : Edmx
    {
        /// <summary>
        /// Hexdecimal binary model
        /// </summary>
        /// <example>0x1F8B0800000000000400CD57DB6EDB38107D5F60FF81...</example>
        public Hex Value { get; }

        public DatabaseEdmx(string hexInString) : this(new Hex(hexInString)) { }

        public DatabaseEdmx(Hex value)
            : base(EdmxTypeEnum.Database)
        {
            Value = value;
        }

        public override string ToString() => Value.Value;
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
