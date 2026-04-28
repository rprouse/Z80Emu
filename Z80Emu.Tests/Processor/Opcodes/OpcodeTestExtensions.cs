using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public static class OpcodeTestExtensions
{
    public static Opcode FetchVerifyAndExecuteInstruction(this OpcodeHandler opcodeHandler, string expectedMnemonic)
    {
        var op = opcodeHandler.FetchInstruction();
        op.ShouldNotBeNull();
        op.Mnemonic.ShouldBe(expectedMnemonic);
        op.Execute();
        return op;
    }
}
