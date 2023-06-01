using Z80Emu.Core.Graphics;
using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public abstract class BaseOpcodeHandler
{
    protected readonly Registers _reg;
    protected readonly MMU _mmu;
    protected readonly VPU _vpu;
    protected readonly Interupts _int;

    protected Dictionary<byte, Opcode> _opcodes;

    public BaseOpcodeHandler(Registers registers, MMU mmu, VPU vpu, Interupts interupts)
    {
        _reg = registers;
        _mmu = mmu;
        _vpu = vpu;
        _int = interupts;
        _opcodes = Initialize();
    }

    protected abstract Dictionary<byte, Opcode> Initialize();

    public virtual Opcode FetchInstruction()
    {
        byte value = NextByte();
        if (_opcodes.ContainsKey(value))
        {
            return _opcodes[value];
        }
        throw new NotImplementedException($"Opcode 0x{value:X2} does not exist");
    }

    /// <summary>
    /// Reads the next byte from memory and increments PC
    /// </summary>
    /// <returns></returns>
    protected byte NextByte() => _mmu[_reg.PC++];

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
}
