namespace Z80Emu.Core.Processor.Opcodes;

public partial class CbOpcodeHandler
{
    protected override Dictionary<byte, Opcode> Initialize() => new Dictionary<byte, Opcode>
    {
        // x8/rsb
        { 0x00, new Opcode(0x00, "RLC B", 
            () => { 
                _reg.B = RLC(_reg.B); 
            }
        ) },
        { 0x01, new Opcode(0x01, "RLC C", 
            () => { 
                _reg.C = RLC(_reg.C); 
            }
        ) },
        { 0x02, new Opcode(0x02, "RLC D", 
            () => { 
                _reg.D = RLC(_reg.D); 
            }
        ) },
        { 0x03, new Opcode(0x03, "RLC E", 
            () => { 
                _reg.E = RLC(_reg.E); 
            }
        ) },
        { 0x04, new Opcode(0x04, "RLC H", 
            () => { 
                _reg.H = RLC(_reg.H); 
            }
        ) },
        { 0x05, new Opcode(0x05, "RLC L", 
            () => { 
                _reg.L = RLC(_reg.L); 
            }
        ) },
        { 0x06, new Opcode(0x06, "RLC [HL]",
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = RLC(_operand); 
            }
        ) },
        { 0x07, new Opcode(0x07, "RLC A", 
            () => { 
                _reg.A = RLC(_reg.A); 
            }
        ) },
        { 0x08, new Opcode(0x08, "RRC B", 
            () => { 
                _reg.B = RRC(_reg.B); 
            }
        ) },
        { 0x09, new Opcode(0x09, "RRC C", 
            () => { 
                _reg.C = RRC(_reg.C); 
            }
        ) },
        { 0x0A, new Opcode(0x0A, "RRC D", 
            () => { 
                _reg.D = RRC(_reg.D); 
            }
        ) },
        { 0x0B, new Opcode(0x0B, "RRC E", 
            () => { 
                _reg.E = RRC(_reg.E); 
            }
        ) },
        { 0x0C, new Opcode(0x0C, "RRC H", 
            () => { 
                _reg.H = RRC(_reg.H); 
            }
        ) },
        { 0x0D, new Opcode(0x0D, "RRC L", 
            () => { 
                _reg.L = RRC(_reg.L); 
            }
        ) },
        { 0x0E, new Opcode(0x0E, "RRC [HL]",
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = RRC(_operand); 
            }
        ) },
        { 0x0F, new Opcode(0x0F, "RRC A", 
            () => { 
                _reg.A = RRC(_reg.A); 
            }
        ) },
        { 0x10, new Opcode(0x10, "RL B", 
            () => { 
                _reg.B = RL(_reg.B); 
            }
        ) },
        { 0x11, new Opcode(0x11, "RL C", 
            () => { 
                _reg.C = RL(_reg.C); 
            }
        ) },
        { 0x12, new Opcode(0x12, "RL D", 
            () => { 
                _reg.D = RL(_reg.D); 
            }
        ) },
        { 0x13, new Opcode(0x13, "RL E", 
            () => { 
                _reg.E = RL(_reg.E); 
            }
        ) },
        { 0x14, new Opcode(0x14, "RL H", 
            () => { 
                _reg.H = RL(_reg.H); 
            }
        ) },
        { 0x15, new Opcode(0x15, "RL L", 
            () => { 
                _reg.L = RL(_reg.L); 
            }
        ) },
        { 0x16, new Opcode(0x16, "RL [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = RL(_operand); 
            }
        ) },
        { 0x17, new Opcode(0x17, "RL A", 
            () => { 
                _reg.A = RL(_reg.A); 
            }
        ) },
        { 0x18, new Opcode(0x18, "RR B", 
            () => { 
                _reg.B = RR(_reg.B); 
            }
        ) },
        { 0x19, new Opcode(0x19, "RR C", 
            () => { 
                _reg.C = RR(_reg.C); 
            }
        ) },
        { 0x1A, new Opcode(0x1A, "RR D", 
            () => { 
                _reg.D = RR(_reg.D); 
            }
        ) },
        { 0x1B, new Opcode(0x1B, "RR E", 
            () => { 
                _reg.E = RR(_reg.E); 
            }
        ) },
        { 0x1C, new Opcode(0x1C, "RR H", 
            () => { 
                _reg.H = RR(_reg.H); 
            }
        ) },
        { 0x1D, new Opcode(0x1D, "RR L", 
            () => { 
                _reg.L = RR(_reg.L); 
            }
        ) },
        { 0x1E, new Opcode(0x1E, "RR [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = RR(_operand); 
            }
        ) },
        { 0x1F, new Opcode(0x1F, "RR A", 
            () => { 
                _reg.A = RR(_reg.A); 
            }
        ) },
        { 0x20, new Opcode(0x20, "SLA B", 
            () => { 
                _reg.B = SLA(_reg.B); 
            }
        ) },
        { 0x21, new Opcode(0x21, "SLA C", 
            () => { 
                _reg.C = SLA(_reg.C); 
            }
        ) },
        { 0x22, new Opcode(0x22, "SLA D", 
            () => { 
                _reg.D = SLA(_reg.D); 
            }
        ) },
        { 0x23, new Opcode(0x23, "SLA E", 
            () => { 
                _reg.E = SLA(_reg.E); 
            }
        ) },
        { 0x24, new Opcode(0x24, "SLA H", 
            () => { 
                _reg.H = SLA(_reg.H); 
            }
        ) },
        { 0x25, new Opcode(0x25, "SLA L", 
            () => { 
                _reg.L = SLA(_reg.L); 
            }
        ) },
        { 0x26, new Opcode(0x26, "SLA [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = SLA(_operand); 
            }
        ) },
        { 0x27, new Opcode(0x27, "SLA A", 
            () => { 
                _reg.A = SLA(_reg.A); 
            }
        ) },
        { 0x28, new Opcode(0x28, "SRA B", 
            () => { 
                _reg.B = SRA(_reg.B); 
            }
        ) },
        { 0x29, new Opcode(0x29, "SRA C", 
            () => { 
                _reg.C = SRA(_reg.C); 
            }
        ) },
        { 0x2A, new Opcode(0x2A, "SRA D", 
            () => { 
                _reg.D = SRA(_reg.D); 
            }
        ) },
        { 0x2B, new Opcode(0x2B, "SRA E", 
            () => { 
                _reg.E = SRA(_reg.E); 
            }
        ) },
        { 0x2C, new Opcode(0x2C, "SRA H", 
            () => { 
                _reg.H = SRA(_reg.H); 
            }
        ) },
        { 0x2D, new Opcode(0x2D, "SRA L", 
            () => { 
                _reg.L = SRA(_reg.L); 
            }
        ) },
        { 0x2E, new Opcode(0x2E, "SRA [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = SRA(_operand); 
            }
        ) },
        { 0x2F, new Opcode(0x2F, "SRA A", 
            () => { 
                _reg.A = SRA(_reg.A); 
            }
        ) },
        { 0x30, new Opcode(0x30, "SWAP B", 
            () => { 
                _reg.B = SWAP(_reg.B); 
            }
        ) },
        { 0x31, new Opcode(0x31, "SWAP C", 
            () => { 
                _reg.C = SWAP(_reg.C); 
            }
        ) },
        { 0x32, new Opcode(0x32, "SWAP D", 
            () => { 
                _reg.D = SWAP(_reg.D); 
            }
        ) },
        { 0x33, new Opcode(0x33, "SWAP E", 
            () => { 
                _reg.E = SWAP(_reg.E); 
            }
        ) },
        { 0x34, new Opcode(0x34, "SWAP H", 
            () => { 
                _reg.H = SWAP(_reg.H); 
            }
        ) },
        { 0x35, new Opcode(0x35, "SWAP L", 
            () => { 
                _reg.L = SWAP(_reg.L); 
            }
        ) },
        { 0x36, new Opcode(0x36, "SWAP [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = SWAP(_operand); 
            }
        ) },
        { 0x37, new Opcode(0x37, "SWAP A", 
            () => { 
                _reg.A = SWAP(_reg.A); 
            }
        ) },
        { 0x38, new Opcode(0x38, "SRL B", 
            () => { 
                _reg.B = SRL(_reg.B); 
            }
        ) },
        { 0x39, new Opcode(0x39, "SRL C", 
            () => { 
                _reg.C = SRL(_reg.C); 
            }
        ) },
        { 0x3A, new Opcode(0x3A, "SRL D", 
            () => { 
                _reg.D = SRL(_reg.D); 
            }
        ) },
        { 0x3B, new Opcode(0x3B, "SRL E", 
            () => { 
                _reg.E = SRL(_reg.E); 
            }
        ) },
        { 0x3C, new Opcode(0x3C, "SRL H", 
            () => { 
                _reg.H = SRL(_reg.H); 
            }
        ) },
        { 0x3D, new Opcode(0x3D, "SRL L", 
            () => { 
                _reg.L = SRL(_reg.L); 
            }
        ) },
        { 0x3E, new Opcode(0x3E, "SRL [HL]", 
            () => {
                _operand = _mmu[_reg.HL];
                _mmu[_reg.HL] = SRL(_operand); 
            }
        ) },
        { 0x3F, new Opcode(0x3F, "SRL A", 
            () => { 
                _reg.A = SRL(_reg.A); 
            }
        ) },
        { 0x40, new Opcode(0x40, "BIT 0,B", 
            () => { 
                BIT(0, _reg.B); 
            }
        ) },
        { 0x41, new Opcode(0x41, "BIT 0,C", 
            () => { 
                BIT(0, _reg.C); 
            }
        ) },
        { 0x42, new Opcode(0x42, "BIT 0,D", 
            () => { 
                BIT(0, _reg.D); 
            }
        ) },
        { 0x43, new Opcode(0x43, "BIT 0,E", 
            () => { 
                BIT(0, _reg.E); 
            }
        ) },
        { 0x44, new Opcode(0x44, "BIT 0,H", 
            () => { 
                BIT(0, _reg.H); 
            }
        ) },
        { 0x45, new Opcode(0x45, "BIT 0,L", 
            () => { 
                BIT(0, _reg.L); 
            }
        ) },
        { 0x46, new Opcode(0x46, "BIT 0,[HL]",
            () => { 
                BIT(0, _mmu[_reg.HL]);
            }
        ) },
        { 0x47, new Opcode(0x47, "BIT 0,A", 
            () => { 
                BIT(0, _reg.A); 
            }
        ) },
        { 0x48, new Opcode(0x48, "BIT 1,B", 
            () => { 
                BIT(1, _reg.B); 
            }
        ) },
        { 0x49, new Opcode(0x49, "BIT 1,C", 
            () => { 
                BIT(1, _reg.C); 
            }
        ) },
        { 0x4A, new Opcode(0x4A, "BIT 1,D", 
            () => { 
                BIT(1, _reg.D); 
            }
        ) },
        { 0x4B, new Opcode(0x4B, "BIT 1,E", 
            () => { 
                BIT(1, _reg.E); 
            }
        ) },
        { 0x4C, new Opcode(0x4C, "BIT 1,H", 
            () => { 
                BIT(1, _reg.H); 
            }
        ) },
        { 0x4D, new Opcode(0x4D, "BIT 1,L", 
            () => { 
                BIT(1, _reg.L); 
            }
        ) },
        { 0x4E, new Opcode(0x4E, "BIT 1,[HL]", 
            () => { 
                BIT(1, _mmu[_reg.HL]);
            }
        ) },
        { 0x4F, new Opcode(0x4F, "BIT 1,A", 
            () => { 
                BIT(1, _reg.A); 
            }
        ) },
        { 0x50, new Opcode(0x50, "BIT 2,B", 
            () => { 
                BIT(2, _reg.B); 
            }
        ) },
        { 0x51, new Opcode(0x51, "BIT 2,C", 
            () => { 
                BIT(2, _reg.C); 
            }
        ) },
        { 0x52, new Opcode(0x52, "BIT 2,D", 
            () => { 
                BIT(2, _reg.D); 
            }
        ) },
        { 0x53, new Opcode(0x53, "BIT 2,E", 
            () => { 
                BIT(2, _reg.E); 
            }
        ) },
        { 0x54, new Opcode(0x54, "BIT 2,H", 
            () => { 
                BIT(2, _reg.H); 
            }
        ) },
        { 0x55, new Opcode(0x55, "BIT 2,L", 
            () => { 
                BIT(2, _reg.L); 
            }
        ) },
        { 0x56, new Opcode(0x56, "BIT 2,[HL]", 
            () => { 
                BIT(2, _mmu[_reg.HL]);
            }
        ) },
        { 0x57, new Opcode(0x57, "BIT 2,A", 
            () => { 
                BIT(2, _reg.A); 
            }
        ) },
        { 0x58, new Opcode(0x58, "BIT 3,B", 
            () => { 
                BIT(3, _reg.B); 
            }
        ) },
        { 0x59, new Opcode(0x59, "BIT 3,C", 
            () => { 
                BIT(3, _reg.C); 
            }
        ) },
        { 0x5A, new Opcode(0x5A, "BIT 3,D", 
            () => { 
                BIT(3, _reg.D); 
            }
        ) },
        { 0x5B, new Opcode(0x5B, "BIT 3,E", 
            () => { 
                BIT(3, _reg.E); 
            }
        ) },
        { 0x5C, new Opcode(0x5C, "BIT 3,H", 
            () => { 
                BIT(3, _reg.H); 
            }
        ) },
        { 0x5D, new Opcode(0x5D, "BIT 3,L", 
            () => { 
                BIT(3, _reg.L); 
            }
        ) },
        { 0x5E, new Opcode(0x5E, "BIT 3,[HL]", 
            () => { 
                BIT(3, _mmu[_reg.HL]);
            }
        ) },
        { 0x5F, new Opcode(0x5F, "BIT 3,A", 
            () => { 
                BIT(3, _reg.A); 
            }
        ) },
        { 0x60, new Opcode(0x60, "BIT 4,B", 
            () => { 
                BIT(4, _reg.B); 
            }
        ) },
        { 0x61, new Opcode(0x61, "BIT 4,C", 
            () => { 
                BIT(4, _reg.C); 
            }
        ) },
        { 0x62, new Opcode(0x62, "BIT 4,D", 
            () => { 
                BIT(4, _reg.D); 
            }
        ) },
        { 0x63, new Opcode(0x63, "BIT 4,E", 
            () => { 
                BIT(4, _reg.E); 
            }
        ) },
        { 0x64, new Opcode(0x64, "BIT 4,H", 
            () => { 
                BIT(4, _reg.H); 
            }
        ) },
        { 0x65, new Opcode(0x65, "BIT 4,L", 
            () => { 
                BIT(4, _reg.L); 
            }
        ) },
        { 0x66, new Opcode(0x66, "BIT 4,[HL]", 
            () => { 
                BIT(4, _mmu[_reg.HL]);
            }
        ) },
        { 0x67, new Opcode(0x67, "BIT 4,A", 
            () => { 
                BIT(4, _reg.A); 
            }
        ) },
        { 0x68, new Opcode(0x68, "BIT 5,B", 
            () => { 
                BIT(5, _reg.B); 
            }
        ) },
        { 0x69, new Opcode(0x69, "BIT 5,C", 
            () => { 
                BIT(5, _reg.C); 
            }
        ) },
        { 0x6A, new Opcode(0x6A, "BIT 5,D", 
            () => { 
                BIT(5, _reg.D); 
            }
        ) },
        { 0x6B, new Opcode(0x6B, "BIT 5,E", 
            () => { 
                BIT(5, _reg.E); 
            }
        ) },
        { 0x6C, new Opcode(0x6C, "BIT 5,H", 
            () => { 
                BIT(5, _reg.H); 
            }
        ) },
        { 0x6D, new Opcode(0x6D, "BIT 5,L", 
            () => { 
                BIT(5, _reg.L); 
            }
        ) },
        { 0x6E, new Opcode(0x6E, "BIT 5,[HL]", 
            () => { 
                BIT(5, _mmu[_reg.HL]);
            }
        ) },
        { 0x6F, new Opcode(0x6F, "BIT 5,A", 
            () => { 
                BIT(5, _reg.A); 
            }
        ) },
        { 0x70, new Opcode(0x70, "BIT 6,B", 
            () => { 
                BIT(6, _reg.B); 
            }
        ) },
        { 0x71, new Opcode(0x71, "BIT 6,C", 
            () => { 
                BIT(6, _reg.C); 
            }
        ) },
        { 0x72, new Opcode(0x72, "BIT 6,D", 
            () => { 
                BIT(6, _reg.D); 
            }
        ) },
        { 0x73, new Opcode(0x73, "BIT 6,E", 
            () => { 
                BIT(6, _reg.E); 
            }
        ) },
        { 0x74, new Opcode(0x74, "BIT 6,H", 
            () => { 
                BIT(6, _reg.H); 
            }
        ) },
        { 0x75, new Opcode(0x75, "BIT 6,L", 
            () => { 
                BIT(6, _reg.L); 
            }
        ) },
        { 0x76, new Opcode(0x76, "BIT 6,[HL]", 
            () => { 
                BIT(6, _mmu[_reg.HL]); 
            }
        ) },
        { 0x77, new Opcode(0x77, "BIT 6,A", 
            () => { 
                BIT(6, _reg.A); 
            }
        ) },
        { 0x78, new Opcode(0x78, "BIT 7,B", 
            () => { 
                BIT(7, _reg.B); 
            }
        ) },
        { 0x79, new Opcode(0x79, "BIT 7,C", 
            () => { 
                BIT(7, _reg.C); 
            }
        ) },
        { 0x7A, new Opcode(0x7A, "BIT 7,D", 
            () => { 
                BIT(7, _reg.D); 
            }
        ) },
        { 0x7B, new Opcode(0x7B, "BIT 7,E", 
            () => { 
                BIT(7, _reg.E); 
            }
        ) },
        { 0x7C, new Opcode(0x7C, "BIT 7,H", 
            () => { 
                BIT(7, _reg.H); 
            }
        ) },
        { 0x7D, new Opcode(0x7D, "BIT 7,L", 
            () => { 
                BIT(7, _reg.L); 
            }
        ) },
        { 0x7E, new Opcode(0x7E, "BIT 7,[HL]", 
            () => { 
                BIT(7, _mmu[_reg.HL]);
            }
        ) },
        { 0x7F, new Opcode(0x7F, "BIT 7,A", 
            () => { 
                BIT(7, _reg.A); 
            }
        ) },
        { 0x80, new Opcode(0x80, "RES 0,B", 
            () => { 
                _reg.B = RES(0, _reg.B); 
            }
        ) },
        { 0x81, new Opcode(0x81, "RES 0,C", 
            () => { 
                _reg.C = RES(0, _reg.C); 
            }
        ) },
        { 0x82, new Opcode(0x82, "RES 0,D", 
            () => { 
                _reg.D = RES(0, _reg.D); 
            }
        ) },
        { 0x83, new Opcode(0x83, "RES 0,E", 
            () => { 
                _reg.E = RES(0, _reg.E); 
            }
        ) },
        { 0x84, new Opcode(0x84, "RES 0,H", 
            () => { 
                _reg.H = RES(0, _reg.H); 
            }
        ) },
        { 0x85, new Opcode(0x85, "RES 0,L", 
            () => { 
                _reg.L = RES(0, _reg.L); 
            }
        ) },
        { 0x86, new Opcode(0x86, "RES 0,[HL]", 
            () => {
                _result = RES(0, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0x87, new Opcode(0x87, "RES 0,A", 
            () => { 
                _reg.A = RES(0, _reg.A); 
            }
        ) },
        { 0x88, new Opcode(0x88, "RES 1,B", 
            () => { 
                _reg.B = RES(1, _reg.B); 
            }
        ) },
        { 0x89, new Opcode(0x89, "RES 1,C", 
            () => { 
                _reg.C = RES(1, _reg.C); 
            }
        ) },
        { 0x8A, new Opcode(0x8A, "RES 1,D", 
            () => { 
                _reg.D = RES(1, _reg.D); 
            }
        ) },
        { 0x8B, new Opcode(0x8B, "RES 1,E", 
            () => { 
                _reg.E = RES(1, _reg.E); 
            }
        ) },
        { 0x8C, new Opcode(0x8C, "RES 1,H", 
            () => { 
                _reg.H = RES(1, _reg.H); 
            }
        ) },
        { 0x8D, new Opcode(0x8D, "RES 1,L", 
            () => { 
                _reg.L = RES(1, _reg.L); 
            }
        ) },
        { 0x8E, new Opcode(0x8E, "RES 1,[HL]", 
            () => {
                _result = RES(1, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0x8F, new Opcode(0x8F, "RES 1,A", 
            () => { 
                _reg.A = RES(1, _reg.A); 
            }
        ) },
        { 0x90, new Opcode(0x90, "RES 2,B", 
            () => { 
                _reg.B = RES(2, _reg.B); 
            }
        ) },
        { 0x91, new Opcode(0x91, "RES 2,C", 
            () => { 
                _reg.C = RES(2, _reg.C); 
            }
        ) },
        { 0x92, new Opcode(0x92, "RES 2,D", 
            () => { 
                _reg.D = RES(2, _reg.D); 
            }
        ) },
        { 0x93, new Opcode(0x93, "RES 2,E", 
            () => { 
                _reg.E = RES(2, _reg.E); 
            }
        ) },
        { 0x94, new Opcode(0x94, "RES 2,H", 
            () => { 
                _reg.H = RES(2, _reg.H); 
            }
        ) },
        { 0x95, new Opcode(0x95, "RES 2,L", 
            () => { 
                _reg.L = RES(2, _reg.L); 
            }
        ) },
        { 0x96, new Opcode(0x96, "RES 2,[HL]", 
            () => {
                _result = RES(2, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0x97, new Opcode(0x97, "RES 2,A", 
            () => { 
                _reg.A = RES(2, _reg.A); 
            }
        ) },
        { 0x98, new Opcode(0x98, "RES 3,B", 
            () => { 
                _reg.B = RES(3, _reg.B); 
            }
        ) },
        { 0x99, new Opcode(0x99, "RES 3,C", 
            () => { 
                _reg.C = RES(3, _reg.C); 
            }
        ) },
        { 0x9A, new Opcode(0x9A, "RES 3,D", 
            () => { 
                _reg.D = RES(3, _reg.D); 
            }
        ) },
        { 0x9B, new Opcode(0x9B, "RES 3,E", 
            () => { 
                _reg.E = RES(3, _reg.E); 
            }
        ) },
        { 0x9C, new Opcode(0x9C, "RES 3,H", 
            () => { 
                _reg.H = RES(3, _reg.H); 
            }
        ) },
        { 0x9D, new Opcode(0x9D, "RES 3,L", 
            () => { 
                _reg.L = RES(3, _reg.L); 
            }
        ) },
        { 0x9E, new Opcode(0x9E, "RES 3,[HL]", 
            () => {
                _result = RES(3, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0x9F, new Opcode(0x9F, "RES 3,A", 
            () => { 
                _reg.A = RES(3, _reg.A); 
            }
        ) },
        { 0xA0, new Opcode(0xA0, "RES 4,B", 
            () => { 
                _reg.B = RES(4, _reg.B); 
            }
        ) },
        { 0xA1, new Opcode(0xA1, "RES 4,C", 
            () => { 
                _reg.C = RES(4, _reg.C); 
            }
        ) },
        { 0xA2, new Opcode(0xA2, "RES 4,D", 
            () => { 
                _reg.D = RES(4, _reg.D); 
            }
        ) },
        { 0xA3, new Opcode(0xA3, "RES 4,E", 
            () => { 
                _reg.E = RES(4, _reg.E); 
            }
        ) },
        { 0xA4, new Opcode(0xA4, "RES 4,H", 
            () => { 
                _reg.H = RES(4, _reg.H); 
            }
        ) },
        { 0xA5, new Opcode(0xA5, "RES 4,L", 
            () => { 
                _reg.L = RES(4, _reg.L); 
            }
        ) },
        { 0xA6, new Opcode(0xA6, "RES 4,[HL]", 
            () => {
                _result = RES(4, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xA7, new Opcode(0xA7, "RES 4,A", 
            () => { 
                _reg.A = RES(4, _reg.A); 
            }
        ) },
        { 0xA8, new Opcode(0xA8, "RES 5,B", 
            () => { 
                _reg.B = RES(5, _reg.B); 
            }
        ) },
        { 0xA9, new Opcode(0xA9, "RES 5,C", 
            () => { 
                _reg.C = RES(5, _reg.C); 
            }
        ) },
        { 0xAA, new Opcode(0xAA, "RES 5,D", 
            () => { 
                _reg.D = RES(5, _reg.D); 
            }
        ) },
        { 0xAB, new Opcode(0xAB, "RES 5,E", 
            () => { 
                _reg.E = RES(5, _reg.E); 
            }
        ) },
        { 0xAC, new Opcode(0xAC, "RES 5,H", 
            () => { 
                _reg.H = RES(5, _reg.H); 
            }
        ) },
        { 0xAD, new Opcode(0xAD, "RES 5,L", 
            () => { 
                _reg.L = RES(5, _reg.L); 
            }
        ) },
        { 0xAE, new Opcode(0xAE, "RES 5,[HL]", 
            () => {
                _result = RES(5, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xAF, new Opcode(0xAF, "RES 5,A", 
            () => { 
                _reg.A = RES(5, _reg.A); 
            }
        ) },
        { 0xB0, new Opcode(0xB0, "RES 6,B", 
            () => { 
                _reg.B = RES(6, _reg.B); 
            }
        ) },
        { 0xB1, new Opcode(0xB1, "RES 6,C", 
            () => { 
                _reg.C = RES(6, _reg.C); 
            }
        ) },
        { 0xB2, new Opcode(0xB2, "RES 6,D", 
            () => { 
                _reg.D = RES(6, _reg.D); 
            }
        ) },
        { 0xB3, new Opcode(0xB3, "RES 6,E", 
            () => { 
                _reg.E = RES(6, _reg.E); 
            }
        ) },
        { 0xB4, new Opcode(0xB4, "RES 6,H", 
            () => { 
                _reg.H = RES(6, _reg.H); 
            }
        ) },
        { 0xB5, new Opcode(0xB5, "RES 6,L", 
            () => { 
                _reg.L = RES(6, _reg.L); 
            }
        ) },
        { 0xB6, new Opcode(0xB6, "RES 6,[HL]", 
            () => {
                _result = RES(6, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xB7, new Opcode(0xB7, "RES 6,A", 
            () => { 
                _reg.A = RES(6, _reg.A); 
            }
        ) },
        { 0xB8, new Opcode(0xB8, "RES 7,B", 
            () => { 
                _reg.B = RES(7, _reg.B); 
            }
        ) },
        { 0xB9, new Opcode(0xB9, "RES 7,C", 
            () => { 
                _reg.C = RES(7, _reg.C); 
            }
        ) },
        { 0xBA, new Opcode(0xBA, "RES 7,D", 
            () => { 
                _reg.D = RES(7, _reg.D); 
            }
        ) },
        { 0xBB, new Opcode(0xBB, "RES 7,E", 
            () => { 
                _reg.E = RES(7, _reg.E); 
            }
        ) },
        { 0xBC, new Opcode(0xBC, "RES 7,H", 
            () => { 
                _reg.H = RES(7, _reg.H); 
            }
        ) },
        { 0xBD, new Opcode(0xBD, "RES 7,L", 
            () => { 
                _reg.L = RES(7, _reg.L); 
            }
        ) },
        { 0xBE, new Opcode(0xBE, "RES 7,[HL]", 
            () => {
                _result = RES(7, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xBF, new Opcode(0xBF, "RES 7,A", 
            () => { 
                _reg.A = RES(7, _reg.A); 
            }
        ) },
        { 0xC0, new Opcode(0xC0, "SET 0,B", 
            () => { 
                _reg.B = SET(0, _reg.B); 
            }
        ) },
        { 0xC1, new Opcode(0xC1, "SET 0,C", 
            () => { 
                _reg.C = SET(0, _reg.C); 
            }
        ) },
        { 0xC2, new Opcode(0xC2, "SET 0,D", 
            () => { 
                _reg.D = SET(0, _reg.D); 
            }
        ) },
        { 0xC3, new Opcode(0xC3, "SET 0,E", 
            () => { 
                _reg.E = SET(0, _reg.E); 
            }
        ) },
        { 0xC4, new Opcode(0xC4, "SET 0,H", 
            () => { 
                _reg.H = SET(0, _reg.H); 
            }
        ) },
        { 0xC5, new Opcode(0xC5, "SET 0,L", 
            () => { 
                _reg.L = SET(0, _reg.L); 
            }
        ) },
        { 0xC6, new Opcode(0xC6, "SET 0,[HL]", 
            () => {
                _result = SET(0, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xC7, new Opcode(0xC7, "SET 0,A", 
            () => { 
                _reg.A = SET(1, _reg.A); 
            }
        ) },
        { 0xC8, new Opcode(0xC8, "SET 1,B", 
            () => { 
                _reg.B = SET(1, _reg.B); 
            }
        ) },
        { 0xC9, new Opcode(0xC9, "SET 1,C", 
            () => { 
                _reg.C = SET(1, _reg.C); 
            }
        ) },
        { 0xCA, new Opcode(0xCA, "SET 1,D", 
            () => { 
                _reg.D = SET(1, _reg.D); 
            }
        ) },
        { 0xCB, new Opcode(0xCB, "SET 1,E", 
            () => { 
                _reg.E = SET(1, _reg.E); 
            }
        ) },
        { 0xCC, new Opcode(0xCC, "SET 1,H", 
            () => { 
                _reg.H = SET(1, _reg.H); 
            }
        ) },
        { 0xCD, new Opcode(0xCD, "SET 1,L", 
            () => { 
                _reg.L = SET(1, _reg.L); 
            }
        ) },
        { 0xCE, new Opcode(0xCE, "SET 1,[HL]", 
            () => {
                _result = SET(1, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xCF, new Opcode(0xCF, "SET 1,A", 
            () => { 
                _reg.A = SET(1, _reg.A); 
            }
        ) },
        { 0xD0, new Opcode(0xD0, "SET 2,B", 
            () => { 
                _reg.B = SET(2, _reg.B); 
            }
        ) },
        { 0xD1, new Opcode(0xD1, "SET 2,C", 
            () => { 
                _reg.C = SET(2, _reg.C); 
            }
        ) },
        { 0xD2, new Opcode(0xD2, "SET 2,D", 
            () => { 
                _reg.D = SET(2, _reg.D); 
            }
        ) },
        { 0xD3, new Opcode(0xD3, "SET 2,E", 
            () => { 
                _reg.E = SET(2, _reg.E); 
            }
        ) },
        { 0xD4, new Opcode(0xD4, "SET 2,H", 
            () => { 
                _reg.H = SET(2, _reg.H); 
            }
        ) },
        { 0xD5, new Opcode(0xD5, "SET 2,L", 
            () => { 
                _reg.L = SET(2, _reg.L); 
            }
        ) },
        { 0xD6, new Opcode(0xD6, "SET 2,[HL]", 
            () => {
                _result = SET(2, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xD7, new Opcode(0xD7, "SET 2,A", 
            () => { 
                _reg.A = SET(2, _reg.A); 
            }
        ) },
        { 0xD8, new Opcode(0xD8, "SET 3,B", 
            () => { 
                _reg.B = SET(3, _reg.B); 
            }
        ) },
        { 0xD9, new Opcode(0xD9, "SET 3,C", 
            () => { 
                _reg.C = SET(3, _reg.C); 
            }
        ) },
        { 0xDA, new Opcode(0xDA, "SET 3,D", 
            () => { 
                _reg.D = SET(3, _reg.D); 
            }
        ) },
        { 0xDB, new Opcode(0xDB, "SET 3,E", 
            () => { 
                _reg.E = SET(3, _reg.E); 
            }
        ) },
        { 0xDC, new Opcode(0xDC, "SET 3,H", 
            () => { 
                _reg.H = SET(3, _reg.H); 
            }
        ) },
        { 0xDD, new Opcode(0xDD, "SET 3,L", 
            () => { 
                _reg.L = SET(3, _reg.L); 
            }
        ) },
        { 0xDE, new Opcode(0xDE, "SET 3,[HL]", 
            () => {
                _result = SET(3, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xDF, new Opcode(0xDF, "SET 3,A", 
            () => { 
                _reg.A = SET(3, _reg.A); 
            }
        ) },
        { 0xE0, new Opcode(0xE0, "SET 4,B", 
            () => { 
                _reg.B = SET(4, _reg.B); 
            }
        ) },
        { 0xE1, new Opcode(0xE1, "SET 4,C", 
            () => { 
                _reg.C = SET(4, _reg.C); 
            }
        ) },
        { 0xE2, new Opcode(0xE2, "SET 4,D", 
            () => { 
                _reg.D = SET(4, _reg.D); 
            }
        ) },
        { 0xE3, new Opcode(0xE3, "SET 4,E", 
            () => { 
                _reg.E = SET(4, _reg.E); 
            }
        ) },
        { 0xE4, new Opcode(0xE4, "SET 4,H", 
            () => { 
                _reg.H = SET(4, _reg.H); 
            }
        ) },
        { 0xE5, new Opcode(0xE5, "SET 4,L", 
            () => { 
                _reg.L = SET(4, _reg.L); 
            }
        ) },
        { 0xE6, new Opcode(0xE6, "SET 4,[HL]", 
            () => {
                _result = SET(4, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xE7, new Opcode(0xE7, "SET 4,A", 
            () => { 
                _reg.A = SET(4, _reg.A); 
            }
        ) },
        { 0xE8, new Opcode(0xE8, "SET 5,B", 
            () => { 
                _reg.B = SET(5, _reg.B); 
            }
        ) },
        { 0xE9, new Opcode(0xE9, "SET 5,C", 
            () => { 
                _reg.C = SET(5, _reg.C); 
            }
        ) },
        { 0xEA, new Opcode(0xEA, "SET 5,D", 
            () => { 
                _reg.D = SET(5, _reg.D); 
            }
        ) },
        { 0xEB, new Opcode(0xEB, "SET 5,E", 
            () => { 
                _reg.E = SET(5, _reg.E); 
            }
        ) },
        { 0xEC, new Opcode(0xEC, "SET 5,H", 
            () => { 
                _reg.H = SET(5, _reg.H); 
            }
        ) },
        { 0xED, new Opcode(0xED, "SET 5,L", 
            () => { 
                _reg.L = SET(5, _reg.L); 
            }
        ) },
        { 0xEE, new Opcode(0xEE, "SET 5,[HL]", 
            () => {
                _result = SET(5, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xEF, new Opcode(0xEF, "SET 5,A", 
            () => { 
                _reg.A = SET(5, _reg.A); 
            }
        ) },
        { 0xF0, new Opcode(0xF0, "SET 6,B", 
            () => { 
                _reg.B = SET(6, _reg.B); 
            }
        ) },
        { 0xF1, new Opcode(0xF1, "SET 6,C", 
            () => { 
                _reg.C = SET(6, _reg.C); 
            }
        ) },
        { 0xF2, new Opcode(0xF2, "SET 6,D", 
            () => { 
                _reg.D = SET(6, _reg.D); 
            }
        ) },
        { 0xF3, new Opcode(0xF3, "SET 6,E", 
            () => { 
                _reg.E = SET(6, _reg.E); 
            }
        ) },
        { 0xF4, new Opcode(0xF4, "SET 6,H", 
            () => { 
                _reg.H = SET(6, _reg.H); 
            }
        ) },
        { 0xF5, new Opcode(0xF5, "SET 6,L", 
            () => { 
                _reg.L = SET(6, _reg.L); 
            }
        ) },
        { 0xF6, new Opcode(0xF6, "SET 6,[HL]", 
            () => {
                _result = SET(6, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xF7, new Opcode(0xF7, "SET 6,A", 
            () => { 
                _reg.A = SET(6, _reg.A); 
            }
        ) },
        { 0xF8, new Opcode(0xF8, "SET 7,B", 
            () => { 
                _reg.B = SET(7, _reg.B); 
            }
        ) },
        { 0xF9, new Opcode(0xF9, "SET 7,C", 
            () => { 
                _reg.C = SET(7, _reg.C); 
            }
        ) },
        { 0xFA, new Opcode(0xFA, "SET 7,D", 
            () => { 
                _reg.D = SET(7, _reg.D); 
            }
        ) },
        { 0xFB, new Opcode(0xFB, "SET 7,E", 
            () => { 
                _reg.E = SET(7, _reg.E); 
            }
        ) },
        { 0xFC, new Opcode(0xFC, "SET 7,H", 
            () => { 
                _reg.H = SET(7, _reg.H); 
            }
        ) },
        { 0xFD, new Opcode(0xFD, "SET 7,L", 
            () => { 
                _reg.L = SET(7, _reg.L); 
            }
        ) },
        { 0xFE, new Opcode(0xFE, "SET 7,[HL]", 
            () => {
                _result = SET(7, _mmu[_reg.HL]);
                _mmu[_reg.HL] = _result; 
            }
        ) },
        { 0xFF, new Opcode(0xFF, "SET 7,A", 
            () => { 
                _reg.A = SET(7, _reg.A); 
            }
        ) },

    };
}

