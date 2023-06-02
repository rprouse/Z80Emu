using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class OpcodeHandler
{
    protected override Dictionary<byte, Opcode> Initialize() => new Dictionary<byte, Opcode>
    {
        // control/br
        { 0x18, new Opcode(0x18, "JR {0}", () => { _reg.PC = (word)(_reg.PC + (sbyte)NextByte()); } ) },
        { 0x20, new Opcode(0x20, "JR NZ,{0}", 
            () => { 
                _operand = NextByte(); 
                if (_reg.FlagZ) return;
                _reg.PC = (word)(_reg.PC + (sbyte)_operand); 
        } ) },
        { 0x28, new Opcode(0x28, "JR Z,{0}", 
            () => { 
                _operand = NextByte(); 
                if (!_reg.FlagZ) return;
                _reg.PC = (word)(_reg.PC + (sbyte)_operand); 
        } ) },
        { 0x30, new Opcode(0x30, "JR NC,{0}", 
            () => { 
                _operand = NextByte(); 
                if (_reg.FlagC) return;
                _reg.PC = (word)(_reg.PC + (sbyte)_operand); 
        } ) },
        { 0x38, new Opcode(0x38, "JR C,{0}", 
            () => { 
                _operand = NextByte(); 
                if (!_reg.FlagC) return;
                _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        } ) },
        { 0xC0, new Opcode(0xC0, "RET NZ",  
            () => { 
                if (_reg.FlagZ) return;
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC2, new Opcode(0xC2, "JP NZ,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (_reg.FlagZ) return;
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC3, new Opcode(0xC3, "JP ${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC4, new Opcode(0xC4, "CALL NZ,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (_reg.FlagZ);
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC7, new Opcode(0xC7, "RST 00h", () => RST(0x00) ) },
        { 0xC8, new Opcode(0xC8, "RET Z",  
            () => { 
                if (!_reg.FlagZ) return;
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC9, new Opcode(0xC9, "RET", 
            () => { 
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xCA, new Opcode(0xCA, "JP Z,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (!_reg.FlagZ) return;
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xCC, new Opcode(0xCC, "CALL Z,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (!_reg.FlagZ) return;
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xCD, new Opcode(0xCD, "CALL ${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xCF, new Opcode(0xCF, "RST 08h", () => RST(0x08) ) },
        { 0xD0, new Opcode(0xD0, "RET NC",  
            () => { 
                if (_reg.FlagC) return;
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xD2, new Opcode(0xD2, "JP NC,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (_reg.FlagC) return;
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xD4, new Opcode(0xD4, "CALL NC,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (_reg.FlagC) return;
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xD7, new Opcode(0xD7, "RST 10h", () => RST(0x10) ) },
        { 0xD8, new Opcode(0xD8, "RET C",  
            () => { 
                if (!_reg.FlagC) return;
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xD9, new Opcode(0xD9, "RETI", 
            () => { 
                _lsb = _mmu[_reg.SP++];
                _msb = _mmu[_reg.SP++];
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
                _int.Enable(false); 
        } ) },
        { 0xDA, new Opcode(0xDA, "JP C,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (!_reg.FlagC) return;
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xDC, new Opcode(0xDC, "CALL C,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                if (!_reg.FlagC) return;
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
                _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
                _reg.PC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xDF, new Opcode(0xDF, "RST 18h", () => RST(0x18) ) },
        { 0xE7, new Opcode(0xE7, "RST 20h", () => RST(0x20) ) },
        { 0xE9, new Opcode(0xE9, "JP HL", () => { _reg.PC = _mmu[_reg.HL]; } ) },
        { 0xEF, new Opcode(0xEF, "RST 28h", () => RST(0x28) ) },
        { 0xF7, new Opcode(0xF7, "RST 30h", () => RST(0x30) ) },
        { 0xFF, new Opcode(0xFF, "RST 38h", () => RST(0x38) ) },

        // control/misc
        { 0x00, new Opcode(0x00, "NOP", () => { } ) },
        { 0x10, new Opcode(0x10, "STOP", () => { throw new NotImplementedException(); } ) },
        { 0x76, new Opcode(0x76, "HALT", () => { _reg.PC--; } ) },
        { 0xCB, new Opcode(0xCB, "PREFIX CB", () => { } ) },
        { 0xF3, new Opcode(0xF3, "DI", () => { _int.Disable(); } ) },
        { 0xFB, new Opcode(0xFB, "EI", () => { _int.Enable(true); } ) },

        // unused
        { 0xD3, new Opcode(0xD3, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xDB, new Opcode(0xDB, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xDD, new Opcode(0xDD, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xE3, new Opcode(0xE3, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xE4, new Opcode(0xE4, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xEB, new Opcode(0xEB, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xEC, new Opcode(0xEC, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xED, new Opcode(0xED, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xF4, new Opcode(0xF4, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xFC, new Opcode(0xFC, "UNUSED",() => { throw new NotImplementedException(); } ) },
        { 0xFD, new Opcode(0xFD, "UNUSED",() => { throw new NotImplementedException(); } ) },

        // x16/alu
        { 0x03, new Opcode(0x03, "INC BC", () => { _reg.BC++; } ) },
        { 0x09, new Opcode(0x09, "ADD HL,BC", () => { ADDHL(_reg.BC); } ) },
        { 0x0B, new Opcode(0x0B, "DEC BC", () => { _reg.BC--; } ) },
        { 0x13, new Opcode(0x13, "INC DE", () => { _reg.DE++; } ) },
        { 0x19, new Opcode(0x19, "ADD HL,DE", () => { ADDHL(_reg.DE); } ) },
        { 0x1B, new Opcode(0x1B, "DEC DE", () => { _reg.DE--; } ) },
        { 0x23, new Opcode(0x23, "INC HL", () => { _reg.HL++; } ) },
        { 0x29, new Opcode(0x29, "ADD HL,HL", () => { ADDHL(_reg.HL); } ) },
        { 0x2B, new Opcode(0x2B, "DEC HL", () => { _reg.HL--; } ) },
        { 0x33, new Opcode(0x33, "INC SP", () => { _reg.SP++; } ) },
        { 0x39, new Opcode(0x39, "ADD HL,SP", () => { ADDHL(_reg.SP); } ) },
        { 0x3B, new Opcode(0x3B, "DEC SP", () => { _reg.SP--; } ) },
        { 0xE8, new Opcode(0xE8, "ADD SP,${0:X2}", () => { ADDSP(NextByte()); } ) },
        { 0xF8, new Opcode(0xF8, "LD HL,SP+${0:X2}", 
            () => { 
                _reg.HL = (word)(_reg.SP + (sbyte)NextByte()); 
        } ) },

        // x16/lsm
        { 0x01, new Opcode(0x01, "LD BC,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                _reg.BC = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0x08, new Opcode(0x08, "LD (${0:X4}),SP", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                _address = BitUtils.ToWord(_msb, _lsb);
                _mmu[_address] = _reg.SP.Lsb();
                _mmu[_address + 1] = _reg.SP.Msb(); 
        } ) },
        { 0x11, new Opcode(0x11, "LD DE,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                _reg.DE = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0x21, new Opcode(0x21, "LD HL,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                _reg.HL = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0x31, new Opcode(0x31, "LD SP,${0:X4}", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte(); 
                _reg.SP = BitUtils.ToWord(_msb, _lsb); 
        } ) },
        { 0xC1, new Opcode(0xC1, "POP BC", 
            () => { 
                _reg.C = _mmu[_reg.SP++];
                _reg.B = _mmu[_reg.SP++]; 
        } ) },
        { 0xC5, new Opcode(0xC5, "PUSH BC", 
            () => {
                _mmu[--_reg.SP] = _reg.B;
                _mmu[--_reg.SP] = _reg.C; 
        } ) },
        { 0xD1, new Opcode(0xD1, "POP DE", 
            () => { 
                _reg.E = _mmu[_reg.SP++];
                _reg.D = _mmu[_reg.SP++]; 
        } ) },
        { 0xD5, new Opcode(0xD5, "PUSH DE", 
            () => {
                _mmu[--_reg.SP] = _reg.D;
                _mmu[--_reg.SP] = _reg.E; 
        } ) },
        { 0xE1, new Opcode(0xE1, "POP HL", 
            () => { 
                _reg.L = _mmu[_reg.SP++];
                _reg.H = _mmu[_reg.SP++]; 
        } ) },
        { 0xE5, new Opcode(0xE5, "PUSH HL", 
            () => {
                _mmu[--_reg.SP] = _reg.H;
                _mmu[--_reg.SP] = _reg.L; 
        } ) },
        { 0xF1, new Opcode(0xF1, "POP AF", 
            () => { 
                _reg.F = _mmu[_reg.SP++];
                _reg.A = _mmu[_reg.SP++]; 
        } ) },
        { 0xF5, new Opcode(0xF5, "PUSH AF", 
            () => {
                _mmu[--_reg.SP] = _reg.A;
                _mmu[--_reg.SP] = _reg.F; 
        } ) },
        { 0xF9, new Opcode(0xF9, "LD SP,HL", () => { _reg.SP = _reg.HL; } ) },

        // x8/alu
        { 0x04, new Opcode(0x04, "INC B", () => { _reg.B = INC(_reg.B); } ) },
        { 0x05, new Opcode(0x05, "DEC B", () => { _reg.B = DEC(_reg.B); } ) },
        { 0x0C, new Opcode(0x0C, "INC C", () => { _reg.C = INC(_reg.C); } ) },
        { 0x0D, new Opcode(0x0D, "DEC C", () => { _reg.C = DEC(_reg.C); } ) },
        { 0x14, new Opcode(0x14, "INC D", () => { _reg.D = INC(_reg.D); } ) },
        { 0x15, new Opcode(0x15, "DEC D", () => { _reg.D = DEC(_reg.D); } ) },
        { 0x1C, new Opcode(0x1C, "INC E", () => { _reg.E = INC(_reg.E); } ) },
        { 0x1D, new Opcode(0x1D, "DEC E", () => { _reg.E = DEC(_reg.E); } ) },
        { 0x24, new Opcode(0x24, "INC H", () => { _reg.H = INC(_reg.H); } ) },
        { 0x25, new Opcode(0x25, "DEC H", () => { _reg.H = DEC(_reg.H); } ) },
        { 0x27, new Opcode(0x27, "DAA", () => { DAA(); } ) },
        { 0x2C, new Opcode(0x2C, "INC L", () => { _reg.L = INC(_reg.L); } ) },
        { 0x2D, new Opcode(0x2D, "DEC L", () => { _reg.L = DEC(_reg.L); } ) },
        { 0x2F, new Opcode(0x2F, "CPL", 
            () => { 
                _reg.A = (byte)~_reg.A; 
                _reg.FlagN = true; 
                _reg.FlagH = true; 
        } ) },
        { 0x34, new Opcode(0x34, "INC (HL)", 
            () => { 
                 _operand = INC(_mmu[_reg.HL]);
                _mmu[_reg.HL] = _operand; 
        } ) },
        { 0x35, new Opcode(0x35, "DEC (HL)", 
            () => { 
                 _operand = DEC(_mmu[_reg.HL]);
                _mmu[_reg.HL] = _operand; 
        } ) },
        { 0x37, new Opcode(0x37, "SCF", 
            () => { 
                _reg.FlagN = false; 
                _reg.FlagH = false; 
                _reg.FlagC = true; 
        } ) },
        { 0x3C, new Opcode(0x3C, "INC A", () => { _reg.A = INC(_reg.A); } ) },
        { 0x3D, new Opcode(0x3D, "DEC A", () => { _reg.A = DEC(_reg.A); } ) },
        { 0x3F, new Opcode(0x3F, "CCF", 
            () => { 
                _reg.FlagC = !_reg.FlagC; 
                _reg.FlagN = false; 
                _reg.FlagH = false; 
        } ) },
        { 0x80, new Opcode(0x80, "ADD A,B", () => { ADD(_reg.B); } ) },
        { 0x81, new Opcode(0x81, "ADD A,C", () => { ADD(_reg.C); } ) },
        { 0x82, new Opcode(0x82, "ADD A,D", () => { ADD(_reg.D); } ) },
        { 0x83, new Opcode(0x83, "ADD A,E", () => { ADD(_reg.E); } ) },
        { 0x84, new Opcode(0x84, "ADD A,H", () => { ADD(_reg.H); } ) },
        { 0x85, new Opcode(0x85, "ADD A,L", () => { ADD(_reg.L); } ) },
        { 0x86, new Opcode(0x86, "ADD A,(HL)", () => { ADD(_mmu[_reg.HL]); } ) },
        { 0x87, new Opcode(0x87, "ADD A,A", () => { ADD(_reg.A); } ) },
        { 0x88, new Opcode(0x88, "ADC A,B", () => { ADC(_reg.B); } ) },
        { 0x89, new Opcode(0x89, "ADC A,C", () => { ADC(_reg.C); } ) },
        { 0x8A, new Opcode(0x8A, "ADC A,D", () => { ADC(_reg.D); } ) },
        { 0x8B, new Opcode(0x8B, "ADC A,E", () => { ADC(_reg.E); } ) },
        { 0x8C, new Opcode(0x8C, "ADC A,H", () => { ADC(_reg.H); } ) },
        { 0x8D, new Opcode(0x8D, "ADC A,L", () => { ADC(_reg.L); } ) },
        { 0x8E, new Opcode(0x8E, "ADC A,(HL)", () => { ADC(_mmu[_reg.HL]); } ) },
        { 0x8F, new Opcode(0x8F, "ADC A,A", () => { ADC(_reg.A); } ) },
        { 0x90, new Opcode(0x90, "SUB A,B", () => { SUB(_reg.B); } ) },
        { 0x91, new Opcode(0x91, "SUB A,C", () => { SUB(_reg.C); } ) },
        { 0x92, new Opcode(0x92, "SUB A,D", () => { SUB(_reg.D); } ) },
        { 0x93, new Opcode(0x93, "SUB A,E", () => { SUB(_reg.E); } ) },
        { 0x94, new Opcode(0x94, "SUB A,H", () => { SUB(_reg.H); } ) },
        { 0x95, new Opcode(0x95, "SUB A,L", () => { SUB(_reg.L); } ) },
        { 0x96, new Opcode(0x96, "SUB A,(HL)", () => { SUB(_mmu[_reg.HL]); } ) },
        { 0x97, new Opcode(0x97, "SUB A,A", () => { SUB(_reg.A); } ) },
        { 0x98, new Opcode(0x98, "SBC A,B", () => { SBC(_reg.B); } ) },
        { 0x99, new Opcode(0x99, "SBC A,C", () => { SBC(_reg.C); } ) },
        { 0x9A, new Opcode(0x9A, "SBC A,D", () => { SBC(_reg.D); } ) },
        { 0x9B, new Opcode(0x9B, "SBC A,E", () => { SBC(_reg.E); } ) },
        { 0x9C, new Opcode(0x9C, "SBC A,H", () => { SBC(_reg.H); } ) },
        { 0x9D, new Opcode(0x9D, "SBC A,L", () => { SBC(_reg.L); } ) },
        { 0x9E, new Opcode(0x9E, "SBC A,(HL)", () => { SBC(_mmu[_reg.HL]); } ) },
        { 0x9F, new Opcode(0x9F, "SBC A,A", () => { SBC(_reg.A); } ) },
        { 0xA0, new Opcode(0xA0, "AND B", () => { AND(_reg.B); } ) },
        { 0xA1, new Opcode(0xA1, "AND C", () => { AND(_reg.C); } ) },
        { 0xA2, new Opcode(0xA2, "AND D", () => { AND(_reg.D); } ) },
        { 0xA3, new Opcode(0xA3, "AND E", () => { AND(_reg.E); } ) },
        { 0xA4, new Opcode(0xA4, "AND H", () => { AND(_reg.H); } ) },
        { 0xA5, new Opcode(0xA5, "AND L", () => { AND(_reg.L); } ) },
        { 0xA6, new Opcode(0xA6, "AND (HL)", () => { AND(_mmu[_reg.HL]); } ) },
        { 0xA7, new Opcode(0xA7, "AND A", () => { AND(_reg.A); } ) },
        { 0xA8, new Opcode(0xA8, "XOR B", () => { XOR(_reg.B); } ) },
        { 0xA9, new Opcode(0xA9, "XOR C", () => { XOR(_reg.C); } ) },
        { 0xAA, new Opcode(0xAA, "XOR D", () => { XOR(_reg.D); } ) },
        { 0xAB, new Opcode(0xAB, "XOR E", () => { XOR(_reg.E); } ) },
        { 0xAC, new Opcode(0xAC, "XOR H", () => { XOR(_reg.H); } ) },
        { 0xAD, new Opcode(0xAD, "XOR L", () => { XOR(_reg.L); } ) },
        { 0xAE, new Opcode(0xAE, "XOR (HL)", () => { XOR(_mmu[_reg.HL]); } ) },
        { 0xAF, new Opcode(0xAF, "XOR A", () => { XOR(_reg.A); } ) },
        { 0xB0, new Opcode(0xB0, "OR B", () => { OR(_reg.B); } ) },
        { 0xB1, new Opcode(0xB1, "OR C", () => { OR(_reg.C); } ) },
        { 0xB2, new Opcode(0xB2, "OR D", () => { OR(_reg.D); } ) },
        { 0xB3, new Opcode(0xB3, "OR E", () => { OR(_reg.E); } ) },
        { 0xB4, new Opcode(0xB4, "OR H", () => { OR(_reg.H); } ) },
        { 0xB5, new Opcode(0xB5, "OR L", () => { OR(_reg.L); } ) },
        { 0xB6, new Opcode(0xB6, "OR (HL)", () => { OR(_mmu[_reg.HL]); } ) },
        { 0xB7, new Opcode(0xB7, "OR A", () => { OR(_reg.A); } ) },
        { 0xB8, new Opcode(0xB8, "CP A,B", () => { CP(_reg.B); } ) },
        { 0xB9, new Opcode(0xB9, "CP A,C", () => { CP(_reg.C); } ) },
        { 0xBA, new Opcode(0xBA, "CP A,D", () => { CP(_reg.D); } ) },
        { 0xBB, new Opcode(0xBB, "CP A,E", () => { CP(_reg.E); } ) },
        { 0xBC, new Opcode(0xBC, "CP A,H", () => { CP(_reg.H); } ) },
        { 0xBD, new Opcode(0xBD, "CP A,L", () => { CP(_reg.L); } ) },
        { 0xBE, new Opcode(0xBE, "CP A,(HL)", () => { CP(_mmu[_reg.HL]); } ) },
        { 0xBF, new Opcode(0xBF, "CP A,A", () => { CP(_reg.A); } ) },
        { 0xC6, new Opcode(0xC6, "ADD A,${0:X2}", () => { ADD(NextByte()); } ) },
        { 0xCE, new Opcode(0xCE, "ADC A,${0:X2}", () => { ADC(NextByte()); } ) },
        { 0xD6, new Opcode(0xD6, "SUB A,${0:X2}", () => { SUB(NextByte()); } ) },
        { 0xDE, new Opcode(0xDE, "SBC A,${0:X2}", () => { SBC(NextByte()); } ) },
        { 0xE6, new Opcode(0xE6, "AND ${0:X2}", () => { AND(NextByte()); } ) },
        { 0xEE, new Opcode(0xEE, "XOR ${0:X2}", () => { XOR(NextByte()); } ) },
        { 0xF6, new Opcode(0xF6, "OR ${0:X2}", () => { OR(NextByte()); } ) },
        { 0xFE, new Opcode(0xFE, "CP A,${0:X2}", () => { CP(NextByte()); } ) },

        // x8/lsm
        { 0x02, new Opcode(0x02, "LD (BC),A", () => { _mmu[_reg.BC] = _reg.A; } ) },
        { 0x06, new Opcode(0x06, "LD B,${0:X2}", () => { _reg.B = NextByte(); } ) },
        { 0x0A, new Opcode(0x0A, "LD A,(BC)", () => { _reg.A = _mmu[_reg.BC]; } ) },
        { 0x0E, new Opcode(0x0E, "LD C,${0:X2}", () => { _reg.C = NextByte(); } ) },
        { 0x12, new Opcode(0x12, "LD (DE),A", () => { _mmu[_reg.DE] = _reg.A; } ) },
        { 0x16, new Opcode(0x16, "LD D,${0:X2}", () => { _reg.D = NextByte(); } ) },
        { 0x1A, new Opcode(0x1A, "LD A,(DE)", () => { _reg.A = _mmu[_reg.DE]; } ) },
        { 0x1E, new Opcode(0x1E, "LD E,${0:X2}", () => { _reg.E = NextByte(); } ) },
        { 0x22, new Opcode(0x22, "LD [HLI],A", () => { _reg.A = _mmu[_reg.HL++]; } ) },
        { 0x26, new Opcode(0x26, "LD H,${0:X2}", () => { _reg.H = NextByte(); } ) },
        { 0x2A, new Opcode(0x2A, "LD A,[HLI]", () => { _reg.A = _mmu[_reg.HL++]; } ) },
        { 0x2E, new Opcode(0x2E, "LD L,${0:X2}", () => { _reg.L = NextByte(); } ) },
        { 0x32, new Opcode(0x32, "LD (HLD),A", () => { _mmu[_reg.HL--] = _reg.A; } ) },
        { 0x36, new Opcode(0x36, "LD (HL),${0:X2}", () => { _mmu[_reg.HL] = NextByte(); } ) },
        { 0x3A, new Opcode(0x3A, "LD A,(HLD)", () => { _mmu[_reg.HL--] = _reg.A; } ) },
        { 0x3E, new Opcode(0x3E, "LD A,${0:X2}", () => { _reg.A = NextByte(); } ) },
        { 0x40, new Opcode(0x40, "LD B,B", () => { } ) },
        { 0x41, new Opcode(0x41, "LD B,C", () => { _reg.B = _reg.C; } ) },
        { 0x42, new Opcode(0x42, "LD B,D", () => { _reg.B = _reg.D; } ) },
        { 0x43, new Opcode(0x43, "LD B,E", () => { _reg.B = _reg.E; } ) },
        { 0x44, new Opcode(0x44, "LD B,H", () => { _reg.B = _reg.H; } ) },
        { 0x45, new Opcode(0x45, "LD B,L", () => { _reg.B = _reg.L; } ) },
        { 0x46, new Opcode(0x46, "LD B,(HL)", () => { _reg.B = _mmu[_reg.HL]; } ) },
        { 0x47, new Opcode(0x47, "LD B,A", () => { _reg.B = _reg.A; } ) },
        { 0x48, new Opcode(0x48, "LD C,B", () => { _reg.C = _reg.B; } ) },
        { 0x49, new Opcode(0x49, "LD C,C", () => { } ) },
        { 0x4A, new Opcode(0x4A, "LD C,D", () => { _reg.C = _reg.D; } ) },
        { 0x4B, new Opcode(0x4B, "LD C,E", () => { _reg.C = _reg.E; } ) },
        { 0x4C, new Opcode(0x4C, "LD C,H", () => { _reg.C = _reg.H; } ) },
        { 0x4D, new Opcode(0x4D, "LD C,L", () => { _reg.C = _reg.L; } ) },
        { 0x4E, new Opcode(0x4E, "LD C,(HL)", () => { _reg.C = _mmu[_reg.HL]; } ) },
        { 0x4F, new Opcode(0x4F, "LD C,A", () => { _reg.C = _reg.A; } ) },
        { 0x50, new Opcode(0x50, "LD D,B", () => { _reg.D = _reg.B; } ) },
        { 0x51, new Opcode(0x51, "LD D,C", () => { _reg.D = _reg.C; } ) },
        { 0x52, new Opcode(0x52, "LD D,D", () => { } ) },
        { 0x53, new Opcode(0x53, "LD D,E", () => { _reg.D = _reg.E; } ) },
        { 0x54, new Opcode(0x54, "LD D,H", () => { _reg.D = _reg.H; } ) },
        { 0x55, new Opcode(0x55, "LD D,L", () => { _reg.D = _reg.L; } ) },
        { 0x56, new Opcode(0x56, "LD D,(HL)", () => { _reg.D = _mmu[_reg.HL]; } ) },
        { 0x57, new Opcode(0x57, "LD D,A", () => { _reg.D = _reg.A; } ) },
        { 0x58, new Opcode(0x58, "LD E,B", () => { _reg.E = _reg.B; } ) },
        { 0x59, new Opcode(0x59, "LD E,C", () => { _reg.E = _reg.C; } ) },
        { 0x5A, new Opcode(0x5A, "LD E,D", () => { _reg.E = _reg.D; } ) },
        { 0x5B, new Opcode(0x5B, "LD E,E", () => { } ) },
        { 0x5C, new Opcode(0x5C, "LD E,H", () => { _reg.E = _reg.H; } ) },
        { 0x5D, new Opcode(0x5D, "LD E,L", () => { _reg.E = _reg.L; } ) },
        { 0x5E, new Opcode(0x5E, "LD E,(HL)", () => { _reg.E = _mmu[_reg.HL]; } ) },
        { 0x5F, new Opcode(0x5F, "LD E,A", () => { _reg.E = _reg.A; } ) },
        { 0x60, new Opcode(0x60, "LD H,B", () => { _reg.H = _reg.B; } ) },
        { 0x61, new Opcode(0x61, "LD H,C", () => { _reg.H = _reg.C; } ) },
        { 0x62, new Opcode(0x62, "LD H,D", () => { _reg.H = _reg.D; } ) },
        { 0x63, new Opcode(0x63, "LD H,E", () => { _reg.H = _reg.E; } ) },
        { 0x64, new Opcode(0x64, "LD H,H", () => { } ) },
        { 0x65, new Opcode(0x65, "LD H,L", () => { _reg.H = _reg.L; } ) },
        { 0x66, new Opcode(0x66, "LD H,(HL)", () => { _reg.H = _mmu[_reg.HL]; } ) },
        { 0x67, new Opcode(0x67, "LD H,A", () => { _reg.H = _reg.A; } ) },
        { 0x68, new Opcode(0x68, "LD L,B", () => { _reg.L = _reg.B; } ) },
        { 0x69, new Opcode(0x69, "LD L,C", () => { _reg.L = _reg.C; } ) },
        { 0x6A, new Opcode(0x6A, "LD L,D", () => { _reg.L = _reg.D; } ) },
        { 0x6B, new Opcode(0x6B, "LD L,E", () => { _reg.L = _reg.E; } ) },
        { 0x6C, new Opcode(0x6C, "LD L,H", () => { _reg.L = _reg.H; } ) },
        { 0x6D, new Opcode(0x6D, "LD L,L", () => { } ) },
        { 0x6E, new Opcode(0x6E, "LD L,(HL)", () => { _reg.L = _mmu[_reg.HL]; } ) },
        { 0x6F, new Opcode(0x6F, "LD L,A", () => { _reg.L = _reg.A; } ) },
        { 0x70, new Opcode(0x70, "LD (HL),B", () => { _mmu[_reg.HL] = _reg.B; } ) },
        { 0x71, new Opcode(0x71, "LD (HL),C", () => { _mmu[_reg.HL] = _reg.C; } ) },
        { 0x72, new Opcode(0x72, "LD (HL),D", () => { _mmu[_reg.HL] = _reg.D; } ) },
        { 0x73, new Opcode(0x73, "LD (HL),E", () => { _mmu[_reg.HL] = _reg.E; } ) },
        { 0x74, new Opcode(0x74, "LD (HL),H", () => { _mmu[_reg.HL] = _reg.H; } ) },
        { 0x75, new Opcode(0x75, "LD (HL),L", () => { _mmu[_reg.HL] = _reg.L; } ) },
        { 0x77, new Opcode(0x77, "LD (HL),A", () => { _mmu[_reg.HL] = _reg.A; } ) },
        { 0x78, new Opcode(0x78, "LD A,B", () => { _reg.A = _reg.B; } ) },
        { 0x79, new Opcode(0x79, "LD A,C", () => { _reg.A = _reg.C; } ) },
        { 0x7A, new Opcode(0x7A, "LD A,D", () => { _reg.A = _reg.D; } ) },
        { 0x7B, new Opcode(0x7B, "LD A,E", () => { _reg.A = _reg.E; } ) },
        { 0x7C, new Opcode(0x7C, "LD A,H", () => { _reg.A = _reg.H; } ) },
        { 0x7D, new Opcode(0x7D, "LD A,L", () => { _reg.A = _reg.L; } ) },
        { 0x7E, new Opcode(0x7E, "LD A,(HL)", () => { _reg.A = _mmu[_reg.HL]; } ) },
        { 0x7F, new Opcode(0x7F, "LD A,A", () => { } ) },
        // LDH - Put memory address $FF00+n into A
        { 0xE0, new Opcode(0xE0, "LDH [${0:X2}],A", () => { _mmu[0xFF00 + NextByte()] = _reg.A; } ) },
        { 0xE2, new Opcode(0xE2, "LDH [C],A", () => { _mmu[0xFF00 + _reg.C] = _reg.A; } ) },
        { 0xEA, new Opcode(0xEA, "LD [${0:X4}],A", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte();
                _mmu[BitUtils.ToWord(_msb, _lsb)] = _reg.A; 
        } ) },
        { 0xF0, new Opcode(0xF0, "LDH A,[${0:X2}]", () => { _reg.A = _mmu[0xFF00 + NextByte()]; } ) },
        { 0xF2, new Opcode(0xF2, "LDH A,[C]", () => { _reg.A = _mmu[0xFF00 + _reg.C]; } ) },
        { 0xFA, new Opcode(0xFA, "LD A,[${0:X4}]", 
            () => { 
                _lsb = NextByte();
                _msb = NextByte();
                _reg.A = _mmu[BitUtils.ToWord(_msb, _lsb)]; 
        } ) },

        // x8/rsb
        { 0x07, new Opcode(0x07, "RLCA", 
            () => { 
                _reg.A = RLC(_reg.A); 
                _reg.FlagZ = false; 
        } ) },
        { 0x0F, new Opcode(0x0F, "RRCA", 
            () => { 
                _reg.A = RRC(_reg.A); 
                _reg.FlagZ = false; 
        } ) },
        { 0x17, new Opcode(0x17, "RLA", 
            () => { 
                _reg.A = RL(_reg.A); 
                _reg.FlagZ = false; 
        } ) },
        { 0x1F, new Opcode(0x1F, "RRA", 
            () => { 
                _reg.A = RR(_reg.A); 
                _reg.FlagZ = false; 
        } ) },
    };
}
