using Z80Emu.Core;
using Z80Emu.Core.OS;

namespace Z80Emu.Tests;

internal class MockOperatingSytem : IDos
{
    public bool ExecuteCalled { get; set; } = false;
    public bool InitializeCalled { get; set; } = false;

    public string Name => "MockOperatingSytem";

    public IEnumerable<ushort> CallVectors => new word[] { 0x0005 };

    public void Execute(Emulator emulator)
    {
        ExecuteCalled = true;
    }

    public void Initialize(Emulator emulator)
    {
        InitializeCalled = true;
    }
}

public class EmulatorTests
{
    Emulator _emulator;
    MockOperatingSytem _os;

    [SetUp]
    public void Setup()
    {
        _os = new MockOperatingSytem();
        _emulator = new Emulator(_os);
    }

    [Test]
    public void SettingPortFiresEvent()
    {
        bool eventFired = false;
        _emulator.OnPortChanged += (sender, args) =>
        {
            eventFired = true;
            args.Port.ShouldBe(0x01);
            args.Value.ShouldBe(0xFF);
        };

        _emulator.Ports[0x01] = 0xFF;

        eventFired.ShouldBeTrue();
    }

    [Test]
    public void SettingPortFiresEventAfterReset()
    {
        bool eventFired = false;
        _emulator.OnPortChanged += (sender, args) =>
        {
            eventFired = true;
            args.Port.ShouldBe(0x01);
            args.Value.ShouldBe(0xFF);
        };

        _emulator.Reset();
        _emulator.Ports[0x01] = 0xFF;

        eventFired.ShouldBeTrue();
    }

    [Test]
    public void ResetClearsWarmBoot()
    {
        _emulator.WarmBoot = true;

        _emulator.Reset();

        _emulator.WarmBoot.ShouldBeFalse();
    }

    [Test]
    public void ResetClearsInterupts()
    {
        _emulator.Interupts.IFF1 = true;

        _emulator.Reset();

        _emulator.Interupts.IFF1.ShouldBeFalse();
    }

    [Test]
    public void ResetClearsCPUFlags()
    {
        _emulator.CPU.Registers.FlagC = true;

        _emulator.Reset();

        _emulator.CPU.Registers.FlagC.ShouldBeFalse();
    }

    [Test]
    public void ResetClearsCPURegisters()
    {
        _emulator.CPU.Registers.A = 0xFF;

        _emulator.Reset();

        _emulator.CPU.Registers.A.ShouldBe(0x00);
    }

    [Test]
    public void ResetClearsMemory()
    {
        _emulator.Memory[0x0100] = 0xFF;

        _emulator.Reset();

        _emulator.Memory[0x0100].ShouldBe(0x00);
    }

    [Test]
    public void ResetClearsPorts()
    {
        _emulator.Ports[0x01] = 0xFF;

        _emulator.Reset();

        _emulator.Ports[0x01].ShouldBe(0x00);
    }

    [Test]
    public void CanLoadProgramToDefaultAddress()
    {
        _emulator.LoadProgram("Test.com");

        _emulator.Memory[0x0100].ShouldBe(0x3A);
        _emulator.Memory[0x0101].ShouldBe(0x0B);
        _emulator.Memory[0x0102].ShouldBe(0x01);
        _emulator.Memory[0x0103].ShouldBe(0x21);
        _emulator.Memory[0x0104].ShouldBe(0x0C);
        _emulator.Memory[0x0105].ShouldBe(0x01);
        // ...
        _emulator.Memory[0x010B].ShouldBe(0x38);
        _emulator.Memory[0x010C].ShouldBe(0x2B);
        _emulator.Memory[0x010D].ShouldBe(0x00);
    }

    [Test]
    public void CanLoadProgramAtSpecifiedAddress()
    {
        _emulator.LoadProgram("Test.com", 0x0200);

        _emulator.Memory[0x0200].ShouldBe(0x3A);
        _emulator.Memory[0x0201].ShouldBe(0x0B);
        _emulator.Memory[0x0202].ShouldBe(0x01);
        _emulator.Memory[0x0203].ShouldBe(0x21);
        _emulator.Memory[0x0204].ShouldBe(0x0C);
        _emulator.Memory[0x0205].ShouldBe(0x01);
        // ...
        _emulator.Memory[0x020B].ShouldBe(0x38);
        _emulator.Memory[0x020C].ShouldBe(0x2B);
        _emulator.Memory[0x020D].ShouldBe(0x00);
    }

    [Test]
    public void LoadingProgramToSpecifiedAddressSetsPC()
    {
        _emulator.LoadProgram("Test.com", 0x0200);

        _emulator.CPU.Registers.PC.ShouldBe(0x0200);
    }

    [Test]
    public void ResetReloadsProgram()
    {
        _emulator.LoadProgram("Test.com", 0x0200);
        _emulator.Memory[0x0200] = 0x00;

        _emulator.Reset();

        _emulator.Memory[0x0200].ShouldBe(0x3A);
    }

    [Test]
    public void CanPeekInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.PeekInstruction();

        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe("LD A,(0x010B)");
    }

    [Test]
    public void CanDisassembleInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.Disassemble(0x0107);

        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe("LD (0x010D),A");
    }

    [Test]
    public void CanExecuteInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.Tick();

        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe("LD A,(0x010B)");

        _emulator.CPU.Registers.A.ShouldBe(0x38);
        _emulator.CPU.Registers.PC.ShouldBe(0x0103);
    }

    [Test]
    public void EmulatorInitizesOperatingSystem()
    {
        _os.InitializeCalled.ShouldBeTrue();
    }

    [Test]
    public void ResetInitizesOperatingSystem()
    {
        _os.InitializeCalled = false;
        _emulator.Reset();
        _os.InitializeCalled.ShouldBeTrue();
    }

    [Test]
    public void EmulatorDoesNotExecutesOperatingSystemWhenCallVectorIncorrect()
    {
        _emulator.LoadProgram("Test.com");

        _emulator.Tick();

        _os.ExecuteCalled.ShouldBeFalse();
    }

    [Test]
    public void EmulatorExecutesOperatingSystemWhenCallVectorCorrect()
    {
        _emulator.Memory[0x0100] = 0xC3;    // JP 0x0005
        _emulator.Memory[0x0101] = 0x05;
        _emulator.Memory[0x0102] = 0x00;

        _emulator.Tick();

        _os.ExecuteCalled.ShouldBeTrue();
    }
}
