namespace EdmxConv.Schema
{
    public struct Hex
    {
        public string Value { get; }

        public Hex(string hex)
        {
            Value = hex;
        }
    }
}