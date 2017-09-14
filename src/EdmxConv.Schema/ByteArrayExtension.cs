namespace EdmxConv.Schema
{
    public static class ByteArrayExtension
    {
        public static ByteArray ToByteArray(this byte[] array) => new ByteArray(array);
    }
}
