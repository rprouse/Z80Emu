using System;
using System.Text;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core.Processor;

public class Registers
{
    public byte A { get; set; }
    public byte F { get; set; }
    public byte B { get; set; }
    public byte C { get; set; }
    public byte D { get; set; }
    public byte E { get; set; }
    public byte H { get; set; }
    public byte L { get; set; }

    public word AF
    {
        get => (word)(A << 8 | F);
        set
        {
            A = (byte)(value >> 8);
            F = (byte)(value & 0x00FF);
        }
    }

    public word BC
    {
        get => (word)(B << 8 | C);
        set
        {
            B = (byte)(value >> 8);
            C = (byte)(value & 0x00FF);
        }
    }

    public word DE
    {
        get => (word)(D << 8 | E);
        set
        {
            D = (byte)(value >> 8);
            E = (byte)(value & 0x00FF);
        }
    }

    public word HL
    {
        get => (word)(H << 8 | L);
        set
        {
            H = (byte)(value >> 8);
            L = (byte)(value & 0x00FF);
        }
    }

    public word IX { get; set; }
    public word IY { get; set; }

    public word SP { get; set; }
    public word PC { get; set; }


    public byte A2 { get; set; }
    public byte F2 { get; set; }
    public byte B2 { get; set; }
    public byte C2 { get; set; }
    public byte D2 { get; set; }
    public byte E2 { get; set; }
    public byte H2 { get; set; }
    public byte L2 { get; set; }

    public word AF2
    {
        get => (word)(A2 << 8 | F2);
        set
        {
            A2 = (byte)(value >> 8);
            F2 = (byte)(value & 0x00FF);
        }
    }

    public word BC2
    {
        get => (word)(B2 << 8 | C2);
        set
        {
            B2 = (byte)(value >> 8);
            C2 = (byte)(value & 0x00FF);
        }
    }

    public word DE2
    {
        get => (word)(D2 << 8 | E2);
        set
        {
            D2 = (byte)(value >> 8);
            E2 = (byte)(value & 0x00FF);
        }
    }

    public word HL2
    {
        get => (word)(H2 << 8 | L2);
        set
        {
            H2 = (byte)(value >> 8);
            L2 = (byte)(value & 0x00FF);
        }
    }

    public word IX2 { get; set; }
    public word IY2 { get; set; }

    public word SP2 { get; set; }
    public word PC2 { get; set; }

    // Bit 7: Sign Flag
    // Bit 6: Zero Flag
    // Bit 5: Not Used
    // Bit 4: Half Carry Flag
    // Bit 3: Not Used
    // Bit 2: Parity / Overflow Flag
    // Bit 1: Add / Subtract Flag
    // Bit 0: Carry Flag

    // Sign
    public bool FlagS
    {
        get => (F & 0b1000_0000) != 0;
        set => F = (byte)(value ? F | 0b1000_0000 : F & 0b0111_1111);
    }

    // Zero
    public bool FlagZ
    {
        get => (F & 0b0100_0000) != 0;
        set => F = (byte)(value ? F | 0b0100_0000 : F & 0b1011_1111);
    }

    // Parity/Overflow
    public bool FlagPV
    {
        get => (F & 0b0000_0100) != 0;
        set => F = (byte)(value ? F | 0b0000_0100 : F & 0b1111_1011);
    }

    // Add/Subtract
    public bool FlagN
    {
        get => (F & 0b0000_0010) != 0;
        set => F = (byte)(value ? F | 0b0000_0010 : F & 0b1111_1101);
    }

    // Half-Carry
    public bool FlagH
    {
        get => (F & 0b0001_0000) != 0;
        set => F = (byte)(value ? F | 0b0001_0000 : F & 0b1110_1111);
    }

    // Carry
    public bool FlagC
    {
        get => (F & 0b0000_0001) != 0;
        set => F = (byte)(value ? F | 0b0000_0001 : F & 0b1111_1110);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"AF:{AF:X4} BC:{BC:X4} DE:{DE:X4} HL:{HL:X4}");
        sb.AppendLine($"SP:{SP:X4} PC:{PC:X4}");
        sb.AppendLine($"Z:{(FlagZ ? '1' : '0')} N:{(FlagN ? '1' : '0')} H:{(FlagH ? '1' : '0')} C:{(FlagC ? '1' : '0')}");
        return sb.ToString();
    }
}