using Monitor = Z80Emu.Monitor;

var monitor = new Monitor(new Emulator());
monitor.Banner();

if (args.Length != 1)
{
    AnsiConsole.MarkupLine("[Red]Usage:[/] [silver]Z80Emu <program.com>[/]");
    return -1;
}

return monitor.Run(args[0]);