using Z80Emu.Core;

var gameboy = new Emulator();

while(true)
{
    gameboy.Tick();
}

Console.ReadLine();