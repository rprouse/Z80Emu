using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class BitOpcodeTests
{
    Registers _reg;
    MMU _mmu;
    Interupts _int;
    OpcodeHandler _opcodeHandler;
    Ports _ports;

    [SetUp]
    public void Setup()
    {
        _reg = new Registers();
        _reg.PC = 0x0100;
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _ports = new Ports();
        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int, _ports);
    }

    [Test]
    public void BIT_B([Range(0, 7)]byte bit)
    {
        _reg.B = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x40 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},B");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_C([Range(0, 7)] byte bit)
    {
        _reg.C = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x41 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},C");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_D([Range(0, 7)] byte bit)
    {
        _reg.D = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x42 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},D");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_E([Range(0, 7)] byte bit)
    {
        _reg.E = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x43 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},E");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_H([Range(0, 7)] byte bit)
    {
        _reg.H = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x44 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},H");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_L([Range(0, 7)] byte bit)
    {
        _reg.L = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x45 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},L");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_A([Range(0, 7)] byte bit)
    {
        _reg.A = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x47 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},A");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_HL([Range(0, 7)] byte bit)
    {
        _reg.HL = 0x0200;
        _mmu[0x0200] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x46 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},(HL)");

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_IX([Range(0, 7)] byte bit)
    {
        _reg.IX = 0x0200;
        _mmu[0x0204] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0x46 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},(IX+4)");

        _reg.PC.Should().Be(0x0104);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_IY([Range(0, 7)] byte bit)
    {
        _reg.IY = 0x0200;
        _mmu[0x0204] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0x46 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"BIT {bit},(IY+4)");

        _reg.PC.Should().Be(0x0104);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RES_B([Range(0, 7)] byte bit)
    {
        _reg.B = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x80 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x00);
    }

    [Test]
    public void RES_C([Range(0, 7)] byte bit)
    {
        _reg.C = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x81 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0x00);
    }

    [Test]
    public void RES_D([Range(0, 7)] byte bit)
    {
        _reg.D = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x82 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0x00);
    }

    [Test]
    public void RES_E([Range(0, 7)] byte bit)
    {
        _reg.E = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x83 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0x00);
    }

    [Test]
    public void RES_H([Range(0, 7)] byte bit)
    {
        _reg.H = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x84 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0x00);
    }

    [Test]
    public void RES_L([Range(0, 7)] byte bit)
    {
        _reg.L = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x85 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0x00);
    }

    [Test]
    public void RES_A([Range(0, 7)] byte bit)
    {
        _reg.A = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x87 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x00);
    }

    [Test]
    public void RES_HL([Range(0, 7)] byte bit)
    {
        _reg.HL = 0x0200;
        _mmu[0x0200] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x86 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},(HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be(0x00);
    }

    [Test]
    public void RES_IX([Range(0, 7)] byte bit)
    {
        _reg.IX = 0x0200;
        _mmu[0x0204] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0x86 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},(IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0x00);
    }

    [Test]
    public void RES_IY([Range(0, 7)] byte bit)
    {
        _reg.IY = 0x0200;
        _mmu[0x0204] = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0x86 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"RES {bit},(IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0x00);
    }

    [Test]
    public void SET_B([Range(0, 7)] byte bit)
    {
        _reg.B = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC0 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_C([Range(0, 7)] byte bit)
    {
        _reg.C = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC1 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_D([Range(0, 7)] byte bit)
    {
        _reg.D = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC2 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_E([Range(0, 7)] byte bit)
    {
        _reg.E = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC3 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_H([Range(0, 7)] byte bit)
    {
        _reg.H = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC4 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_L([Range(0, 7)] byte bit)
    {
        _reg.L = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC5 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_A([Range(0, 7)] byte bit)
    {
        _reg.A = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC7 + bit * 8);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_HL([Range(0, 7)] byte bit)
    {
        _reg.HL = 0x0200;
        _mmu[0x0200] = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC6 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},(HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_IX([Range(0, 7)] byte bit)
    {
        _reg.IX = 0x0200;
        _mmu[0x0204] = 0b0000_0000;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0xC6 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},(IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_IY([Range(0, 7)] byte bit)
    {
        _reg.IY = 0x0200;
        _mmu[0x0204] = 0b0000_0000;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = (byte)(0xC6 + bit * 0x08);

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"SET {bit},(IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void RLA()
    {
        _reg.FlagC = true;
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0x17;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLA");

        _reg.PC.Should().Be(0x0101);
        _reg.A.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RL_A()
    {
        _reg.FlagC = true;
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x17;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_B()
    {
        _reg.FlagC = true;
        _reg.B = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x10;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_C()
    {
        _reg.FlagC = true;
        _reg.C = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x11;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_D()
    {
        _reg.FlagC = true;
        _reg.D = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x12;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_E()
    {
        _reg.FlagC = true;
        _reg.E = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x13;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_H()
    {
        _reg.FlagC = true;
        _reg.H = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x14;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_L()
    {
        _reg.FlagC = true;
        _reg.L = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x15;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_HL()
    {
        _reg.FlagC = true;
        _reg.HL = 0x0200;
        _mmu[0x0200] = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x16;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL (HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_IX()
    {
        _reg.FlagC = true;
        _reg.IX = 0x0200;
        _mmu[0x0204] = 0b1000_0000;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x16;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL (IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RL_IY()
    {
        _reg.FlagC = true;
        _reg.IY = 0x0200;
        _mmu[0x0204] = 0b1000_0000;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x16;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RL (IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRA()
    {
        _reg.FlagC = true;
        _reg.A = 0b0000_0001;
        _mmu[0x0100] = 0x1F;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRA");

        _reg.PC.Should().Be(0x0101);
        _reg.A.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RR_A()
    {
        _reg.FlagC = true;
        _reg.A = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1F;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_B()
    {
        _reg.FlagC = true;
        _reg.B = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x18;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_C()
    {
        _reg.FlagC = true;
        _reg.C = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x19;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_D()
    {
        _reg.FlagC = true;
        _reg.D = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1A;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_E()
    {
        _reg.FlagC = true;
        _reg.E = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1B;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_H()
    {
        _reg.FlagC = true;
        _reg.H = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1C;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_L()
    {
        _reg.FlagC = true;
        _reg.L = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1D;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_HL()
    {
        _reg.FlagC = true;
        _reg.HL = 0x0200;
        _mmu[0x0200] = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x1E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR (HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_IX()
    {
        _reg.FlagC = true;
        _reg.IX = 0x0200;
        _mmu[0x0204] = 0b0000_0001;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x1E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR (IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RR_IY()
    {
        _reg.FlagC = true;
        _reg.IY = 0x0200;
        _mmu[0x0204] = 0b0000_0001;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x1E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RR (IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLCA()
    {
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0x07;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLCA");

        _reg.PC.Should().Be(0x0101);
        _reg.A.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RLC_A()
    {
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x07;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_B()
    {
        _reg.B = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x00;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_C()
    {
        _reg.C = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x01;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_D()
    {
        _reg.D = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x02;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_E()
    {
        _reg.E = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x03;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_H()
    {
        _reg.H = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x04;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_L()
    {
        _reg.L = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x05;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_HL()
    {
        _reg.HL = 0x0200;
        _mmu[0x0200] = 0b1000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x06;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC (HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_IX()
    {
        _reg.IX = 0x0200;
        _mmu[0x0204] = 0b1000_0000;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x06;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC (IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLC_IY()
    {
        _reg.IY = 0x0200;
        _mmu[0x0204] = 0b1000_0000;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x06;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLC (IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRCA()
    {
        _reg.A = 0b0000_0001;
        _mmu[0x0100] = 0x0F;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRCA");

        _reg.PC.Should().Be(0x0101);
        _reg.A.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RRC_A()
    {
        _reg.A = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0F;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC A");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_B()
    {
        _reg.B = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x08;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC B");

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_C()
    {
        _reg.C = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x09;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC C");

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_D()
    {
        _reg.D = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0A;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC D");

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_E()
    {
        _reg.E = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0B;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC E");

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_H()
    {
        _reg.H = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0C;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC H");

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_L()
    {
        _reg.L = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0D;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC L");

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_HL()
    {
        _reg.HL = 0x0200;
        _mmu[0x0200] = 0b0000_0001;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = 0x0E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC (HL)");

        _reg.PC.Should().Be(0x0102);
        _mmu[0x0200].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_IX()
    {
        _reg.IX = 0x0200;
        _mmu[0x0204] = 0b0000_0001;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x0E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC (IX+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRC_IY()
    {
        _reg.IY = 0x0200;
        _mmu[0x0204] = 0b0000_0001;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xCB;
        _mmu[0x0102] = 0x04;
        _mmu[0x0103] = 0x0E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRC (IY+4)");

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    // The contents of the low-order nibble of (HL) are copied to the
    // low-order nibble of A. The previous contents are copied to the
    // high-order nibble of (HL). The previous contents are copied to
    // the low-order nibble of (HL).
    // A = 0x7F, (HL) = 0x12 => A = 0x72, (HL) = 0xF1
    [Test]
    public void RRD()
    {
        _reg.HL = 0x0200;
        _reg.A = 0x7F;
        _mmu[0x0200] = 0x12;
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x67;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RRD");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x72);
        _mmu[0x0200].Should().Be(0xF1);
        _reg.FlagZ.Should().BeFalse();
        _reg.FlagS.Should().BeFalse();
    }

    // The contents of the low-order nibble of (HL) are copied to the
    // high-order nibble of (HL). The previous contents are copied to
    // the low-order nibble of A. The previous contents are copied to
    // the low-order nibble of (HL).
    // A = 0x7F, (HL) = 0x12 => A = 0x71, (HL) = 0x2F
    [Test]
    public void RLD()
    {
        _reg.HL = 0x0200;
        _reg.A = 0x7F;
        _mmu[0x0200] = 0x12;
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x6F;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RLD");

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x71);
        _mmu[0x0200].Should().Be(0x2F);
        _reg.FlagZ.Should().BeFalse();
        _reg.FlagS.Should().BeFalse();
    }
}
