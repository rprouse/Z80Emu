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
            if (_d != null) d = d.Replace("$d", $"{_d.Value}");
            return d;
        }
    }

    public string Mnemonic 
    { 
        get
        {
            string m = _mnemonic;
            if (_nn != null) m = m.Replace("nn", $"0x{_nn.Value:X4}");
            if (_n != null) m = m.Replace("n", $"0x{_n.Value:X2}");
            if (_d != null) m = m.Replace("d", $"{_d.Value}");
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

    public Opcode(string mnemonic, string[] bytes, string cycles, string description)
    {
        Bytes = bytes;
        _mnemonic = mnemonic;
        Cycles = cycles;
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
        $"new Opcode(\"{Mnemonic}\", new [] {{ {string.Join(", ", Bytes.Select(b => $"\"{b}\""))} }}, \"{Cycles}\", \"{Description}\")";
}
