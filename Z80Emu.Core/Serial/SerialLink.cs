using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Serial;

public class SerialLink
{
    private readonly MMU _mmu;

    public byte SB
    {
        get => _mmu[0xFF01];
        set => _mmu[0xFF01] = value;
    }
    public byte SC
    {
        get => _mmu[0xFF02];
        set => _mmu[0xFF02] = value;
    }

    public SerialLink(MMU mmu)
    {
        _mmu = mmu;
        SC = 0x7E;
    }

    public void Tick()
    {
    }
}
