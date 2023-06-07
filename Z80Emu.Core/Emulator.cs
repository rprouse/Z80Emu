using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

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

    public bool LoadProgram(string filename) => Memory.LoadProgram(filename);

    public Opcode Tick() => CPU.Tick();

    public Opcode Disassemble(word addr) => CPU.Disassemble(addr);
}
