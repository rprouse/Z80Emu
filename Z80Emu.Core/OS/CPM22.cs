using System.Text;

namespace Z80Emu.Core.OS;

/// <summary>
/// Implements CP/M 2.2 BDOS calls.
/// </summary>
/// <remarks>
/// See https://www.seasip.info/Cpm/bdos.html and 
/// http://www.gaby.de/cpm/manuals/archive/cpm22htm/ch5.htm for details.
/// </remarks>
public class CPM22 : IDos
{
    static readonly IEnumerable<word> _callVectors = new word[] { 0x0000, 0x0005 };

    IDosConsole _console;

    public enum SystemCalls : byte
    {
        P_TERMCPM = 0,
        C_READ = 1,
        C_WRITE = 2,
        A_READ = 3,
        A_WRITE = 4,
        L_WRITE = 5,
        C_RAWIO = 6,
        A_STATIN = 7,
        A_STATOUT = 8,
        C_WRITESTR = 9,
        C_READSTR = 10,
        C_STAT = 11,
        S_BDOSVER = 12,
        DRV_ALLRESET = 13,
        DRV_SET = 14,
        F_OPEN = 15,
        F_CLOSE = 16,
        F_SFIRST = 17,
        F_SNEXT = 18,
        F_DELETE = 19,
        F_READ = 20,
        F_WRITE = 21,
        F_MAKE = 22,
        F_RENAME = 23,
        DRV_LOGINVEC = 24,
        DRV_GET = 25,
        F_DMAOFF = 26,
        DRV_ALLOCVEC = 27,
        DRV_SETRO = 28,
        DRV_ROVEC = 29,
        F_ATTRIB = 30,
        DRV_DPB = 31,
        F_USERNUM = 32,
        F_READRAND = 33,
        F_WRITERAND = 34,
        F_SIZE = 35,
        F_RANDREC = 36,
        DRV_RESET = 37,
        F_WRITEZF = 40,
    }

    public string Name => "CP/M 2.2";

    public IEnumerable<word> CallVectors => _callVectors;

    public CPM22(IDosConsole console)
    {
        _console = console;
    }

    public void Execute(Emulator emulator)
    {
        switch (emulator.CPU.Registers.PC)
        {
            case 0x0000:    // Warm Boot
                emulator.WarmBoot = true;
                emulator.CPU.Return();
                break;
            case 0x0005:    // BDOS Call
                ExecuteBdosCall(emulator);
                break;
            default:
                throw new InvalidOperationException("Invalid CP/M Call");
        }
    }

    public void Initialize(Emulator emulator)
    {
        // Initialize CP/M Zero Page memory
        // See https://en.wikipedia.org/wiki/Zero_page_(CP/M)

        emulator.Memory[0x0000] = 0xC3; // JP to warm boot entry point
        emulator.Memory[0x0001] = 0x03; 
        emulator.Memory[0x0002] = 0xE6;
        emulator.Memory[0x0003] = 0x94; // I/O byte
        emulator.Memory[0x0004] = 0x00; // Current command processor drive (low 4 bits) and user number (high 4 bits)
        emulator.Memory[0x0005] = 0xC3; // JP to BDOS entry point
        emulator.Memory[0x0006] = 0x06;
        emulator.Memory[0x0007] = 0xBF;
        // 08-3A Restart/Interupt vectors (TODO?)
        // 3B-5B Reserved
        // File Control Blocks one and two (FCBs) containing "A:DA.COM"
        byte[] fcb = new byte[] { 0x01, 0x44, 0x41, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x43, 0x4F, 0x4D, 0x00, 0x80, 0x80, 0x0F };
        for (int i = 0; i < fcb.Length; i++)
        {
            emulator.Memory[0x005C + i] = fcb[i];
            emulator.Memory[0x006C + i] = fcb[i];
        }

        // Initialize the Command Tail (127 bytes)
        string commandTail = "A:DA.COM A:DA.COM /D";
        emulator.Memory[0x0080] = (byte)commandTail.Length; // Command tail length
        for (int i = 0; i < commandTail.Length; i++)
        {
            emulator.Memory[0x0081 + i] = (byte)commandTail[i];
        }
    }

    private void ExecuteBdosCall(Emulator emulator)
    { 
        var call = (SystemCalls)emulator.CPU.Registers.C;

        switch (call)
        {
            case SystemCalls.C_READ:
                emulator.CPU.Registers.A = _console.Read();
                emulator.CPU.Registers.L = emulator.CPU.Registers.A;
                break;
            case SystemCalls.C_WRITE:
                _console.Write((char)emulator.CPU.Registers.E);
                break;
            case SystemCalls.C_READSTR:
                ReadString(emulator);
                break;
            case SystemCalls.C_WRITESTR:
                WriteString(emulator);
                break;
            default:
                _console.WriteLine($"BDOS call {call} not implemented");
                break;
        }
        emulator.CPU.Return();
    }

    private void ReadString(Emulator emulator)
    {
        var addr = emulator.CPU.Registers.DE;        
        var str = _console.ReadString();
        byte maxLen = emulator.Memory[addr];
        emulator.Memory[addr++] = (byte)Math.Min(str.Length, maxLen);
        for (var i = 0; i < maxLen && i < str.Length; i++)
            emulator.Memory[addr++] = (byte)str[i];
    }

    private void WriteString(Emulator emulator)
    {
        var addr = emulator.CPU.Registers.DE;
        var sb = new StringBuilder();
        while (true)
        {
            var c = emulator.Memory[addr++];
            if (c == '$')
                break;
            sb.Append((char)c);
        }
        _console.Write(sb.ToString());
    }
}
