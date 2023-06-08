using Z80Emu.Core.Memory;
using Z80Emu.Core.Utilities;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class OpcodeHandler
{
    private void InitializeMethods()
    {
        _opcodes["8E ADC A,(HL)"].Execute = () => { ADC(_mmu[_reg.HL]); };
        _opcodes["DD ADC A,(IX+d)"].Execute = () => { ADC(_mmu[(word)(_reg.IX + (sbyte)NextByte())]); };
        _opcodes["FD ADC A,(IY+d)"].Execute = () => { ADC(_mmu[(word)(_reg.IY + (sbyte)NextByte())]); };
        _opcodes["CE ADC A,n"].Execute = () => ADC(NextByte());
        _opcodes["88 ADC A,B"].Execute = () => ADC(_reg.B);
        _opcodes["89 ADC A,C"].Execute = () => ADC(_reg.C);
        _opcodes["8A ADC A,D"].Execute = () => ADC(_reg.D);
        _opcodes["8B ADC A,E"].Execute = () => ADC(_reg.E);
        _opcodes["8C ADC A,H"].Execute = () => ADC(_reg.H);
        _opcodes["8D ADC A,L"].Execute = () => ADC(_reg.L);
        _opcodes["8F ADC A,A"].Execute = () => ADC(_reg.A);
        _opcodes["ED ADC HL,BC"].Execute = () => ADCHL(_reg.BC);
        _opcodes["ED ADC HL,DE"].Execute = () => ADCHL(_reg.DE);
        _opcodes["ED ADC HL,HL"].Execute = () => ADCHL(_reg.HL);
        _opcodes["ED ADC HL,SP"].Execute = () => ADCHL(_reg.SP);
        _opcodes["86 ADD A,(HL)"].Execute = () => ADD(_mmu[_reg.HL]);
        _opcodes["DD ADD A,(IX+d)"].Execute = () => { ADD(_mmu[(word)(_reg.IX + (sbyte)NextByte())]); };
        _opcodes["FD ADD A,(IY+d)"].Execute = () => { ADD(_mmu[(word)(_reg.IY + (sbyte)NextByte())]); };
        _opcodes["C6 ADD A,n"].Execute = () => ADD(NextByte());
        _opcodes["80 ADD A,B"].Execute = () => ADD(_reg.B);
        _opcodes["81 ADD A,C"].Execute = () => ADD(_reg.C);
        _opcodes["82 ADD A,D"].Execute = () => ADD(_reg.D);
        _opcodes["83 ADD A,E"].Execute = () => ADD(_reg.E);
        _opcodes["84 ADD A,H"].Execute = () => ADD(_reg.H);
        _opcodes["85 ADD A,L"].Execute = () => ADD(_reg.L);
        _opcodes["87 ADD A,A"].Execute = () => ADD(_reg.A);
        _opcodes["09 ADD HL,BC"].Execute = () => ADDHL(_reg.BC);
        _opcodes["19 ADD HL,DE"].Execute = () => ADDHL(_reg.DE);
        _opcodes["29 ADD HL,HL"].Execute = () => ADDHL(_reg.HL);
        _opcodes["39 ADD HL,SP"].Execute = () => ADDHL(_reg.SP);
        _opcodes["DD ADD IX,BC"].Execute = () => ADDIX(_reg.BC);
        _opcodes["DD ADD IX,DE"].Execute = () => ADDIX(_reg.DE);
        _opcodes["DD ADD IX,IX"].Execute = () => ADDIX(_reg.IX);
        _opcodes["DD ADD IX,SP"].Execute = () => ADDIX(_reg.SP);
        _opcodes["FD ADD IY,BC"].Execute = () => ADDIY(_reg.BC);
        _opcodes["FD ADD IY,DE"].Execute = () => ADDIY(_reg.DE);
        _opcodes["FD ADD IY,IY"].Execute = () => ADDIY(_reg.IX);
        _opcodes["FD ADD IY,SP"].Execute = () => ADDIY(_reg.SP);
        _opcodes["A6 AND (HL)"].Execute = () => AND(_mmu[_reg.HL]);
        _opcodes["DD AND (IX+d)"].Execute = () => { AND(_mmu[(word)(_reg.IX + (sbyte)NextByte())]); };
        _opcodes["FD AND (IY+d)"].Execute = () => { AND(_mmu[(word)(_reg.IY + (sbyte)NextByte())]); };
        _opcodes["E6 AND n"].Execute = () => AND(NextByte());
        _opcodes["A0 AND B"].Execute = () => AND(_reg.B);
        _opcodes["A1 AND C"].Execute = () => AND(_reg.C);
        _opcodes["A2 AND D"].Execute = () => AND(_reg.D);
        _opcodes["A3 AND E"].Execute = () => AND(_reg.E);
        _opcodes["A4 AND H"].Execute = () => AND(_reg.H);
        _opcodes["A5 AND L"].Execute = () => AND(_reg.L);
        _opcodes["A7 AND A"].Execute = () => AND(_reg.A);
        _opcodes["CB BIT 0,(HL)"].Execute = () => BIT(0, _mmu[_reg.HL]);
        _opcodes["CB BIT 1,(HL)"].Execute = () => BIT(1, _mmu[_reg.HL]);
        _opcodes["CB BIT 2,(HL)"].Execute = () => BIT(2, _mmu[_reg.HL]);
        _opcodes["CB BIT 3,(HL)"].Execute = () => BIT(3, _mmu[_reg.HL]);
        _opcodes["CB BIT 4,(HL)"].Execute = () => BIT(4, _mmu[_reg.HL]);
        _opcodes["CB BIT 5,(HL)"].Execute = () => BIT(5, _mmu[_reg.HL]);
        _opcodes["CB BIT 6,(HL)"].Execute = () => BIT(6, _mmu[_reg.HL]);
        _opcodes["CB BIT 7,(HL)"].Execute = () => BIT(7, _mmu[_reg.HL]);
        _opcodes["DD BIT 0,(IX+d)"].Execute = () => BIT(0, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 1,(IX+d)"].Execute = () => BIT(1, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 2,(IX+d)"].Execute = () => BIT(2, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 3,(IX+d)"].Execute = () => BIT(3, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 4,(IX+d)"].Execute = () => BIT(4, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 5,(IX+d)"].Execute = () => BIT(5, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 6,(IX+d)"].Execute = () => BIT(6, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["DD BIT 7,(IX+d)"].Execute = () => BIT(7, _mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["FD BIT 0,(IY+d)"].Execute = () => BIT(0, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 1,(IY+d)"].Execute = () => BIT(1, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 2,(IY+d)"].Execute = () => BIT(2, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 3,(IY+d)"].Execute = () => BIT(3, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 4,(IY+d)"].Execute = () => BIT(4, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 5,(IY+d)"].Execute = () => BIT(5, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 6,(IY+d)"].Execute = () => BIT(6, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FD BIT 7,(IY+d)"].Execute = () => BIT(7, _mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["CB BIT 0,B"].Execute = () => BIT(0, _reg.B);
        _opcodes["CB BIT 1,B"].Execute = () => BIT(1, _reg.B);
        _opcodes["CB BIT 2,B"].Execute = () => BIT(2, _reg.B);
        _opcodes["CB BIT 3,B"].Execute = () => BIT(3, _reg.B);
        _opcodes["CB BIT 4,B"].Execute = () => BIT(4, _reg.B);
        _opcodes["CB BIT 5,B"].Execute = () => BIT(5, _reg.B);
        _opcodes["CB BIT 6,B"].Execute = () => BIT(6, _reg.B);
        _opcodes["CB BIT 7,B"].Execute = () => BIT(7, _reg.B);
        _opcodes["CB BIT 0,C"].Execute = () => BIT(0, _reg.C);
        _opcodes["CB BIT 1,C"].Execute = () => BIT(1, _reg.C);
        _opcodes["CB BIT 2,C"].Execute = () => BIT(2, _reg.C);
        _opcodes["CB BIT 3,C"].Execute = () => BIT(3, _reg.C);
        _opcodes["CB BIT 4,C"].Execute = () => BIT(4, _reg.C);
        _opcodes["CB BIT 5,C"].Execute = () => BIT(5, _reg.C);
        _opcodes["CB BIT 6,C"].Execute = () => BIT(6, _reg.C);
        _opcodes["CB BIT 7,C"].Execute = () => BIT(7, _reg.C);
        _opcodes["CB BIT 0,D"].Execute = () => BIT(0, _reg.D);
        _opcodes["CB BIT 1,D"].Execute = () => BIT(1, _reg.D);
        _opcodes["CB BIT 2,D"].Execute = () => BIT(2, _reg.D);
        _opcodes["CB BIT 3,D"].Execute = () => BIT(3, _reg.D);
        _opcodes["CB BIT 4,D"].Execute = () => BIT(4, _reg.D);
        _opcodes["CB BIT 5,D"].Execute = () => BIT(5, _reg.D);
        _opcodes["CB BIT 6,D"].Execute = () => BIT(6, _reg.D);
        _opcodes["CB BIT 7,D"].Execute = () => BIT(7, _reg.D);
        _opcodes["CB BIT 0,E"].Execute = () => BIT(0, _reg.E);
        _opcodes["CB BIT 1,E"].Execute = () => BIT(1, _reg.E);
        _opcodes["CB BIT 2,E"].Execute = () => BIT(2, _reg.E);
        _opcodes["CB BIT 3,E"].Execute = () => BIT(3, _reg.E);
        _opcodes["CB BIT 4,E"].Execute = () => BIT(4, _reg.E);
        _opcodes["CB BIT 5,E"].Execute = () => BIT(5, _reg.E);
        _opcodes["CB BIT 6,E"].Execute = () => BIT(6, _reg.E);
        _opcodes["CB BIT 7,E"].Execute = () => BIT(7, _reg.E);
        _opcodes["CB BIT 0,H"].Execute = () => BIT(0, _reg.H);
        _opcodes["CB BIT 1,H"].Execute = () => BIT(1, _reg.H);
        _opcodes["CB BIT 2,H"].Execute = () => BIT(2, _reg.H);
        _opcodes["CB BIT 3,H"].Execute = () => BIT(3, _reg.H);
        _opcodes["CB BIT 4,H"].Execute = () => BIT(4, _reg.H);
        _opcodes["CB BIT 5,H"].Execute = () => BIT(5, _reg.H);
        _opcodes["CB BIT 6,H"].Execute = () => BIT(6, _reg.H);
        _opcodes["CB BIT 7,H"].Execute = () => BIT(7, _reg.H);
        _opcodes["CB BIT 0,L"].Execute = () => BIT(0, _reg.L);
        _opcodes["CB BIT 1,L"].Execute = () => BIT(1, _reg.L);
        _opcodes["CB BIT 2,L"].Execute = () => BIT(2, _reg.L);
        _opcodes["CB BIT 3,L"].Execute = () => BIT(3, _reg.L);
        _opcodes["CB BIT 4,L"].Execute = () => BIT(4, _reg.L);
        _opcodes["CB BIT 5,L"].Execute = () => BIT(5, _reg.L);
        _opcodes["CB BIT 6,L"].Execute = () => BIT(6, _reg.L);
        _opcodes["CB BIT 7,L"].Execute = () => BIT(7, _reg.L);
        _opcodes["CB BIT 0,A"].Execute = () => BIT(0, _reg.A);
        _opcodes["CB BIT 1,A"].Execute = () => BIT(1, _reg.A);
        _opcodes["CB BIT 2,A"].Execute = () => BIT(2, _reg.A);
        _opcodes["CB BIT 3,A"].Execute = () => BIT(3, _reg.A);
        _opcodes["CB BIT 4,A"].Execute = () => BIT(4, _reg.A);
        _opcodes["CB BIT 5,A"].Execute = () => BIT(5, _reg.A);
        _opcodes["CB BIT 6,A"].Execute = () => BIT(6, _reg.A);
        _opcodes["CB BIT 7,A"].Execute = () => BIT(7, _reg.A);
        _opcodes["DC CALL C,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (!_reg.FlagC) return;
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["FC CALL M,nn"].Execute = () => { };
        _opcodes["D4 CALL NC,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (_reg.FlagC) return;
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["C4 CALL NZ,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (_reg.FlagZ) return;
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["F4 CALL P,nn"].Execute = () => { };
        _opcodes["EC CALL PE,nn"].Execute = () => { };
        _opcodes["E4 CALL PO,nn"].Execute = () => { };
        _opcodes["CC CALL Z,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (!_reg.FlagZ) return;
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["CD CALL nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Msb();
            _reg.SP--; _mmu[_reg.SP] = _reg.PC.Lsb();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["3F CCF"].Execute = () =>
        {
            _reg.FlagC = !_reg.FlagC;
            _reg.FlagN = false;
            _reg.FlagH = false;
        };
        _opcodes["BE CP (HL)"].Execute = () => CP(_mmu[_reg.HL]);
        _opcodes["DD CP (IX+d)"].Execute = () => CP(_mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["FD CP (IY+d)"].Execute = () => CP(_mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["FE CP n"].Execute = () => CP(NextByte());
        _opcodes["B8 CP B"].Execute = () => CP(_reg.B);
        _opcodes["B9 CP C"].Execute = () => CP(_reg.C);
        _opcodes["BA CP D"].Execute = () => CP(_reg.D);
        _opcodes["BB CP E"].Execute = () => CP(_reg.E);
        _opcodes["BC CP H"].Execute = () => CP(_reg.H);
        _opcodes["BD CP L"].Execute = () => CP(_reg.L);
        _opcodes["BF CP A"].Execute = () => CP(_reg.A);
        _opcodes["ED CPD"].Execute = () => { };
        _opcodes["ED CPDR"].Execute = () => { };
        _opcodes["ED CPI"].Execute = () => { };
        _opcodes["ED CPIR"].Execute = () => { };
        _opcodes["2F CPL"].Execute = () =>
        {
            _reg.A = (byte)~_reg.A;
            _reg.FlagN = true;
            _reg.FlagH = true;
        };
        _opcodes["27 DAA"].Execute = () => DAA();
        _opcodes["35 DEC (HL)"].Execute = () => { _mmu[_reg.HL] = DEC(_mmu[_reg.HL]); };
        _opcodes["DD DEC (IX+d)"].Execute = () =>
        {
            _operand = NextByte();
            _mmu[_reg.IX + (sbyte)_operand] = DEC(_mmu[_reg.IX + (sbyte)_operand]);
        };
        _opcodes["FD DEC (IY+d)"].Execute = () =>
        {
            _operand = NextByte();
            _mmu[_reg.IY + (sbyte)_operand] = DEC(_mmu[_reg.IY + (sbyte)_operand]);
        };
        _opcodes["DD DEC IX"].Execute = () => { _reg.IX--; };
        _opcodes["FD DEC IY"].Execute = () => { _reg.IY--; };
        _opcodes["0B DEC BC"].Execute = () => { _reg.BC--; };
        _opcodes["1B DEC DE"].Execute = () => { _reg.DE--; };
        _opcodes["2B DEC HL"].Execute = () => { _reg.HL--; };
        _opcodes["3B DEC SP"].Execute = () => { _reg.SP--; };
        _opcodes["05 DEC B"].Execute = () => { _reg.B = DEC(_reg.B); };
        _opcodes["06 DEC C"].Execute = () => { _reg.C = DEC(_reg.C); };
        _opcodes["07 DEC D"].Execute = () => { _reg.D = DEC(_reg.D); };
        _opcodes["08 DEC E"].Execute = () => { _reg.E = DEC(_reg.E); };
        _opcodes["09 DEC H"].Execute = () => { _reg.H = DEC(_reg.H); };
        _opcodes["0A DEC L"].Execute = () => { _reg.L = DEC(_reg.L); };
        _opcodes["0C DEC A"].Execute = () => { _reg.A = DEC(_reg.A); };
        _opcodes["F3 DI"].Execute = () => { _int.Disable(); };
        _opcodes["10 DJNZ d"].Execute = () =>
        {
            _operand = NextByte();
            _reg.B--;
            if (_reg.B == 0) return;
            _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        };
        _opcodes["FB EI"].Execute = () => { _int.Enable(); };
        _opcodes["E3 EX (SP),HL"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            word temp = _reg.HL;
            _reg.HL = _mmu[_address];
            _mmu[_address] = temp.Lsb();
            _mmu[_address + 1] = temp.Msb();
        };
        _opcodes["DD EX (SP),IX"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            word temp = _reg.IX;
            _reg.IX = _mmu[_address];
            _mmu[_address] = temp.Lsb();
            _mmu[_address + 1] = temp.Msb();
        };
        _opcodes["FD EX (SP),IY"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            word temp = _reg.IY;
            _reg.IY = _mmu[_address];
            _mmu[_address] = temp.Lsb();
            _mmu[_address + 1] = temp.Msb();
        };
        _opcodes["08 EX AF,AF'"].Execute = () =>
        {
            word temp = _reg.AF;
            _reg.AF = _reg.AF_;
            _reg.AF_ = temp;
        };
        _opcodes["EB EX DE,HL"].Execute = () =>
        {
            word temp = _reg.DE;
            _reg.DE = _reg.HL;
            _reg.HL = temp;
        };
        _opcodes["D9 EXX"].Execute = () =>
        {
            word temp = _reg.BC;
            _reg.BC = _reg.BC_;
            _reg.BC_ = temp;

            temp = _reg.DE;
            _reg.DE = _reg.DE_;
            _reg.DE_ = temp;

            temp = _reg.HL;
            _reg.HL = _reg.HL_;
            _reg.HL_ = temp;
        };
        _opcodes["76 HALT"].Execute = () => { _reg.PC--; };
        _opcodes["ED IM 0"].Execute = () => { };
        _opcodes["ED IM 1"].Execute = () => { };
        _opcodes["ED IM 2"].Execute = () => { };
        _opcodes["DB IN A,(n)"].Execute = () => { };
        _opcodes["ED IN B,(C)"].Execute = () => { };
        _opcodes["ED IN C,(C)"].Execute = () => { };
        _opcodes["ED IN D,(C)"].Execute = () => { };
        _opcodes["ED IN E,(C)"].Execute = () => { };
        _opcodes["ED IN H,(C)"].Execute = () => { };
        _opcodes["ED IN L,(C)"].Execute = () => { };
        _opcodes["ED IN A,(C)"].Execute = () => { };
        _opcodes["34 INC (HL)"].Execute = () => { _mmu[_reg.HL] = INC(_mmu[_reg.HL]); };
        _opcodes["DD INC (IX+d)"].Execute = () =>
        {
            _operand = NextByte();
            _mmu[_reg.IX + (sbyte)_operand] = INC(_mmu[_reg.IX + (sbyte)_operand]);
        };
        _opcodes["FD INC (IY+d)"].Execute = () =>
        {
            _operand = NextByte();
            _mmu[_reg.IY + (sbyte)_operand] = INC(_mmu[_reg.IY + (sbyte)_operand]);
        };
        _opcodes["DD INC IX"].Execute = () => { _reg.IX++; };
        _opcodes["FD INC IY"].Execute = () => { _reg.IY++; };
        _opcodes["03 INC BC"].Execute = () => { _reg.BC++; };
        _opcodes["13 INC DE"].Execute = () => { _reg.DE++; };
        _opcodes["23 INC HL"].Execute = () => { _reg.HL++; };
        _opcodes["33 INC SP"].Execute = () => { _reg.SP++; };
        _opcodes["04 INC B"].Execute = () => { _reg.B = INC(_reg.B); };
        _opcodes["05 INC C"].Execute = () => { _reg.C = INC(_reg.C); };
        _opcodes["06 INC D"].Execute = () => { _reg.D = INC(_reg.D); };
        _opcodes["07 INC E"].Execute = () => { _reg.E = INC(_reg.E); };
        _opcodes["08 INC H"].Execute = () => { _reg.H = INC(_reg.H); };
        _opcodes["09 INC L"].Execute = () => { _reg.L = INC(_reg.L); };
        _opcodes["0B INC A"].Execute = () => { _reg.A = INC(_reg.A); };
        _opcodes["ED IND"].Execute = () => { };
        _opcodes["ED INDR"].Execute = () => { };
        _opcodes["ED INI"].Execute = () => { };
        _opcodes["ED INIR"].Execute = () => { };
        _opcodes["E9 JP (HL)"].Execute = () => { _reg.PC = _mmu[_reg.HL]; };
        _opcodes["DD JP (IX)"].Execute = () => { _reg.PC = _mmu[_reg.IX]; };
        _opcodes["FD JP (IY)"].Execute = () => { _reg.PC = _mmu[_reg.IY]; };
        _opcodes["DA JP C,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (!_reg.FlagC) return;
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["FA JP M,nn"].Execute = () => { };
        _opcodes["D2 JP NC,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (_reg.FlagC) return;
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["C2 JP NZ,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (_reg.FlagZ) return;
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["F2 JP P,nn"].Execute = () => { };
        _opcodes["EA JP PE,nn"].Execute = () => { };
        _opcodes["E2 JP PO,nn"].Execute = () => { };
        _opcodes["CA JP Z,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            if (!_reg.FlagZ) return;
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["C3 JP nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["38 JR C,d"].Execute = () =>
        {
            _operand = NextByte();
            if (!_reg.FlagC) return;
            _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        };
        _opcodes["30 JR NC,d"].Execute = () =>
        {
            _operand = NextByte();
            if (_reg.FlagC) return;
            _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        };
        _opcodes["20 JR NZ,d"].Execute = () =>
        {
            _operand = NextByte();
            if (_reg.FlagZ) return;
            _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        };
        _opcodes["28 JR Z,d"].Execute = () =>
        {
            _operand = NextByte();
            if (!_reg.FlagZ) return;
            _reg.PC = (word)(_reg.PC + (sbyte)_operand);
        };
        _opcodes["18 JR d"].Execute = () => { _reg.PC = (word)(_reg.PC + (sbyte)NextByte()); };
        _opcodes["02 LD (BC),A"].Execute = () => { _mmu[_reg.BC] = _reg.A; };
        _opcodes["12 LD (DE),A"].Execute = () => { _mmu[_reg.DE] = _reg.A; };
        _opcodes["36 LD (HL),n"].Execute = () => { _mmu[_reg.HL] = NextByte(); };
        _opcodes["70 LD (HL),B"].Execute = () => { _mmu[_reg.HL] = _reg.B; };
        _opcodes["71 LD (HL),C"].Execute = () => { _mmu[_reg.HL] = _reg.C; };
        _opcodes["72 LD (HL),D"].Execute = () => { _mmu[_reg.HL] = _reg.D; };
        _opcodes["73 LD (HL),E"].Execute = () => { _mmu[_reg.HL] = _reg.E; };
        _opcodes["74 LD (HL),H"].Execute = () => { _mmu[_reg.HL] = _reg.H; };
        _opcodes["75 LD (HL),L"].Execute = () => { _mmu[_reg.HL] = _reg.L; };
        _opcodes["77 LD (HL),A"].Execute = () => { _mmu[_reg.HL] = _reg.A; };
        _opcodes["DD LD (IX+d),n"].Execute = () =>
        {
            sbyte d = (sbyte)NextByte();
            _mmu[_reg.IX + d] = NextByte();
        };
        _opcodes["DD LD (IX+d),B"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.B; };
        _opcodes["DD LD (IX+d),C"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.C; };
        _opcodes["DD LD (IX+d),D"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.D; };
        _opcodes["DD LD (IX+d),E"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.E; };
        _opcodes["DD LD (IX+d),H"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.H; };
        _opcodes["DD LD (IX+d),L"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.L; };
        _opcodes["DD LD (IX+d),A"].Execute = () => { _mmu[_reg.IX + (sbyte)NextByte()] = _reg.A; };
        _opcodes["FD LD (IY+d),n"].Execute = () =>
        {
            sbyte d = (sbyte)NextByte();
            _mmu[_reg.IX + d] = NextByte();
        };
        _opcodes["FD LD (IY+d),B"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.B; };
        _opcodes["FD LD (IY+d),C"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.C; };
        _opcodes["FD LD (IY+d),D"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.D; };
        _opcodes["FD LD (IY+d),E"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.E; };
        _opcodes["FD LD (IY+d),H"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.H; };
        _opcodes["FD LD (IY+d),L"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.L; };
        _opcodes["FD LD (IY+d),A"].Execute = () => { _mmu[_reg.IY + (sbyte)NextByte()] = _reg.A; };
        _opcodes["32 LD (nn),A"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _mmu[BitUtils.ToWord(_msb, _lsb)] = _reg.A;
        };
        _opcodes["ED LD (nn),BC"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.H;
            _mmu[_address + 1] = _reg.L;
        };
        _opcodes["ED LD (nn),DE"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.H;
            _mmu[_address + 1] = _reg.L;
        };
        _opcodes["22 LD (nn),HL"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.H;
            _mmu[_address + 1] = _reg.L;
        };
        _opcodes["DD LD (nn),IX"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.H;
            _mmu[_address + 1] = _reg.L;
        };
        _opcodes["FD LD (nn),IY"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.H;
            _mmu[_address + 1] = _reg.L;
        };
        _opcodes["ED LD (nn),SP"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _mmu[_address] = _reg.SP.Lsb();
            _mmu[_address + 1] = _reg.SP.Msb();
        };
        _opcodes["0A LD A,(BC)"].Execute = () => { _reg.A = _mmu[_reg.BC]; };
        _opcodes["1A LD A,(DE)"].Execute = () => { _reg.A = _mmu[_reg.DE]; };
        _opcodes["3A LD A,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.A = _mmu[BitUtils.ToWord(_msb, _lsb)];
        };
        _opcodes["ED LD A,I"].Execute = () =>
        {
            _reg.A = _reg.I;
            _reg.FlagS = (_reg.A & 0x80) != 0;
            _reg.FlagZ = _reg.A == 0;
            _reg.FlagH = false;
            _reg.FlagN = false;
            _reg.FlagPV = _int.IFF2;
        };
        _opcodes["ED LD A,R"].Execute = () =>
        {
            _reg.A = _reg.R;
            _reg.FlagS = (_reg.A & 0x80) != 0;
            _reg.FlagZ = _reg.A == 0;
            _reg.FlagH = false;
            _reg.FlagN = false;
            _reg.FlagPV = _int.IFF2;
        };
        _opcodes["ED LD BC,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _reg.B = _mmu[_address];
            _reg.C = _mmu[_address + 1];
        };
        _opcodes["ED LD DE,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _reg.D = _mmu[_address];
            _reg.E = _mmu[_address + 1];
        };
        _opcodes["2A LD HL,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _address = BitUtils.ToWord(_msb, _lsb);
            _reg.H = _mmu[_address];
            _reg.L = _mmu[_address + 1];
        };
        _opcodes["ED LD I,A"].Execute = () => { _reg.I = _reg.A; };
        _opcodes["DD LD IX,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.IX = BitUtils.ToWord(_mmu[_msb], _mmu[_lsb]);
        };
        _opcodes["DD LD IX,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.IX = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["FD LD IY,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.IY = BitUtils.ToWord(_mmu[_msb], _mmu[_lsb]);
        };
        _opcodes["FD LD IY,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.IY = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["ED LD R,A"].Execute = () => { _reg.R = _reg.A; };
        _opcodes["ED LD SP,(nn)"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.SP = BitUtils.ToWord(_mmu[_msb], _mmu[_lsb]);
        };
        _opcodes["F9 LD SP,HL"].Execute = () => { _reg.SP = _reg.HL; };
        _opcodes["DD LD SP,IX"].Execute = () => { _reg.SP = _reg.IY; };
        _opcodes["FD LD SP,IY"].Execute = () => { _reg.SP = _reg.IY; };
        _opcodes["01 LD BC,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.BC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["11 LD DE,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.DE = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["21 LD HL,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.HL = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["31 LD SP,nn"].Execute = () =>
        {
            _lsb = NextByte();
            _msb = NextByte();
            _reg.SP = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["46 LD B,(HL)"].Execute = () => { _reg.B = _mmu[_reg.HL]; };
        _opcodes["47 LD C,(HL)"].Execute = () => { _reg.C = _mmu[_reg.HL]; };
        _opcodes["48 LD D,(HL)"].Execute = () => { _reg.D = _mmu[_reg.HL]; };
        _opcodes["49 LD E,(HL)"].Execute = () => { _reg.E = _mmu[_reg.HL]; };
        _opcodes["4A LD H,(HL)"].Execute = () => { _reg.H = _mmu[_reg.HL]; };
        _opcodes["4B LD L,(HL)"].Execute = () => { _reg.L = _mmu[_reg.HL]; };
        _opcodes["4D LD A,(HL)"].Execute = () => { _reg.A = _mmu[_reg.HL]; };
        _opcodes["DD LD B,(IX+d)"].Execute = () => { _reg.B = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD C,(IX+d)"].Execute = () => { _reg.C = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD D,(IX+d)"].Execute = () => { _reg.D = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD E,(IX+d)"].Execute = () => { _reg.E = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD H,(IX+d)"].Execute = () => { _reg.H = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD L,(IX+d)"].Execute = () => { _reg.L = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["DD LD A,(IX+d)"].Execute = () => { _reg.A = _mmu[_reg.IX + (sbyte)NextByte()]; };
        _opcodes["FD LD B,(IY+d)"].Execute = () => { _reg.B = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD C,(IY+d)"].Execute = () => { _reg.C = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD D,(IY+d)"].Execute = () => { _reg.D = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD E,(IY+d)"].Execute = () => { _reg.E = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD H,(IY+d)"].Execute = () => { _reg.H = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD L,(IY+d)"].Execute = () => { _reg.L = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["FD LD A,(IY+d)"].Execute = () => { _reg.A = _mmu[_reg.IY + (sbyte)NextByte()]; };
        _opcodes["06 LD B,n"].Execute = () => { _reg.B = NextByte(); };
        _opcodes["07 LD C,n"].Execute = () => { _reg.C = NextByte(); };
        _opcodes["08 LD D,n"].Execute = () => { _reg.D = NextByte(); };
        _opcodes["09 LD E,n"].Execute = () => { _reg.E = NextByte(); };
        _opcodes["0A LD H,n"].Execute = () => { _reg.H = NextByte(); };
        _opcodes["0B LD L,n"].Execute = () => { _reg.L = NextByte(); };
        _opcodes["0D LD A,n"].Execute = () => { _reg.A = NextByte(); };
        _opcodes["40 LD B,B"].Execute = () => { _reg.B = _reg.B; };
        _opcodes["41 LD B,C"].Execute = () => { _reg.B = _reg.C; };
        _opcodes["42 LD B,D"].Execute = () => { _reg.B = _reg.D; };
        _opcodes["43 LD B,E"].Execute = () => { _reg.B = _reg.E; };
        _opcodes["44 LD B,H"].Execute = () => { _reg.B = _reg.H; };
        _opcodes["45 LD B,L"].Execute = () => { _reg.B = _reg.L; };
        _opcodes["47 LD B,A"].Execute = () => { _reg.B = _reg.A; };
        _opcodes["48 LD C,B"].Execute = () => { _reg.C = _reg.B; };
        _opcodes["49 LD C,C"].Execute = () => { _reg.C = _reg.C; };
        _opcodes["4A LD C,D"].Execute = () => { _reg.C = _reg.D; };
        _opcodes["4B LD C,E"].Execute = () => { _reg.C = _reg.E; };
        _opcodes["4C LD C,H"].Execute = () => { _reg.C = _reg.H; };
        _opcodes["4D LD C,L"].Execute = () => { _reg.C = _reg.L; };
        _opcodes["4F LD C,A"].Execute = () => { _reg.C = _reg.A; };
        _opcodes["50 LD D,B"].Execute = () => { _reg.D = _reg.B; };
        _opcodes["51 LD D,C"].Execute = () => { _reg.D = _reg.C; };
        _opcodes["52 LD D,D"].Execute = () => { _reg.D = _reg.D; };
        _opcodes["53 LD D,E"].Execute = () => { _reg.D = _reg.E; };
        _opcodes["54 LD D,H"].Execute = () => { _reg.D = _reg.H; };
        _opcodes["55 LD D,L"].Execute = () => { _reg.D = _reg.L; };
        _opcodes["57 LD D,A"].Execute = () => { _reg.D = _reg.A; };
        _opcodes["58 LD E,B"].Execute = () => { _reg.E = _reg.B; };
        _opcodes["59 LD E,C"].Execute = () => { _reg.E = _reg.C; };
        _opcodes["5A LD E,D"].Execute = () => { _reg.E = _reg.D; };
        _opcodes["5B LD E,E"].Execute = () => { _reg.E = _reg.E; };
        _opcodes["5C LD E,H"].Execute = () => { _reg.E = _reg.H; };
        _opcodes["5D LD E,L"].Execute = () => { _reg.E = _reg.L; };
        _opcodes["5F LD E,A"].Execute = () => { _reg.E = _reg.A; };
        _opcodes["60 LD H,B"].Execute = () => { _reg.H = _reg.B; };
        _opcodes["61 LD H,C"].Execute = () => { _reg.H = _reg.C; };
        _opcodes["62 LD H,D"].Execute = () => { _reg.H = _reg.D; };
        _opcodes["63 LD H,E"].Execute = () => { _reg.H = _reg.E; };
        _opcodes["64 LD H,H"].Execute = () => { _reg.H = _reg.H; };
        _opcodes["65 LD H,L"].Execute = () => { _reg.H = _reg.L; };
        _opcodes["67 LD H,A"].Execute = () => { _reg.H = _reg.A; };
        _opcodes["68 LD L,B"].Execute = () => { _reg.L = _reg.B; };
        _opcodes["69 LD L,C"].Execute = () => { _reg.L = _reg.C; };
        _opcodes["6A LD L,D"].Execute = () => { _reg.L = _reg.D; };
        _opcodes["6B LD L,E"].Execute = () => { _reg.L = _reg.E; };
        _opcodes["6C LD L,H"].Execute = () => { _reg.L = _reg.H; };
        _opcodes["6D LD L,L"].Execute = () => { _reg.L = _reg.L; };
        _opcodes["6F LD L,A"].Execute = () => { _reg.L = _reg.A; };
        _opcodes["78 LD A,B"].Execute = () => { _reg.A = _reg.B; };
        _opcodes["79 LD A,C"].Execute = () => { _reg.A = _reg.C; };
        _opcodes["7A LD A,D"].Execute = () => { _reg.A = _reg.D; };
        _opcodes["7B LD A,E"].Execute = () => { _reg.A = _reg.E; };
        _opcodes["7C LD A,H"].Execute = () => { _reg.A = _reg.H; };
        _opcodes["7D LD A,L"].Execute = () => { _reg.A = _reg.L; };
        _opcodes["7F LD A,A"].Execute = () => { _reg.A = _reg.A; };
        _opcodes["ED LDD"].Execute = () => { };
        _opcodes["ED LDDR"].Execute = () => { };
        _opcodes["ED LDI"].Execute = () => { };
        _opcodes["ED LDIR"].Execute = () => { };
        _opcodes["ED NEG"].Execute = () => { };
        _opcodes["00 NOP"].Execute = () => { };
        _opcodes["B6 OR (HL)"].Execute = () => { OR(_mmu[_reg.HL]); };
        _opcodes["DD OR (IX+d)"].Execute = () => { OR(_mmu[_reg.IX + (sbyte)NextByte()]); };
        _opcodes["FD OR (IY+d)"].Execute = () => { OR(_mmu[_reg.IY + (sbyte)NextByte()]); };
        _opcodes["F6 OR n"].Execute = () => OR(NextByte());
        _opcodes["B0 OR B"].Execute = () => OR(_reg.B);
        _opcodes["B1 OR C"].Execute = () => OR(_reg.C);
        _opcodes["B2 OR D"].Execute = () => OR(_reg.D);
        _opcodes["B3 OR E"].Execute = () => OR(_reg.E);
        _opcodes["B4 OR H"].Execute = () => OR(_reg.H);
        _opcodes["B5 OR L"].Execute = () => OR(_reg.L);
        _opcodes["B7 OR A"].Execute = () => OR(_reg.A);
        _opcodes["ED OTDR"].Execute = () => { };
        _opcodes["ED OTIR"].Execute = () => { };
        _opcodes["ED OUT (C),B"].Execute = () => { };
        _opcodes["ED OUT (C),C"].Execute = () => { };
        _opcodes["ED OUT (C),D"].Execute = () => { };
        _opcodes["ED OUT (C),E"].Execute = () => { };
        _opcodes["ED OUT (C),H"].Execute = () => { };
        _opcodes["ED OUT (C),L"].Execute = () => { };
        _opcodes["ED OUT (C),A"].Execute = () => { };
        _opcodes["D3 OUT (n),A"].Execute = () => { };
        _opcodes["ED OUTD"].Execute = () => { };
        _opcodes["ED OUTI"].Execute = () => { };
        _opcodes["F1 POP AF"].Execute = () =>
        {
            _reg.F = _mmu[_reg.SP++];
            _reg.A = _mmu[_reg.SP++];
        };
        _opcodes["C1 POP BC"].Execute = () =>
        {
            _reg.C = _mmu[_reg.SP++];
            _reg.B = _mmu[_reg.SP++];
        };
        _opcodes["D1 POP DE"].Execute = () =>
        {
            _reg.E = _mmu[_reg.SP++];
            _reg.D = _mmu[_reg.SP++];
        };
        _opcodes["E1 POP HL"].Execute = () =>
        {
            _reg.L = _mmu[_reg.SP++];
            _reg.H = _mmu[_reg.SP++];
        };
        _opcodes["DD POP IX"].Execute = () => { };
        _opcodes["FD POP IY"].Execute = () => { };
        _opcodes["F5 PUSH AF"].Execute = () =>
        {
            _mmu[--_reg.SP] = _reg.A;
            _mmu[--_reg.SP] = _reg.F;
        };
        _opcodes["C5 PUSH BC"].Execute = () =>
        {
            _mmu[--_reg.SP] = _reg.B;
            _mmu[--_reg.SP] = _reg.C;
        };
        _opcodes["D5 PUSH DE"].Execute = () =>
        {
            _mmu[--_reg.SP] = _reg.D;
            _mmu[--_reg.SP] = _reg.E;
        };
        _opcodes["E5 PUSH HL"].Execute = () =>
        {
            _mmu[--_reg.SP] = _reg.H;
            _mmu[--_reg.SP] = _reg.L;
        };
        _opcodes["DD PUSH IX"].Execute = () => { };
        _opcodes["FD PUSH IY"].Execute = () => { };
        _opcodes["CB RES 0,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(0, _mmu[_reg.HL]); };
        _opcodes["CB RES 1,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(2, _mmu[_reg.HL]); };
        _opcodes["CB RES 2,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(2, _mmu[_reg.HL]); };
        _opcodes["CB RES 3,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(3, _mmu[_reg.HL]); };
        _opcodes["CB RES 4,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(4, _mmu[_reg.HL]); };
        _opcodes["CB RES 5,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(5, _mmu[_reg.HL]); };
        _opcodes["CB RES 6,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(6, _mmu[_reg.HL]); };
        _opcodes["CB RES 7,(HL)"].Execute = () => { _mmu[_reg.HL] = RES(7, _mmu[_reg.HL]); };
        _opcodes["DD RES 0,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(0, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 1,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(1, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 2,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(2, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 3,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(3, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 4,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(4, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 5,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(5, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 6,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(6, _mmu[_reg.IX + d]); };
        _opcodes["DD RES 7,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RES(7, _mmu[_reg.IX + d]); };
        _opcodes["FD RES 0,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(0, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 1,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(1, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 2,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(2, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 3,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(3, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 4,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(4, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 5,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(5, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 6,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(6, _mmu[_reg.IY + d]); };
        _opcodes["FD RES 7,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RES(7, _mmu[_reg.IY + d]); };
        _opcodes["CB RES 0,B"].Execute = () => { _reg.B = RES(1, _reg.B); };
        _opcodes["CB RES 1,B"].Execute = () => { _reg.B = RES(2, _reg.B); };
        _opcodes["CB RES 2,B"].Execute = () => { _reg.B = RES(3, _reg.B); };
        _opcodes["CB RES 3,B"].Execute = () => { _reg.B = RES(4, _reg.B); };
        _opcodes["CB RES 4,B"].Execute = () => { _reg.B = RES(5, _reg.B); };
        _opcodes["CB RES 5,B"].Execute = () => { _reg.B = RES(6, _reg.B); };
        _opcodes["CB RES 6,B"].Execute = () => { _reg.B = RES(7, _reg.B); };
        _opcodes["CB RES 7,B"].Execute = () => { _reg.B = RES(0, _reg.B); };
        _opcodes["CB RES 0,C"].Execute = () => { _reg.C = RES(0, _reg.C); };
        _opcodes["CB RES 1,C"].Execute = () => { _reg.C = RES(1, _reg.C); };
        _opcodes["CB RES 2,C"].Execute = () => { _reg.C = RES(2, _reg.C); };
        _opcodes["CB RES 3,C"].Execute = () => { _reg.C = RES(3, _reg.C); };
        _opcodes["CB RES 4,C"].Execute = () => { _reg.C = RES(4, _reg.C); };
        _opcodes["CB RES 5,C"].Execute = () => { _reg.C = RES(5, _reg.C); };
        _opcodes["CB RES 6,C"].Execute = () => { _reg.C = RES(6, _reg.C); };
        _opcodes["CB RES 7,C"].Execute = () => { _reg.C = RES(7, _reg.C); };
        _opcodes["CB RES 0,D"].Execute = () => { _reg.D = RES(0, _reg.D); };
        _opcodes["CB RES 1,D"].Execute = () => { _reg.D = RES(1, _reg.D); };
        _opcodes["CB RES 2,D"].Execute = () => { _reg.D = RES(2, _reg.D); };
        _opcodes["CB RES 3,D"].Execute = () => { _reg.D = RES(3, _reg.D); };
        _opcodes["CB RES 4,D"].Execute = () => { _reg.D = RES(4, _reg.D); };
        _opcodes["CB RES 5,D"].Execute = () => { _reg.D = RES(5, _reg.D); };
        _opcodes["CB RES 6,D"].Execute = () => { _reg.D = RES(6, _reg.D); };
        _opcodes["CB RES 7,D"].Execute = () => { _reg.D = RES(7, _reg.D); };
        _opcodes["CB RES 0,E"].Execute = () => { _reg.E = RES(0, _reg.E); };
        _opcodes["CB RES 1,E"].Execute = () => { _reg.E = RES(1, _reg.E); };
        _opcodes["CB RES 2,E"].Execute = () => { _reg.E = RES(2, _reg.E); };
        _opcodes["CB RES 3,E"].Execute = () => { _reg.E = RES(3, _reg.E); };
        _opcodes["CB RES 4,E"].Execute = () => { _reg.E = RES(4, _reg.E); };
        _opcodes["CB RES 5,E"].Execute = () => { _reg.E = RES(5, _reg.E); };
        _opcodes["CB RES 6,E"].Execute = () => { _reg.E = RES(6, _reg.E); };
        _opcodes["CB RES 7,E"].Execute = () => { _reg.E = RES(7, _reg.E); };
        _opcodes["CB RES 0,H"].Execute = () => { _reg.H = RES(0, _reg.H); };
        _opcodes["CB RES 1,H"].Execute = () => { _reg.H = RES(1, _reg.H); };
        _opcodes["CB RES 2,H"].Execute = () => { _reg.H = RES(2, _reg.H); };
        _opcodes["CB RES 3,H"].Execute = () => { _reg.H = RES(3, _reg.H); };
        _opcodes["CB RES 4,H"].Execute = () => { _reg.H = RES(4, _reg.H); };
        _opcodes["CB RES 5,H"].Execute = () => { _reg.H = RES(5, _reg.H); };
        _opcodes["CB RES 6,H"].Execute = () => { _reg.H = RES(6, _reg.H); };
        _opcodes["CB RES 7,H"].Execute = () => { _reg.H = RES(7, _reg.H); };
        _opcodes["CB RES 0,L"].Execute = () => { _reg.L = RES(0, _reg.L); };
        _opcodes["CB RES 1,L"].Execute = () => { _reg.L = RES(1, _reg.L); };
        _opcodes["CB RES 2,L"].Execute = () => { _reg.L = RES(2, _reg.L); };
        _opcodes["CB RES 3,L"].Execute = () => { _reg.L = RES(3, _reg.L); };
        _opcodes["CB RES 4,L"].Execute = () => { _reg.L = RES(4, _reg.L); };
        _opcodes["CB RES 5,L"].Execute = () => { _reg.L = RES(5, _reg.L); };
        _opcodes["CB RES 6,L"].Execute = () => { _reg.L = RES(6, _reg.L); };
        _opcodes["CB RES 7,L"].Execute = () => { _reg.L = RES(7, _reg.L); };
        _opcodes["CB RES 0,A"].Execute = () => { _reg.A = RES(0, _reg.A); };
        _opcodes["CB RES 1,A"].Execute = () => { _reg.A = RES(1, _reg.A); };
        _opcodes["CB RES 2,A"].Execute = () => { _reg.A = RES(2, _reg.A); };
        _opcodes["CB RES 3,A"].Execute = () => { _reg.A = RES(3, _reg.A); };
        _opcodes["CB RES 4,A"].Execute = () => { _reg.A = RES(4, _reg.A); };
        _opcodes["CB RES 5,A"].Execute = () => { _reg.A = RES(5, _reg.A); };
        _opcodes["CB RES 6,A"].Execute = () => { _reg.A = RES(6, _reg.A); };
        _opcodes["CB RES 7,A"].Execute = () => { _reg.A = RES(7, _reg.A); };
        _opcodes["C9 RET"].Execute = () =>
        {
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["D8 RET C"].Execute = () =>
        {
            if (!_reg.FlagC) return;
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["F8 RET M"].Execute = () => { };
        _opcodes["D0 RET NC"].Execute = () =>
        {
            if (_reg.FlagC) return;
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["C0 RET NZ"].Execute = () =>
        {
            if (_reg.FlagZ) return;
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["F0 RET P"].Execute = () => { };
        _opcodes["E8 RET PE"].Execute = () => { };
        _opcodes["E0 RET PO"].Execute = () => { };
        _opcodes["C8 RET Z"].Execute = () =>
        {
            if (!_reg.FlagZ) return;
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
        };
        _opcodes["ED RETI"].Execute = () =>
        {
            _lsb = _mmu[_reg.SP++];
            _msb = _mmu[_reg.SP++];
            _reg.PC = BitUtils.ToWord(_msb, _lsb);
            _int.Enable();
        };
        _opcodes["ED RETN"].Execute = () => { };
        _opcodes["CB RL (HL)"].Execute = () => { _mmu[_reg.HL] = RL(_mmu[_reg.HL]); };
        _opcodes["DD RL (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RL(_mmu[_reg.IX + d]); };
        _opcodes["FD RL (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RL(_mmu[_reg.IY + d]); };
        _opcodes["CB RL B"].Execute = () => { _reg.B = RL(_reg.B); };
        _opcodes["CB RL C"].Execute = () => { _reg.C = RL(_reg.C); };
        _opcodes["CB RL D"].Execute = () => { _reg.D = RL(_reg.D); };
        _opcodes["CB RL E"].Execute = () => { _reg.E = RL(_reg.E); };
        _opcodes["CB RL H"].Execute = () => { _reg.H = RL(_reg.H); };
        _opcodes["CB RL L"].Execute = () => { _reg.L = RL(_reg.L); };
        _opcodes["CB RL A"].Execute = () => { _reg.A = RL(_reg.A); };
        _opcodes["17 RLA"].Execute = () =>
        {
            _reg.A = RL(_reg.A);
            _reg.FlagZ = false;
        };
        _opcodes["CB RLC (HL)"].Execute = () => { _mmu[_reg.HL] = RLC(_mmu[_reg.HL]); };
        _opcodes["DD RLC (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RLC(_mmu[_reg.IX + d]); };
        _opcodes["FD RLC (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RLC(_mmu[_reg.IY + d]); };
        _opcodes["CB RLC B"].Execute = () => { _reg.B = RLC(_reg.B); };
        _opcodes["CB RLC C"].Execute = () => { _reg.C = RLC(_reg.C); };
        _opcodes["CB RLC D"].Execute = () => { _reg.D = RLC(_reg.D); };
        _opcodes["CB RLC E"].Execute = () => { _reg.E = RLC(_reg.E); };
        _opcodes["CB RLC H"].Execute = () => { _reg.H = RLC(_reg.H); };
        _opcodes["CB RLC L"].Execute = () => { _reg.L = RLC(_reg.L); };
        _opcodes["CB RLC A"].Execute = () => { _reg.A = RLC(_reg.A); };
        _opcodes["07 RLCA"].Execute = () =>
        {
            _reg.A = RLC(_reg.A);
            _reg.FlagZ = false;
        };
        _opcodes["ED RLD"].Execute = () => { };
        _opcodes["CB RR (HL)"].Execute = () => { _mmu[_reg.HL] = RR(_mmu[_reg.HL]); };
        _opcodes["DD RR (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RR(_mmu[_reg.IX + d]); };
        _opcodes["FD RR (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RR(_mmu[_reg.IY + d]); };
        _opcodes["CB RR B"].Execute = () => { _reg.B = RR(_reg.B); };
        _opcodes["CB RR C"].Execute = () => { _reg.C = RR(_reg.C); };
        _opcodes["CB RR D"].Execute = () => { _reg.D = RR(_reg.D); };
        _opcodes["CB RR E"].Execute = () => { _reg.E = RR(_reg.E); };
        _opcodes["CB RR H"].Execute = () => { _reg.H = RR(_reg.H); };
        _opcodes["CB RR L"].Execute = () => { _reg.L = RR(_reg.L); };
        _opcodes["CB RR A"].Execute = () => { _reg.A = RR(_reg.A); };
        _opcodes["1F RRA"].Execute = () =>
        {
            _reg.A = RR(_reg.A);
            _reg.FlagZ = false;
        };
        _opcodes["CB RRC (HL)"].Execute = () => { _mmu[_reg.HL] = RRC(_mmu[_reg.HL]); };
        _opcodes["DD RRC (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = RRC(_mmu[_reg.IX + d]); };
        _opcodes["FD RRC (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = RRC(_mmu[_reg.IY + d]); };
        _opcodes["CB RRC B"].Execute = () => { _reg.B = RRC(_reg.B); };
        _opcodes["CB RRC C"].Execute = () => { _reg.C = RRC(_reg.C); };
        _opcodes["CB RRC D"].Execute = () => { _reg.D = RRC(_reg.D); };
        _opcodes["CB RRC E"].Execute = () => { _reg.E = RRC(_reg.E); };
        _opcodes["CB RRC H"].Execute = () => { _reg.H = RRC(_reg.H); };
        _opcodes["CB RRC L"].Execute = () => { _reg.L = RRC(_reg.L); };
        _opcodes["CB RRC A"].Execute = () => { _reg.A = RRC(_reg.A); };
        _opcodes["0F RRCA"].Execute = () =>
        {
            _reg.A = RRC(_reg.A);
            _reg.FlagZ = false;
        };
        _opcodes["ED RRD"].Execute = () => { };
        _opcodes["C7 RST 0"].Execute = () => RST(0x00);
        _opcodes["CF RST 8"].Execute = () => RST(0x08);
        _opcodes["D7 RST 16"].Execute = () => RST(0x10);
        _opcodes["DF RST 24"].Execute = () => RST(0x18);
        _opcodes["E7 RST 32"].Execute = () => RST(0x20);
        _opcodes["EF RST 40"].Execute = () => RST(0x28);
        _opcodes["F7 RST 48"].Execute = () => RST(0x30);
        _opcodes["FF RST 56"].Execute = () => RST(0x38);
        _opcodes["9E SBC A,(HL)"].Execute = () => { SBC(_mmu[_reg.HL]); };
        _opcodes["DD SBC A,(IX+d)"].Execute = () => { SBC(_mmu[(word)(_reg.IX + (sbyte)NextByte())]); };
        _opcodes["FD SBC A,(IY+d)"].Execute = () => { SBC(_mmu[(word)(_reg.IY + (sbyte)NextByte())]); };
        _opcodes["DE SBC A,n"].Execute = () => SBC(NextByte());
        _opcodes["98 SBC A,B"].Execute = () => SBC(_reg.B);
        _opcodes["99 SBC A,C"].Execute = () => SBC(_reg.C);
        _opcodes["9A SBC A,D"].Execute = () => SBC(_reg.D);
        _opcodes["9B SBC A,E"].Execute = () => SBC(_reg.E);
        _opcodes["9C SBC A,H"].Execute = () => SBC(_reg.H);
        _opcodes["9D SBC A,L"].Execute = () => SBC(_reg.L);
        _opcodes["9F SBC A,A"].Execute = () => SBC(_reg.A);
        _opcodes["ED SBC HL,BC"].Execute = () => SBCHL(_reg.BC);
        _opcodes["ED SBC HL,DE"].Execute = () => SBCHL(_reg.DE);
        _opcodes["ED SBC HL,HL"].Execute = () => SBCHL(_reg.HL);
        _opcodes["ED SBC HL,SP"].Execute = () => SBCHL(_reg.SP);
        _opcodes["37 SCF"].Execute = () =>
        {
            _reg.FlagN = false;
            _reg.FlagH = false;
            _reg.FlagC = true;
        };
        _opcodes["CB SET 0,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(0, _mmu[_reg.HL]); };
        _opcodes["CB SET 1,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(1, _mmu[_reg.HL]); };
        _opcodes["CB SET 2,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(2, _mmu[_reg.HL]); };
        _opcodes["CB SET 3,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(3, _mmu[_reg.HL]); };
        _opcodes["CB SET 4,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(4, _mmu[_reg.HL]); };
        _opcodes["CB SET 5,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(5, _mmu[_reg.HL]); };
        _opcodes["CB SET 6,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(6, _mmu[_reg.HL]); };
        _opcodes["CB SET 7,(HL)"].Execute = () => { _mmu[_reg.HL] = SET(7, _mmu[_reg.HL]); };
        _opcodes["DD SET 0,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(0, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 1,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(1, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 2,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(2, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 3,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(3, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 4,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(4, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 5,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(5, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 6,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(6, _mmu[_reg.IX + d]); };
        _opcodes["DD SET 7,(IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SET(7, _mmu[_reg.IX + d]); };
        _opcodes["FD SET 0,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(0, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 1,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(1, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 2,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(2, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 3,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(3, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 4,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(4, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 5,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(5, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 6,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(6, _mmu[_reg.IY + d]); };
        _opcodes["FD SET 7,(IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SET(7, _mmu[_reg.IY + d]); };
        _opcodes["CB SET 0,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 1,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 2,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 3,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 4,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 5,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 6,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 7,B"].Execute = () => { _reg.B = SET(0, _reg.B); };
        _opcodes["CB SET 0,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 1,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 2,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 3,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 4,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 5,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 6,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 7,C"].Execute = () => { _reg.C = SET(0, _reg.C); };
        _opcodes["CB SET 0,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 1,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 2,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 3,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 4,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 5,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 6,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 7,D"].Execute = () => { _reg.D = SET(0, _reg.D); };
        _opcodes["CB SET 0,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 1,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 2,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 3,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 4,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 5,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 6,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 7,E"].Execute = () => { _reg.E = SET(0, _reg.E); };
        _opcodes["CB SET 0,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 1,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 2,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 3,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 4,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 5,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 6,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 7,H"].Execute = () => { _reg.H = SET(0, _reg.H); };
        _opcodes["CB SET 0,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 1,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 2,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 3,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 4,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 5,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 6,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 7,L"].Execute = () => { _reg.L = SET(0, _reg.L); };
        _opcodes["CB SET 0,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 1,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 2,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 3,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 4,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 5,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 6,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SET 7,A"].Execute = () => { _reg.A = SET(0, _reg.A); };
        _opcodes["CB SLA (HL)"].Execute = () => { _mmu[_reg.HL] = SLA(_mmu[_reg.HL]); };
        _opcodes["DD SLA (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SLA(_mmu[_reg.IX + d]); };
        _opcodes["FD SLA (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SLA(_mmu[_reg.IY + d]); };
        _opcodes["CB SLA B"].Execute = () => { _reg.B = SLA(_reg.B); };
        _opcodes["CB SLA C"].Execute = () => { _reg.C = SLA(_reg.C); };
        _opcodes["CB SLA D"].Execute = () => { _reg.D = SLA(_reg.D); };
        _opcodes["CB SLA E"].Execute = () => { _reg.E = SLA(_reg.E); };
        _opcodes["CB SLA H"].Execute = () => { _reg.H = SLA(_reg.H); };
        _opcodes["CB SLA L"].Execute = () => { _reg.L = SLA(_reg.L); };
        _opcodes["CB SLA A"].Execute = () => { _reg.A = SLA(_reg.A); };
        _opcodes["CB SRA (HL)"].Execute = () => { _mmu[_reg.HL] = SRA(_mmu[_reg.HL]); };
        _opcodes["DD SRA (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SLA(_mmu[_reg.IX + d]); };
        _opcodes["FD SRA (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SLA(_mmu[_reg.IY + d]); };
        _opcodes["CB SRA B"].Execute = () => { _reg.B = SRA(_reg.B); };
        _opcodes["CB SRA C"].Execute = () => { _reg.C = SRA(_reg.C); };
        _opcodes["CB SRA D"].Execute = () => { _reg.D = SRA(_reg.D); };
        _opcodes["CB SRA E"].Execute = () => { _reg.E = SRA(_reg.E); };
        _opcodes["CB SRA H"].Execute = () => { _reg.H = SRA(_reg.H); };
        _opcodes["CB SRA L"].Execute = () => { _reg.L = SRA(_reg.L); };
        _opcodes["CB SRA A"].Execute = () => { _reg.A = SRA(_reg.A); };
        _opcodes["CB SRL (HL)"].Execute = () => { _mmu[_reg.HL] = SRL(_mmu[_reg.HL]); };
        _opcodes["DD SRL (IX+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IX + d] = SLA(_mmu[_reg.IX + d]); };
        _opcodes["FD SRL (IY+d)"].Execute = () => { var d = (sbyte)NextByte(); _mmu[_reg.IY + d] = SLA(_mmu[_reg.IY + d]); };
        _opcodes["CB SRL B"].Execute = () => { _reg.B = SRL(_reg.B); };
        _opcodes["CB SRL C"].Execute = () => { _reg.C = SRL(_reg.C); };
        _opcodes["CB SRL D"].Execute = () => { _reg.D = SRL(_reg.D); };
        _opcodes["CB SRL E"].Execute = () => { _reg.E = SRL(_reg.E); };
        _opcodes["CB SRL H"].Execute = () => { _reg.H = SRL(_reg.H); };
        _opcodes["CB SRL L"].Execute = () => { _reg.L = SRL(_reg.L); };
        _opcodes["CB SRL A"].Execute = () => { _reg.A = SRL(_reg.A); };
        _opcodes["96 SUB (HL)"].Execute = () => { SUB(_mmu[_reg.HL]); };
        _opcodes["DD SUB (IX+d)"].Execute = () => { SUB(_mmu[(word)(_reg.IX + (sbyte)NextByte())]); };
        _opcodes["FD SUB (IY+d)"].Execute = () => { SUB(_mmu[(word)(_reg.IY + (sbyte)NextByte())]); };
        _opcodes["D6 SUB n"].Execute = () => SUB(NextByte());
        _opcodes["90 SUB B"].Execute = () => SUB(_reg.B);
        _opcodes["91 SUB C"].Execute = () => SUB(_reg.C);
        _opcodes["92 SUB D"].Execute = () => SUB(_reg.D);
        _opcodes["93 SUB E"].Execute = () => SUB(_reg.E);
        _opcodes["94 SUB H"].Execute = () => SUB(_reg.H);
        _opcodes["95 SUB L"].Execute = () => SUB(_reg.L);
        _opcodes["97 SUB A"].Execute = () => SUB(_reg.A);
        _opcodes["AE XOR (HL)"].Execute = () => XOR(_mmu[_reg.HL]);
        _opcodes["DD XOR (IX+d)"].Execute = () => XOR(_mmu[_reg.IX + (sbyte)NextByte()]);
        _opcodes["FD XOR (IY+d)"].Execute = () => XOR(_mmu[_reg.IY + (sbyte)NextByte()]);
        _opcodes["EE XOR n"].Execute = () => XOR(NextByte());
        _opcodes["A8 XOR B"].Execute = () => XOR(_reg.B);
        _opcodes["A9 XOR C"].Execute = () => XOR(_reg.C);
        _opcodes["AA XOR D"].Execute = () => XOR(_reg.D);
        _opcodes["AB XOR E"].Execute = () => XOR(_reg.E);
        _opcodes["AC XOR H"].Execute = () => XOR(_reg.H);
        _opcodes["AD XOR L"].Execute = () => XOR(_reg.L);
        _opcodes["AF XOR A"].Execute = () => XOR(_reg.A);
    }
}
