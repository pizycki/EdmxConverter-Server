using LanguageExt;

namespace EdmxConverter.DomainLogic.Structs
{
    public static class ByteArrayExtension
    {
        public static Option<ByteArray> ToByteArray(this byte[] array) => new ByteArray(array);
    }
}
