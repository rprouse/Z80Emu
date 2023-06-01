using System.Text;
using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Graphics;

public class VPU
{
    private readonly MMU _mmu;

    private readonly byte[] frameBuffer = new byte[160 * 144 * 4];

    /// <summary>
    /// GPU Registers
    /// LCD and GPU Control
    /// </summary>
    /// <remarks>
    /// Bit Function
    /// 7	Display: on/off
    /// 6	Window Tile Map Display Select (0 = 9800-9BFF, 1 = 9C00-9FFF)
    /// 5	Window Display: on/off
    /// 4	BG & Window Tile Data Select   (0 = 8800-97FF, 1 = 8000-8FFF)
    /// 3	BG Tile Map Display Select     (0 = 9800-9BFF, 1 = 9C00-9FFF)
    /// 2	OBJ (Sprite) Size (0=8x8, 1=8x16)
    /// 1	OBJ (Sprite) Display Enable (0=Off, 1=On)
    /// 0	Background
    /// </remarks>
    public byte LCDC
    {
        get => _mmu[0xFF40];
        set => _mmu[0xFF40] = value;
    }

    /// <summary>
    /// Bits
    /// 0-1
    ///         00: H-Blank
    ///         01: V-Blank
    ///         10: Searching Sprites Atts
    ///         11: Transfering Data to LCD Driver
    /// 2       Set to 1 if register (0xFF44) is the same value as (0xFF45)
    /// 3, 4, 5 are interrupt enabled flags (similar to how the IE Register works), when the mode changes the
    ///         corresponding bit 3,4,5 is set
    /// 6       LYC=LY Coincidence Interrupt (1=Enable) (Read/Write)
    /// </summary>
    public byte STAT
    {
        get => _mmu[0xFF41];
        set => _mmu[0xFF41] = value;
    }

    /// <summary>
    /// The Y Position of the BACKGROUND where to
    /// start drawing the viewing area from
    /// </summary>
    public byte SCY
    {
        get => _mmu[0xFF42];
        set => _mmu[0xFF42] = value;
    }

    /// <summary>
    /// The X Position of the BACKGROUND to start
    /// drawing the viewing area from
    /// </summary>
    public byte SCX
    {
        get => _mmu[0xFF43];
        set => _mmu[0xFF43] = value;
    }

    /// <summary>
    /// The Y coordinate of the current line
    /// </summary>
    public byte LY
    {
        get => _mmu[0xFF44];
        set => _mmu[0xFF44] = value;
    }

    /// <summary>
    /// Scanline compare register
    /// </summary>
    public byte LYC
    {
        get => _mmu[0xFF45];
        set => _mmu[0xFF45] = value;
    }

    /// <summary>
    /// Every two bits in the palette data byte represent a color.
    /// Bits 7-6 maps to color id 11, bits 5-4 map to color id 10, bits 3-2 map to color id 01 and bits 1-0 map to color id 00.
    /// Each two bits will give the color to use like so:
    /// 00: White
    /// 01: Light Grey
    /// 10: Dark Grey
    /// 11: Black
    /// </summary>
    public byte BGP
    {
        get => _mmu[0xFF47];
        set => _mmu[0xFF47] = value;
    }

    public byte OBP0
    {
        get => _mmu[0xFF48];
        set => _mmu[0xFF48] = value;
    }

    public byte OBP1
    {
        get => _mmu[0xFF49];
        set => _mmu[0xFF49] = value;
    }

    /// <summary>
    /// The Y Position of the VIEWING AREA to
    /// start drawing the window from
    /// </summary>
    public byte WY
    {
        get => _mmu[0xFF4A];
        set => _mmu[0xFF4A] = value;
    }

    /// <summary>
    /// The X Positions -7 of the VIEWING AREA
    /// to start drawing the window from
    /// </summary>
    public byte WX
    {
        get => _mmu[0xFF4B];
        set => _mmu[0xFF4B] = value;
    }

    public VPU(MMU mmu)
    {
        _mmu = mmu;
        LCDC = 0x91;
        STAT = 0x85;
        BGP = 0xFC;
        OBP0 = 0xFF;
        OBP1 = 0xFF;
    }

    public void Tick()
    {
    }

    override public string ToString()
    {
        // Base address for tile memory LCDC.3 (0 = 9800-9BFF, 1 = 9C00-9FFF)
        var addr = (LCDC & 0b0000_0100) == 0 ? 0x9800 : 0x9C00;

        // For now, just dump the background tile memory
        // The tile memory is 32x32 bytes
        var sb = new StringBuilder();
        for (int r = 0; r < 32; r++)
        {
            for (int c = 0; c < 32; c++)
            {
                sb.Append($"{_mmu[addr + (r * 32) + c]:X2} ");
            }
            sb.AppendLine();
        }
        return sb.ToString();
    }
}
