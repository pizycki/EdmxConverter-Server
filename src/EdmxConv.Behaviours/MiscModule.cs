using System;
using System.Linq;
using CSharpFunctionalExtensions;
using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public class MiscModule
    {
        public static Result<XmlEdmx> GZipToXml(GZipBinary gzip) =>
            With(gzip)
                .Map(edmx => edmx.ByteArray.Bytes)
                .OnSuccess(edmx => GzipModule.Decompress(edmx))
                .Map(edmx => edmx.ToXml());

        public static Result<ResourceEdmx> HexToBase64(DatabaseEdmx databaseEdmx) =>
            With(databaseEdmx)
                .Map(edmx => HexModule.CutOffHexPrefix(databaseEdmx.Value))
                .OnSuccess(edmx => TryHexToBytes(edmx))
                .OnSuccess(edmx => Base64Module.BytesToBase64(edmx))
                .OnSuccess(base64 => base64.ToResourceEdmx());

        public static Result<ByteArray> TryHexToBytes(Hex hex)
        {
            try
            {
                var byteArray = HexToBytes(hex);
                return Result.Ok(byteArray);
            }
            catch (FormatException)
            {
                return Result.Fail<ByteArray>("Invalid hexidecimal format.");
            }
        }

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
            With(source)
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