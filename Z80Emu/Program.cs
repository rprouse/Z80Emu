using Z80Emu.Core;

var gameboy = new GameBoy();

while(true)
{
    gameboy.Tick();
}

Console.ReadLine();