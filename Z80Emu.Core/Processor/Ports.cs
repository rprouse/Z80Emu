namespace Z80Emu.Core.Processor;

public class Ports
{
    private byte[] _ports = new byte[256];

    public byte this[byte port]
    {
        get => _ports[port];
        set
        {
            _ports[port] = value;
            OnPortChanged?.Invoke(this, new PortChangedEventArgs(port, value));
        }
    }

    public event PortChangedEventHandler? OnPortChanged;
}

public delegate void PortChangedEventHandler(object sender, PortChangedEventArgs args);

public class PortChangedEventArgs : EventArgs
{
    public byte Port { get; }
    public byte Value { get; }

    public PortChangedEventArgs(byte port, byte value)
    {
        Port = port;
        Value = value;
    }
}
