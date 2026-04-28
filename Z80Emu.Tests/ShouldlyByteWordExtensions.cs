namespace Z80Emu.Tests;

// Shouldly ships ShouldBe overloads for int/long/decimal/etc. but not for byte or ushort,
// so byteValue.ShouldBe(0x3A) fails to resolve (the int literal pulls type inference toward
// the int overload, which then rejects the byte/ushort receiver). These shims forward to the
// int overload so all 8/16-bit register and memory assertions work without per-call casts.
internal static class ShouldlyByteWordExtensions
{
    public static void ShouldBe(this byte actual, int expected, string customMessage = null)
        => ((int)actual).ShouldBe(expected, customMessage);

    public static void ShouldBe(this ushort actual, int expected, string customMessage = null)
        => ((int)actual).ShouldBe(expected, customMessage);
}
