using Spectre.Console;
using Z80Emu.Core;
using Z80Emu.Core.Processor.Opcodes;

FigletFont font = FigletFont.Load(@"font/ANSI Shadow.flf");

AnsiConsole.Write(
    new FigletText(font, "Zilog Z80")
        .LeftJustified()
        .Color(Color.Blue));
AnsiConsole.MarkupLine("[Yellow]8-Bit Retro Emulator by Rob Prouse[/]");

// TODO: Pass in the program to load as a command line argument
var emulator = new Emulator();
if (!emulator.LoadProgram(@"../../../../bin/8BitAdd.com"))
{
    AnsiConsole.MarkupLine("[Red]File not found[/]");
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
    AnsiConsole.Markup($"[blue]AF [/][aqua]{r.AF:X4}[/] [blue]BC [/][aqua]{r.BC:X4}[/] [blue]DE [/][aqua]{r.DE:X4}[/] [blue]HL [/][aqua]{r.HL:X4}[/]");
    AnsiConsole.MarkupLine($"    [blue]SP [/][aqua]{r.SP:X4}[/] [blue]PC [/][aqua]{r.PC:X4}[/]");
    AnsiConsole.Markup($"[blue]AF`[/][aqua]{r.AF2:X4}[/] [blue]BC`[/][aqua]{r.BC2:X4}[/] [blue]DE`[/][aqua]{r.DE2:X4}[/] [blue]HL`[/][aqua]{r.HL2:X4}[/]");
    AnsiConsole.MarkupLine($"    [blue]SP`[/][aqua]{r.SP2:X4}[/] [blue]PC`[/][aqua]{r.PC2:X4}[/]");
    AnsiConsole.MarkupLine($"[blue]S:[/][aqua]{(r.FlagS ? '1' : '0')}[/] [blue]Z:[/][aqua]{(r.FlagZ ? '1' : '0')}[/] [blue]H:[/][aqua]{(r.FlagH ? '1' : '0')}[/] [blue]PV:[/][aqua]{(r.FlagPV ? '1' : '0')}[/] [blue]N:[/][aqua]{(r.FlagN ? '1' : '0')}[/] [blue]C:[/][aqua]{(r.FlagC ? '1' : '0')}[/]");
    AnsiConsole.WriteLine();
}

static void ViewMemory(Emulator emulator, word startAddr = 0x0100, word len = 0x6F)
{
    startAddr = (word)(startAddr / 0xF * 0xF + 1);
    for (word addr = startAddr; addr < startAddr + len; addr += 0xF)
    {
        AnsiConsole.Markup($"[cyan]{addr:X4}[/] ");
        for (word i = 0; i < 0xF; i++)
        {
            AnsiConsole.Markup($"[silver]{emulator.Memory[addr + i]:X2}[/] ");
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
        AnsiConsole.Markup($"[cyan]{addr:X4}[/] ");
        try
        {
            Opcode opcode = emulator.Disassemble(addr);
            addr += opcode.Length;
            AnsiConsole.Markup($"[silver]{opcode.Mnemonic}[/]");
            AnsiConsole.MarkupLine($"\t[green]; {opcode.Description}[/]");

        }
        catch (Exception)
        {
            AnsiConsole.MarkupLine($"[silver]{emulator.Memory[addr]:X2}[/]");
            addr++;
        }
    }
    AnsiConsole.WriteLine();
}