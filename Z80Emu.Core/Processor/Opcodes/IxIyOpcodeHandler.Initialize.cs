namespace Z80Emu.Core.Processor.Opcodes;

public partial class IxIyOpcodeHandler
{
    protected override Dictionary<byte, Opcode> Initialize()
    {
        var opcodes = new Dictionary<byte, Opcode>
        {
            { 0x09, new Opcode(0x09, "ADD IX,BC", () => throw new NotImplementedException() ) },
            { 0x19, new Opcode(0x19, "ADD IX,DE", () => throw new NotImplementedException() ) },
            { 0x21, new Opcode(0x21, "LD IX,nn", () => throw new NotImplementedException() ) },
            { 0x22, new Opcode(0x22, "LD (nn),IX", () => throw new NotImplementedException() ) },
            { 0x23, new Opcode(0x23, "INC IX", () => throw new NotImplementedException() ) },
            { 0x24, new Opcode(0x24, "INC IXh", () => throw new NotImplementedException() ) },
            { 0x25, new Opcode(0x25, "DEC IXl", () => throw new NotImplementedException() ) },
            { 0x26, new Opcode(0x26, "LD IXh,n", () => throw new NotImplementedException() ) },
            { 0x29, new Opcode(0x29, "ADD IX,IX", () => throw new NotImplementedException() ) },
            { 0x2A, new Opcode(0x2A, "LD IX,(nn)", () => throw new NotImplementedException() ) },
            { 0x2B, new Opcode(0x2B, "DEC IX", () => throw new NotImplementedException() ) },
            { 0x2C, new Opcode(0x2C, "INC IXl", () => throw new NotImplementedException() ) },
            { 0x2D, new Opcode(0x2D, "DEC IXh", () => throw new NotImplementedException() ) },
            { 0x2E, new Opcode(0x2E, "LD IXl,n", () => throw new NotImplementedException() ) },
            { 0x34, new Opcode(0x34, "INC (IX+o)", () => throw new NotImplementedException() ) },
            { 0x35, new Opcode(0x35, "DEC (IX+o)", () => throw new NotImplementedException() ) },
            { 0x36, new Opcode(0x36, "LD (IX+o),n", () => throw new NotImplementedException() ) },
            { 0x39, new Opcode(0x39, "ADD IX,SP", () => throw new NotImplementedException() ) },
            // IXl 5
            { 0x40, new Opcode(0x40, "LD B,IX+B", () => throw new NotImplementedException() ) },
            { 0x41, new Opcode(0x41, "LD B,IX+C", () => throw new NotImplementedException() ) },
            { 0x42, new Opcode(0x42, "LD B,IX+D", () => throw new NotImplementedException() ) },
            { 0x43, new Opcode(0x43, "LD B,IX+E", () => throw new NotImplementedException() ) },
            { 0x44, new Opcode(0x44, "LD B,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x45, new Opcode(0x45, "LD B,IX+IXl", () => throw new NotImplementedException() ) },
            { 0x46, new Opcode(0x46, "LD B,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x47, new Opcode(0x47, "LD B,IX+A", () => throw new NotImplementedException() ) },
            // DD 48+p
            { 0x48, new Opcode(0x48, "LD C,IX+B", () => throw new NotImplementedException() ) },
            { 0x49, new Opcode(0x49, "LD C,IX+C", () => throw new NotImplementedException() ) },
            { 0x4A, new Opcode(0x4A, "LD C,IX+D", () => throw new NotImplementedException() ) },
            { 0x4B, new Opcode(0x4B, "LD C,IX+E", () => throw new NotImplementedException() ) },
            { 0x4C, new Opcode(0x4C, "LD C,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x4D, new Opcode(0x4D, "LD C,IX+IXl", () => throw new NotImplementedException() ) },
            { 0x4E, new Opcode(0x4E, "LD C,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x4F, new Opcode(0x4F, "LD C,IX+A", () => throw new NotImplementedException() ) },
            // DD 50+p
            { 0x50, new Opcode(0x50, "LD D,IX+B", () => throw new NotImplementedException() ) },
            { 0x51, new Opcode(0x51, "LD D,IX+C", () => throw new NotImplementedException() ) },
            { 0x52, new Opcode(0x52, "LD D,IX+D", () => throw new NotImplementedException() ) },
            { 0x53, new Opcode(0x53, "LD D,IX+E", () => throw new NotImplementedException() ) },
            { 0x54, new Opcode(0x54, "LD D,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x55, new Opcode(0x55, "LD D,IX+IXl", () => throw new NotImplementedException() ) },
            { 0x56, new Opcode(0x56, "LD D,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x57, new Opcode(0x57, "LD D,IX+A", () => throw new NotImplementedException() ) },
            // DD 58+p
            { 0x58, new Opcode(0x58, "LD E,IX+B", () => throw new NotImplementedException() ) },
            { 0x59, new Opcode(0x59, "LD E,IX+C", () => throw new NotImplementedException() ) },
            { 0x5A, new Opcode(0x5A, "LD E,IX+D", () => throw new NotImplementedException() ) },
            { 0x5B, new Opcode(0x5B, "LD E,IX+E", () => throw new NotImplementedException() ) },
            { 0x5C, new Opcode(0x5C, "LD E,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x5D, new Opcode(0x5D, "LD E,IX+IXl", () => throw new NotImplementedException() ) },
            { 0x5E, new Opcode(0x5E, "LD E,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x5F, new Opcode(0x5F, "LD E,IX+A", () => throw new NotImplementedException() ) },
            // DD 60+p
            { 0x60, new Opcode(0x60, "LD IXh,B", () => throw new NotImplementedException() ) },
            { 0x61, new Opcode(0x61, "LD IXh,C", () => throw new NotImplementedException() ) },
            { 0x62, new Opcode(0x62, "LD IXh,D", () => throw new NotImplementedException() ) },
            { 0x63, new Opcode(0x63, "LD IXh,E", () => throw new NotImplementedException() ) },
            { 0x64, new Opcode(0x64, "LD IXh,IXh", () => throw new NotImplementedException() ) },
            { 0x65, new Opcode(0x65, "LD IXh,IXl", () => throw new NotImplementedException() ) },
            { 0x66, new Opcode(0x66, "LD H,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x67, new Opcode(0x67, "LD IXh,A", () => throw new NotImplementedException() ) },
            // DD 68+p
            { 0x68, new Opcode(0x68, "LD IXl,B", () => throw new NotImplementedException() ) },
            { 0x69, new Opcode(0x69, "LD IXl,C", () => throw new NotImplementedException() ) },
            { 0x6A, new Opcode(0x6A, "LD IXl,D", () => throw new NotImplementedException() ) },
            { 0x6B, new Opcode(0x6B, "LD IXl,E", () => throw new NotImplementedException() ) },
            { 0x6C, new Opcode(0x6C, "LD IXl,IXh", () => throw new NotImplementedException() ) },
            { 0x6D, new Opcode(0x6D, "LD IXl,IXl", () => throw new NotImplementedException() ) },
            { 0x6E, new Opcode(0x6E, "LD L,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x6F, new Opcode(0x6F, "LD IXl,A", () => throw new NotImplementedException() ) },

            // o 8-bit offset, 2’s complement.
            { 0x70, new Opcode(0x70, "LD (IX+o),B", () => throw new NotImplementedException() ) },
            { 0x71, new Opcode(0x71, "LD (IX+o),C", () => throw new NotImplementedException() ) },
            { 0x72, new Opcode(0x72, "LD (IX+o),D", () => throw new NotImplementedException() ) },
            { 0x73, new Opcode(0x73, "LD (IX+o),E", () => throw new NotImplementedException() ) },
            { 0x74, new Opcode(0x74, "LD (IX+o),H", () => throw new NotImplementedException() ) },
            { 0x75, new Opcode(0x75, "UNUSED", () => throw new NotImplementedException() ) },
            { 0x76, new Opcode(0x76, "LD (IX+o),L", () => throw new NotImplementedException() ) },
            { 0x77, new Opcode(0x77, "LD (IX+o),A", () => throw new NotImplementedException() ) },
            // DD 78+p
            { 0x78, new Opcode(0x78, "LD A,IX+B", () => throw new NotImplementedException() ) },
            { 0x79, new Opcode(0x79, "LD A,IX+C", () => throw new NotImplementedException() ) },
            { 0x7A, new Opcode(0x7A, "LD A,IX+D", () => throw new NotImplementedException() ) },
            { 0x7B, new Opcode(0x7B, "LD A,IX+E", () => throw new NotImplementedException() ) },
            { 0x7C, new Opcode(0x7C, "LD A,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x7D, new Opcode(0x7D, "LD A,IX+IXL", () => throw new NotImplementedException() ) },
            { 0x7E, new Opcode(0x7E, "LD A,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x7F, new Opcode(0x7F, "LD A,IX+A", () => throw new NotImplementedException() ) },
            // DD 80+p
            { 0x80, new Opcode(0x80, "ADD A,IX+B", () => throw new NotImplementedException() ) },
            { 0x81, new Opcode(0x81, "ADD A,IX+C", () => throw new NotImplementedException() ) },
            { 0x82, new Opcode(0x82, "ADD A,IX+D", () => throw new NotImplementedException() ) },
            { 0x83, new Opcode(0x83, "ADD A,IX+E", () => throw new NotImplementedException() ) },
            { 0x84, new Opcode(0x84, "ADD A,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x85, new Opcode(0x85, "ADD A,IX+IXL", () => throw new NotImplementedException() ) },
            { 0x86, new Opcode(0x86, "ADD A,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x87, new Opcode(0x87, "ADD A,IX+A", () => throw new NotImplementedException() ) },
            // DD 88+p
            { 0x88, new Opcode(0x88, "ADC A,IX+B", () => throw new NotImplementedException() ) },
            { 0x89, new Opcode(0x89, "ADC A,IX+C", () => throw new NotImplementedException() ) },
            { 0x8A, new Opcode(0x8A, "ADC A,IX+D", () => throw new NotImplementedException() ) },
            { 0x8B, new Opcode(0x8B, "ADC A,IX+E", () => throw new NotImplementedException() ) },
            { 0x8C, new Opcode(0x8C, "ADC A,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x8D, new Opcode(0x8D, "ADC A,IX+IXL", () => throw new NotImplementedException() ) },
            { 0x8E, new Opcode(0x8E, "ADC A,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x8F, new Opcode(0x8F, "ADC A,IX+A", () => throw new NotImplementedException() ) },
            // DD 90+p
            { 0x90, new Opcode(0x90, "SUB IX+B", () => throw new NotImplementedException() ) },
            { 0x91, new Opcode(0x91, "SUB IX+C", () => throw new NotImplementedException() ) },
            { 0x92, new Opcode(0x92, "SUB IX+D", () => throw new NotImplementedException() ) },
            { 0x93, new Opcode(0x93, "SUB IX+E", () => throw new NotImplementedException() ) },
            { 0x94, new Opcode(0x94, "SUB IX+IXh", () => throw new NotImplementedException() ) },
            { 0x95, new Opcode(0x95, "SUB IX+IXl", () => throw new NotImplementedException() ) },
            { 0x96, new Opcode(0x96, "SUB (IX+o)", () => throw new NotImplementedException() ) },
            { 0x97, new Opcode(0x97, "SUB IX+A", () => throw new NotImplementedException() ) },
            // DD 98+p
            { 0x98, new Opcode(0x98, "SBC A,IX+B", () => throw new NotImplementedException() ) },
            { 0x99, new Opcode(0x99, "SBC A,IX+C", () => throw new NotImplementedException() ) },
            { 0x9A, new Opcode(0x9A, "SBC A,IX+D", () => throw new NotImplementedException() ) },
            { 0x9B, new Opcode(0x9B, "SBC A,IX+E", () => throw new NotImplementedException() ) },
            { 0x9C, new Opcode(0x9C, "SBC A,IX+IXh", () => throw new NotImplementedException() ) },
            { 0x9D, new Opcode(0x9D, "SBC A,IX+IXL", () => throw new NotImplementedException() ) },
            { 0x9E, new Opcode(0x9E, "SBC A,(IX+o)", () => throw new NotImplementedException() ) },
            { 0x9F, new Opcode(0x9F, "SBC A,IX+A", () => throw new NotImplementedException() ) },
            // DD A0+p
            { 0xA0, new Opcode(0xA0, "AND IX+B", () => throw new NotImplementedException() ) },
            { 0xA1, new Opcode(0xA1, "AND IX+C", () => throw new NotImplementedException() ) },
            { 0xA2, new Opcode(0xA2, "AND IX+D", () => throw new NotImplementedException() ) },
            { 0xA3, new Opcode(0xA3, "AND IX+E", () => throw new NotImplementedException() ) },
            { 0xA4, new Opcode(0xA4, "AND IX+IXh", () => throw new NotImplementedException() ) },
            { 0xA5, new Opcode(0xA5, "AND IX+IXl", () => throw new NotImplementedException() ) },
            { 0xA6, new Opcode(0xA6, "AND (IX+o)", () => throw new NotImplementedException() ) },
            { 0xA7, new Opcode(0xA7, "AND IX+A", () => throw new NotImplementedException() ) },
            // DD A8+p
            { 0xA8, new Opcode(0xA8, "XOR IX+B", () => throw new NotImplementedException() ) },
            { 0xA9, new Opcode(0xA9, "XOR IX+C", () => throw new NotImplementedException() ) },
            { 0xAA, new Opcode(0xAA, "XOR IX+D", () => throw new NotImplementedException() ) },
            { 0xAB, new Opcode(0xAB, "XOR IX+E", () => throw new NotImplementedException() ) },
            { 0xAC, new Opcode(0xAC, "XOR IX+IXh", () => throw new NotImplementedException() ) },
            { 0xAD, new Opcode(0xAD, "XOR IX+IXl", () => throw new NotImplementedException() ) },
            { 0xAE, new Opcode(0xAE, "XOR (IX+o)", () => throw new NotImplementedException() ) },
            { 0xAF, new Opcode(0xAF, "XOR IX+A", () => throw new NotImplementedException() ) },
            // DD B0+p
            { 0xB0, new Opcode(0xB0, "OR IX+B", () => throw new NotImplementedException() ) },
            { 0xB1, new Opcode(0xB1, "OR IX+C", () => throw new NotImplementedException() ) },
            { 0xB2, new Opcode(0xB2, "OR IX+D", () => throw new NotImplementedException() ) },
            { 0xB3, new Opcode(0xB3, "OR IX+E", () => throw new NotImplementedException() ) },
            { 0xB4, new Opcode(0xB4, "OR IX+IXh", () => throw new NotImplementedException() ) },
            { 0xB5, new Opcode(0xB5, "OR IX+IXl", () => throw new NotImplementedException() ) },
            { 0xB6, new Opcode(0xB6, "OR (IX+o)", () => throw new NotImplementedException() ) },
            { 0xB7, new Opcode(0xB7, "OR IX+A", () => throw new NotImplementedException() ) },
            // DD B8+p
            { 0xB8, new Opcode(0xB8, "CP IX+B", () => throw new NotImplementedException() ) },
            { 0xB9, new Opcode(0xB9, "CP IX+C", () => throw new NotImplementedException() ) },
            { 0xBA, new Opcode(0xBA, "CP IX+D", () => throw new NotImplementedException() ) },
            { 0xBB, new Opcode(0xBB, "CP IX+E", () => throw new NotImplementedException() ) },
            { 0xBC, new Opcode(0xBC, "CP IX+IXh", () => throw new NotImplementedException() ) },
            { 0xBD, new Opcode(0xBD, "CP IX+IXl", () => throw new NotImplementedException() ) },
            { 0xBE, new Opcode(0xBE, "CP (IX+o)", () => throw new NotImplementedException() ) },
            { 0xBF, new Opcode(0xBF, "CP IX+A", () => throw new NotImplementedException() ) },

            // TODO: Need a "DD DB o XX" opcode handler
            { 0xCB, new Opcode(0xCB, "PREFIX CB", () => throw new NotImplementedException() ) },

            { 0xE1, new Opcode(0xE1, "POP IX", () => throw new NotImplementedException() ) },
            { 0xE3, new Opcode(0xE3, "EX (SP),IX", () => throw new NotImplementedException() ) },
            { 0xE5, new Opcode(0xE5, "PUSH IX", () => throw new NotImplementedException() ) },
            { 0xE9, new Opcode(0xE9, "JP (IX)", () => throw new NotImplementedException() ) },
            { 0xF9, new Opcode(0xF9, "LD SP,IX", () => throw new NotImplementedException() ) },
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
