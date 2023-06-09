using Z80Emu.Core.Processor;

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

    /// <summary>
    /// Get the value of a bit in a byte
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static bool IsBitSet(this byte value, int bit) =>
        (value & (1 << bit)) != 0;

    /// <summary>
    /// Set a bit in a byte
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static byte SetBit(this byte value, int bit) =>
        (byte)(value | (1 << bit));

    /// <summary>
    /// Reset a bit in a byte
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static byte ResetBit(this byte value, int bit) =>
        (byte)(value & ~(1 << bit));

    /// <summary>
    /// Get a bit in the flag register
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <returns></returns>
    public static bool GetFlag(this byte value, Registers.Flag flag) =>
        (value & (byte)flag) != 0;

    /// <summary>
    /// Set a bit in the flag register
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static byte SetFlag(this byte value, Registers.Flag flag) =>
        (byte)(value | (byte)flag);

    /// <summary>
    /// Reset a bit in the flag register
    /// </summary>
    /// <param name="value"></param>
    /// <param name="bit"></param>
    /// <returns></returns>
    public static byte ResetFlag(this byte value, Registers.Flag flag) =>
        (byte)(value & ~(byte)flag);

    /// <summary>
    /// Set or reset a bit in the flag register
    /// </summary>
    /// <param name="value"></param>
    /// <param name="flag"></param>
    /// <param name="set"></param>
    /// <returns></returns>
    public static byte SetFlag(this byte value, Registers.Flag flag, bool set) =>
        set ? value.SetFlag(flag) : value.ResetFlag(flag);
}
