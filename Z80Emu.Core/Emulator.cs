using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;

namespace Z80Emu.Core;

public class Emulator
{
    public CPU _cpu;
    public MMU _mmu;
    public Interupts _int;

    public Emulator()
    {
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _cpu = new CPU(_int, _mmu);
    }

    public bool LoadProgram(string filename) =>
        _mmu.LoadProgram(filename);

    public int Tick()
    {
        _cpu.Tick();
        return 1;
    }

    public override string ToString()
    {
        return _cpu.ToString();
    }
}
