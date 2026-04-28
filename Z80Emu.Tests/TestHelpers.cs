using Z80Emu.Core.Processor;

namespace Z80Emu.Tests;

public static class TestHelpers
{
    public static void FlagsShouldBe(this Registers reg, bool s, bool z, bool h, bool pv, bool n, bool c)
    {
        reg.FlagS.ShouldBe(s);
        reg.FlagZ.ShouldBe(z);
        reg.FlagH.ShouldBe(h);
        reg.FlagPV.ShouldBe(pv);
        reg.FlagN.ShouldBe(n);
        reg.FlagC.ShouldBe(c);
    }
}
