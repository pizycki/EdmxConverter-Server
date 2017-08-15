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
    }
}
