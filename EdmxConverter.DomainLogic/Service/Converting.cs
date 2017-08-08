using System;
using System.IO;
using System.IO.Compression;
using LanguageExt;

namespace EdmxConverter.DomainLogic.Service
{
    public static class Converting
    {
        public static Result<GZipBinary> Base64ToGZipBinary(ResourceEdmx source) => new GZipBinary(System.Convert.FromBase64String(source.Value));

        public static Result<XmlEdmx> GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.Value))
            using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip))
            {
                return new XmlEdmx(reader.ReadToEnd());
            }
        }

        public static Result<GZipBinary> XmlEdmxToGzipBinary(XmlEdmx xmlEdmx)
        {
            using (var outStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                {
                    xmlEdmx.Value.Save(gzipStream);
                }

                return new GZipBinary(outStream.ToArray());
            }
        }

        public static ResourceEdmx GZipBinaryToBase64(GZipBinary source) =>
            Convert.ToBase64String(source.Value)
                .Then(v => new ResourceEdmx(v));
    }
}