using EdmxConverter.DomainLogic.Types;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Converting
{
    internal static class ToResource
    {
        public static Option<ResourceEdmx> FromDatabaseModel(DatabaseEdmx databaseEdmx) =>
            Some(databaseEdmx)
                .Bind(Mixed.HexToBase64);

        public static Option<ResourceEdmx> FromXml(XmlEdmx xmlEdmx) =>
            Some(xmlEdmx)
                .Bind(Mixed.XmlToGZip)
                .Bind(Mixed.GZipToBase64);
    }
}