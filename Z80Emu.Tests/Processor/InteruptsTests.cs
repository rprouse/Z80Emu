using Z80Emu.Core;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Utilities;
using Z80Emu.Tests;  // MockOperatingSytem lives in the parent namespace

namespace Z80Emu.Tests.Processor;

public class InteruptsTests
{
    Emulator _emulator;

    [SetUp]
    public void Setup()
    {
        _emulator = new Emulator(new MockOperatingSytem());
        // Put a NOP at PC so any normal Tick advances by one and stays in range.
        _emulator.Memory[0x0100] = 0x00;
        _emulator.Memory[0x0101] = 0x00;
        _emulator.Memory[0x0102] = 0x00;
    }

    [Test]
    public void Nmi_PushesPC_JumpsTo0x0066_ClearsLatch()
    {
        word originalPC = _emulator.CPU.Registers.PC;
        word originalSP = _emulator.CPU.Registers.SP;

        _emulator.Interupts.RaiseNmi();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NMI");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0066);
        _emulator.CPU.Registers.SP.ShouldBe((word)(originalSP - 2));
        _emulator.Memory[(word)(originalSP - 2)].ShouldBe(originalPC.Lsb());
        _emulator.Memory[(word)(originalSP - 1)].ShouldBe(originalPC.Msb());
        _emulator.Interupts.IsNmiRequested.ShouldBeFalse();
    }

    [Test]
    public void Nmi_CopiesIFF1_ToIFF2_AndClearsIFF1()
    {
        // Artificial pre-state: IFF1=1, IFF2=0 (simulates a nested NMI scenario)
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = false;

        _emulator.Interupts.RaiseNmi();
        _emulator.Tick();

        _emulator.Interupts.IFF2.ShouldBeTrue();   // took IFF1's value
        _emulator.Interupts.IFF1.ShouldBeFalse();  // cleared
    }

    [Test]
    public void Im1_Entry_PushesPC_JumpsTo0x0038_ClearsIffAndLatch()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;
        word originalPC = _emulator.CPU.Registers.PC;
        word originalSP = _emulator.CPU.Registers.SP;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
        _emulator.CPU.Registers.SP.ShouldBe((word)(originalSP - 2));
        _emulator.Memory[(word)(originalSP - 2)].ShouldBe(originalPC.Lsb());
        _emulator.Memory[(word)(originalSP - 1)].ShouldBe(originalPC.Msb());
        _emulator.Interupts.IFF1.ShouldBeFalse();
        _emulator.Interupts.IFF2.ShouldBeFalse();
        _emulator.Interupts.IsRequested.ShouldBeFalse();
    }

    [Test]
    public void Im0_Entry_BehavesLikeRst38h()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode0;
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
        _emulator.Interupts.IFF1.ShouldBeFalse();
    }
}
