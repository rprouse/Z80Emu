using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class EdOpcodeHandler
{
    protected override Dictionary<byte, Opcode> Initialize() => new Dictionary<byte, Opcode>
    {
        { 0x40, new Opcode(0x40, "IN B,(C)", () => throw new NotImplementedException() ) },
        { 0x41, new Opcode(0x41, "OUT (C),B", () => throw new NotImplementedException() ) },
        { 0x42, new Opcode(0x42, "SBC HL,BC", () => throw new NotImplementedException() ) },
        { 0x43, new Opcode(0x43, "LD (nn),BC", () => throw new NotImplementedException() ) },
        { 0x44, new Opcode(0x44, "NEG", () => throw new NotImplementedException() ) },
        { 0x45, new Opcode(0x45, "RETN", () => throw new NotImplementedException() ) },
        { 0x46, new Opcode(0x46, "IM 0", () => throw new NotImplementedException() ) },
        { 0x47, new Opcode(0x47, "LD I,A", () => throw new NotImplementedException() ) },
        { 0x48, new Opcode(0x48, "IN C,(C)", () => throw new NotImplementedException() ) },
        { 0x49, new Opcode(0x49, "OUT (C),C", () => throw new NotImplementedException() ) },
        { 0x4A, new Opcode(0x4A, "ADC HL,BC", () => throw new NotImplementedException() ) },
        { 0x4B, new Opcode(0x4B, "LD BC,(nn)", () => throw new NotImplementedException() ) },
        { 0x4D, new Opcode(0x4D, "RETI",
            () => {
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb);
                _int.Enable(false);
        } ) },
        { 0x4F, new Opcode(0x4F, "LD R,A", () => throw new NotImplementedException() ) },
        { 0x50, new Opcode(0x50, "IN D,(C)", () => throw new NotImplementedException() ) },
        { 0x51, new Opcode(0x51, "OUT (C),D", () => throw new NotImplementedException() ) },
        { 0x52, new Opcode(0x52, "SBC HL,DE", () => throw new NotImplementedException() ) },
        { 0x53, new Opcode(0x53, "LD (nn),DE", () => throw new NotImplementedException() ) },
        { 0x56, new Opcode(0x56, "IM 1", () => throw new NotImplementedException() ) },
        { 0x57, new Opcode(0x57, "LD A,I", () => throw new NotImplementedException() ) },
        { 0x58, new Opcode(0x58, "IN E,(C)", () => throw new NotImplementedException() ) },
        { 0x59, new Opcode(0x59, "OUT (C),E", () => throw new NotImplementedException() ) },
        { 0x5A, new Opcode(0x5A, "ADC HL,DE", () => throw new NotImplementedException() ) },
        { 0x5B, new Opcode(0x5B, "LD DE,(nn)", () => throw new NotImplementedException() ) },
        { 0x5E, new Opcode(0x5E, "IM 2", () => throw new NotImplementedException() ) },
        { 0x5F, new Opcode(0x5F, "LD A,R", () => throw new NotImplementedException() ) },
        { 0x60, new Opcode(0x60, "IN H,(C)", () => throw new NotImplementedException() ) },
        { 0x61, new Opcode(0x61, "OUT (C),H", () => throw new NotImplementedException() ) },
        { 0x62, new Opcode(0x62, "SBC HL,HL", () => throw new NotImplementedException() ) },
        { 0x67, new Opcode(0x67, "RRD", () => throw new NotImplementedException() ) },
        { 0x68, new Opcode(0x68, "IN L,(C)", () => throw new NotImplementedException() ) },
        { 0x69, new Opcode(0x69, "OUT (C),L", () => throw new NotImplementedException() ) },
        { 0x6A, new Opcode(0x6A, "ADC HL,HL", () => throw new NotImplementedException() ) },
        { 0x6F, new Opcode(0x6F, "RLD", () => throw new NotImplementedException() ) },
        { 0x70, new Opcode(0x70, "IN F,(C)", () => throw new NotImplementedException() ) },
        { 0x72, new Opcode(0x72, "SBC HL,SP", () => throw new NotImplementedException() ) },
        { 0x73, new Opcode(0x73, "LD (${0:X4}),SP",
            () => {
                _lsb = NextByte();
                _msb = NextByte();
                _address = BitUtils.ToWord(_msb, _lsb);
                _mmu[_address] = _reg.SP.Lsb();
                _mmu[_address + 1] = _reg.SP.Msb();
        } ) },
        { 0x78, new Opcode(0x78, "IN A,(C)", () => throw new NotImplementedException() ) },
        { 0x79, new Opcode(0x79, "OUT (C),A", () => throw new NotImplementedException() ) },
        { 0x7A, new Opcode(0x7A, "ADC HL,SP", () => throw new NotImplementedException() ) },
        { 0x7B, new Opcode(0x7B, "LD SP,(nn)", () => throw new NotImplementedException() ) },
        { 0xA0, new Opcode(0xA0, "LDI", () => throw new NotImplementedException() ) },
        { 0xA1, new Opcode(0xA1, "CPI", () => throw new NotImplementedException() ) },
        { 0xA2, new Opcode(0xA2, "INI", () => throw new NotImplementedException() ) },
        { 0xA3, new Opcode(0xA3, "OUTI", () => throw new NotImplementedException() ) },
        { 0xA8, new Opcode(0xA8, "LDD", () => throw new NotImplementedException() ) },
        { 0xA9, new Opcode(0xA9, "CPD", () => throw new NotImplementedException() ) },
        { 0xAA, new Opcode(0xAA, "IND", () => throw new NotImplementedException() ) },
        { 0xAB, new Opcode(0xAB, "OUTD", () => throw new NotImplementedException() ) },
        { 0xB0, new Opcode(0xB0, "LDIR", () => throw new NotImplementedException() ) },
        { 0xB1, new Opcode(0xB1, "CPIR", () => throw new NotImplementedException() ) },
        { 0xB2, new Opcode(0xB2, "INIR", () => throw new NotImplementedException() ) },
        { 0xB3, new Opcode(0xB3, "OTIR", () => throw new NotImplementedException() ) },
        { 0xB8, new Opcode(0xB8, "LDDR", () => throw new NotImplementedException() ) },
        { 0xB9, new Opcode(0xB9, "CPDR", () => throw new NotImplementedException() ) },
        { 0xBA, new Opcode(0xBA, "INDR", () => throw new NotImplementedException() ) },
        { 0xBB, new Opcode(0xBB, "OTDR", () => throw new NotImplementedException() ) },
        { 0xC1, new Opcode(0xC1, "MULUB A,B", () => throw new NotImplementedException() ) },
        { 0xC3, new Opcode(0xC3, "MULUW HL,BC", () => throw new NotImplementedException() ) },
        { 0xC9, new Opcode(0xC9, "MULUB A,C", () => throw new NotImplementedException() ) },
        { 0xD1, new Opcode(0xD1, "MULUB A,D", () => throw new NotImplementedException() ) },
        { 0xD9, new Opcode(0xD9, "MULUB A,E", () => throw new NotImplementedException() ) },
        { 0xE1, new Opcode(0xE1, "MULUB A,H", () => throw new NotImplementedException() ) },
        { 0xE9, new Opcode(0xE9, "MULUB A,L", () => throw new NotImplementedException() ) },
        { 0xF3, new Opcode(0xF3, "MULUW HL,SP", () => throw new NotImplementedException() ) },
        { 0xF9, new Opcode(0xF9, "MULUB A,A", () => throw new NotImplementedException() ) },
    };
}
