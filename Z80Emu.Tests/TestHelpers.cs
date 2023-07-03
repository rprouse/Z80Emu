using Z80Emu.Core.Processor;

namespace Z80Emu.Tests;

public static class TestHelpers
{
    public static void FlagsShouldBe(this Registers reg, bool s, bool z, bool h, bool pv, bool n, bool c)
    {
        reg.FlagS.Should().Be(s);
        reg.FlagZ.Should().Be(z);
        reg.FlagH.Should().Be(h);
        reg.FlagPV.Should().Be(pv);
        reg.FlagN.Should().Be(n);
        reg.FlagC.Should().Be(c);
    }
}
