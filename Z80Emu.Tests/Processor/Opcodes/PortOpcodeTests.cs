using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class PortOpcodeTests
{
    Registers _reg;
    MMU _mmu;
    Interupts _int;
    OpcodeHandler _opcodeHandler;
    Ports _ports;

    byte _lastPort;
    byte _lastValue;

    [SetUp]
    public void Setup()
    {
        _lastPort = 0;
        _lastValue = 0;
        _reg = new Registers
        {
            A = 0xAA,
            B = 0xBB,
            C = 0xCC,
            D = 0xDD,
            E = 0xEE,
            H = 0x88,
            L = 0x99,
            PC = 0x0100,
        };
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _ports = new Ports();
        _ports[0xCC] = 0x69;
        _ports.OnPortChanged += (sender, args) =>
        {
            _lastPort = args.Port;
            _lastValue = args.Value;
        };
        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int, _ports);
    }

    [Test]
    public void IN_A_n()
    {
        _ports[0x01] = 0x69;
        _mmu[0x0100] = 0xDB;
        _mmu[0x0101] = 0x01;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN A,(0x01)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x69);
    }

    [Test]
    public void IND()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xAA;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IND");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x01FF);
        _mmu[0x0200].Should().Be(0x69);
    }

    [Test]
    public void INDR()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xBA;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("INDR");

        op.Execute();

        _reg.PC.Should().Be(0x0100);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x01FF);
        _mmu[0x0200].Should().Be(0x69);
    }

    [Test]
    public void INI()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xA2;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("INI");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x0201);
        _mmu[0x0200].Should().Be(0x69);
    }

    [Test]
    public void INIR()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xB2;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("INIR");

        op.Execute();

        _reg.PC.Should().Be(0x0100);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x0201);
        _mmu[0x0200].Should().Be(0x69);
    }

    [Test]
    public void IN_A_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x78;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN A,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.A.Should().Be(0x69);
    }

    [Test]
    public void IN_B_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x40;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN B,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x69);
    }

    [Test]
    public void IN_C_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x48;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN C,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.C.Should().Be(0x69);
    }

    [Test]
    public void IN_D_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x50;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN D,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.D.Should().Be(0x69);
    }

    [Test]
    public void IN_E_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x58;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN E,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.E.Should().Be(0x69);
    }

    [Test]
    public void IN_H_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x60;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN H,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.H.Should().Be(0x69);
    }

    [Test]
    public void IN_L_C()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x68;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("IN L,(C)");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.L.Should().Be(0x69);
    }

    [TestCase(0x79, 'A', 0xAA)]
    [TestCase(0x41, 'B', 0xBB)]
    [TestCase(0x49, 'C', 0xCC)]
    [TestCase(0x51, 'D', 0xDD)]
    [TestCase(0x59, 'E', 0xEE)]
    [TestCase(0x61, 'H', 0x88)]
    [TestCase(0x69, 'L', 0x99)]
    public void OUT_C_REG(byte regByte, char reg, byte expected)
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = regByte;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be($"OUT (C),{reg}");

        op.Execute();

        _reg.PC.Should().Be(0x0102);

        _ports[0xCC].Should().Be(expected);
        _lastPort.Should().Be(0xCC);
        _lastValue.Should().Be(expected);
    }

    [Test]
    public void OUT_N_A()
    {
        _mmu[0x0100] = 0xD3;
        _mmu[0x0101] = 0x21;
        _reg.A = 0x69;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("OUT (0x21),A");

        op.Execute();

        _reg.PC.Should().Be(0x0102);

        _ports[0x21].Should().Be(0x69);
        _lastPort.Should().Be(0x21);
        _lastValue.Should().Be(0x69);
    }

    [Test]
    public void OUTD()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xAB;
        _mmu[0x01FF] = 0x52;
        _mmu[0x0200] = 0x51;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("OUTD");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x01FF);
        _ports[0xCC].Should().Be(0x51);
        _lastPort.Should().Be(0xCC);
        _lastValue.Should().Be(0x51);
    }

    [Test]
    public void OTDR()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xBB;
        _mmu[0x01FF] = 0x52;
        _mmu[0x0200] = 0x51;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("OTDR");

        op.Execute();

        _reg.PC.Should().Be(0x0100);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x01FF);
        _ports[0xCC].Should().Be(0x51);
        _lastPort.Should().Be(0xCC);
        _lastValue.Should().Be(0x51);
    }

    [Test]
    public void OUTI()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xA3;
        _mmu[0x0200] = 0x51;
        _mmu[0x0201] = 0x52;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("OUTI");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x0201);
        _ports[0xCC].Should().Be(0x51);
        _lastPort.Should().Be(0xCC);
        _lastValue.Should().Be(0x51);
    }

    [Test]
    public void OTIR()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0xB3;
        _mmu[0x0200] = 0x51;
        _mmu[0x0201] = 0x52;

        _reg.B = 0x02;
        _reg.HL = 0x0200;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("OTIR");

        op.Execute();

        _reg.PC.Should().Be(0x0100);
        _reg.B.Should().Be(0x01);
        _reg.HL.Should().Be(0x0201);
        _ports[0xCC].Should().Be(0x51);
        _lastPort.Should().Be(0xCC);
        _lastValue.Should().Be(0x51);
    }
}
