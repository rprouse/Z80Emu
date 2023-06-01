using Z80Emu.Core.Utilities;

namespace Z80Emu.Tests.Utilities;

public class TestBitUtils
{
    [Test]
    public void TestLsb()
    {
        word b = 0xABCD;
        b.Lsb().Should().Be(0xCD);
    }

    [Test]
    public void TestMsb()
    {
        word b = 0xABCD;
        b.Msb().Should().Be(0xAB);
    }

    [Test]
    public void TestToWord()
    {
        BitUtils.ToWord(0xAB, 0xCD).Should().Be(0xABCD);
    }
}
