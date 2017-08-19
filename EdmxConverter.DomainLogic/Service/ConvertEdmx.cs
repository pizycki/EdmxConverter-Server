using System.Diagnostics.Contracts;
using EdmxConverter.DomainLogic.ConvertArguments;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    [Pure]
    public static class ConvertEdmx
    {
        public static Option<string> Convert(ConvertEdmxArgs convertArgs)
        {
            switch (convertArgs)
            {
                // To Resource
                case XmlToResourceArgs args:
                    return ConvertToResource.FromXml(args.Model).Map(res => res.ToString());

                case DatabaseToResourceArgs args:
                    return ConvertToResource.FromDatabaseModel(args.Model).Map(res => res.ToString());

                // To Database model ( varbinary(max) )
                case XmlToDatabaseArgs args:
                    return ConvertToDatabase.FromXml(args.Model).Map(res => res.ToString());

                case ResourceToDatabaseArgs args:
                    return ConvertToDatabase.FromResource(args.Model).Map(res => res.ToString());

                // To XML
                case DatabaseToXmlArgs args:
                    return ConvertToXml.FromDatabaseModel(args.Model).Map(xml => xml.ToString());

                case ResourceToXmlArgs args:
                    return ConvertToXml.FromResource(args.Model).Map(xml => xml.ToString());

                default:
                    return None;
            }
        }
    }
}