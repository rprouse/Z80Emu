using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class ExchangeOpcodes
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
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _ports = new Ports();
        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int, _ports);
        _reg.PC = 0x0100;
        _reg.SP = 0xFFFD;
        _mmu[0xFFFD] = 0xE2;
        _mmu[0xFFFE] = 0x3A;
    }

    [Test]
    public void EX_SP_HL()
    {
        _reg.HL = 0x21FA;
        _mmu[0x0100] = 0xE3;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EX (SP),HL");

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFD);
        _mmu[0xFFFD].Should().Be(0x21);
        _mmu[0xFFFE].Should().Be(0xFA);
        _reg.HL.Should().Be(0x3AE2);
    }

    [Test]
    public void EX_SP_IX()
    {
        _reg.IX = 0x21FA;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xE3;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EX (SP),IX");

        _reg.PC.Should().Be(0x0102);
        _reg.SP.Should().Be(0xFFFD);
        _mmu[0xFFFD].Should().Be(0x21);
        _mmu[0xFFFE].Should().Be(0xFA);
        _reg.IX.Should().Be(0x3AE2);
    }

    [Test]
    public void EX_SP_IY()
    {
        _reg.IY = 0x21FA;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xE3;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EX (SP),IY");

        _reg.PC.Should().Be(0x0102);
        _reg.SP.Should().Be(0xFFFD);
        _mmu[0xFFFD].Should().Be(0x21);
        _mmu[0xFFFE].Should().Be(0xFA);
        _reg.IY.Should().Be(0x3AE2);
    }

    [Test]
    public void EX_AF_AF_()
    {
        _reg.AF = 0x1234;
        _reg.AF_ = 0x5678;
        _mmu[0x0100] = 0x08;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EX AF,AF'");

        _reg.PC.Should().Be(0x0101);
        _reg.AF.Should().Be(0x5678);
        _reg.AF_.Should().Be(0x1234);
    }

    [Test]
    public void EX_DE_HL()
    {
        _reg.DE = 0x1234;
        _reg.HL = 0x5678;
        _mmu[0x0100] = 0xEB;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EX DE,HL");

        _reg.PC.Should().Be(0x0101);
        _reg.DE.Should().Be(0x5678);
        _reg.HL.Should().Be(0x1234);
    }

    [Test]
    public void EXX()
    {
        _reg.BC = 0x1234;
        _reg.DE = 0x5678;
        _reg.HL = 0x9ABC;
        _reg.BC_ = 0x4321;
        _reg.DE_ = 0x8765;
        _reg.HL_ = 0xCBA9;
        _mmu[0x0100] = 0xD9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EXX");

        _reg.PC.Should().Be(0x0101);
        _reg.BC.Should().Be(0x4321);
        _reg.DE.Should().Be(0x8765);
        _reg.HL.Should().Be(0xCBA9);
        _reg.BC_.Should().Be(0x1234);
        _reg.DE_.Should().Be(0x5678);
        _reg.HL_.Should().Be(0x9ABC);
    }
}
