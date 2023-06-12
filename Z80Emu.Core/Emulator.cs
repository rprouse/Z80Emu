using Z80Emu.Core.Memory;
using Z80Emu.Core.OS;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core;

public class Emulator
{
    string? _filename;

    public IDos OperatingSystem { get; }

    public CPU CPU { get; private set; }

    public MMU Memory { get; private set; }
    
    public Interupts Interupts { get; private set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Emulator(IDos os)
    {
        OperatingSystem = os;
        Reset();
    }
#pragma warning restore CS8618

    public bool LoadProgram(string filename)
    {
        _filename = filename;
        return Memory.LoadProgram(filename);
    }

    public void Reset()
    {
        Memory = new MMU();
        Interupts = new Interupts(Memory);
        CPU = new CPU(Interupts, Memory);
        if (_filename != null )
            Memory.LoadProgram(_filename);
    }

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
