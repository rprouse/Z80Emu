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
    static readonly IEnumerable<word> _callVectors = new word[] { 0x0005 };

    public enum SystemCalls
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

    public void Execute(Emulator emulator)
    {
        if (!CallVectors.Contains(emulator.CPU.Registers.PC))
            throw new InvalidOperationException("Invalid BDOS call");

        var call = (SystemCalls)emulator.CPU.Registers.C;

        switch (call)
        {
            case SystemCalls.C_READ:
                emulator.CPU.Registers.A = (byte)Console.Read();
                emulator.CPU.Registers.L = emulator.CPU.Registers.A;
                break;
            case SystemCalls.C_WRITE:
                Console.Write((char)emulator.CPU.Registers.E);
                break;
            case SystemCalls.C_READSTR:
                ReadString(emulator);
                break;
            case SystemCalls.C_WRITESTR:
                WriteString(emulator);
                break;
            default:
                Console.WriteLine($"BDOS call {call} not implemented");
                break;
        }

        emulator.CPU.Return();
    }

    private static void ReadString(Emulator emulator)
    {
        var addr = emulator.CPU.Registers.DE;
        var sb = new StringBuilder();
        while (true)
        {
            var c = Console.ReadKey(true);
            if (c.Key == ConsoleKey.Enter)
                break;
            if (c.Key == ConsoleKey.Backspace || c.Key == ConsoleKey.Delete)
            {
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Console.Write("\b \b");
                }
                continue;
            }
            sb.Append(c.KeyChar);
        }
        var str = sb.ToString();
        byte maxLen = emulator.Memory[addr];
        emulator.Memory[addr++] = (byte)Math.Min(str.Length, maxLen);
        emulator.Memory[addr++] = (byte)'$';
        for (var i = 0; i < maxLen && i < str.Length; i++)
            emulator.Memory[addr++] = (byte)str[i];
    }

    private static void WriteString(Emulator emulator)
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
        Console.Write(sb.ToString());
    }
}
