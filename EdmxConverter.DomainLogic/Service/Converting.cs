using System;
using System.IO;
using System.IO.Compression;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    public static class Converting
    {
        public static Option<GZipBinary> Base64ToGZipBinary(ResourceEdmx source) =>
            new GZipBinary(Convert.FromBase64String(source.Value)); // TODO catch ex: invalid length of base64

        public static Option<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            Some(databaseEdmx)
                .Bind(edmx => HexToByteArray(edmx.Value))
                .Bind(BytesToBase64)
                .Map(base64 => new ResourceEdmx(base64));

        public static Option<XmlEdmx> GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.ByteArray.Bytes))
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
            Some(source)
                .Bind(gzip => BytesToBase64(gzip.ByteArray))
                .Map(base64 => new ResourceEdmx(base64));


        public static Option<string> BytesToBase64(ByteArray bytes) =>
            Some(bytes).Bind(ToBase64String);


        private static Option<string> ToBase64String(ByteArray array) =>
            Convert.ToBase64String(array.Bytes);


        public static Option<ByteArray> HexToByteArray(Hex hex)
        {
            hex = hex.Value.StartsWith("0x") ? new Hex(hex.Value.Substring(2)) : hex;
            int NumberChars = hex.Value.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Value.Substring(i, 2), 16);

            return new ByteArray(bytes);
        }

    }
}