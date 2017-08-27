using EdmxConverter.DomainLogic.Types;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Converting
{
    internal static class ToDatabase
    {
        public static Option<DatabaseEdmx> FromResource(ResourceEdmx source) =>
            Prelude.Some(source)
                .Bind(Mixed.Base64ToByteArray)
                .Bind(Mixed.BytesToHex)
                .Map(hex => new DatabaseEdmx(hex));

        public static Option<DatabaseEdmx> FromXml(XmlEdmx source) =>
            Prelude.Some(source)
                .Bind(ToResource.FromXml)
                .Bind(ToDatabase.FromResource);
    }
}