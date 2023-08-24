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
}
