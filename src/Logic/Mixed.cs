﻿using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using EdmxConverter.Schema;
using LanguageExt;
using static LanguageExt.Prelude;

namespace EdmxConverter.Logic
{
    internal static class Mixed
    {
        [Pure]
        public static Option<GZipBinary> Base64ToGZip(ResourceEdmx source) =>
            new GZipBinary(Convert.FromBase64String(source.Value)); // TODO catch ex: invalid length of base64

        [Pure]
        public static Option<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            Prelude.Some(databaseEdmx)
                .Map(edmx => edmx.Value)
                .Bind(CutOffHexPrefix)
                .Bind(HexToBytes)
                .Bind(BytesToBase64)
                .Map(base64 => new ResourceEdmx(base64));

        [Pure]
        public static Option<XmlEdmx> GZipToXml(GZipBinary source)
        {
            var bytes = source.ByteArray.Bytes;
            var xDoc = Decompress(bytes);
            return Some(new XmlEdmx(xDoc));
        }

        [Pure]
        public static Option<GZipBinary> XmlToGZip(XmlEdmx xmlEdmx)
        {
            var xDoc = xmlEdmx.Value;
            var bytes = Compress(xDoc);
            return new GZipBinary(bytes);
        }

        [Pure]
        public static byte[] Compress(XDocument model)
        {
            using (var outStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                    model.Save(gzipStream);

                return outStream.ToArray();
            }
        }

        [Pure]
        public static XDocument Decompress(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    return XDocument.Load(gzipStream);
            }
        }

        [Pure]
        public static Option<ResourceEdmx> GZipToBase64(GZipBinary source) =>
            Prelude.Some(source)
                .Map(gzip => gzip.ByteArray)
                .Bind(BytesToBase64)
                .Map(base64 => new ResourceEdmx(base64));

        [Pure]
        private static Option<string> BytesToBase64(ByteArray array) =>
            Prelude.Some(array)
                .Map(byteArray => byteArray.Bytes)
                .Map(Convert.ToBase64String);

        [Pure]
        private static Option<ByteArray> Base64ToBytes(string base64) =>
            Prelude.Some(base64)
                .Map(Convert.FromBase64String)
                .Map(bytes => new ByteArray(bytes));


        [Pure]
        public static Option<ByteArray> HexToBytes(Hex hex) =>
            Enumerable.Range(0, hex.Value.Length)
                      .Where(n => n % 2 == 0)
                      .Select(n => Convert.ToByte(hex.Value.Substring(n, 2), 16))
                      .ToArray()
                      .ToByteArray();

        [Pure]
        public static Option<Hex> CutOffHexPrefix(Hex hex) =>
            // Cut off '0x' at the beginning
            hex.Value.StartsWith(HexPrefix)
                ? new Hex(hex.Value.Substring(2))
                : hex;

        [Pure]
        public static Option<ByteArray> Base64ToByteArray(ResourceEdmx databaseEdmx) =>
            Prelude.Some(databaseEdmx)
                .Map(x => x.Value)
                .Bind(Base64ToBytes);

        [Pure]
        public static Option<Hex> BytesToHex(ByteArray array) =>
            Prelude.Some(array)
                .Map(arr => BitConverter.ToString(arr.Bytes))
                .Map(s => s.Replace("-", ""))
                .Map(s => new Hex(s))
                .Bind(AppendWithHexPrefix);

        [Pure]
        public static Option<Hex> AppendWithHexPrefix(Hex hex) =>
            // Append hexidecimal prefix at the beggining
            hex.Value.StartsWith(HexPrefix)
               ? hex
               : new Hex(HexPrefix + hex.Value);

        public const string HexPrefix = "0x";
    }
}