using EdmxConverter.DomainLogic.DataTypes;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    internal static class ConvertToXml
    {
        public static Option<XmlEdmx> FromResource(ResourceEdmx source) =>
            Prelude.Some(source)
                .Bind(Convert.Base64ToGZip)
                .Bind(Convert.GZipBinaryToXml);

        public static Option<XmlEdmx> FromDatabaseModel(DatabaseEdmx source) =>
            Prelude.Some(source)
                .Bind(ConvertToResource.FromDatabaseModel)
                .Bind(ConvertToXml.FromResource);
    }
}