using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor.Opcodes;

/// <summary>
/// Handles the IX and IY opcodes
/// </summary>
public partial class IxIyOpcodeHandler : BaseOpcodeHandler
{
    private readonly CbOpcodeHandler _cbOpcodeHandler;

    private string _register;
    private Func<word> Get { get; }
    private Action<word> Set { get; }

    public IxIyOpcodeHandler(Registers registers, MMU mmu, Interupts interupts, string register, Func<word> get, Action<word> set)
        : base(registers, mmu, interupts)
    {
        _cbOpcodeHandler = new CbOpcodeHandler(registers, mmu, interupts, register, get, set);
        _register = register;
        Get = get;
        Set = set;
    }

    public override Opcode FetchInstruction()
    {
        var opcode = base.FetchInstruction();

        return opcode.Value switch
        {
            0xCB => _cbOpcodeHandler.FetchInstruction(),
            _ => opcode,
        };
    }

    public class CbOpcodeHandler : BaseOpcodeHandler
    {
        private string _register;
        private Func<word> Get { get; }
        private Action<word> Set { get; }

        public CbOpcodeHandler(Registers registers, MMU mmu, Interupts interupts, string register, Func<word> get, Action<word> set)
            : base(registers, mmu, interupts)
        {
            _register = register;
            Get = get;
            Set = set;
        }

        protected override Dictionary<byte, Opcode> Initialize()
        {
            var opcodes = new Dictionary<byte, Opcode>
            {
                { 0x06, new Opcode(0x06, "RLC (IX+o)", () => throw new NotImplementedException() ) },
                { 0x0E, new Opcode(0x0E, "RRC (IX+o)", () => throw new NotImplementedException() ) },
                { 0x16, new Opcode(0x16, "RL (IX+o)", () => throw new NotImplementedException() ) },
                { 0x1E, new Opcode(0x1E, "RR (IX+o)", () => throw new NotImplementedException() ) },
                { 0x26, new Opcode(0x26, "SLA (IX+o)", () => throw new NotImplementedException() ) },
                { 0x2E, new Opcode(0x2E, "SRA (IX+o)", () => throw new NotImplementedException() ) },
                { 0x3E, new Opcode(0x3E, "SRL (IX+o)", () => throw new NotImplementedException() ) },

                { 0x46, new Opcode(0x46, "BIT 0,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x4E, new Opcode(0x4E, "BIT 1,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x56, new Opcode(0x56, "BIT 2,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x5E, new Opcode(0x5E, "BIT 3,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x66, new Opcode(0x66, "BIT 4,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x6E, new Opcode(0x6E, "BIT 5,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x76, new Opcode(0x76, "BIT 6,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x7E, new Opcode(0x7E, "BIT 7,(IX+o)", () => throw new NotImplementedException() ) },

                { 0x86, new Opcode(0x86, "RES 0,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x8E, new Opcode(0x8E, "RES 1,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x96, new Opcode(0x96, "RES 2,(IX+o)", () => throw new NotImplementedException() ) },
                { 0x9E, new Opcode(0x9E, "RES 3,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xA6, new Opcode(0xA6, "RES 4,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xAE, new Opcode(0xAE, "RES 5,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xB6, new Opcode(0xB6, "RES 6,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xBE, new Opcode(0xBE, "RES 7,(IX+o)", () => throw new NotImplementedException() ) },

                { 0xC6, new Opcode(0xC6, "SET 0,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xCE, new Opcode(0xCE, "SET 1,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xD6, new Opcode(0xD6, "SET 2,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xDE, new Opcode(0xDE, "SET 3,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xE6, new Opcode(0xE6, "SET 4,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xEE, new Opcode(0xEE, "SET 5,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xF6, new Opcode(0xF6, "SET 6,(IX+o)", () => throw new NotImplementedException() ) },
                { 0xFE, new Opcode(0xFE, "SET 7,(IX+o)", () => throw new NotImplementedException() ) },
            };

            // Hack to easily support IX and IY from the same class
            if (_register == "IY")
            {
                foreach (var opcode in opcodes.Values)
                {
                    opcode.Name = opcode.Name.Replace("IX", _register);
                }
            }
            return opcodes;
        }
    }
}
