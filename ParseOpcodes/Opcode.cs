using OC = Z80Emu.Core.Processor.Opcodes.Opcode;
using System.Globalization;

namespace ParseOpcodes;

public class Opcode
{
    public string Category => category ?? "";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string[] bytes { get; set; }
    public string cycles { get; set; }
    public string description { get; set; }
    public Flags flags { get; set; }
    public string mnemonic { get; set; }
    public string category { get; set; }
    public bool undocumented { get; set; }
    public bool z180 { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    static Dictionary<string, string[]> EightBitRegisterNames = new Dictionary<string, string[]> {
        {"", new [] { "B", "C", "D", "E", "H", "L", "", "A" } },
        {"ix", new [] { "B", "C", "D", "E", "IXH", "IXL", "", "A" } },
        {"iy", new [] { "B", "C", "D", "E", "IYH", "IXL", "", "A" } },
    };

    static Dictionary<string, string[]> SixteenBitRegisterNames = new Dictionary<string, string[]> {
        {"", new [] { "BC", "DE", "HL", "SP" } },
        {"ix", new [] { "BC", "DE", "IX", "SP" } },
        {"iy", new [] { "BC", "DE", "IY", "SP" } },
    };

    static byte[] Ports = new byte[] { 0x00, 0x08, 0x10, 0x18, 0x20, 0x28, 0x30, 0x38 };

    static string[] IteratedParameters = new[] { "b", "dd", "p", "r", "r1", "r2" };

    public IEnumerable<OC> GenerateOpcodes()
    {
        if (!bytes.Any(b =>IteratedParameters.Any(p => b.Contains(p))))
        {
            yield return new OC(mnemonic, bytes, cycles, description);
        }
        else if (bytes.Any(b => b.Equals("d-$-2")))
        {
            yield return new OC(mnemonic, bytes, cycles, description);
        }
        else if (bytes.Any(b => b.StartsWith("p+$C7")))
        {
            foreach (var port in Ports)
            {
                string by = (port + 0xC7).ToString("X2");
                string mn = mnemonic.Replace("p", port.ToString());
                string ds = description.Replace("$p", port.ToString());
                yield return new OC(mn, new[] { by }, cycles, ds);
            }
        }
        else if (bytes.Any(b => b.Equals("r")))
        {
            int i = Array.IndexOf(bytes, "r");
            for (byte r = 0; r < 8; r++)
            {
                if (r == 6) continue;
                bytes[i] = r.ToString("X2");
                string mn = mnemonic.Replace("r", EightBitRegisterNames[Category][r]);
                string ds = description.Replace("$r", EightBitRegisterNames[Category][r]);
                yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
            }
        }
        else if (bytes.Any(b => b.StartsWith("r+$")))
        {
            int i = IndexOfByteStartingWith("r+$");
            int offset = int.Parse(bytes[i].Substring(3), NumberStyles.HexNumber);
            for (byte r = 0; r < 8; r++)
            {
                if (r == 6) continue;
                bytes[i] = (r + offset).ToString("X2");
                string mn = mnemonic.Replace("r", EightBitRegisterNames[Category][r]);
                string ds = description.Replace("$r", EightBitRegisterNames[Category][r]);
                yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
            }
        }
        else if (bytes.Any(b => b.StartsWith("(r<<3)+$")))
        {
            int i = IndexOfByteStartingWith("(r<<3)+$");
            int offset = int.Parse(bytes[i].Substring(8), NumberStyles.HexNumber);
            for (byte r = 0; r < 8; r++)
            {
                if (r == 6) continue;
                bytes[i] = ((r << 3) + offset).ToString("X2");
                string mn = mnemonic.Replace("r", EightBitRegisterNames[Category][r]);
                string ds = description.Replace("$r", EightBitRegisterNames[Category][r]);
                yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
            }
        }
        else if (bytes.Any(b => b.StartsWith("(r1<<3)+r2+$")))
        {
            int i = IndexOfByteStartingWith("(r1<<3)+r2+$");
            int offset = int.Parse(bytes[i].Substring(12), NumberStyles.HexNumber);
            for (byte r1 = 0; r1 < 8; r1++)
            {
                if (r1 == 6) continue;
                for (byte r2 = 0; r2 < 8; r2++)
                {
                    if (r2 == 6) continue;
                    bytes[i] = ((r1 << 3) + r2 + offset).ToString("X2");
                    string mn = mnemonic.Replace("r1", EightBitRegisterNames[Category][r1]).Replace("r2", EightBitRegisterNames[Category][r2]);
                    string ds = description.Replace("$r1", EightBitRegisterNames[Category][r1]).Replace("$r2", EightBitRegisterNames[Category][r2]);
                    yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
                }
            }
        }
        else if (bytes.Any(b => b.StartsWith("(dd<<4)+$")))
        {
            int i = IndexOfByteStartingWith("(dd<<4)+$");
            int offset = int.Parse(bytes[i].Substring(9), NumberStyles.HexNumber);
            for (byte dd = 0; dd < 4; dd++)
            {
                bytes[i] = ((dd << 4) + offset).ToString("X2");
                string mn = mnemonic.Replace("dd", SixteenBitRegisterNames[Category][dd]);
                string ds = description.Replace("$dd", SixteenBitRegisterNames[Category][dd]);
                yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
            }
        }
        else if (bytes.Any(b => b.StartsWith("(b<<3)+$")))
        {
            int i = IndexOfByteStartingWith("(b<<3)+$");
            int offset = int.Parse(bytes[i].Substring(8), NumberStyles.HexNumber);
            for (byte b = 0; b < 8; b++)
            {
                bytes[i] = ((b << 3) + offset).ToString("X2");
                string mn = mnemonic.Replace("b", b.ToString());
                string ds = description.Replace("$b", b.ToString());
                yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
            }
        }
        else if (bytes.Any(b => b.StartsWith("(b<<3)+r+$")))
        {
            int i = IndexOfByteStartingWith("(b<<3)+r+$");
            int offset = int.Parse(bytes[i].Substring(10), NumberStyles.HexNumber);
            for (byte r = 0; r < 8; r++)
            {
                if (r == 6) continue;
                for (byte b = 0; b < 8; b++)
                {
                    bytes[i] = ((b << 3) + r + offset).ToString("X2");
                    string mn = mnemonic.Replace("b", b.ToString()).Replace("r", EightBitRegisterNames[Category][r]);
                    string ds = description.Replace("$b", b.ToString()).Replace("$r", EightBitRegisterNames[Category][r]);
                    yield return new OC(mn, (string[])bytes.Clone(), cycles, ds);
                }
            }
        }
        else
        {
            Console.WriteLine($"ERROR: {mnemonic} => {bytes.First(b =>IteratedParameters.Any(p => b.Contains(p)))}");
        }
    }

    private int IndexOfByteStartingWith(string prefix)
    {
        for (int i = 0; i < bytes.Length; i++)
        {
            if (bytes[i].StartsWith(prefix))
            {
                return i;
            }
        }
        return 0;
    }
}

public class Flags
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string c { get; set; }
    public string h { get; set; }
    public string n { get; set; }
    public string pv { get; set; }
    public string s { get; set; }
    public string z { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}