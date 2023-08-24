using Z80Emu.Core.Processor;

namespace Z80Emu.Tests.Processor;

public class TestPorts
{
    Ports _ports;

    [SetUp]
    public void Setup()
    {
        _ports = new Ports();
    }

    [Test]
    public void UpdatingPortsFiresEvent()
    {
        byte port = 0x01;
        byte value = 0x69;
        var eventFired = false;
        _ports.OnPortChanged += (sender, args) =>
        {
            eventFired = true;
            args.Port.Should().Be(port);
            args.Value.Should().Be(value);
        };

        _ports[port] = value;

        eventFired.Should().BeTrue();
    }

    [Test]
    public void UpdatingPortsSetsValue()
    {
        byte port = 0x01;
        byte value = 0x69;

        _ports[port] = value;

        _ports[port].Should().Be(value);
    }
}