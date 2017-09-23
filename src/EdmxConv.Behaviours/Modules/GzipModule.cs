using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using CSharpFunctionalExtensions;
using EdmxConv.Core;
using static EdmxConv.Core.FlowHelpers;

namespace EdmxConv.Behaviours
{
    public class GzipModule
    {
        public static Result<XDocument> Decompress(byte[] bytes) =>
            With(bytes)
                .OnSuccessTry<byte[], XDocument, InvalidDataException>(edmx =>
                    DecompressUnsafe(edmx), "Corrupted GZip file. Cannot uncompress.");

        public static XDocument DecompressUnsafe(byte[] bytes)
        {
            using (var memoryStream = new MemoryStream(bytes))
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                return XDocument.Load(gzipStream);
        }

        public static Result<byte[]> Compress(XDocument document)
        {
            try
            {
                using (var outStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outStream, CompressionMode.Compress))
                        document.Save(gzipStream);

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
    }
}