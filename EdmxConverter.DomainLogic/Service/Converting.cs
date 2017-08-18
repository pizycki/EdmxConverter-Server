using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.DomainLogic.Service
{
    public static class Converting
    {
        [Pure]
        public static Option<GZipBinary> Base64ToGZipBinary(ResourceEdmx source) =>
            new GZipBinary(Convert.FromBase64String(source.Value)); // TODO catch ex: invalid length of base64

        [Pure]
        public static Option<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            Some(databaseEdmx)
                .Map(edmx => edmx.Value)
                .Bind(CutOffHexPrefix)
                .Bind(HexToByteArray)
                .Bind(BytesToBase64)
                .Map(base64 => new ResourceEdmx(base64));

        [Pure]
        public static Option<XmlEdmx> GZipBinaryToPlainXml(GZipBinary source)
        {
            using (var memoryStream = new MemoryStream(source.ByteArray.Bytes))
            using (var gzip = new GZipStream(memoryStream, CompressionMode.Decompress))
            using (var reader = new StreamReader(gzip))
            {
                return new XmlEdmx(reader.ReadToEnd());
            }
        }

        [Pure]
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

        [Pure]
        public static Option<ResourceEdmx> GZipBinaryToBase64(GZipBinary source) =>
            Some(source)
                .Map(gzip => gzip.ByteArray)
                .Bind(BytesToBase64)
                .Map(base64 => new ResourceEdmx(base64));

        [Pure]
        public static Option<string> BytesToBase64(ByteArray bytes) =>
            Some(bytes).Bind(ToBase64String);


        [Pure]
        private static Option<string> ToBase64String(ByteArray array) =>
            Some(array)
                .Map(byteArray => byteArray.Bytes)
                .Map(Convert.ToBase64String);

        private static Option<ByteArray> ToBase64Bytes(string base64) =>
            Some(base64)
                .Map(Convert.FromBase64String)
                .Map(bytes => new ByteArray(bytes));


        [Pure]
        public static Option<ByteArray> HexToByteArray(Hex hex)
        {
            return Enumerable.Range(0, hex.Value.Length)
                             .Where(n => n % 2 == 0)
                             .Select(n => Convert.ToByte(hex.Value.Substring(n, 2), 16))
                             .ToArray()
                             .ToByteArray();
        }

        private static Option<Hex> CutOffHexPrefix(Hex hex) =>
            // Cut off '0x' at the beginning
            hex.Value.StartsWith("0x")
                      ? new Hex(hex.Value.Substring(2))
                      : hex;

        public static Option<ByteArray> Base64ToByteArray(ResourceEdmx databaseEdmx) =>
            Some(databaseEdmx)
                .Map(x => x.Value)
                .Bind(ToBase64Bytes);

        public static Option<Hex> BytesToHex(ByteArray arg)
        {
            throw new NotImplementedException();
        }
    }
}