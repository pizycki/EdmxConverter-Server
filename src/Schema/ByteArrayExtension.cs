using LanguageExt;

namespace EdmxConverter.Schema
{
    public static class ByteArrayExtension
    {
        public static Option<ByteArray> ToByteArray(this byte[] array) => new ByteArray(array);
    }
}
