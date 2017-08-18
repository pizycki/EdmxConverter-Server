namespace EdmxConverter.DomainLogic.Service
{
    public struct Hex
    {
        public string Value { get; }

        public Hex(string hex)
        {
            Value = hex;
        }
    }

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

    public static class ByteArrayExtension
    {
        public static ByteArray ToByteArray(this byte[] array) => new ByteArray(array);
    }
}
