using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;

namespace Z80Emu.Core;

public class Emulator
{
    public CPU _cpu;
    public MMU _mmu;
    public Interupts _int;

    // constructor
    public Emulator()
    {
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _cpu = new CPU(_int, _mmu);
    }

    public int Tick()
    {
        _cpu.Tick();
        return 1;
    }
}
