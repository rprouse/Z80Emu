using Z80Emu.Core;

namespace Z80Emu.Tests.Utilities;

public class ByteExtensionTests
{
    [TestCase("0x00", 0x00)]
    [TestCase("0xDE", 0xDE)]
    [TestCase("0xFF", 0xFF)]
    [TestCase("00", 0x00)]
    [TestCase("DE", 0xDE)]
    [TestCase("FF", 0xFF)]
    [TestCase("A1", 0xA1)]
    [TestCase("$00", 0x00)]
    [TestCase("$DE", 0xDE)]
    [TestCase("$FF", 0xFF)]
    public void TestParseHexByte(string s, byte expected)
    {
        var actual = s.ParseHexByte();
        actual.Should().Be(expected);
    }

    [TestCase("0x00", 0x00)]
    [TestCase("0xDE", 0xDE)]
    [TestCase("0xFF", 0xFF)]
    [TestCase("00", 0x00)]
    [TestCase("DE", 0xDE)]
    [TestCase("FF", 0xFF)]
    [TestCase("A1", 0xA1)]
    [TestCase("$00", 0x00)]
    [TestCase("$DE", 0xDE)]
    [TestCase("$FF", 0xFF)]
    public void TestTryParseHexByte_Success(string s, byte expected)
    {
        var result = s.TryParseHexByte(out var actual);
        result.Should().BeTrue();
        actual.Should().Be(expected);
    }

    [TestCase("-1")]
    [TestCase("0xHE")]
    [TestCase("HE")]
    [TestCase("%00")]
    public void TestTryParseHex_Failure(string s)
    {
        var result = s.TryParseHexByte(out var actual);
        result.Should().BeFalse();
    }
}
