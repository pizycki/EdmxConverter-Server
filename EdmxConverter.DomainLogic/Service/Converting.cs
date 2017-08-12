using System;
using System.IO;
using System.IO.Compression;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    public static class Converting
    {
        public static Option<GZipBinary> Base64ToGZipBinary(ResourceEdmx source) => new GZipBinary(Convert.FromBase64String(source.Value));

        public static Option<XmlEdmx> GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.Value))
            using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip))
            {
                return new XmlEdmx(reader.ReadToEnd());
            }
        }

        public static Option<GZipBinary> XmlEdmxToGzipBinary(XmlEdmx xmlEdmx)
        {
            using (var outStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                {
                    xmlEdmx.Value.Save(gzipStream);
                }

                return Some(new GZipBinary(outStream.ToArray()));
            }
        }

        public static Option<ResourceEdmx> GZipBinaryToBase64(GZipBinary source) =>
            Some(Convert.ToBase64String(source.Value))
                .Bind(base64 => Some(new ResourceEdmx(base64)));
    }
}