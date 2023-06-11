using Z80Emu.Core.OS;
using Monitor = Z80Emu.Monitor;

var operatingSystem = new CPM22();
var emulator = new Emulator(operatingSystem);
var monitor = new Monitor(emulator);
monitor.Banner();

if (args.Length != 1)
{
    AnsiConsole.MarkupLine("[Red]Usage:[/] [silver]Z80Emu <program.com>[/]");
    return -1;
}

return monitor.Run(args[0]);