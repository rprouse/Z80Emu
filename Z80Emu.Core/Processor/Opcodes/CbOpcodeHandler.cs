using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class CbOpcodeHandler : BaseOpcodeHandler
{
    // Some variables to carry values between ticks
    private byte _operand;
    private byte _result;

    public CbOpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
        : base(registers, mmu, interupts) { }

    private void BIT(int bit, byte value)
    {
        _reg.FlagZ = (value & 1 << bit) == 0;
        _reg.FlagN = false;
        _reg.FlagH = true;
    }

    // Reset bit in value
    private byte RES(int bit, byte value) =>
        (byte)(value & ~(1 << bit));

    // Set bit in value
    private byte SET(int bit, byte value) =>
        (byte)(value | (1 << bit));

    // Shift Left Arithmetically register r8
    // C<- [7 < -0] <- 0
    private byte SLA(byte value)
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
    private byte SRA(byte value)
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
    private byte SRL(byte value)
    {
        byte result = (byte)(value >> 1);
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = (value & 1) == 1;
        return result;
    }

    // Swap the upper 4 bits in register r8 and the lower 4 ones.
    private byte SWAP(byte value)
    {
        byte result = (byte)((value >> 4) | (value << 4));
        _reg.FlagZ = result == 0;
        _reg.FlagN = false;
        _reg.FlagH = false;
        _reg.FlagC = false;
        return result;
    }
}
