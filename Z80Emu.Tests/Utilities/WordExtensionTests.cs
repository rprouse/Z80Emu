using Z80Emu.Core;

namespace Z80Emu.Tests.Utilities;

public class WordExtensionTests
{
    [TestCase("0x0000", 0x0000)]
    [TestCase("0xDEAD", 0xDEAD)]
    [TestCase("0xFFFF", 0xFFFF)]
    [TestCase("0000", 0x0000)]
    [TestCase("DEAD", 0xDEAD)]
    [TestCase("FFFF", 0xFFFF)]
    [TestCase("A123", 0xA123)]
    [TestCase("$0000", 0x0000)]
    [TestCase("$DEAD", 0xDEAD)]
    [TestCase("$FFFF", 0xFFFF)]
    public void TestParseHex(string s, int expected)
    {
        var actual = s.ParseHex();
        actual.Should().Be((word)expected);
    }

    [TestCase("0x0000", 0x0000)]
    [TestCase("0xDEAD", 0xDEAD)]
    [TestCase("0xFFFF", 0xFFFF)]
    [TestCase("0000", 0x0000)]
    [TestCase("DEAD", 0xDEAD)]
    [TestCase("FFFF", 0xFFFF)]
    [TestCase("A123", 0xA123)]
    [TestCase("$0000", 0x0000)]
    [TestCase("$DEAD", 0xDEAD)]
    [TestCase("$FFFF", 0xFFFF)]
    public void TestTryParseHex_Success(string s, int expected)
    {
        var result = s.TryParseHex(out var actual);
        result.Should().BeTrue();
        actual.Should().Be((word)expected);
    }

    [TestCase("-1")]
    [TestCase("0xHEAD")]
    [TestCase("HEAD")]
    [TestCase("%0000")]
    public void TestTryParseHex_Failure(string s)
    {
        var result = s.TryParseHex(out var actual);
        result.Should().BeFalse();
    }
}
