using System.Text;
using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;

namespace Z80Emu.Core;

public class GameBoy
{
    public CPU _cpu;
    public MMU _mmu;
    public Dma _dma;
    public Interupts _int;
    public Clock _clock;

    // constructor
    public GameBoy()
    {
        _mmu = new MMU();

        _dma = new Dma(_mmu);
        _clock = new Clock(_mmu);
        _int = new Interupts(_mmu);

        _cpu = new CPU(_clock, _int, _mmu);
    }

    public int Tick()
    {
        _clock.Tick();
        _cpu.Tick();
        _dma.Tick();
        return 1;
    }
}
