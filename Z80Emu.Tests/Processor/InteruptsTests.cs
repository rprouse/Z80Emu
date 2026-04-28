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

    [Test]
    public void Im2_Entry_ReadsVectorFromTable()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode2;
        _emulator.Interupts.IFF1 = true;
        _emulator.CPU.Registers.I = 0x80;
        // Vector table at 0x80NN — entry 0x04 contains 0x1234 (little-endian).
        _emulator.Memory[0x8004] = 0x34;
        _emulator.Memory[0x8005] = 0x12;

        _emulator.Interupts.RaiseInterrupt(0x04);
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x1234);
    }

    [Test]
    public void Im2_Entry_DefaultByteIs0xFF()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode2;
        _emulator.Interupts.IFF1 = true;
        _emulator.CPU.Registers.I = 0x80;
        // No data byte supplied — should read from 0x80FF / 0x8100.
        _emulator.Memory[0x80FF] = 0xCD;
        _emulator.Memory[0x8100] = 0xAB;

        _emulator.Interupts.RaiseInterrupt();
        _emulator.Tick();

        _emulator.CPU.Registers.PC.ShouldBe((word)0xABCD);
    }

    [Test]
    public void MaskableInt_NotServiced_When_IFF1_False()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = false;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NOP");                          // executed memory, not INT
        _emulator.Interupts.IsRequested.ShouldBeTrue();       // latch still set
        _emulator.CPU.Registers.PC.ShouldNotBe((word)0x0038);
    }

    [Test]
    public void MaskableInt_OneShot_DoesNotReFire()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = true;

        _emulator.Interupts.RaiseInterrupt();
        _emulator.Tick();   // services the interrupt
        // Put a NOP at the vector so the next tick has something to fetch.
        _emulator.Memory[0x0038] = 0x00;
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NOP");                          // not another INT
    }

    [Test]
    public void Nmi_NotGatedBy_IFF1()
    {
        _emulator.Interupts.IFF1 = false;

        _emulator.Interupts.RaiseNmi();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NMI");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0066);
    }

    [Test]
    public void Im2_Entry_VectorPointerWrapsAt0xFFFF()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode2;
        _emulator.Interupts.IFF1 = true;
        _emulator.CPU.Registers.I = 0xFF;
        // RequestData = 0xFF -> vectorPtr = 0xFFFF; high-byte read should wrap to 0x0000.
        _emulator.Memory[0xFFFF] = 0x78;
        _emulator.Memory[0x0000] = 0x56;

        _emulator.Interupts.RaiseInterrupt(0xFF);
        _emulator.Tick();

        _emulator.CPU.Registers.PC.ShouldBe((word)0x5678);
    }

    [Test]
    public void EiShadow_DefersInterruptByOneInstruction()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        // Program: EI ; NOP ; NOP
        _emulator.Memory[0x0100] = 0xFB;  // EI
        _emulator.Memory[0x0101] = 0x00;  // NOP
        _emulator.Memory[0x0102] = 0x00;  // NOP

        var op1 = _emulator.Tick();       // executes EI; sets IFF1=IFF2=true, EiPending=true
        op1.Mnemonic.ShouldBe("EI");

        _emulator.Interupts.RaiseInterrupt();

        var op2 = _emulator.Tick();       // EI shadow active — should run NOP, not INT
        op2.Mnemonic.ShouldBe("NOP");
        _emulator.Interupts.IsRequested.ShouldBeTrue();   // still latched

        var op3 = _emulator.Tick();       // shadow expired — should service now
        op3.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
    }

    [Test]
    public void EiShadow_AlsoDefersNmi()
    {
        _emulator.Memory[0x0100] = 0xFB;  // EI
        _emulator.Memory[0x0101] = 0x00;  // NOP

        _emulator.Tick();                 // EI

        _emulator.Interupts.RaiseNmi();

        var op2 = _emulator.Tick();       // EI shadow defers NMI too
        op2.Mnemonic.ShouldBe("NOP");
        _emulator.Interupts.IsNmiRequested.ShouldBeTrue();

        _emulator.Memory[0x0102] = 0x00;
        var op3 = _emulator.Tick();
        op3.Mnemonic.ShouldBe("NMI");
    }

    [Test]
    public void Retn_RestoresIFF1_FromIFF2_AfterNmi()
    {
        // Pre-state: IFF1=IFF2=true (as if EI had run earlier)
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;
        // Place RETN at the NMI vector so the second tick will execute it.
        _emulator.Memory[0x0066] = 0xED;
        _emulator.Memory[0x0067] = 0x45;

        _emulator.Interupts.RaiseNmi();
        _emulator.Tick();   // NMI entry: IFF2 takes IFF1's value (still true), IFF1 cleared
        _emulator.Interupts.IFF1.ShouldBeFalse();
        _emulator.Interupts.IFF2.ShouldBeTrue();

        var op2 = _emulator.Tick();   // executes RETN at 0x0066
        op2.Mnemonic.ShouldBe("RETN");
        _emulator.Interupts.IFF1.ShouldBeTrue();   // restored from IFF2
    }
}
