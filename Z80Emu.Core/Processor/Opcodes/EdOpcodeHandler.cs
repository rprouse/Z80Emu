using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class EdOpcodeHandler : BaseOpcodeHandler
{
    // Some variables to carry values between ticks
    private byte _lsb;
    private byte _msb;
    private byte _operand;
    private word _address;

    public EdOpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
        : base(registers, mmu, interupts) { }
}
