using System;
using Z80Emu.Core.Memory;
using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

// https://map.grauw.nl/resources/z80instr.php
public partial class OpcodeHandler : BaseOpcodeHandler
{
    // Some variables to carry values between ticks
    private byte _lsb;
    private byte _msb;
    private byte _operand;
    private word _address;

    // Multi-byte instruction sets 0xCB, 0xDD, 0xED, 0xFD 
    private readonly CbOpcodeHandler _cbOpcodeHandler;
    private readonly IxIyOpcodeHandler _ddOpcodeHandler;
    private readonly EdOpcodeHandler _edOpcodeHandler;
    private readonly IxIyOpcodeHandler _fdOpcodeHandler;

    public OpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
        : base(registers, mmu, interupts)
    {
        _cbOpcodeHandler = new CbOpcodeHandler(registers, mmu, interupts);
        _ddOpcodeHandler = new IxIyOpcodeHandler(registers, mmu, interupts, "IX", () => _reg.IX, (word value) => _reg.IX = value);
        _edOpcodeHandler = new EdOpcodeHandler(registers, mmu, interupts);
        _fdOpcodeHandler = new IxIyOpcodeHandler(registers, mmu, interupts, "IY", () => _reg.IY, (word value) => _reg.IY = value);
    }

    public override Opcode FetchInstruction()
    {
        var opcode = base.FetchInstruction();

        // Multi-byte instruction sets 0xCB, 0xDD, 0xED, 0xFD 
        return opcode.Value switch
        {
            0xCB => _cbOpcodeHandler.FetchInstruction(),
            0xDD => _ddOpcodeHandler.FetchInstruction(),
            0xED => _edOpcodeHandler.FetchInstruction(),
            0xFD => _fdOpcodeHandler.FetchInstruction(),
            _ => opcode,
        };
    }

    private void ADC(byte value)
    {
        int carry = _reg.FlagC ? 1 : 0;
        int result = _reg.A + value + carry;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.A & 0x0F) + (value & 0x0F) + carry > 0x0F;
        _reg.FlagC = result > 0xFF;
        _reg.A = (byte)result;
    }

    private void ADD(byte value)
    {
        int result = _reg.A + value;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.A & 0x0F) + (value & 0x0F) > 0x0F;
        _reg.FlagC = result > 0xFF;
        _reg.A = (byte)result;
    }

    private void ADDHL(word value)
    {
        int result = _reg.HL + value;
        _reg.FlagN = false;
        _reg.FlagH = (_reg.HL & 0x0FFF) + (value & 0x0FFF) > 0x0FFF;
        _reg.FlagC = result > 0xFFFF;
        _reg.HL = (word)result;
    }

    private void AND(byte value)
    {
        _reg.A &= value;
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = true;
        _reg.FlagC = false;
    }

    private void CP(byte value)
    {
        _reg.FlagZ = _reg.A == value;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value > _reg.A;
    }

    private void DAA()
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

    private byte DEC(byte value)
    {
        int result = value - 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) == 0x00;
        return (byte)result;
    }

    private byte INC(byte value)
    {
        int result = value + 1;
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = (value & 0x0F) == 0x0F;
        return (byte)result;
    }

    private void OR(byte value)
    {
        _reg.A = (byte)(_reg.A | value);
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
    }

    private void XOR(byte value)
    {
        _reg.A = (byte)(_reg.A ^ value);
        _reg.FlagZ = _reg.A == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
    }

    private Action RST(word address) =>
        () =>
        {
            _mmu[--_reg.SP] = _reg.PC.Msb();
            _mmu[--_reg.SP] = _reg.PC.Lsb();
            _reg.PC = address;
        };

    private void SBC(byte value)
    {
        int carry = _reg.FlagC ? 1 : 0;
        int result = _reg.A - value - carry;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = ((value + carry) & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value + carry > _reg.A;
        _reg.A = (byte)result;
    }

    private void SUB(byte value)
    {
        int result = _reg.A - value;
        _reg.FlagZ = result == 0;
        _reg.FlagN = true;
        _reg.FlagH = (value & 0x0F) > (_reg.A & 0x0F);
        _reg.FlagC = value > _reg.A;
        _reg.A = (byte)result;
    }
}
