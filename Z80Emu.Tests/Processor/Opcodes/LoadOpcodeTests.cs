using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class LoadOpcodeTests
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
