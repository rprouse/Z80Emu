using Spectre.Console;
using Z80Emu.Core;
using Z80Emu.Core.Processor.Opcodes;

FigletFont font = FigletFont.Load(@"font/ANSI Shadow.flf");

AnsiConsole.Write(
    new FigletText(font, "Zilog Z80")
        .LeftJustified()
        .Color(Color.Blue));
AnsiConsole.MarkupLine("[Yellow]8-Bit Retro Emulator by Rob Prouse[/]");

if (args.Length != 1)
{
    AnsiConsole.MarkupLine("[Red]Usage: Z80Emu <program.com>[/]");
    return -1;
}

var emulator = new Emulator();
if (!emulator.LoadProgram(args[0]))
{
    AnsiConsole.MarkupLine($"[Red]File {args[0]} not found[/]");
    return -1;
}

string command = "s";

while(true)
{
    command = AnsiConsole.Prompt(
        new TextPrompt<string>(">")
            .HideChoices()
            .HideDefaultValue()
            .PromptStyle("white")
            .AddChoice("?")
            .AddChoice("s")
            .AddChoice("m")
            .AddChoice("r")
            .AddChoice("d")
            .AddChoice("q")
            .DefaultValue(command)
        );

    switch (command.FirstOrDefault())
    {
        case 's':   // Step
            Opcode? opcode = emulator.Tick();
            if (opcode != null)
            {
                AnsiConsole.Markup($"[silver]{opcode.Mnemonic}[/]");
                AnsiConsole.MarkupLine($"\t[green]; {opcode.Description}[/]");
                AnsiConsole.WriteLine();
            }
            ViewRegisters(emulator);
            break;
        case 'm':   // Memory
            ViewMemory(emulator);
            break;
        case 'r':   // Registers
            ViewRegisters(emulator);
            break;
        case 'd':   // Disassemble
            ViewDisassembly(emulator);
            break;
        case '?':
            AnsiConsole.MarkupLine("[blue]s[/]tep");
            AnsiConsole.MarkupLine("[blue]m[/]emory");
            AnsiConsole.MarkupLine("[blue]r[/]egisters");
            AnsiConsole.MarkupLine("[blue]d[/]isassemble");
            AnsiConsole.MarkupLine("[blue]h[/]elp");
            AnsiConsole.MarkupLine("[blue]q[/]uit");
            break;
        case 'q':   // Quit
            return 0;
        default:
            AnsiConsole.MarkupLine("[red]Unknown command[/]");
            break;
    }
}

static void ViewRegisters(Emulator emulator)
{
    var r = emulator.CPU.Registers;
    AnsiConsole.Markup($"[blue]AF [/][aqua]{r.AF:X4}[/] [blue]BC [/][aqua]{r.BC:X4}[/] [blue]DE [/][aqua]{r.DE:X4}[/] [blue]HL [/][aqua]{r.HL:X4}[/] [blue]IX [/][aqua]{r.IX:X4}[/]");
    AnsiConsole.Markup($"   [blue]SP [/][aqua]{r.SP:X4}[/] [blue]PC [/][aqua]{r.PC:X4}[/]");
    AnsiConsole.MarkupLine($"   [maroon]S:[/][aqua]{(r.FlagS ? '1' : '0')}[/] [maroon]Z:[/][aqua]{(r.FlagZ ? '1' : '0')}[/] [maroon]H:[/][aqua]{(r.FlagH ? '1' : '0')}[/] [maroon]P/V:[/][aqua]{(r.FlagPV ? '1' : '0')}[/] [maroon]N:[/][aqua]{(r.FlagN ? '1' : '0')}[/] [maroon]C:[/][aqua]{(r.FlagC ? '1' : '0')}[/]");
    AnsiConsole.Markup($"[blue]AF'[/][aqua]{r.AF_:X4}[/] [blue]BC'[/][aqua]{r.BC_:X4}[/] [blue]DE'[/][aqua]{r.DE_:X4}[/] [blue]HL'[/][aqua]{r.HL_:X4}[/] [blue]IX [/][aqua]{r.IY:X4}[/]");
    AnsiConsole.MarkupLine($"   [blue]SP'[/][aqua]{r.SP_:X4}[/] [blue]PC'[/][aqua]{r.PC_:X4}[/]");
    AnsiConsole.WriteLine();
}

static void ViewMemory(Emulator emulator, word startAddr = 0x0100, word len = 0x6F)
{
    startAddr = (word)(startAddr / 0xF * 0xF + 1);
    for (word addr = startAddr; addr < startAddr + len; addr += 0xF)
    {
        AnsiConsole.Markup($"[blue]{addr:X4}[/] ");
        for (word i = 0; i < 0xF; i++)
        {
            AnsiConsole.Markup($"[aqua]{emulator.Memory[addr + i]:X2}[/] ");
        }
        for (word i = 0; i < 0xF; i++)
        {
            char c = (char)emulator.Memory[addr + i];
            AnsiConsole.Markup($"[green]{(char.IsControl(c) ? '.' : c)}[/]");
        }
        AnsiConsole.WriteLine();
    }
}

/// <summary>
/// View the disassembly of the program from the current PC
/// </summary>
static void ViewDisassembly(Emulator emulator)
{
    word len = 0x1F;
    word startAddr = emulator.CPU.Registers.PC;
    word addr = startAddr;

    while (addr < startAddr + len)
    {
        AnsiConsole.Markup($"[blue]{addr:X4}[/] ");
        try
        {
            Opcode opcode = emulator.Disassemble(addr);

            for (int i = 0; i < 4; i++)
            {
                if (i < opcode.Length)
                    AnsiConsole.Markup($"[aqua]{emulator.Memory[addr + i]:X2}[/] ");
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
            AnsiConsole.MarkupLine($"[aqua]{emulator.Memory[addr]:X2}[/]");
            addr++;
        }
    }
    AnsiConsole.WriteLine();
}