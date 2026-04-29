using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor;

public class CpuTests
{
    CPU _cpu;
    MMU _mmu;

    [SetUp]
    public void Setup()
    {
        _mmu = new MMU();
        _cpu = new CPU(new Interupts(), _mmu, new Ports());
    }

    [Test]
    public void RegistersAreInitialisedCorrectly()
    {
        _cpu.Registers.AF.ShouldBe(0x0000);
        _cpu.Registers.BC.ShouldBe(0x0000);
        _cpu.Registers.DE.ShouldBe(0x0000);
        _cpu.Registers.HL.ShouldBe(0x0000);
        _cpu.Registers.PC.ShouldBe(0x0100);
        _cpu.Registers.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void TickFetchesExecutesAndReturnsOpcode()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x43;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _cpu.Registers.BC = 0x2A0F;

        Opcode op = _cpu.Tick();

        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe("LD (0x0104),BC");

        _cpu.Registers.PC.ShouldBe(0x0104);

        _mmu[0x0104].ShouldBe(0x0F);
        _mmu[0x0105].ShouldBe(0x2A);
    }

    [Test]
    public void PeekFetchesOpcodeButDoesNotExecuteIt()
    {
        _mmu[0x100] = 0xED;
        _mmu[0x101] = 0x43;
        _mmu[0x102] = 0x04;
        _mmu[0x103] = 0x01;

        _cpu.Registers.BC = 0x2A0F;

        Opcode op = _cpu.PeekInstruction(0x100);

        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe("LD (0x0104),BC");

        _cpu.Registers.PC.ShouldBe(0x0100);

        _mmu[0x0104].ShouldBe(0x00);
    }

    [Test]
    public void ReturnReturns()
    {
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _cpu.Registers.SP = 0x0FFFC;

        _cpu.Return();

        _cpu.Registers.PC.ShouldBe(0x0302);
        _cpu.Registers.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void ToStringReturnsRegisters()
    {
        _cpu.ToString().ShouldBe(_cpu.Registers.ToString());
    }
}
