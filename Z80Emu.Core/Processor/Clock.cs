using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor;

public class Clock
{
    private readonly MMU _mmu;

    public byte DIV
    {
        get => _mmu[0xFF04];
        set => _mmu[0xFF04] = value;
    }
    public byte TIMA
    {
        get => _mmu[0xFF05];
        set => _mmu[0xFF05] = value;
    }
    public byte TMA
    {
        get => _mmu[0xFF06];
        set => _mmu[0xFF06] = value;
    }
    public byte TAC
    {
        get => _mmu[0xFF07];
        set => _mmu[0xFF07] = value;
    }

    public Clock(MMU mmu)
    {
        _mmu = mmu;
        DIV = 0xAB;
        TAC = 0xF8;
    }

    public void Tick()
    {
    }
}
