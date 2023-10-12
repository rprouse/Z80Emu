using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public static class OpcodeTestExtensions
{
    public static Opcode FetchVerifyAndExecuteInstruction(this OpcodeHandler opcodeHandler, string expectedMnemonic)
    {
        var op = opcodeHandler.FetchInstruction();
        op.Should().NotBeNull();
        op.Mnemonic.Should().Be(expectedMnemonic);
        op.Execute();
        return op;
    }
}
