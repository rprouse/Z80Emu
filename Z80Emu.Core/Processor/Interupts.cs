using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor;

public class Interupts
{
    private readonly MMU _mmu;

    public bool IFF1 { get; set; }
    public bool IFF2 { get; set; }

    public InterruptMode Mode { get; set; }

    /// <summary>
    /// True after EI for one tick — interrupt sampling is suppressed for that
    /// tick so an EI immediately followed by RET runs the RET before any
    /// pending IRQ re-enters the ISR.
    /// </summary>
    public bool EiPending { get; set; }

    public bool IsRequested { get; private set; }
    public byte? RequestData { get; private set; }
    public bool IsNmiRequested { get; private set; }

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

    /// <summary>
    /// Latch a maskable interrupt request. The optional data byte is used
    /// as the IM 2 vector low byte; ignored in IM 0 / IM 1.
    /// </summary>
    public void RaiseInterrupt(byte? data = null)
    {
        IsRequested = true;
        RequestData = data;
    }

    /// <summary>
    /// Latch a non-maskable interrupt request.
    /// </summary>
    public void RaiseNmi()
    {
        IsNmiRequested = true;
    }

    /// <summary>
    /// Called by Emulator.ServiceInterrupts after a maskable INT is taken.
    /// </summary>
    public void ConsumeInterrupt()
    {
        IsRequested = false;
        RequestData = null;
    }

    /// <summary>
    /// Called by Emulator.ServiceInterrupts after an NMI is taken.
    /// </summary>
    public void ConsumeNmi()
    {
        IsNmiRequested = false;
    }
}
