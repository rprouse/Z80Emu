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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},B");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_C([Range(0, 7)] byte bit)
    {
        _reg.C = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x41 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},C");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_D([Range(0, 7)] byte bit)
    {
        _reg.D = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x42 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},D");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_E([Range(0, 7)] byte bit)
    {
        _reg.E = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x43 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},E");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_H([Range(0, 7)] byte bit)
    {
        _reg.H = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x44 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},H");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_L([Range(0, 7)] byte bit)
    {
        _reg.L = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x45 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},L");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void BIT_A([Range(0, 7)] byte bit)
    {
        _reg.A = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x47 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},(HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},(IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"BIT {bit},(IY+4)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.FlagZ.Should().BeFalse();
    }

    [Test]
    public void RES_B([Range(0, 7)] byte bit)
    {
        _reg.B = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x80 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},B");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x00);
    }

    [Test]
    public void RES_C([Range(0, 7)] byte bit)
    {
        _reg.C = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x81 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},C");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0x00);
    }

    [Test]
    public void RES_D([Range(0, 7)] byte bit)
    {
        _reg.D = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x82 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},D");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0x00);
    }

    [Test]
    public void RES_E([Range(0, 7)] byte bit)
    {
        _reg.E = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x83 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},E");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0x00);
    }

    [Test]
    public void RES_H([Range(0, 7)] byte bit)
    {
        _reg.H = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x84 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},H");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0x00);
    }

    [Test]
    public void RES_L([Range(0, 7)] byte bit)
    {
        _reg.L = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x85 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},L");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0x00);
    }

    [Test]
    public void RES_A([Range(0, 7)] byte bit)
    {
        _reg.A = (byte)(0b0000_0001 << bit);
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0x87 + bit * 0x08);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},(HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},(IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"RES {bit},(IY+4)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0x00);
    }

    [Test]
    public void SET_B([Range(0, 7)] byte bit)
    {
        _reg.B = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC0 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},B");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_C([Range(0, 7)] byte bit)
    {
        _reg.C = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC1 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},C");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_D([Range(0, 7)] byte bit)
    {
        _reg.D = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC2 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},D");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_E([Range(0, 7)] byte bit)
    {
        _reg.E = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC3 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},E");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_H([Range(0, 7)] byte bit)
    {
        _reg.H = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC4 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},H");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_L([Range(0, 7)] byte bit)
    {
        _reg.L = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC5 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},L");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void SET_A([Range(0, 7)] byte bit)
    {
        _reg.A = 0b0000_0000;
        _mmu[0x0100] = 0xCB;
        _mmu[0x0101] = (byte)(0xC7 + bit * 8);

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},(HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},(IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"SET {bit},(IY+4)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be((byte)(0b0000_0001 << bit));
    }

    [Test]
    public void RLA()
    {
        _reg.FlagC = true;
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0x17;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLA");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL B");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL C");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL D");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL E");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL H");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL L");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL (HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL (IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RL (IY+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRA");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR B");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR C");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR D");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR E");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR H");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR L");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR (HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR (IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RR (IY+4)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b1000_0000);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RLCA()
    {
        _reg.A = 0b1000_0000;
        _mmu[0x0100] = 0x07;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLCA");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC B");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC C");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC D");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC E");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC H");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC L");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC (HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC (IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLC (IY+4)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _mmu[0x0204].Should().Be(0b0000_0001);
        _reg.FlagC.Should().BeTrue();
    }

    [Test]
    public void RRCA()
    {
        _reg.A = 0b0000_0001;
        _mmu[0x0100] = 0x0F;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRCA");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC A");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC B");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC C");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC D");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC E");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC H");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC L");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC (HL)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC (IX+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRC (IY+4)");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RRD");

        op.Execute();

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

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RLD");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x71);
        _mmu[0x0200].Should().Be(0x2F);
        _reg.FlagZ.Should().BeFalse();
        _reg.FlagS.Should().BeFalse();
    }
}
