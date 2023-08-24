using System.Globalization;

namespace Z80Emu.Core;

public static class ByteExtensions
{
      public static byte ParseHexByte(this string s) =>
        byte.Parse(StripHexIdentifiers(s), NumberStyles.HexNumber, null);

    public static bool TryParseHexByte(this string s, out byte hex) =>
        byte.TryParse(StripHexIdentifiers(s), NumberStyles.HexNumber, null, out hex);

    private static string StripHexIdentifiers(string s)
    {
        if (s.StartsWith("0x"))
            return s.Substring(2);

        if (s.StartsWith("$"))
            return s.Substring(1);

        return s;
    }
}
