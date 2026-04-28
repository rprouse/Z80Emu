using Z80Emu.Core.Memory;
using Z80Emu.Core.OS;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;
using Z80Emu.Core.Utilities;

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
        bool result = Memory.LoadProgram(filename, baseAddress);
        if (result) CPU.Registers.PC = baseAddress;
        return result;
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
        Opcode? interruptOpcode = ServiceInterrupts();
        if (interruptOpcode != null)
            return interruptOpcode;

        Opcode opcode = CPU.Tick();

        // If we are using an OS, check if we need to make a system call
        if (OperatingSystem != null && OperatingSystem.CallVectors.Contains(CPU.Registers.PC))
            OperatingSystem.Execute(this);

        return opcode;
    }

    private Opcode? ServiceInterrupts()
    {
        if (Interupts.EiPending)
        {
            // EI shadow expires — skip sampling for this tick.
            Interupts.EiPending = false;
            return null;
        }

        if (Interupts.IsNmiRequested)
        {
            CPU.AcceptInterrupt(0x0066);
            Interupts.IFF2 = Interupts.IFF1;
            Interupts.IFF1 = false;
            Interupts.ConsumeNmi();
            return new Opcode("NMI", new string[0], "0", "Non-maskable interrupt accepted -> 0x0066");
        }

        if (Interupts.IsRequested && Interupts.IFF1)
        {
            word vector;
            string desc;
            switch (Interupts.Mode)
            {
                case InterruptMode.Mode2:
                    {
                        word vectorPtr = (word)((CPU.Registers.I << 8) | (Interupts.RequestData ?? 0xFF));
                        byte lo = Memory[vectorPtr];
                        byte hi = Memory[(word)(vectorPtr + 1)];
                        vector = BitUtils.ToWord(hi, lo);
                        desc = $"Maskable interrupt accepted (IM 2) -> 0x{vector:X4}";
                        break;
                    }
                case InterruptMode.Mode1:
                    vector = 0x0038;
                    desc = "Maskable interrupt accepted (IM 1) -> 0x0038";
                    break;
                default: // Mode0
                    vector = 0x0038;
                    desc = "Maskable interrupt accepted (IM 0) -> 0x0038 (RST 38h)";
                    break;
            }
            CPU.AcceptInterrupt(vector);
            Interupts.IFF1 = false;
            Interupts.IFF2 = false;
            Interupts.ConsumeInterrupt();
            return new Opcode("INT", new string[0], "0", desc);
        }

        return null;
    }

    public Opcode PeekInstruction() => CPU.PeekInstruction(CPU.Registers.PC);

    public Opcode Disassemble(word addr) => CPU.PeekInstruction(addr);
}
