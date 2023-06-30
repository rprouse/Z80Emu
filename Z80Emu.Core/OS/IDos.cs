using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core.OS;

public interface IDos
{
    string Name { get; }
    IEnumerable<word> CallVectors { get; }
    void Execute(Emulator emulator);
}
