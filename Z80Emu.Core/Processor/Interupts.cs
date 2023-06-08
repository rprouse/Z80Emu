using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor;

public class Interupts
{
    private readonly MMU _mmu;

    public bool IFF1 { get; set; }
    public bool IFF2 { get; set; }

    public Interupts(MMU mmu)
    {
        _mmu = mmu;
    }

    /// <summary>
    /// Disable Interrupts
    /// </summary>
    public void Disable()
    {
    }

    /// <summary>
    /// Enable Interrupts
    /// </summary>
    public void Enable()
    {
    }
}
