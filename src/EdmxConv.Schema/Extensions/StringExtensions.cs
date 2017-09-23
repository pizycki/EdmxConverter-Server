using CSharpFunctionalExtensions;

namespace EdmxConv.Schema.Extensions
{
    public static class StringExtensions
    {
        public static Hex ToHex(this string target) => new Hex(target);

        public static Result<ResourceEdmx> ToResourceEdmx(this string edmx) => ResourceEdmx.Create(edmx);

        public static string RemoveDashes(this string edmx) => edmx.Replace("-", string.Empty);

        public static Result<XmlEdmx> ToXmlEdmx(this string edmx) => XmlEdmx.Create(edmx);

        public static Result<DatabaseEdmx> ToDatabaseEdmx(this string edmx) => DatabaseEdmx.Create(edmx);
    }
}
