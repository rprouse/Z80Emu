using System.Globalization;
using Z80Emu.Core.OS;
using Monitor = Z80Emu.Monitor;

var operatingSystem = new CPM22();
var emulator = new Emulator(operatingSystem);
var monitor = new Monitor(emulator);
monitor.Banner();

if (args.Length < 1 || args.Length > 2)
{
    AnsiConsole.WriteLine();
    AnsiConsole.MarkupLine("[blue]Usage:[/] [silver]Z80Emu <program.com> [[baseAddress]][/]");
    AnsiConsole.MarkupLine("[blue]Example:[/] [silver]Z80Emu hello.com 0x0100[/]");
    AnsiConsole.MarkupLine("[blue]Defaul base address is CPM's 0x0100. Use HEX.[/]");
    return -1;
}

word baseAddress = 0x0100;
if (args.Length == 2)
{
    if (args[1].StartsWith("0x")) args[1] =
            args[1].Substring(2);

    if (args[1].TryParseHexWord(out var address))
        baseAddress = address;
    else
        AnsiConsole.MarkupLine("[red]Invalid base address[/]");
}

return monitor.Run(args[0], baseAddress);