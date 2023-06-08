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

    public Opcode FetchInstruction()
    {
        Opcode opcode = Disassemble(_reg.PC);

        // Consume the initial opcode bytes. Operands are consumed by the opcode execution.
        _reg.PC += opcode.OpcodeLength;

        return opcode;
    }

    public Opcode Disassemble(word addr)
    {
        Opcode? opcode = _opcodes.Values.FirstOrDefault(o => o.Match(_mmu, addr));
        if (opcode == null) throw new NotImplementedException($"Opcode 0x{_mmu[addr]:X2} does not exist");
        return opcode;
    }

    /// <summary>
    /// Reads the next byte from memory and increments PC
    /// </summary>
    /// <returns></returns>
    protected byte NextByte() => _mmu[_reg.PC++];

    // Parity lookup table
    static byte[] ParityTable = {
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        0, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 1, 0,
        1, 0, 0, 1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1 };

    static bool IsEvenParity(byte value) => ParityTable[value] == 1;

    private byte AddSubtractByte(byte value, bool withCarry, bool subtract)
    {
        ushort result;  // To detect carry and overflow

        if (subtract)
        {
            _reg.FlagN = true;
            _reg.FlagH = (_reg.A & 0x0F) < (value & 0x0F) + (withCarry ? 1 : 0);
            result = (ushort)(_reg.A - value - (withCarry ? 1 : 0));
        }
        else
        {
            _reg.FlagN = false;
            _reg.FlagH = (_reg.A & 0x0F) + (value & 0x0F) + (withCarry ? 1 : 0) > 0x0F;
            result = (ushort)(_reg.A + value + (withCarry ? 1 : 0));
        }
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagC = (result & 0x100) != 0;
        _reg.FlagZ = (result & 0xFF) == 0;

        // Overflow is set if the result is greater than 127 or less than -128
        int minuend_sign = _reg.A & 0x80;
        int subtrahend_sign = value & 0x80;
        int result_sign = result & 0x80;
        if (subtract)
            _reg.FlagPV = minuend_sign != subtrahend_sign && result_sign != minuend_sign;
        else
            _reg.FlagPV = minuend_sign == subtrahend_sign && result_sign != minuend_sign;

        return (byte)result;
    }

    private word AddSubtractWord(word value1, word value2, bool withCarry, bool subtract)
    {
        if (withCarry && _reg.FlagC)
            value2++;

        int sum;

        if (subtract)
        {
            sum = value1 - value2;
            _reg.FlagH = (value1 & 0x0FFF) < (value2 & 0x0FFF);
        }
        else
        {
            sum = value1 + value2;
            _reg.FlagH = (value1 & 0x0FFF) + (value2 & 0x0FFF) > 0x0FFF;
        }
        _reg.FlagC = sum > 0xFFFF;
        _reg.FlagN = subtract;
        if (withCarry || subtract)
        {
            int minuend_sign = value1 & 0x8000;
            int subtrahend_sign = value2 & 0x8000;
            int result_sign = sum & 0x8000;
            if (subtract)
                _reg.FlagPV = minuend_sign != subtrahend_sign && result_sign != minuend_sign;
            else
                _reg.FlagPV = minuend_sign == subtrahend_sign && result_sign != minuend_sign;
            _reg.FlagS = (sum & 0x8000) != 0;
            _reg.FlagZ = (sum & 0xFFFF) == 0;
        }
        return (word)sum;
    }

    private void SetLogicFlags(bool flagH)
    {
        _reg.FlagS = (_reg.A & 0x80) != 0;
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagH = flagH;
        _reg.FlagN = false;
        _reg.FlagC = false;
        _reg.FlagPV = IsEvenParity(_reg.A);
    }

    protected void ADC(byte value)
    {        
        _reg.A = AddSubtractByte(value, true, false);
    }

    protected void ADCHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, true, false);
    }

    protected void ADD(byte value)
    {
        _reg.A = AddSubtractByte(value, false, false);
    }

    protected void ADDHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, false, false);
    }

    protected void ADDIX(word value)
    {
        _reg.IX = AddSubtractWord(_reg.IX, value, false, false);
    }

    protected void ADDIY(word value)
    {
        _reg.IY = AddSubtractWord(_reg.IY, value, false, false);
    }

    protected void AND(byte value)
    {
        _reg.A &= value;
        SetLogicFlags(true);
    }

    protected void BIT(int bit, byte value)
    {
        _reg.FlagZ = (value & 1 << bit) == 0;
        _reg.FlagPV = _reg.FlagZ;
        _reg.FlagN = false;
        _reg.FlagH = true;
        _reg.FlagS = false;
        if (bit == 7 && !_reg.FlagZ)
            _reg.FlagS = true;
    }

    protected void CP(byte value)
    {
        AddSubtractByte(value, false, true);
    }

    protected void DAA()
    {
        var a = _reg.A;
        if (_reg.FlagN)
        {
            _reg.A -= (byte)(_reg.FlagC ? 0x60 : 0x06);
            _reg.FlagC = false;
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
                _reg.FlagC = false;
            }
        }
        _reg.FlagH = ((a ^ _reg.A) & 0x10) != 0;
        _reg.FlagS = (_reg.A & 0x80) != 0;
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagPV = IsEvenParity(_reg.A);
    }

    protected byte DEC(byte value)
    {
        _reg.FlagPV = (value & 0x80) == 0 && ((value - 1) & 0x80) != 0;
        int result = value - 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) == 0x0F;
        _reg.FlagS = (result & 0x80) != 0;
        return (byte)result;
    }

    protected byte INC(byte value)
    {
        _reg.FlagPV = (value & 0x80) != 0 && ((value + 1) & 0x80) == 0;
        int result = value + 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (value & 0x0F) == 0x00;
        _reg.FlagS = (result & 0x80) != 0;
        return (byte)result;
    }

    protected void OR(byte value)
    {
        _reg.A = (byte)(_reg.A | value);
        SetLogicFlags(false);
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
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
        return result;
    }

    protected byte RLC(byte value)
    {
        _reg.FlagC = (value & 0x80) == 0x80;
        byte result = (byte)((value << 1) | (_reg.FlagC ? 1 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
        return result;
    }

    protected byte RR(byte value)
    {
        byte result = (byte)((value >> 1) | (_reg.FlagC ? 0x80 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
        return result;
    }

    protected byte RRC(byte value)
    {
        byte result = (byte)((value >> 1) | (value << 7));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
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
        _reg.A = AddSubtractByte(value, true, true);
    }

    protected void SBCHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, true, true);
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
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
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
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
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
        _reg.FlagS = (result & 0x80) != 0;
        _reg.FlagPV = IsEvenParity(result);
        return result;
    }

    protected void SUB(byte value)
    {
        _reg.A = AddSubtractByte(value, false, true);
    }

    protected void XOR(byte value)
    {
        _reg.A = (byte)(_reg.A ^ value);
        SetLogicFlags(false);
    }
}
