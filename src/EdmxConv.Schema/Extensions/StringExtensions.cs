namespace EdmxConv.Schema.Extensions
{
    public static class StringExtensions
    {
        public static Hex ToHex(this string target) => new Hex(target);

        public static ResourceEdmx ToResourceEdmx(this string target) => new ResourceEdmx(target);

        public static string RemoveDashes(this string edmx) => edmx.Replace("-", string.Empty);

        public static XmlEdmx ToXmlEdmx(this string edmx) => new XmlEdmx(edmx);
    }
}
