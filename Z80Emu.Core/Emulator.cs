using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;

namespace Z80Emu.Core;

public class Emulator
{
    public CPU CPU { get; }

    public MMU Memory { get; }
    
    public Interupts Interupts { get; }

    public Emulator()
    {
        Memory = new MMU();
        Interupts = new Interupts(Memory);
        CPU = new CPU(Interupts, Memory);
    }

    public bool LoadProgram(string filename) =>
        Memory.LoadProgram(filename);

    public int Tick()
    {
        CPU.Tick();
        return 1;
    }

    public override string ToString()
    {
        return CPU.ToString();
    }
}
