using Z80Emu.Core;

var emulator = new Emulator();
if (!emulator.LoadProgram(@"../../../../bin/8BitAdd.com"))
{
    Console.WriteLine("File not found");
    return -1;
}

while(true)
{
    emulator.Tick();
    Console.WriteLine(emulator.ToString());
    Console.ReadLine();
}