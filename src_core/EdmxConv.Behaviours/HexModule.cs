using EdmxConv.Schema;
using EdmxConv.Schema.Extensions;

namespace EdmxConv.Behaviours
{
    public class HexModule
    {
        private const string HexPrefix = "0x";

        public static Hex CutOffHexPrefix(Hex hex) =>
            // Cut off '0x' at the beginning
            hex.Value.StartsWith(HexPrefix)
                ? hex.Value.Substring(2).ToHex()
                : hex;

        public static Hex AppendWithHexPrefix(Hex hex) =>
            // Append hexidecimal prefix at the beggining
            hex.Value.StartsWith(HexPrefix)
                ? hex
                : (HexPrefix + hex.Value).ToHex();
    }
}