using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class ControlOpcodeTests
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
        _reg.SP = 0xFFFE;
    }

    [Test]
    public void RET()
    {
        _mmu[0x0100] = 0xC9;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;

        var op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("RET");

        op.Execute();

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
    }
}
