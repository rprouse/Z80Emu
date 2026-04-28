using Z80Emu.Core.Processor;

namespace Z80Emu.Tests.Processor;

public class RegisterTests
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
        _reg.AF.ShouldBe(0xCA00);
    }

    [Test]
    public void FSetsLowNibbleOfAF()
    {
        _reg.F = 0xCA;
        _reg.AF.ShouldBe(0x00CA);
    }

    [Test]
    public void AFSetsAAndF()
    {
        _reg.AF = 0xDEAD;
        _reg.A.ShouldBe(0xDE);
        _reg.F.ShouldBe(0xAD);
    }

    [Test]
    public void BSetsHighNibbleOfBC()
    {
        _reg.B = 0xCA;
        _reg.BC.ShouldBe(0xCA00);
    }

    [Test]
    public void CSetsLowNibbleOfBC()
    {
        _reg.C = 0xCA;
        _reg.BC.ShouldBe(0x00CA);
    }

    [Test]
    public void BCSetsBAndC()
    {
        _reg.BC = 0xDEAD;
        _reg.B.ShouldBe(0xDE);
        _reg.C.ShouldBe(0xAD);
    }

    [Test]
    public void DSetsHighNibbleOfDE()
    {
        _reg.D = 0xCA;
        _reg.DE.ShouldBe(0xCA00);
    }

    [Test]
    public void ESetsLowNibbleOfDE()
    {
        _reg.E = 0xCA;
        _reg.DE.ShouldBe(0x00CA);
    }

    [Test]
    public void DESetsDAndE()
    {
        _reg.DE = 0xDEAD;
        _reg.D.ShouldBe(0xDE);
        _reg.E.ShouldBe(0xAD);
    }

    [Test]
    public void HSetsHighNibbleOfHL()
    {
        _reg.H = 0xCA;
        _reg.HL.ShouldBe(0xCA00);
    }

    [Test]
    public void LSetsLowNibbleOfHL()
    {
        _reg.L = 0xCA;
        _reg.HL.ShouldBe(0x00CA);
    }

    [Test]
    public void HLSetsHAndL()
    {
        _reg.HL = 0xDEAD;
        _reg.H.ShouldBe(0xDE);
        _reg.L.ShouldBe(0xAD);
    }

    [Test]
    public void TestSetUnsetFlagS()
    {
        _reg.F = 0b0111_1111;
        _reg.FlagS = true;
        _reg.FlagS.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagS = false;
        _reg.FlagS.ShouldBe(false);
        _reg.F.ShouldBe(0b0111_1111);
    }

    [Test]
    public void TestSetUnsetFlagZ()
    {
        _reg.F = 0b1011_1111;
        _reg.FlagZ = true;
        _reg.FlagZ.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagZ = false;
        _reg.FlagZ.ShouldBe(false);
        _reg.F.ShouldBe(0b1011_1111);
    }

    [Test]
    public void TestSetUnsetFlagH()
    {
        _reg.F = 0b1110_1111;
        _reg.FlagH = true;
        _reg.FlagH.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagH = false;
        _reg.FlagH.ShouldBe(false);
        _reg.F.ShouldBe(0b1110_1111);
    }

    [Test]
    public void TestSetUnsetFlagPV()
    {
        _reg.F = 0b1111_1011;
        _reg.FlagPV = true;
        _reg.FlagPV.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagPV = false;
        _reg.FlagPV.ShouldBe(false);
        _reg.F.ShouldBe(0b1111_1011);
    }

    [Test]
    public void TestSetUnsetFlagN()
    {
        _reg.F = 0b1111_1101;
        _reg.FlagN = true;
        _reg.FlagN.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagN = false;
        _reg.FlagN.ShouldBe(false);
        _reg.F.ShouldBe(0b1111_1101);
    }

    [Test]
    public void TestSetUnsetFlagC()
    {
        _reg.F = 0b1111_1110;
        _reg.FlagC = true;
        _reg.FlagC.ShouldBe(true);
        _reg.F.ShouldBe(0b1111_1111);
        _reg.FlagC = false;
        _reg.FlagC.ShouldBe(false);
        _reg.F.ShouldBe(0b1111_1110);
    }

    [Test]
    public void A_SetsHighNibbleOfAF_()
    {
        _reg.A_ = 0xCA;
        _reg.AF_.ShouldBe(0xCA00);
    }

    [Test]
    public void F_SetsLowNibbleOfAF_()
    {
        _reg.F_ = 0xCA;
        _reg.AF_.ShouldBe(0x00CA);
    }

    [Test]
    public void AF_SetsA_AndF_()
    {
        _reg.AF_ = 0xDEAD;
        _reg.A_.ShouldBe(0xDE);
        _reg.F_.ShouldBe(0xAD);
    }

    [Test]
    public void B_SetsHighNibbleOfBC_()
    {
        _reg.B_ = 0xCA;
        _reg.BC_.ShouldBe(0xCA00);
    }

    [Test]
    public void C_SetsLowNibbleOfBC_()
    {
        _reg.C_ = 0xCA;
        _reg.BC_.ShouldBe(0x00CA);
    }

    [Test]
    public void BC_SetsBAndC_()
    {
        _reg.BC_ = 0xDEAD;
        _reg.B_.ShouldBe(0xDE);
        _reg.C_.ShouldBe(0xAD);
    }

    [Test]
    public void D_SetsHighNibbleOfDE_()
    {
        _reg.D_ = 0xCA;
        _reg.DE_.ShouldBe(0xCA00);
    }

    [Test]
    public void E_SetsLowNibbleOfDE_()
    {
        _reg.E_ = 0xCA;
        _reg.DE_.ShouldBe(0x00CA);
    }

    [Test]
    public void DE_SetsDAndE_()
    {
        _reg.DE_ = 0xDEAD;
        _reg.D_.ShouldBe(0xDE);
        _reg.E_.ShouldBe(0xAD);
    }

    [Test]
    public void H_SetsHighNibbleOfHL_()
    {
        _reg.H_ = 0xCA;
        _reg.HL_.ShouldBe(0xCA00);
    }

    [Test]
    public void L_SetsLowNibbleOfHL_()
    {
        _reg.L_ = 0xCA;
        _reg.HL_.ShouldBe(0x00CA);
    }

    [Test]
    public void HL_SetsH_AndL_()
    {
        _reg.HL_ = 0xDEAD;
        _reg.H_.ShouldBe(0xDE);
        _reg.L_.ShouldBe(0xAD);
    }

    [Test]
    public void GetSetI()
    {
        _reg.I = 0xDE;
        _reg.I.ShouldBe(0xDE);
    }

    [Test]
    public void GetSetR()
    {
        _reg.R = 0xDE;
        _reg.R.ShouldBe(0xDE);
    }
}