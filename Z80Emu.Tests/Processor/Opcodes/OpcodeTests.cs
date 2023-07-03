using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class OpcodeTests
{
    Registers _reg;
    MMU _mmu;
    Interupts _int;
    OpcodeHandler _opcodeHandler;

    [SetUp]
    public void Setup()
    {
        _reg = new Registers();
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int);
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

    [Test]
    public void LD_NN_BC()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x43;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _reg.BC = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),BC");

        op.Execute();

        _reg.PC.Should().Be(0x0104);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_NN_DE()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x53;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _reg.DE = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),DE");

        op.Execute();

        _reg.PC.Should().Be(0x0104);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_NN_HL()
    {
        _mmu[0x100] = 0x22;
        _mmu[0x101] = 0x04;
        _mmu[0x102] = 0x01;

        _reg.HL = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),HL");

        op.Execute();

        _reg.PC.Should().Be(0x0103);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_NN_IX()
    {
        _mmu[0x100] = 0xDD;
        _mmu[0x101] = 0x22;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _reg.IX = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),IX");

        op.Execute();

        _reg.PC.Should().Be(0x0104);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_NN_IY()
    {
        _mmu[0x100] = 0xFD;
        _mmu[0x101] = 0x22;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _reg.IY = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),IY");

        op.Execute();

        _reg.PC.Should().Be(0x0104);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_NN_SP()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x73;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _reg.SP = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x0104),SP");

        op.Execute();

        _reg.PC.Should().Be(0x0104);

        _mmu[0x0104].Should().Be(0x0F);
        _mmu[0x0105].Should().Be(0x2A);
    }

    [Test]
    public void LD_BC_NN_ADDR()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x4B;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD BC,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.BC.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_DE_NN_ADDR()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x5B;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD DE,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.DE.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_HL_NN_ADDR()
    {
        _mmu[0x100] = 0x2A;
        _mmu[0x101] = 0x04;
        _mmu[0x102] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD HL,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0103);
        _reg.HL.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_IX_NN_ADDR()
    {
        _mmu[0x100] = 0xDD;
        _mmu[0x101] = 0x2A;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD IX,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.IX.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_IY_NN_ADDR()
    {
        _mmu[0x100] = 0xFD;
        _mmu[0x101] = 0x2A;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD IY,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.IY.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_IX_NN()
    {
        _mmu[0x100] = 0xDD;
        _mmu[0x101] = 0x21;
        _mmu[0x102] = 0x0F;
        _mmu[0x103] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD IX,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.IX.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_IY_NN()
    {
        _mmu[0x100] = 0xFD;
        _mmu[0x101] = 0x21;
        _mmu[0x102] = 0x0F;
        _mmu[0x103] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD IY,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.IY.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_SP_NN_ADDR()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x7B;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;
        _mmu[0x104] = 0x0F;
        _mmu[0x105] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD SP,(0x0104)");

        op.Execute();

        _reg.PC.Should().Be(0x0104);
        _reg.SP.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_SP_HL()
    {
        _mmu[0x100] = 0xF9;

        _reg.HL = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD SP,HL");

        op.Execute();

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_SP_IX()
    {
        _mmu[0x100] = 0xDD;
        _mmu[0x101] = 0xF9;

        _reg.IX = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD SP,IX");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.SP.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_SP_IY()
    {
        _mmu[0x100] = 0xFD;
        _mmu[0x101] = 0xF9;

        _reg.IY = 0x2A0F;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD SP,IY");

        op.Execute();

        _reg.PC.Should().Be(0x0102);
        _reg.SP.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_BC_NN()
    {
        _mmu[0x100] = 0x01;
        _mmu[0x101] = 0x0F;
        _mmu[0x102] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD BC,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0103);
        _reg.BC.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_DE_NN()
    {
        _mmu[0x100] = 0x11;
        _mmu[0x101] = 0x0F;
        _mmu[0x102] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD DE,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0103);
        _reg.DE.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_HL_NN()
    {
        _mmu[0x100] = 0x21;
        _mmu[0x101] = 0x0F;
        _mmu[0x102] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD HL,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0103);
        _reg.HL.Should().Be(0x2A0F);
    }

    [Test]
    public void LD_SP_NN()
    {
        _mmu[0x100] = 0x31;
        _mmu[0x101] = 0x0F;
        _mmu[0x102] = 0x2A;

        Opcode op = _opcodeHandler.FetchInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD SP,0x2A0F");

        op.Execute();

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0x2A0F);
    }
}
