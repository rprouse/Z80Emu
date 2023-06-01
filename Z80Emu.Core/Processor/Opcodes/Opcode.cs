namespace Z80Emu.Core.Processor.Opcodes;


public class Opcode
{
    public byte Value { get; set; }
    public string Name { get; set; }

    public Action Tick { get; set; }

    public Opcode(byte value, string name, Action tick)
    {
        Value = value;
        Name = name;
        Tick = tick;
    }

    public override string ToString() => Name;
}
