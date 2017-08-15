using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    public static class ConvertEdmx
    {
        public static Option<string> Convert(ConvertEdmxArgs convertArgs)
        {
            switch (convertArgs)
            {
                case ConvertXmlToResource args:
                    return ConvertToResource.FromXml(args.Model).Map(res => res.ToString());

                case ConvertDatabaseToResource args:
                    return ConvertToResource.FromDatabaseModel(args.Model).Map(res => res.ToString());

                case ConvertXmlToDatabase args:
                    return None;// TODO implement

                case ConvertResourceToDatabase args:
                    return None;// TODO implement

                case ConvertDatabaseToXml args:
                    return ConvertToXml.FromDatabaseModel(args.Model).Map(xml => xml.ToString());

                case ConvertResourceToXml args:
                    return ConvertToXml.FromResource(args.Model).Map(xml => xml.ToString());

                default:
                    return None;
            }
        }
    }

    internal static class ConvertToXml
    {
        public static Option<XmlEdmx> FromResource(ResourceEdmx source) =>
            Some(source)
                .Bind(Converting.Base64ToGZipBinary)
                .Bind(Converting.GZipBinaryToPlainXml);

        public static Option<XmlEdmx> FromDatabaseModel(DatabaseEdmx source) => None; // TODO implement
    }

    internal static class ConvertToResource
    {
        public static Option<ResourceEdmx> FromDatabaseModel(DatabaseEdmx databaseEdmx) =>
            Some(databaseEdmx)
                .Bind(Converting.HexToBase64);


        public static Option<ResourceEdmx> FromXml(XmlEdmx xmlEdmx) =>
            Some(xmlEdmx)
                .Bind(Converting.XmlEdmxToGzipBinary)
                .Bind(Converting.GZipBinaryToBase64);
    }
}