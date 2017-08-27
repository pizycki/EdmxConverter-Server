using EdmxConverter.DomainLogic.Types;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Converting
{
    internal static class ToXml
    {
        public static Option<XmlEdmx> FromResource(ResourceEdmx source) =>
            Prelude.Some(source)
                .Bind(Mixed.Base64ToGZip)
                .Bind(Mixed.GZipBinaryToXml);

        public static Option<XmlEdmx> FromDatabaseModel(DatabaseEdmx source) =>
            Prelude.Some(source)
                .Bind(ToResource.FromDatabaseModel)
                .Bind(ToXml.FromResource);
    }
}