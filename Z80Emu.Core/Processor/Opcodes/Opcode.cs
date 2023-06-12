using Z80Emu.Core.Memory;
using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

public class Opcode
{
    // Substitution values
    byte? _n;
    sbyte? _d;
    word? _nn;

    readonly string _mnemonic;
    readonly string _description;

    public string[] Bytes { get; set; }

    public string Cycles { get; set; }

    public string Description 
    {
        get
        {
            string d = _description;
            if (_nn != null) d = d.Replace("$nn", $"0x{_nn.Value:X4}");
            if (_n != null) d = d.Replace("$n", $"0x{_n.Value:X2}");
            if (_d != null) d = d.Replace("$d", $"0x{_d.Value:X2}");
            return d;
        }
    }

    public Flags Flags { get; set; }

    public string Mnemonic 
    { 
        get
        {
            string m = _mnemonic;
            if (_nn != null) m = m.Replace("nn", $"0x{_nn.Value:X4}");
            if (_n != null) m = m.Replace("n", $"0x{_n.Value:X2}");
            if (_d != null) m = m.Replace("d", $"0x{_d.Value:X2}");
            return m;
        }
    }

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
        ResetSubstitutions();
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
        _mnemonic = mnemonic;
        Cycles = cycles;
        Flags = flags;
        _description = description;
    }

    public string Id => $"{Bytes[0]} {Mnemonic}";

    public void ResetSubstitutions()
    {
        _d = null;
        _n = null;
        _nn = null;
    }

    public void SetSubstitutions(MMU mmu, word addr)
    {
        for (int i = 0; i < Bytes.Length; i++)
        {
            if (Bytes[i] == "nn")
            {
                _nn = BitUtils.ToWord(mmu[addr + 1], mmu[addr]);
                addr += 2;
            }
            else if (Bytes[i] == "n")
            {
                _n = mmu[addr++];
            }
            else if (Bytes[i] == "d")
            {
                _d = (sbyte)mmu[addr++];
            }
            else
            {
                addr++;
            }
        }
    }

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
