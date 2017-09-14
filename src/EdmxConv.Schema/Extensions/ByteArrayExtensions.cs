namespace EdmxConv.Schema.Extensions
{
    public static class ByteArrayExtensions
    {
        public static GZipBinary ToGZipBinary(this byte[] target) => new GZipBinary(target);
    }
}
