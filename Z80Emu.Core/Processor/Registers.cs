using System.Text;
using Z80Emu.Core.Utilities;

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

    public byte I { get; set; }
    public byte R { get; set; }

    public byte A_ { get; set; }
    public byte F_ { get; set; }
    public byte B_ { get; set; }
    public byte C_ { get; set; }
    public byte D_ { get; set; }
    public byte E_ { get; set; }
    public byte H_ { get; set; }
    public byte L_ { get; set; }

    public word AF_
    {
        get => (word)(A_ << 8 | F_);
        set
        {
            A_ = (byte)(value >> 8);
            F_ = (byte)(value & 0x00FF);
        }
    }

    public word BC_
    {
        get => (word)(B_ << 8 | C_);
        set
        {
            B_ = (byte)(value >> 8);
            C_ = (byte)(value & 0x00FF);
        }
    }

    public word DE_
    {
        get => (word)(D_ << 8 | E_);
        set
        {
            D_ = (byte)(value >> 8);
            E_ = (byte)(value & 0x00FF);
        }
    }

    public word HL_
    {
        get => (word)(H_ << 8 | L_);
        set
        {
            H_ = (byte)(value >> 8);
            L_ = (byte)(value & 0x00FF);
        }
    }

    public word IX_ { get; set; }
    public word IY_ { get; set; }

    public word SP_ { get; set; }
    public word PC_ { get; set; }

    // Bit 7: Sign Flag
    // Bit 6: Zero Flag
    // Bit 5: Not Used
    // Bit 4: Half Carry Flag
    // Bit 3: Not Used
    // Bit 2: Parity / Overflow Flag
    // Bit 1: Add / Subtract Flag
    // Bit 0: Carry Flag
    public enum Flag : byte
    {
        Sign = 0b1000_0000,
        Zero = 0b0100_0000,
        HalfCarry = 0b0001_0000,
        Parity = 0b0000_0100,
        Subtract = 0b0000_0010,
        Carry = 0b0000_0001
    }

    /// <summary>
    /// Sign status (value of bit 7)
    /// </summary>
    public bool FlagS
    {
        get => F.GetFlag(Flag.Sign);
        set => F = F.SetFlag(Flag.Sign, value);
    }

    /// <summary>
    /// Zero status (1 for zero, 0 for non-zero)
    /// </summary>
    public bool FlagZ
    {
        get => F.GetFlag(Flag.Zero);
        set => F = F.SetFlag(Flag.Zero, value);
    }

    /// <summary>
    /// Half-Carry (carry/borrow from bit 3 to bit 4)
    /// </summary>
    public bool FlagH
    {
        get => F.GetFlag(Flag.HalfCarry);
        set => F = F.SetFlag(Flag.HalfCarry, value);
    }

    /// <summary>
    /// Parity/Overflow (for logical operations 1 for even parity 0 for odd,
    /// for arithmetic operations 1 for overflow)
    /// </summary>
    public bool FlagPV
    {
        get => F.GetFlag(Flag.Parity);
        set => F = F.SetFlag(Flag.Parity, value);
    }

    /// <summary>
    /// Subtract status (1 after subtract operation, 0 otherwise)
    /// </summary>
    public bool FlagN
    {
        get => F.GetFlag(Flag.Subtract);
        set => F = F.SetFlag(Flag.Subtract, value);
    }

    /// <summary>
    /// Carry status (carry out of bit 7)
    /// </summary>
    public bool FlagC
    {
        get => F.GetFlag(Flag.Carry);
        set => F = F.SetFlag(Flag.Carry, value);
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