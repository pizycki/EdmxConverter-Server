using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public static class ConvertModule
    {
        public static Result<Edmx> Convert(ConvertEdmxArgs args) =>
            With(args)
                .OnSuccess(a => Direction.Create(args.Source, args.Target))
                .OnSuccess(d => Converters.GetMaybe(d).ToResult("Unsupported convertion."))
                .OnSuccess(conv => conv(args.Model));


        public static readonly IReadOnlyDictionary<Direction, Func<Edmx, Result<Edmx>>> Converters =
            new Dictionary<Direction, Func<Edmx, Result<Edmx>>>
            {
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Resource), edmx => ConvertToResource(edmx as DatabaseEdmx) },
                { new Direction(EdmxTypeEnum.Database, EdmxTypeEnum.Xml),      edmx => ConvertToXml(edmx as DatabaseEdmx)},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Database), edmx => ConvertToDatabase(edmx as XmlEdmx)},
                { new Direction(EdmxTypeEnum.Xml,      EdmxTypeEnum.Resource), edmx => ConvertToResource(edmx as XmlEdmx)},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Xml),      edmx => ConvertToXml(edmx as ResourceEdmx)},
                { new Direction(EdmxTypeEnum.Resource, EdmxTypeEnum.Database), edmx => ConvertToDatabase(edmx as ResourceEdmx)},
            };

        private static Result<Edmx> ConvertToDatabase(ResourceEdmx xmlEdmx) =>
            Mixed.Base64ToByteArray(xmlEdmx)
                .OnSuccess(edmx => Mixed.BytesToHex(edmx))
                .Map(edmx => new DatabaseEdmx(edmx))
                .Map(edmx => edmx as Edmx);

        private static Result<Edmx> ConvertToDatabase(XmlEdmx xmlEdmx) =>
            ConvertToResource(xmlEdmx)
                .Map(edmx => edmx as ResourceEdmx)
                .OnSuccess(edmx => ConvertToDatabase(edmx));

        private static Result<Edmx> ConvertToXml(ResourceEdmx databaseEdmx) =>
            Mixed.Base64ToGzip(databaseEdmx)
                .OnSuccess(edmx => Mixed.GZipToXml(edmx))
                .Map(edmx => edmx as Edmx);

        private static Result<Edmx> ConvertToXml(DatabaseEdmx databaseEdmx) =>
            ConvertToResource(databaseEdmx)
                .Map(edmx => edmx as ResourceEdmx)
                .OnSuccess(edmx => ConvertToXml(edmx));

        public static Result<Edmx> ConvertToResource(DatabaseEdmx databaseEdmx) =>
            Mixed.HexToBase64(databaseEdmx)
                .Map(x => x as Edmx);

        public static Result<Edmx> ConvertToResource(XmlEdmx xmlEdmx) =>
            Mixed.XmlToGZip(xmlEdmx)
                .OnSuccess(x => Mixed.GZipToBase64(x))
                .Map(x => x as Edmx);
    }

    public class Mixed
    {
        public static Result<ByteArray> Base64ToByteArray(ResourceEdmx resourceEdmx)
        {
            try
            {
                var bytes = Convert.FromBase64String(resourceEdmx.Value);
                return Result.Ok(new ByteArray(bytes));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Result<XmlEdmx> GZipToXml(GZipBinary gzip) =>
            With(gzip)
                .Map(edmx => edmx.ByteArray.Bytes)
                .OnSuccess(edmx => Decompress(edmx))
                .Map(edmx => new XmlEdmx(edmx));

        private static XDocument Decompress(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            {
                using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    return XDocument.Load(gzipStream);
            }
        }

        public static Result<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            With(databaseEdmx)
                .OnSuccess(edmx => CutOffHexPrefix(databaseEdmx.Value))
                .OnSuccess(edmx => HexToBytes(edmx))
                .OnSuccess(edmx => BytesToBase64(edmx))
                .Map(base64 => new ResourceEdmx(base64));

        public static Hex CutOffHexPrefix(Hex hex) =>
            // Cut off '0x' at the beginning
            hex.Value.StartsWith(HexPrefix)
                ? new Hex(hex.Value.Substring(2))
                : hex;

        public static Hex AppendWithHexPrefix(Hex hex) =>
            // Append hexidecimal prefix at the beggining
            hex.Value.StartsWith(HexPrefix)
                ? hex
                : new Hex(HexPrefix + hex.Value);


        public static ByteArray HexToBytes(Hex hex) =>
            Enumerable.Range(0, hex.Value.Length)
                .Where(n => n % 2 == 0)
                .Select(n => Convert.ToByte(hex.Value.Substring(n, 2), 16))
                .ToArray()
                .ToByteArray();

        private static Result<string> BytesToBase64(ByteArray array)
        {
            try
            {
                return Result.Ok(Convert.ToBase64String(array.Bytes));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Result<GZipBinary> XmlToGZip(XmlEdmx xmlEdmx) =>
            Compress(xmlEdmx.Value)
                .Map(bytes => new GZipBinary(bytes));

        public static Result<byte[]> Compress(XDocument model)
        {
            try
            {
                using (var outStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                        model.Save(gzipStream);

                    return Result.Ok(outStream.ToArray());
                }
            }
            // TODO
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Result<ResourceEdmx> GZipToBase64(GZipBinary source) =>
            With(source)
                .OnSuccess(edmx => BytesToBase64(edmx.ByteArray)
                .OnSuccess(base64 => new ResourceEdmx(base64)));


        public static Result<Hex> BytesToHex(ByteArray array) =>
            ConvertBytesToUTF8String(array)
                .OnSuccess(edmx => RemoveDashes(edmx))
                .OnSuccess(edmx => new Hex(edmx))
                .OnSuccess(edmx => AppendWithHexPrefix(edmx));

        private static string RemoveDashes(string edmx) => edmx.Replace("-", string.Empty);

        private static Result<string> ConvertBytesToUTF8String(ByteArray array)
        {
            try
            {
                var s = BitConverter.ToString(array.Bytes);
                return Result.Ok(s);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public const string HexPrefix = "0x";

        public static Result<GZipBinary> Base64ToGzip(ResourceEdmx resourceEdmx)
        {
            try
            {
                var bytes = Convert.FromBase64String(resourceEdmx.Value); // Extension
                return Result.Ok(new GZipBinary(bytes));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
