using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class FdOpcodeHandler : BaseOpcodeHandler
{
    public FdOpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
        : base(registers, mmu, interupts) { }
}
