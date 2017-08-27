using LanguageExt;

namespace EdmxConverter.DomainLogic.Types
{
    public static class ByteArrayExtension
    {
        public static Option<ByteArray> ToByteArray(this byte[] array) => new ByteArray(array);
    }
}
