using Spectre.Console;
using Z80Emu.Core;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu;

internal class Monitor
{
    readonly Emulator _emulator;
    readonly TextPrompt<string> _prompt;

    public Monitor(Emulator emulator)
    {
        _emulator = emulator;
        _prompt = new TextPrompt<string>(">")
                    .HideChoices()
                    .HideDefaultValue()
                    .PromptStyle("white")
                    .AddChoice("h")
                    .AddChoice("s")
                    .AddChoice("m")
                    .AddChoice("r")
                    .AddChoice("d")
                    .AddChoice("q");
    }

    public int Run(string filename)
    {
        AnsiConsole.MarkupLine($"[Blue]Loading {filename}[/]");

        if (!_emulator.LoadProgram(filename))
        {
            AnsiConsole.MarkupLine("[Red]File not found[/]");
            return -1;
        }

        string command = "s";

        while (true)
        {
            command = AnsiConsole.Prompt(_prompt.DefaultValue(command));

            switch (command.FirstOrDefault())
            {
                case 's':   // Step
                    Step();
                    break;
                case 'm':   // Memory
                    ViewMemory();
                    break;
                case 'r':   // Registers
                    ViewRegisters();
                    break;
                case 'd':   // Disassemble
                    ViewDisassembly();
                    break;
                case 'h':   // Help
                    ViewHelp();
                    break;
                case 'q':   // Quit
                    return 0;
                default:
                    AnsiConsole.MarkupLine("[red]Unknown command[/]");
                    break;
            }
        }
    }

    public void Banner()
    {
        FigletFont font = FigletFont.Load(@"font/ANSI Shadow.flf");

        AnsiConsole.Write(
            new FigletText(font, "Zilog Z80")
                .LeftJustified()
                .Color(Color.Blue));
        AnsiConsole.MarkupLine("[Yellow]8-Bit Retro Emulator by Rob Prouse[/]");
    }

    void Step()
    {
        Opcode? opcode = _emulator.Tick();
        if (opcode != null)
        {
            AnsiConsole.Markup($"[silver]{opcode.Mnemonic}[/]");
            AnsiConsole.MarkupLine($"\t[green]; {opcode.Description}[/]");
            AnsiConsole.WriteLine();
        }
        ViewRegisters();
    }

    static void ViewHelp()
    {
        AnsiConsole.MarkupLine("[blue]h[/][silver]elp[/]");
        AnsiConsole.MarkupLine("[blue]s[/][silver]tep[/]");
        AnsiConsole.MarkupLine("[blue]m[/][silver]emory[/]");
        AnsiConsole.MarkupLine("[blue]r[/][silver]egisters[/]");
        AnsiConsole.MarkupLine("[blue]d[/][silver]isassemble[/]");
        AnsiConsole.MarkupLine("[blue]q[/][silver]uit[/]");
        AnsiConsole.WriteLine();
    }

    void ViewRegisters()
    {
        var r = _emulator.CPU.Registers;
        AnsiConsole.Markup($"[blue]AF [/][aqua]{r.AF:X4}[/] [blue]BC [/][aqua]{r.BC:X4}[/] [blue]DE [/][aqua]{r.DE:X4}[/] [blue]HL [/][aqua]{r.HL:X4}[/] [blue]IX [/][aqua]{r.IX:X4}[/]");
        AnsiConsole.Markup($"   [blue]SP [/][aqua]{r.SP:X4}[/] [blue]PC [/][aqua]{r.PC:X4}[/]");
        AnsiConsole.MarkupLine($"   [maroon]S:[/][aqua]{(r.FlagS ? '1' : '0')}[/] [maroon]Z:[/][aqua]{(r.FlagZ ? '1' : '0')}[/] [maroon]H:[/][aqua]{(r.FlagH ? '1' : '0')}[/] [maroon]P/V:[/][aqua]{(r.FlagPV ? '1' : '0')}[/] [maroon]N:[/][aqua]{(r.FlagN ? '1' : '0')}[/] [maroon]C:[/][aqua]{(r.FlagC ? '1' : '0')}[/]");
        AnsiConsole.Markup($"[blue]AF'[/][aqua]{r.AF_:X4}[/] [blue]BC'[/][aqua]{r.BC_:X4}[/] [blue]DE'[/][aqua]{r.DE_:X4}[/] [blue]HL'[/][aqua]{r.HL_:X4}[/] [blue]IX [/][aqua]{r.IY:X4}[/]");
        AnsiConsole.MarkupLine($"   [blue]SP'[/][aqua]{r.SP_:X4}[/] [blue]PC'[/][aqua]{r.PC_:X4}[/]");
        AnsiConsole.WriteLine();
    }

    void ViewMemory(word startAddr = 0x0100, word len = 0x6F)
    {
        startAddr = (word)(startAddr / 0xF * 0xF + 1);
        for (word addr = startAddr; addr < startAddr + len; addr += 0xF)
        {
            AnsiConsole.Markup($"[blue]{addr:X4}[/] ");
            for (word i = 0; i < 0xF; i++)
            {
                AnsiConsole.Markup($"[aqua]{_emulator.Memory[addr + i]:X2}[/] ");
            }
            for (word i = 0; i < 0xF; i++)
            {
                char c = (char)_emulator.Memory[addr + i];
                AnsiConsole.Markup($"[green]{(char.IsControl(c) ? '.' : c)}[/]");
            }
            AnsiConsole.WriteLine();
        }
    }

    /// <summary>
    /// View the disassembly of the program from the current PC
    /// </summary>
    void ViewDisassembly()
    {
        word len = 0x1F;
        word startAddr = _emulator.CPU.Registers.PC;
        word addr = startAddr;

        while (addr < startAddr + len)
        {
            AnsiConsole.Markup($"[blue]{addr:X4}[/] ");
            try
            {
                Opcode opcode = _emulator.Disassemble(addr);

                for (int i = 0; i < 4; i++)
                {
                    if (i < opcode.Length)
                        AnsiConsole.Markup($"[aqua]{_emulator.Memory[addr + i]:X2}[/] ");
                    else
                        AnsiConsole.Markup($"   ");
                }

                // Longest opcode without substitution is 12 characters
                AnsiConsole.Markup($"[silver]{opcode.Mnemonic.PadRight(12)}[/]");

                if (opcode.Mnemonic != "NOP")
                    AnsiConsole.MarkupLine($"\t[green]; {opcode.Description}[/]");
                else
                    AnsiConsole.WriteLine();

                addr += opcode.Length;

            }
            catch (Exception)
            {
                AnsiConsole.MarkupLine($"[aqua]{_emulator.Memory[addr]:X2}[/]");
                addr++;
            }
        }
        AnsiConsole.WriteLine();
    }
}
