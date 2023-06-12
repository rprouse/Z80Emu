using Z80Emu.Core.Memory;
using Z80Emu.Core.OS;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core;

public class Emulator
{
    public IDos OperatingSystem { get; }

    public CPU CPU { get; }

    public MMU Memory { get; }
    
    public Interupts Interupts { get; }

    public Emulator(IDos os)
    {
        Memory = new MMU();
        Interupts = new Interupts(Memory);
        CPU = new CPU(Interupts, Memory);
        OperatingSystem = os;
    }

    public bool LoadProgram(string filename) => Memory.LoadProgram(filename);

    public Opcode Tick()
    {
        Opcode opcode = CPU.Tick();

        // If we are using an OS, check if we need to make a system call
        if (OperatingSystem != null && OperatingSystem.CallVectors.Contains(CPU.Registers.PC))
            OperatingSystem.Execute(this);

        return opcode;
    }

    public Opcode PeekInstruction() => CPU.PeekInstruction(CPU.Registers.PC);

    public Opcode Disassemble(word addr) => CPU.PeekInstruction(addr);
}
