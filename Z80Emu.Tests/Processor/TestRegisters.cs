using Z80Emu.Core.Processor;

namespace Z80Emu.Tests.Processor;

public class TestRegisters
{
    Registers _reg;

    [SetUp]
    public void Setup()
    {
        _reg = new Registers();
    }

    [Test]
    public void ASetsHighNibbleOfAF()
    {
        _reg.A = 0xCA;
        _reg.AF.Should().Be(0xCA00);
    }

    [Test]
    public void FSetsLowNibbleOfAF()
    {
        _reg.F = 0xCA;
        _reg.AF.Should().Be(0x00CA);
    }

    [Test]
    public void AFSetsAAndF()
    {
        _reg.AF = 0xDEAD;
        _reg.A.Should().Be(0xDE);
        _reg.F.Should().Be(0xAD);
    }

    [Test]
    public void BSetsHighNibbleOfBC()
    {
        _reg.B = 0xCA;
        _reg.BC.Should().Be(0xCA00);
    }

    [Test]
    public void CSetsLowNibbleOfBC()
    {
        _reg.C = 0xCA;
        _reg.BC.Should().Be(0x00CA);
    }

    [Test]
    public void BCSetsBAndC()
    {
        _reg.BC = 0xDEAD;
        _reg.B.Should().Be(0xDE);
        _reg.C.Should().Be(0xAD);
    }

    [Test]
    public void DSetsHighNibbleOfDE()
    {
        _reg.D = 0xCA;
        _reg.DE.Should().Be(0xCA00);
    }

    [Test]
    public void ESetsLowNibbleOfDE()
    {
        _reg.E = 0xCA;
        _reg.DE.Should().Be(0x00CA);
    }

    [Test]
    public void DESetsDAndE()
    {
        _reg.DE = 0xDEAD;
        _reg.D.Should().Be(0xDE);
        _reg.E.Should().Be(0xAD);
    }

    [Test]
    public void HSetsHighNibbleOfHL()
    {
        _reg.H = 0xCA;
        _reg.HL.Should().Be(0xCA00);
    }

    [Test]
    public void LSetsLowNibbleOfHL()
    {
        _reg.L = 0xCA;
        _reg.HL.Should().Be(0x00CA);
    }

    [Test]
    public void HLSetsHAndL()
    {
        _reg.HL = 0xDEAD;
        _reg.H.Should().Be(0xDE);
        _reg.L.Should().Be(0xAD);
    }

    [Test]
    public void TestSetUnsetFlagS()
    {
        _reg.F = 0b0111_1111;
        _reg.FlagS = true;
        _reg.FlagS.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagS = false;
        _reg.FlagS.Should().Be(false);
        _reg.F.Should().Be(0b0111_1111);
    }

    [Test]
    public void TestSetUnsetFlagZ()
    {
        _reg.F = 0b1011_1111;
        _reg.FlagZ = true;
        _reg.FlagZ.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagZ = false;
        _reg.FlagZ.Should().Be(false);
        _reg.F.Should().Be(0b1011_1111);
    }

    [Test]
    public void TestSetUnsetFlagH()
    {
        _reg.F = 0b1110_1111;
        _reg.FlagH = true;
        _reg.FlagH.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagH = false;
        _reg.FlagH.Should().Be(false);
        _reg.F.Should().Be(0b1110_1111);
    }

    [Test]
    public void TestSetUnsetFlagPV()
    {
        _reg.F = 0b1111_1011;
        _reg.FlagPV = true;
        _reg.FlagPV.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagPV = false;
        _reg.FlagPV.Should().Be(false);
        _reg.F.Should().Be(0b1111_1011);
    }

    [Test]
    public void TestSetUnsetFlagN()
    {
        _reg.F = 0b1111_1101;
        _reg.FlagN = true;
        _reg.FlagN.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagN = false;
        _reg.FlagN.Should().Be(false);
        _reg.F.Should().Be(0b1111_1101);
    }

    [Test]
    public void TestSetUnsetFlagC()
    {
        _reg.F = 0b1111_1110;
        _reg.FlagC = true;
        _reg.FlagC.Should().Be(true);
        _reg.F.Should().Be(0b1111_1111);
        _reg.FlagC = false;
        _reg.FlagC.Should().Be(false);
        _reg.F.Should().Be(0b1111_1110);
    }
}