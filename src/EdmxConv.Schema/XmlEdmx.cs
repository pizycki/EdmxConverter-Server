using System.Xml;
using System.Xml.Linq;
using CSharpFunctionalExtensions;

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

        public static Result<XmlEdmx> Create(string plainXml)
        {
            try
            {
                return Result.Ok(new XmlEdmx(plainXml));
            }
            catch (XmlException)
            {
                return Result.Fail<XmlEdmx>("Invalid XML.");
            }
        }

        public override string ToString() => Value.ToString();
    }
}