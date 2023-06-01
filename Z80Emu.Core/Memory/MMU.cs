using System.Diagnostics;
using System.Text;

namespace Z80Emu.Core.Memory;

/// <summary>
/// Controls access to all memory on the device
/// </summary>
public class MMU
{
    private readonly MemoryBlock _ram;
    private readonly MemoryBlock[] _memoryBlocks;

    public MMU()
    {
        _ram = new MemoryBlock(0x0000, 0xFFFF);
        _memoryBlocks = new[] { _ram };

        InitializeMemory();
    }

    private void InitializeMemory()
    {
    }

    /// <summary>
    /// Loads a program from a file to 0x0100
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public bool LoadProgram(string filename, int location = 0x0100)
    {
        if (!File.Exists(filename)) return false;
        byte[] data = File.ReadAllBytes(filename);
        _ram.Copy(data, location);
        return true;
    }

    public byte this[int address]
    {
        get => Read(address);
        set => Write(address, value);
    }

    private byte Read(int address)
    {
        Debug.Assert(address >= 0 && address <= 0xFFFF);

        var memory = _memoryBlocks.FirstOrDefault(m => m.HandlesAddress(address));
        if (memory != null) return memory[address];
        
        throw new NotImplementedException($"Address 0x{address:X2} not mapped");
    }

    private void Write(int address, byte data)
    {
        Debug.Assert(address >= 0 && address <= 0xFFFF);

        var memory = _memoryBlocks.FirstOrDefault(m => m.HandlesAddress(address));
        if (memory != null) memory[address] = data;
        else throw new NotImplementedException($"Address 0x{address:X2} not mapped");
    }
}
