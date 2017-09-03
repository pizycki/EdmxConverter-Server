using EdmxConverter.Schema;

namespace EdmxConverter.Xml
{
    public static class XmlToString
    {
        public static string Stringify(this XmlEdmx edmx)
        {
            return edmx.Value.ToString();
        }
    }
}
