using EdmxConverter.Schema;
using LanguageExt;

namespace EdmxConverter.Logic
{
    internal static class ToResource
    {
        public static Option<ResourceEdmx> FromDatabaseModel(DatabaseEdmx databaseEdmx) =>
            Prelude.Some(databaseEdmx)
                .Bind(Mixed.HexToBase64);

        public static Option<ResourceEdmx> FromXml(XmlEdmx xmlEdmx) =>
            Prelude.Some(xmlEdmx)
                .Bind(Mixed.XmlToGZip)
                .Bind(Mixed.GZipToBase64);
    }
}