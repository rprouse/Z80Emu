using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public class Opcode
{
    public string[] Bytes { get; set; }

    public string Cycles { get; set; }

    public string Description { get; set; }

    public Flags Flags { get; set; }

    public string Mnemonic { get; set; }

    public Action? Execute { get; set; }

    /// <summary>
    /// This is just the length of the opcode without the operands
    /// </summary>
    public ushort OpcodeLength => (ushort)Bytes.TakeWhile(b => b.Length == 2 & b != "nn").Count();

    /// <summary>
    /// This is the full length of the opcode including the operands
    /// </summary>
    public ushort Length => (ushort)(Bytes.Length + Bytes.Count(b => b == "nn"));

    public bool Match(MMU mmu, word sp)
    {
        for (int i = 0; i < Bytes.Length; i++)
        {
            if (Bytes[i].Length != 2 || Bytes[i] == "nn") continue;

            if (mmu[sp + i].ToString("X2") != Bytes[i])
            {
                return false;
            }
        }   
        return true;
    }

    public Opcode(string[] bytes, string mnemonic, string cycles, Flags flags, string description)
    {
        Bytes = bytes;
        Mnemonic = mnemonic;
        Cycles = cycles;
        Flags = flags;
        Description = description;
    }

    public string Id => $"{Bytes[0]} {Mnemonic}";

    override public string ToString() => Mnemonic;

    public string ToCodeString() =>
        $"new Opcode(new [] {{ {string.Join(", ", Bytes.Select(b => $"\"{b}\""))} }}, \"{Mnemonic}\", \"{Cycles}\", {Flags.ToCodeString()}, \"{Description}\")";
}

public class Flags
{
    public string C { get; set; }
    public string H { get; set; }
    public string N { get; set; }
    public string PV { get; set; }
    public string S { get; set; }
    public string Z { get; set; }

    public Flags(string c, string h, string n, string pv, string s, string z)
    {
        C = c;
        H = h;
        N = n;
        PV = pv;
        S = s;
        Z = z;
    }
    public string ToCodeString() =>
        $"new Flags(\"{C}\", \"{H}\", \"{N}\", \"{PV}\", \"{S}\", \"{Z}\")";

    static string FlagBehaviour(string behaviour) =>
        behaviour switch
        {
            " " => "undefined",
            "*" => "exceptional",
            "+" => "as defined",
            "-" => "not affected",
            "0" => "reset",
            "1" => "set",
            "p" => "detect parity",
            "v" => "detect overflow",
            _ => throw new ArgumentException($"Unknown flag behaviour: {behaviour}")
        };
}
