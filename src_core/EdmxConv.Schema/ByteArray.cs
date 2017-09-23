namespace EdmxConv.Schema
{
    public struct ByteArray
    {
        public byte[] Bytes { get; }

        public ByteArray(byte[] bytes)
        {
            Bytes = bytes;
        }

        public override string ToString() =>
            System.Text.Encoding.UTF8.GetString(Bytes);
    }
}