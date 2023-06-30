using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Core.OS;

public interface IDos
{
    /// <summary>
    /// The name of the operating system
    /// </summary>
    string Name { get; }

    /// <summary>
    /// The call vectors for the operating system
    /// that the emulator will use to call the OS
    /// </summary>
    IEnumerable<word> CallVectors { get; }

    /// <summary>
    /// Execute a system call
    /// </summary>
    /// <param name="emulator"></param>
    void Execute(Emulator emulator);

    /// <summary>
    /// Initialize the operating system
    /// </summary>
    /// <param name="emulator"></param>
    void Initialize(Emulator emulator);
}
