using System.Xml.Linq;

namespace EdmxConv.Schema.Extensions
{
    public static class XDocumentExtensions
    {
        public static XmlEdmx ToXml(this XDocument document) => new XmlEdmx(document);
    }
}
