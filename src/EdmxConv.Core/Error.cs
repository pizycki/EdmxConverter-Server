namespace EdmxConv.Core
{
    public struct Error
    {
        public readonly string Message;

        public Error(string message)
        {
            Message = message;
        }

        public static Error Create(string message) => new Error(message);

        public static Error Empty => new Error();

        public static bool operator ==(Error a, Error b) => Equals(a, b) || a.Message == b.Message;

        public static bool operator !=(Error a, Error b) => !(a == b);
    }
}
