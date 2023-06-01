namespace Z80Emu.Core.Memory;

public class Dma
{
    private readonly MMU _mmu;

    public byte Register
    {
        get => _mmu[0xFF46];
        set => _mmu[0xFF46] = value;
    }

    public Dma(MMU mmu)
    {
        _mmu = mmu;
        Register = 0xFF;
    }

    public void Tick()
    {
    }
}
