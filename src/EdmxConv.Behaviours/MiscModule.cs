using System;
using System.Linq;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;

namespace EdmxConv.Behaviours
{
    public class MiscModule
    {
        public static Result<XmlEdmx> GZipToXml(GZipBinary gzip) =>
            FlowHelpers.With(gzip)
                .Map(edmx => edmx.ByteArray.Bytes)
                .OnSuccess(edmx => GzipModule.Decompress(edmx))
                .Map(edmx => edmx.ToXml());

        public static Result<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            FlowHelpers.With(databaseEdmx)
                .OnSuccess(edmx => HexModule.CutOffHexPrefix(databaseEdmx.Value))
                .Map(HexToBytes)
                .OnSuccess(edmx => Base64Module.BytesToBase64(edmx))
                .Map(base64 => base64.ToResourceEdmx());


        public static ByteArray HexToBytes(Hex hex) =>
            Enumerable.Range(0, hex.Value.Length)
                .Where(n => n % 2 == 0)
                .Select(n => Convert.ToByte(hex.Value.Substring(n, 2), 16))
                .ToArray()
                .ToByteArray();

        public static Result<GZipBinary> XmlToGZip(XmlEdmx xmlEdmx) =>
            GzipModule.Compress(xmlEdmx.Value)
                .Map(bytes => new GZipBinary(bytes));

        public static Result<ResourceEdmx> GZipToBase64(GZipBinary source) =>
            FlowHelpers.With(source)
                .OnSuccess(edmx => Base64Module.BytesToBase64(edmx.ByteArray)
                .OnSuccess(base64 => new ResourceEdmx(base64)));


        public static Result<Hex> BytesToHex(ByteArray array) =>
            ConvertBytesToUTF8String(array)
                .Map(edmx => edmx.RemoveDashes())
                .Map(edmx => edmx.ToHex())
                .OnSuccess(edmx => HexModule.AppendWithHexPrefix(edmx));

        private static Result<string> ConvertBytesToUTF8String(ByteArray array)
        {
            try
            {
                var str = BitConverter.ToString(array.Bytes);
                return Result.Ok(str);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static Result<GZipBinary> Base64ToGzip(ResourceEdmx resourceEdmx)
        {
            try
            {
                var gZipBinary = Convert.FromBase64String(resourceEdmx.Value).ToGZipBinary();
                return Result.Ok(gZipBinary);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}