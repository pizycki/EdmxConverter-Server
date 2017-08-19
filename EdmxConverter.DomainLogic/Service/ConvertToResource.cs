using EdmxConverter.DomainLogic.DataTypes;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    internal static class ConvertToResource
    {
        public static Option<ResourceEdmx> FromDatabaseModel(DatabaseEdmx databaseEdmx) =>
            Prelude.Some(databaseEdmx)
                .Bind(Convert.HexToBase64);


        public static Option<ResourceEdmx> FromXml(XmlEdmx xmlEdmx) =>
            Prelude.Some(xmlEdmx)
                .Bind(Convert.XmlToGZip)
                .Bind(Convert.GZipToBase64);
    }
}