using System.Text;

namespace Z80Emu.Core.Memory;

/// <summary>
/// Controls access to all memory on the device
/// </summary>
/// <remarks>
/// 0x0000 - 0x00FF Boot ROM
/// 0x0000 - 0x3FFF 16KB ROM Bank 00 (in cartridge, fixed at bank 00)
/// 0x4000 - 0x7FFF 16KB ROM Bank 01..NN(in cartridge, switchable bank number)
/// 0x8000 - 0x9FFF 8KB Video RAM(VRAM)(switchable bank 0 - 1 in CGB Mode)
/// 0xA000 - 0xBFFF 8KB External RAM(in cartridge, switchable bank, if any)
/// 0xC000 - 0xCFFF 4KB Work RAM Bank 0(WRAM)
/// 0xD000 - 0xDFFF 4KB Work RAM Bank 1(WRAM)(switchable bank 1 - 7 in CGB Mode)
/// 0xE000 - 0xFDFF Same as C000 - DDFF(ECHO)(typically not used)
/// 0xFE00 - 0xFE9F Sprite Attribute Table(OAM)
/// 0xFEA0 - 0xFEFF Not Usable
/// 0xFF00 - 0xFF7F I/O Ports
/// 0xFF80 - 0xFFFE High RAM(HRAM)
/// 0xFFFF          Interrupt Enable Register
/// </remarks>
public class MMU
{
    private Cartridge _cartridge;
    private readonly MemoryBlock _bootRom;
    private readonly MemoryBlock _vram;
    private readonly MemoryBlock _wram;
    private readonly MemoryBlock _oam;
    private readonly MemoryBlock _io;
    private readonly MemoryBlock _hram;
    private readonly MemoryBlock[] _memoryBlocks;

    public bool BootRomBankedIn => _io[0xFF50] == 0x00;

    public MMU()
    {
        _bootRom = new MemoryBlock(0x0000, 0x00FF);
        // 0x0000 - 0x7FFF - Cartridge
        _vram = new MemoryBlock(0x8000, 0x9FFF);
        _wram = new MemoryBlock(0xC000, 0xCFFF);
        // 0xE000 - 0xFDFF - Echo RAM
        _oam = new MemoryBlock(0xFE00, 0xFE9F);
        // 0xFEA0 - 0xFEFF - Restricted
        _io = new MemoryBlock(0xFF00, 0xFF7F);
        _hram = new MemoryBlock(0xFF80, 0xFFFF);
        _memoryBlocks = new[] { _vram, _wram, _oam, _io, _hram };

        InitializeMemory();
    }

    private void InitializeMemory()
    {
        // Initialize memory
        this[0xFF0F] = 0xE1;  // IF

        // Sound 1
        this[0xFF10] = 0x80;  // ENT1
        this[0xFF11] = 0xBF;  // LEN1
        this[0xFF12] = 0xF3;  // ENV1
        this[0xFF13] = 0xC1;  // FRQ1
        this[0xFF14] = 0xBF;  // KIK1

        this[0xFF15] = 0xFF;  // N/A
        this[0xFF16] = 0x3F;  // LEN2
        this[0xFF19] = 0xB8;  // KIK2
        this[0xFF1A] = 0x7F;
        this[0xFF1B] = 0xFF;
        this[0xFF1C] = 0x9F;
        this[0xFF1E] = 0xBF;
        this[0xFF20] = 0xFF;
        this[0xFF23] = 0xBF;
        this[0xFF24] = 0x77;
        this[0xFF25] = 0xF3;
        this[0xFF26] = 0xF1;

        this[0xFF70] = 0xFF;  // SVBK
        this[0xFF4F] = 0xFF;  // VBK
        this[0xFF4D] = 0xFF;  // KEY1
        this[0xFF50] = 0x01;  // Boot Rom Disabled
    }

    public bool LoadBootRom(string filename)
    {
        if (!File.Exists(filename)) return false;
        byte[] data = File.ReadAllBytes(filename);
        _bootRom.Copy(data);
        this[0xFF50] = 0x00;  // Boot Rom Enabled
        return true;
    }

    public bool LoadCartridge(string filename)
    {
        if (!File.Exists(filename)) return false;
        byte[] data = File.ReadAllBytes(filename);
        _cartridge = new Cartridge(data);
        return true;
    }

    public byte this[int address]
    {
        get => Read(address);
        set => Write(address, value);
    }

    private byte Read(int address)
    {
        // Cartridge memory unless the boot rom is still loaded
        if (BootRomBankedIn && address <= 0x00FF) return _bootRom[address];
        else if (address < 0x8000) return _cartridge[address];
        // ECHO ram
        else if ((address >= 0xE000) && (address <= 0xFDFF)) return _wram[address - 0x2000];
        // This area is restricted
        else if ((address >= 0xFEA0) && (address <= 0xFEFF)) { return 0xFF; }
        else
        {
            var memory = _memoryBlocks.FirstOrDefault(m => m.HandlesAddress(address));
            if (memory != null) return memory[address];
        }
        throw new NotImplementedException($"Address 0x{address:X2} not mapped");
    }

    private void Write(int address, byte data)
    {
        // Cartridge memory unless the boot rom is still loaded
        if (BootRomBankedIn && address <= 0xFF) _bootRom[address] = data;
        else if (address < 0x8000) _cartridge[address] = data;
        // ECHO ram
        else if ((address >= 0xE000) && (address <= 0xFDFF)) _wram[address - 0x2000] = data;
        // This area is restricted
        else if ((address >= 0xFEA0) && (address <= 0xFEFF)) { }
        else
        {
            var memory = _memoryBlocks.FirstOrDefault(m => m.HandlesAddress(address));
            if (memory != null) memory[address] = data;
            else throw new NotImplementedException($"Address 0x{address:X2} not mapped");
        }
    }
}
