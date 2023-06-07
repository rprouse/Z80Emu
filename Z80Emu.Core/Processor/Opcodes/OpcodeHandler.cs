using Z80Emu.Core.Memory;
using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class OpcodeHandler
{
    // Some variables to carry values between ticks
    byte _lsb;
    byte _msb;
    byte _operand;
    word _address;

    readonly Registers _reg;
    readonly MMU _mmu;
    readonly Interupts _int;

    Dictionary<string, Opcode> _opcodes;

    public OpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
    {
        _reg = registers;
        _mmu = mmu;
        _int = interupts;
        _opcodes = Initialize();
        InitializeMethods();
    }

    public virtual Opcode FetchInstruction()
    {
        Opcode? opcode = _opcodes.Values.FirstOrDefault(o => o.Match(_mmu, _reg.PC));
        if (opcode == null) throw new NotImplementedException($"Opcode 0x{_mmu[_reg.PC]:X2} does not exist");

        // Consume the initial opcode bytes. Operands are consumed by the opcode execution.
        _reg.PC += opcode.Length;

        return opcode;
    }

    /// <summary>
    /// Reads the next byte from memory and increments PC
    /// </summary>
    /// <returns></returns>
    protected byte NextByte() => _mmu[_reg.PC++];

    protected void ADC(byte value)
    {
        int carry = _reg.FlagC ? 1 : 0;
        int result = _reg.A + value + carry;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.A & 0x0F) + (value & 0x0F) + carry > 0x0F;
        _reg.FlagC = result > 0xFF;
        _reg.A = (byte)result;
    }

    protected void ADD(byte value)
    {
        int result = _reg.A + value;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.A & 0x0F) + (value & 0x0F) > 0x0F;
        _reg.FlagC = result > 0xFF;
        _reg.A = (byte)result;
    }

    protected void ADDHL(word value)
    {
        int result = _reg.HL + value;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.HL & 0x0FFF) + (value & 0x0FFF) > 0x0FFF;
        _reg.FlagC = result > 0xFFFF;
        _reg.HL = (word)result;
    }

    protected void AND(byte value)
    {
        _reg.A &= value;
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = true;
        _reg.FlagC = false;
    }

    protected void BIT(int bit, byte value)
    {
        _reg.FlagZ = (value & 1 << bit) == 0;
        _reg.FlagN = false;
        _reg.FlagH = true;
    }

    protected void CP(byte value)
    {
        _reg.FlagZ = _reg.A == value;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value > _reg.A;
    }

    protected void DAA()
    {
        if (_reg.FlagN)
        {
            _reg.A -= (byte)(_reg.FlagC ? 0x60 : 0x06);
        }
        else
        {
            if (_reg.FlagC || (_reg.A > 0x99))
            {
                _reg.A += 0x60;
                _reg.FlagC = true;
            }
            if (_reg.FlagH || (_reg.A & 0x0F) > 0x09)
            {
                _reg.A += 0x06;
            }
        }
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagH = false;
    }

    protected byte DEC(byte value)
    {
        int result = value - 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) == 0x00;
        return (byte)result;
    }

    protected byte INC(byte value)
    {
        int result = value + 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (value & 0x0F) == 0x0F;
        return (byte)result;
    }

    protected void OR(byte value)
    {
        _reg.A = (byte)(_reg.A | value);
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
    }

    // Reset bit in value
    protected byte RES(int bit, byte value) =>
        (byte)(value & ~(1 << bit));

    protected byte RL(byte value)
    {
        byte result = (byte)((value << 1) | (_reg.FlagC ? 1 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 0x80) == 0x80;
        return result;
    }

    protected byte RLC(byte value)
    {
        byte result = (byte)((value << 1) | (value >> 7));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 0x80) == 0x80;
        return result;
    }

    protected byte RR(byte value)
    {
        byte result = (byte)((value >> 1) | (_reg.FlagC ? 0x80 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        return result;
    }

    protected byte RRC(byte value)
    {
        byte result = (byte)((value >> 1) | (value << 7));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        return result;
    }

    protected Action RST(word address) =>
        () =>
        {
            _mmu[--_reg.SP] = _reg.PC.Msb();
            _mmu[--_reg.SP] = _reg.PC.Lsb();
            _reg.PC = address;
        };

    protected void SBC(byte value)
    {
        int carry = _reg.FlagC ? 1 : 0;
        int result = _reg.A - value - carry;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = ((value + carry) & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value + carry > _reg.A;
        _reg.A = (byte)result;
    }

    // Set bit in value
    protected byte SET(int bit, byte value) =>
        (byte)(value | (1 << bit));

    // Shift Left Arithmetically register r8
    // C<- [7 < -0] <- 0
    protected byte SLA(byte value)
    {
        byte result = (byte)(value << 1);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 0x80) == 0x80;
        return result;
    }

    // Shift Right Arithmetically register r8
    // [7] -> [7 -> 0] -> C
    protected byte SRA(byte value)
    {
        byte result = (byte)((value >> 1) | value & 0x80);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        return result;
    }

    // Shift Right Logically register r8.
    // 0 -> [7 -> 0] -> C
    protected byte SRL(byte value)
    {
        byte result = (byte)(value >> 1);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        return result;
    }

    protected void SUB(byte value)
    {
        int result = _reg.A - value;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value > _reg.A;
        _reg.A = (byte)result;
    }

    // Swap the upper 4 bits in register r8 and the lower 4 ones.
    protected byte SWAP(byte value)
    {
        byte result = (byte)((value >> 4) | (value << 4));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
        return result;
    }

    protected void XOR(byte value)
    {
        _reg.A = (byte)(_reg.A ^ value);
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
    }
}
