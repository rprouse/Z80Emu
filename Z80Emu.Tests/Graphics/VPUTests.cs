namespace Z80Emu.Core.Graphics.Tests;

[TestFixture()]
public class VPUTests
{
    [Test()]
    public void TestVPU()
    {
        var vpu = new VPU(new Core.Memory.MMU());
        vpu.LCDC.Should().Be(0x91);
        vpu.STAT.Should().Be(0x85);
        vpu.BGP.Should().Be(0xFC);
        vpu.OBP0.Should().Be(0xFF);
        vpu.OBP1.Should().Be(0xFF);
    }
}