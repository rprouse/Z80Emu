using System.Globalization;

namespace Z80Emu.Core;

public static class WordExtensions
{
    public static word ParseHex(this string s) =>
        word.Parse(StripHexIdentifiers(s), NumberStyles.HexNumber, null);

    public static bool TryParseHex(this string s, out word hex) =>
        word.TryParse(StripHexIdentifiers(s), NumberStyles.HexNumber, null, out hex);

    private static string StripHexIdentifiers(string s)
    {
        if (s.StartsWith("0x"))
            return s.Substring(2);

        if (s.StartsWith("$"))
            return s.Substring(1);

        return s;
    }
}
