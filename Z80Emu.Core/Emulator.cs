using Z80Emu.Core.Memory;
using Z80Emu.Core.OS;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core;

public class Emulator
{
    string? _filename;
    word? _baseAddress;

    public IDos OperatingSystem { get; }

    public CPU CPU { get; private set; }

    public MMU Memory { get; private set; }
    
    public Interupts Interupts { get; private set; }

    public Ports Ports { get; private set; }

    public event PortChangedEventHandler? OnPortChanged;

    public bool WarmBoot { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public Emulator(IDos os)
    {
        OperatingSystem = os;
        Reset();
    }
#pragma warning restore CS8618

    public bool LoadProgram(string filename, word baseAddress = 0x0100)
    {
        _filename = filename;
        _baseAddress = baseAddress;
        return Memory.LoadProgram(filename, baseAddress);
    }

    public void Reset()
    {
        WarmBoot = false;
        Memory = new MMU();
        Interupts = new Interupts(Memory);
        Ports = new Ports();
        Ports.OnPortChanged += (sender, args) => OnPortChanged?.Invoke(sender, args);
        CPU = new CPU(Interupts, Memory, Ports);
        if (_filename != null && _baseAddress.HasValue)
            Memory.LoadProgram(_filename, _baseAddress.Value);

        if (OperatingSystem != null)
            OperatingSystem.Initialize(this);
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
