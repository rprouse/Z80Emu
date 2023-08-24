using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z80Emu.Core;

namespace Z80Emu.Tests;

public class EmulatorTests
{
    Emulator _emulator;

    [SetUp]
    public void Setup()
    {
        _emulator = new Emulator(null);
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


}
