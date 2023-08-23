using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class ArithmeticOpcodeTests
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
    }

    [TestCase(0x00, 0x00, 0x00, false, true, false, false, false)]
    [TestCase(0x09, 0x65, 0x6E, false, false, false, false, false)]
    [TestCase(0x59, 0x85, 0xDE, true, false, false, false, false)]
    [TestCase(0x6E, 0x0E, 0x7C, false, false, true, false, false)]
    [TestCase(0x6E, 0x0E, 0x7C, false, false, true, false, false)]
    [TestCase(0xCD, 0xBA, 0x87, true, false, true, false, true)]
    public void ADD_A_HL(byte a, byte b, byte ex, bool s, bool z, bool h, bool pv, bool c)
    {
        _mmu[0x0100] = 0x86;    // add (hl)

        _reg.A = a;
        _reg.HL = 0x0114;
        _mmu[0x0114] = b;
        _reg.FlagC = true;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("ADD A,(HL)");

        _reg.PC.Should().Be(0x0101);

        op.Execute();

        _reg.A.Should().Be(ex);
        _reg.FlagsShouldBe(s, z, h, pv, false, c);
    }

    [TestCase(0x00, 0x00, false, 0x00, false, true, false, false, false)]
    [TestCase(0x09, 0x65, false, 0x6E, false, false, false, false, false)]
    [TestCase(0x59, 0x85, false, 0xDE, true, false, false, false, false)]
    [TestCase(0x6E, 0x0E, false, 0x7C, false, false, true, false, false)]
    [TestCase(0x6E, 0x0E, false, 0x7C, false, false, true, false, false)]
    [TestCase(0xCD, 0xBA, false, 0x87, true, false, true, false, true)]
    [TestCase(0x00, 0x00, true, 0x01, false, false, false, false, false)]
    [TestCase(0x09, 0x65, true, 0x6F, false, false, false, false, false)]
    [TestCase(0x59, 0x85, true, 0xDF, true, false, false, false, false)]
    [TestCase(0x6E, 0x0E, true, 0x7D, false, false, true, false, false)]
    [TestCase(0x6E, 0x0E, true, 0x7D, false, false, true, false, false)]
    [TestCase(0xCD, 0xBA, true, 0x88, true, false, true, false, true)]
    public void ADC_A_HL(byte a, byte b, bool carry, byte ex, bool s, bool z, bool h, bool pv, bool c)
    {
        _mmu[0x0100] = 0x8E;    // adc (hl)

        _reg.A = a;
        _reg.HL = 0x0114;
        _mmu[0x0114] = b;
        _reg.FlagC = carry;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("ADC A,(HL)");

        _reg.PC.Should().Be(0x0101);

        op.Execute();

        _reg.A.Should().Be(ex);
        _reg.FlagsShouldBe(s, z, h, pv, false, c);
    }
}
