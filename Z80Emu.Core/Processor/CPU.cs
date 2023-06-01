using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core.Processor;

public class CPU
{
    private readonly Interupts _int;
    private readonly MMU _mmu;
    private readonly Registers _reg;
    private readonly OpcodeHandler _opcodeHandler;

    public CPU(Interupts interupts, MMU mmu)
    {
        _int = interupts;
        _mmu = mmu;

        _reg = new Registers
        {
            AF = 0x0000,
            BC = 0x0000,
            DE = 0x0000,
            HL = 0x0000,
            PC = 0x0100,
            SP = 0xFFFE
        };
        _int.IME = false;

        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int);
    }

    public void Tick()
    {
        var opcode = _opcodeHandler.FetchInstruction();
        opcode.Tick();
    }

    public override string ToString()
    {
        return _reg.ToString();
    }
}
