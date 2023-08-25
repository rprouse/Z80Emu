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
            args.Port.Should().Be(0x01);
            args.Value.Should().Be(0xFF);
        };

        _emulator.Ports[0x01] = 0xFF;

        eventFired.Should().BeTrue();
    }

    [Test]
    public void SettingPortFiresEventAfterReset()
    {
        bool eventFired = false;
        _emulator.OnPortChanged += (sender, args) =>
        {
            eventFired = true;
            args.Port.Should().Be(0x01);
            args.Value.Should().Be(0xFF);
        };

        _emulator.Reset();
        _emulator.Ports[0x01] = 0xFF;

        eventFired.Should().BeTrue();
    }

    [Test]
    public void ResetClearsWarmBoot()
    {
        _emulator.WarmBoot = true;

        _emulator.Reset();

        _emulator.WarmBoot.Should().BeFalse();
    }

    [Test]
    public void ResetClearsInterupts()
    {
        _emulator.Interupts.IFF1 = true;

        _emulator.Reset();

        _emulator.Interupts.IFF1.Should().BeFalse();
    }

    [Test]
    public void ResetClearsCPUFlags()
    {
        _emulator.CPU.Registers.FlagC = true;

        _emulator.Reset();

        _emulator.CPU.Registers.FlagC.Should().BeFalse();
    }

    [Test]
    public void ResetClearsCPURegisters()
    {
        _emulator.CPU.Registers.A = 0xFF;

        _emulator.Reset();

        _emulator.CPU.Registers.A.Should().Be(0x00);
    }

    [Test]
    public void ResetClearsMemory()
    {
        _emulator.Memory[0x0100] = 0xFF;

        _emulator.Reset();

        _emulator.Memory[0x0100].Should().Be(0x00);
    }

    [Test]
    public void ResetClearsPorts()
    {
        _emulator.Ports[0x01] = 0xFF;

        _emulator.Reset();

        _emulator.Ports[0x01].Should().Be(0x00);
    }

    [Test]
    public void CanLoadProgramToDefaultAddress()
    {
        _emulator.LoadProgram("Test.com");

        _emulator.Memory[0x0100].Should().Be(0x3A);
        _emulator.Memory[0x0101].Should().Be(0x0B);
        _emulator.Memory[0x0102].Should().Be(0x01);
        _emulator.Memory[0x0103].Should().Be(0x21);
        _emulator.Memory[0x0104].Should().Be(0x0C);
        _emulator.Memory[0x0105].Should().Be(0x01);
        // ...
        _emulator.Memory[0x010B].Should().Be(0x38);
        _emulator.Memory[0x010C].Should().Be(0x2B);
        _emulator.Memory[0x010D].Should().Be(0x00);
    }

    [Test]
    public void CanLoadProgramAtSpecifiedAddress()
    {
        _emulator.LoadProgram("Test.com", 0x0200);

        _emulator.Memory[0x0200].Should().Be(0x3A);
        _emulator.Memory[0x0201].Should().Be(0x0B);
        _emulator.Memory[0x0202].Should().Be(0x01);
        _emulator.Memory[0x0203].Should().Be(0x21);
        _emulator.Memory[0x0204].Should().Be(0x0C);
        _emulator.Memory[0x0205].Should().Be(0x01);
        // ...
        _emulator.Memory[0x020B].Should().Be(0x38);
        _emulator.Memory[0x020C].Should().Be(0x2B);
        _emulator.Memory[0x020D].Should().Be(0x00);
    }

    [Test]
    public void LoadingProgramToSpecifiedAddressSetsPC()
    {
        _emulator.LoadProgram("Test.com", 0x0200);

        _emulator.CPU.Registers.PC.Should().Be(0x0200);
    }

    [Test]
    public void ResetReloadsProgram()
    {
        _emulator.LoadProgram("Test.com", 0x0200);
        _emulator.Memory[0x0200] = 0x00;

        _emulator.Reset();

        _emulator.Memory[0x0200].Should().Be(0x3A);
    }

    [Test]
    public void CanPeekInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.PeekInstruction();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD A,(0x010B)");
    }

    [Test]
    public void CanDisassembleInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.Disassemble(0x0107);

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD (0x010D),A");
    }

    [Test]
    public void CanExecuteInstruction()
    {
        _emulator.LoadProgram("Test.com");

        var op = _emulator.Tick();

        op.Should().NotBeNull();
        op.Mnemonic.Should().Be("LD A,(0x010B)");

        _emulator.CPU.Registers.A.Should().Be(0x38);
        _emulator.CPU.Registers.PC.Should().Be(0x0103);
    }

    [Test]
    public void EmulatorInitizesOperatingSystem()
    {
        _os.InitializeCalled.Should().BeTrue();
    }

    [Test]
    public void ResetInitizesOperatingSystem()
    {
        _os.InitializeCalled = false;
        _emulator.Reset();
        _os.InitializeCalled.Should().BeTrue();
    }

    [Test]
    public void EmulatorDoesNotExecutesOperatingSystemWhenCallVectorIncorrect()
    {
        _emulator.LoadProgram("Test.com");

        _emulator.Tick();

        _os.ExecuteCalled.Should().BeFalse();
    }

    [Test]
    public void EmulatorExecutesOperatingSystemWhenCallVectorCorrect()
    {
        _emulator.Memory[0x0100] = 0xC3;    // JP 0x0005
        _emulator.Memory[0x0101] = 0x05;
        _emulator.Memory[0x0102] = 0x00;

        _emulator.Tick();

        _os.ExecuteCalled.Should().BeTrue();
    }
}
