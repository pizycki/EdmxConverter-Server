using System;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    public static class ConvertEdmx
    {
        public static Result<string> Convert(ConvertEdmxArgs convertArgs)
        {
            switch (convertArgs)
            {
                case ConvertXmlToResource args:
                    return ConvertToResource.FromXml(args.Model)
                                            .Match(Succ: res => res.ToString(),
                                                   Fail: ex => "");

                case ConvertDatabaseToResource args:
                    return ConvertToResource.FromDatabaseModel(args.Model)
                                            .Match(Succ: res => res.ToString(),
                                                   Fail: ex => "");

                case ConvertXmlToDatabase args:
                    throw new NotImplementedException();

                case ConvertResourceToDatabase args:
                    throw new NotImplementedException();

                case ConvertDatabaseToXml args:
                    return ConvertToXml.FromDatabaseModel(args.Model)
                                       .Match(Succ: xml => xml.ToString(),
                                              Fail: ex => "");

                case ConvertResourceToXml args:
                    return ConvertToXml.FromResource(args.Model)
                                       .Match(Succ: xml => xml.ToString(),
                                              Fail: ex => "");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    public static class ConvertToXml
    {
        public static Result<XmlEdmx> FromResource(ResourceEdmx source)
        {
            return Converting.Base64ToGZipBinary(source)
                             .Match(Succ: Converting.GZipBinaryToPlainXml,
                                    Fail: ex => new Result<XmlEdmx>(ex));
        }

        public static Result<XmlEdmx> FromDatabaseModel(DatabaseEdmx source)
        {
            throw new NotImplementedException();
        }
    }

    public static class ConvertToResource
    {
        public static Result<ResourceEdmx> FromDatabaseModel(DatabaseEdmx databaseEdmx)
        {
            throw new NotImplementedException();
        }

        public static Result<ResourceEdmx> FromXml(XmlEdmx xmlEdmx)
        {
            return Converting.XmlEdmxToGzipBinary(xmlEdmx)
                             .Match(Succ: gzip => Converting.GZipBinaryToBase64(gzip),
                                    Fail: ex => new Result<ResourceEdmx>(ex));
        }
    }
}