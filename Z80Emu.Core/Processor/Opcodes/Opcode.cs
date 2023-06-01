namespace Z80Emu.Core.Processor.Opcodes;

public delegate void Tick();

public class Opcode
{
    public byte Value { get; set; }
    public string Name { get; set; }
    public int Length { get; set; }
    public int Cycles { get; set; }

    public Tick[] Ticks { get; set; }

    public Opcode(byte value, string name, int length, int cycles, Tick[] ticks)
    {
        Value = value;
        Name = name;
        Length = length;
        Cycles = cycles;
        Ticks = ticks;
    }

    public override string ToString() => Name;
}
