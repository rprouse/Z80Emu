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
        Opcode opcode = PeekInstruction(_reg.PC);

        // Consume the initial opcode bytes. Operands are consumed by the opcode execution.
        _reg.PC += opcode.OpcodeLength;

        return opcode;
    }

    public Opcode PeekInstruction(word addr)
    {
        Opcode? opcode = _opcodes.Values.FirstOrDefault(o => o.Match(_mmu, addr));
        if (opcode == null) throw new NotImplementedException($"Opcode 0x{_mmu[addr]:X2} does not exist");
        opcode.SetSubstitutions(_mmu, addr);
        return opcode;
    }

    /// <summary>
    /// Reads the next byte from memory and increments PC
    /// </summary>
    /// <returns></returns>
    byte NextByte() => _mmu[_reg.PC++];

    /// <summary>
    /// Reads the next word from memory and increments PC
    /// </summary>
    /// <returns></returns>
    word NextWord()
    {
        _lsb = NextByte();
        _msb = NextByte();
        return BitUtils.ToWord(_msb, _lsb);
    }

    byte AddSubtractByte(byte value, bool withCarry, bool subtract)
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
        _reg.FlagS = ((byte)result).IsNegative();
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

    word AddSubtractWord(word value1, word value2, bool withCarry, bool subtract)
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

    void SetLogicFlags(bool flagH)
    {
        _reg.FlagS = _reg.A.IsNegative();
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagH = flagH;
        _reg.FlagN = false;
        _reg.FlagC = false;
        _reg.FlagPV = _reg.A.IsEvenParity();
    }

    void ADC(byte value)
    {
        _reg.A = AddSubtractByte(value, true, false);
    }

    void ADCHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, true, false);
    }

    void ADD(byte value)
    {
        _reg.A = AddSubtractByte(value, false, false);
    }

    void ADDHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, false, false);
    }

    void ADDIX(word value)
    {
        _reg.IX = AddSubtractWord(_reg.IX, value, false, false);
    }

    void ADDIY(word value)
    {
        _reg.IY = AddSubtractWord(_reg.IY, value, false, false);
    }

    void AND(byte value)
    {
        _reg.A &= value;
        SetLogicFlags(true);
    }

    void BIT(int bit, byte value)
    {
        _reg.FlagZ = (value & 1 << bit) == 0;
        _reg.FlagPV = _reg.FlagZ;
        _reg.FlagN = false;
        _reg.FlagH = true;
        _reg.FlagS = false;
        if (bit == 7 && !_reg.FlagZ)
            _reg.FlagS = true;
    }

    byte CP(byte value) =>
        AddSubtractByte(value, false, true);

    void CPD()
    {
        bool carry = _reg.FlagC;
        CP(_mmu[_reg.HL]);
        _reg.HL--;
        _reg.BC--;
        _reg.FlagPV = _reg.BC != 0;
        _reg.FlagC = carry;
    }

    void CPDR()
    {
        CPD();
        if (_reg.BC != 0 && !_reg.FlagZ)
            _reg.PC -= 2;
    }

    void CPI()
    {
        bool carry = _reg.FlagC;
        CP(_mmu[_reg.HL]);
        _reg.HL++;
        _reg.BC--;
        _reg.FlagPV = _reg.BC != 0;
        _reg.FlagC = carry;
    }

    void CPIR()
    {
        CPI();
        if (_reg.BC != 0 && !_reg.FlagZ)
            _reg.PC -= 2;
    }

    void DAA()
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
        _reg.FlagS = _reg.A.IsNegative();
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagPV = _reg.A.IsEvenParity();
    }

    byte DEC(byte value)
    {
        _reg.FlagPV = (value & 0x80) == 0 && ((value - 1) & 0x80) != 0;
        int result = value - 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) == 0x0F;
        _reg.FlagS = result.IsNegative();
        return (byte)result;
    }

    byte INC(byte value)
    {
        _reg.FlagPV = (value & 0x80) != 0 && ((value + 1) & 0x80) == 0;
        int result = value + 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (value & 0x0F) == 0x00;
        _reg.FlagS = result.IsNegative();
        return (byte)result;
    }

    void LDD()
    {
        _mmu[_reg.DE--] = _mmu[_reg.HL--];
        _reg.BC--;
        _reg.FlagH = false;
        _reg.FlagN = false;
        _reg.FlagPV = _reg.BC != 0;
    }

    void LDDR()
    {
        LDD();
        if (_reg.BC != 0) _reg.PC -= 2;
    }

    void LDI()
    {
        _mmu[_reg.DE++] = _mmu[_reg.HL++];
        _reg.BC--;
        _reg.FlagH = false;
        _reg.FlagN = false;
        _reg.FlagPV = _reg.BC != 0;
    }

    void LDIR()
    {
        LDI();
        if (_reg.BC != 0) _reg.PC -= 2;
    }

    void NEG()
    {
        byte temp = _reg.A;
        _reg.A = 0;
        _reg.A = AddSubtractByte(temp, false, true);
        _reg.FlagN = true;
    }

    void OR(byte value)
    {
        _reg.A = (byte)(_reg.A | value);
        SetLogicFlags(false);
    }

    // Reset bit in value
    byte RES(int bit, byte value) =>
        (byte)(value & ~(1 << bit));

    // This is public so OS calls can return
    public void RET()
    {
        _lsb = _mmu[_reg.SP++];
        _msb = _mmu[_reg.SP++];
        _reg.PC = BitUtils.ToWord(_msb, _lsb);
    }

    byte RL(byte value)
    {
        byte result = (byte)((value << 1) | (_reg.FlagC ? 1 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 0x80) == 0x80;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    byte RLC(byte value)
    {
        _reg.FlagC = (value & 0x80) == 0x80;
        byte result = (byte)((value << 1) | (_reg.FlagC ? 1 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    void RLD()
    {
        byte ah = (byte)(_reg.A & 0xF0);
        byte hl = _mmu[_reg.HL];
        _mmu[_reg.HL] = (byte)((_reg.A << 4) | (hl & 0x0F));
        _reg.A = (byte)((_reg.A & 0xF0) | (hl >> 4));
        _reg.HL = (byte)((hl << 4) | ah);
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagS = _reg.A.IsNegative();
        _reg.FlagPV = _reg.A.IsEvenParity();
        _reg.FlagH = false;
        _reg.FlagN = false;
    }

    byte RR(byte value)
    {
        byte result = (byte)((value >> 1) | (_reg.FlagC ? 0x80 : 0));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    byte RRC(byte value)
    {
        byte result = (byte)((value >> 1) | (value << 7));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    void RRD()
    {
        byte ah = (byte)(_reg.A & 0x0F);
        byte hl = _mmu[_reg.HL];
        _reg.A = (byte)((_reg.A & 0xF0) | (hl & 0x0F));
        _mmu[_reg.HL] = (byte)((_reg.A << 4) | (hl & 0x0F));
        _reg.HL = (byte)((hl >> 4) | (ah << 4));
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagS = _reg.A.IsNegative();
        _reg.FlagPV = _reg.A.IsEvenParity();
        _reg.FlagH = false;
        _reg.FlagN = false;
    }

    Action RST(word address) =>
        () =>
        {
            _mmu[--_reg.SP] = _reg.PC.Msb();
            _mmu[--_reg.SP] = _reg.PC.Lsb();
            _reg.PC = address;
        };

    void SBC(byte value)
    {
        _reg.A = AddSubtractByte(value, true, true);
    }

    void SBCHL(word value)
    {
        _reg.HL = AddSubtractWord(_reg.HL, value, true, true);
    }

    // Set bit in value
    byte SET(int bit, byte value) =>
        (byte)(value | (1 << bit));

    // Shift Left Arithmetically register r8
    // C<- [7 < -0] <- 0
    byte SLA(byte value)
    {
        byte result = (byte)(value << 1);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 0x80) == 0x80;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    // Shift Right Arithmetically register r8
    // [7] -> [7 -> 0] -> C
    byte SRA(byte value)
    {
        byte result = (byte)((value >> 1) | value & 0x80);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    // Shift Right Logically register r8.
    // 0 -> [7 -> 0] -> C
    byte SRL(byte value)
    {
        byte result = (byte)(value >> 1);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        _reg.FlagS = result.IsNegative();
        _reg.FlagPV = result.IsEvenParity();
        return result;
    }

    void SUB(byte value)
    {
        _reg.A = AddSubtractByte(value, false, true);
    }

    void XOR(byte value)
    {
        _reg.A = (byte)(_reg.A ^ value);
        SetLogicFlags(false);
    }
}
