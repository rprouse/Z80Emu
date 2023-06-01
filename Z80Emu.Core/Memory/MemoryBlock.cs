namespace Z80Emu.Core.Memory;

/// <summary>
/// A block of memory in the memory map at a specified address range
/// </summary>
public class MemoryBlock
{
    private byte[] _data;
    private int _start;
    private int _end;

    public byte this[int address]
    {
        get => _data[address - _start];
        set => _data[address - _start] = value;
    }

    /// <summary>
    /// Constructs a block of memory for a given memory range
    /// </summary>
    /// <param name="start">The start address of this block of memory</param>
    /// <param name="end">The end address of this block of memory inclusive</param>
    public MemoryBlock(int start, int end)
    {
        _start = start;
        _end = end;
        _data = new byte[_end - _start + 1];

        // Initialize to non-zero marker bytes
        for (int i = 0; i < _data.Length; i++)
        {
            _data[i] = (byte)(i % 2 == 0 ? 0xBE : 0xEF);
        }
    }

    public void Copy(byte[] data, int offset)
    {
        int len = Math.Min(data.Length, _data.Length - offset);
        Array.Copy(data, 0, _data, offset, len);
    }

    /// <summary>
    /// Is this memory block in the address range for the given address?
    /// </summary>
    /// <param name="address"></param>
    /// <returns></returns>
    public bool HandlesAddress(int address) =>
        address >= _start && address <= _end;
}
