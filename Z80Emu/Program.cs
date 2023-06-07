using System.Diagnostics.Tracing;
using System.Text;
using Spectre.Console;
using Spectre.Console.Rendering;
using Z80Emu.Core;
using Z80Emu.Core.Processor.Opcodes;

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
    //AnsiConsole.Write("> ");
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
            // ViewDisassembly(emulator);
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
    AnsiConsole.Markup($"[blue]AF:[/][aqua]{r.AF:X4}[/] [blue]BC:[/][aqua]{r.BC:X4}[/] [blue]DE:[/][aqua]{r.DE:X4}[/] [blue]HL:[/][aqua]{r.HL:X4}[/]");
    AnsiConsole.MarkupLine($"    [blue]SP:[/][aqua]{r.SP:X4}[/] [blue]PC:[/][aqua]{r.PC:X4}[/]");
    AnsiConsole.MarkupLine($"[blue]Z:[/][aqua]{(r.FlagZ ? '1' : '0')}[/] [blue]N:[/][aqua]{(r.FlagN ? '1' : '0')}[/] [blue]H:[/][aqua]{(r.FlagH ? '1' : '0')}[/] [blue]C:[/][aqua]{(r.FlagC ? '1' : '0')}[/]");
    AnsiConsole.WriteLine();
}

static void ViewMemory(Emulator emulator, ushort startAddr = 0x0100, ushort len = 0x6F)
{
    startAddr = (ushort)(startAddr / 0xF * 0xF);
    for (ushort addr = startAddr; addr < startAddr + len; addr += 0xF)
    {
        AnsiConsole.Markup($"[cyan]{addr:X4}[/] ");
        for (ushort i = 0; i < 0xF; i++)
        {
            AnsiConsole.Markup($"[yellow]{emulator.Memory[addr + i]:X2}[/] ");
        }
        for (ushort i = 0; i < 0xF; i++)
        {
            char c = (char)emulator.Memory[addr + i];
            AnsiConsole.Markup($"[green]{(char.IsControl(c) ? '.' : c)}[/]");
        }
        AnsiConsole.WriteLine();
    }
}