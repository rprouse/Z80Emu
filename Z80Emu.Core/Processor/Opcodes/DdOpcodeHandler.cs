using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class DdOpcodeHandler : BaseOpcodeHandler
{
    public DdOpcodeHandler(Registers registers, MMU mmu, Interupts interupts)
        : base(registers, mmu, interupts) { }
}
