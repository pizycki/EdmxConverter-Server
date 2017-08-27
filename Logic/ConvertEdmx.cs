using System.Diagnostics.Contracts;
using EdmxConverter.DomainLogic.Converting.Arguments;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Converting
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
                    return ToResource.FromXml(args.Model).Map(res => res.ToString());

                case DatabaseToResourceArgs args:
                    return ToResource.FromDatabaseModel(args.Model).Map(res => res.ToString());

                // To Database model ( varbinary(max) )
                case XmlToDatabaseArgs args:
                    return ToDatabase.FromXml(args.Model).Map(res => res.ToString());

                case ResourceToDatabaseArgs args:
                    return ToDatabase.FromResource(args.Model).Map(res => res.ToString());

                // To XML
                case DatabaseToXmlArgs args:
                    return ToXml.FromDatabaseModel(args.Model).Map(xml => xml.ToString());

                case ResourceToXmlArgs args:
                    return ToXml.FromResource(args.Model).Map(xml => xml.ToString());

                default:
                    return Prelude.None;
            }
        }
    }
}