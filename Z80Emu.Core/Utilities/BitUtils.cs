namespace Z80Emu.Core.Utilities;

public static class BitUtils
{
    /// <summary>
    /// Least significant bit
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte Lsb(this word value) =>
        (byte)(value & 0x00FF);

    /// <summary>
    /// Most significant bit
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static byte Msb(this word value) =>
        (byte)((value >> 8) & 0x00FF);

    /// <summary>
    /// Combine an MSB and an LSB into a Word
    /// </summary>
    /// <param name="msb"></param>
    /// <param name="lsb"></param>
    /// <returns></returns>
    public static word ToWord(byte msb, byte lsb) =>
        (word)(msb << 8 | lsb);
}
