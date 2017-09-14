using System;
using System.IO;
using System.IO.Compression;
using System.Xml.Linq;
using CSharpFunctionalExtensions;

namespace EdmxConv.Behaviours
{
    public class GzipModule
    {
        public static Result<XDocument> Decompress(byte[] bytes)
        {
            try
            {
                using (var memoryStream = new MemoryStream(bytes))
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
                    {
                        var document = XDocument.Load(gzipStream);
                        return Result.Ok(document);
                    }
                }
            }
            catch (InvalidDataException)
            {
                return Result.Fail<XDocument>("Corrupted GZip file. Cannot uncompress.");
            }
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