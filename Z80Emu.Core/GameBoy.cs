using System.Text;
using Z80Emu.Core.Graphics;
using Z80Emu.Core.Input;
using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Serial;

namespace Z80Emu.Core;

public class GameBoy
{
    const string BootRom = @"C:\Src\Retro\GameBoy\Z80Emu\roms\dmg_boot.bin";
    const string Cartridge = @"C:\Src\Retro\GameBoy\Z80Emu\roms\Tetris.gb";

    public const int ClockSpeed = 4_194_304;
    public CPU _cpu;
    public VPU _vpu;
    public MMU _mmu;
    public Dma _dma;
    public Interupts _int;
    public Clock _clock;
    public SerialLink _serial;
    public Joypad _joypad;

    // constructor
    public GameBoy()
    {
        _mmu = new MMU();

        // Load before the CPU so the CPU can set the PC
        // Based on whether or not the boot rom is loaded
        _mmu.LoadBootRom(BootRom);
        _mmu.LoadCartridge(Cartridge);

        _joypad = new Joypad(_mmu);
        _dma = new Dma(_mmu);
        _serial = new SerialLink(_mmu);
        _clock = new Clock(_mmu);
        _int = new Interupts(_mmu);
        _vpu = new VPU(_mmu);

        _cpu = new CPU(_clock, _int, _vpu, _mmu);
    }

    public int Tick()
    {
        _clock.Tick();
        _cpu.Tick();
        _dma.Tick();
        _serial.Tick();
        _vpu.Tick();
        return 1;
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine(_vpu.ToString()); // Video memory
        sb.AppendLine();
        sb.AppendLine(_cpu.ToString()); // Registers
        sb.AppendLine();
        return sb.ToString();
    }
}
