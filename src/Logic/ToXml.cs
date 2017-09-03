using EdmxConverter.Schema;
using LanguageExt;

namespace EdmxConverter.Logic
{
    internal static class ToXml
    {
        public static Option<XmlEdmx> FromResource(ResourceEdmx source) =>
            Prelude.Some(source)
                .Bind(Mixed.Base64ToGZip)
                .Bind(Mixed.GZipToXml);

        public static Option<XmlEdmx> FromDatabaseModel(DatabaseEdmx source) =>
            Prelude.Some(source)
                .Bind(ToResource.FromDatabaseModel)
                .Bind(ToXml.FromResource);
    }
}