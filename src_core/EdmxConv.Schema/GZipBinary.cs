namespace EdmxConv.Schema
{
    public sealed class GZipBinary : Edmx
    {
        public ByteArray ByteArray { get; }

        public GZipBinary(byte[] value) : base(EdmxTypeEnum.GzipBinary)
        {
            ByteArray = new ByteArray(value);
        }
    }
}
