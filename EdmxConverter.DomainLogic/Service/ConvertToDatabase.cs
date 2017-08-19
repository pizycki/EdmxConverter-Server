using EdmxConverter.DomainLogic.DataTypes;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    internal static class ConvertToDatabase
    {
        public static Option<DatabaseEdmx> FromResource(ResourceEdmx source) =>
            Prelude.Some(source)
                .Bind(Convert.Base64ToByteArray)
                .Bind(Convert.BytesToHex)
                .Map(hex => new DatabaseEdmx(hex));

        public static Option<DatabaseEdmx> FromXml(XmlEdmx source) =>
            Prelude.Some(source)
                .Bind(ConvertToResource.FromXml)
                .Bind(ConvertToDatabase.FromResource);
    }
}