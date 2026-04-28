using Z80Emu.Core.Memory;
using Z80Emu.Core.Processor;
using Z80Emu.Core.Processor.Opcodes;

namespace Z80Emu.Tests.Processor.Opcodes;

public class ControlOpcodeTests
{
    Registers _reg;
    MMU _mmu;
    Interupts _int;
    OpcodeHandler _opcodeHandler;
    Ports _ports;

    [SetUp]
    public void Setup()
    {
        _reg = new Registers();
        _mmu = new MMU();
        _int = new Interupts(_mmu);
        _ports = new Ports();
        _opcodeHandler = new OpcodeHandler(_reg, _mmu, _int, _ports);
        _reg.PC = 0x0100;
        _reg.SP = 0xFFFE;
    }

    [Test]
    public void CALL()
    {
        _mmu[0x0100] = 0xCD;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL 0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_C_PASS()
    {
        _mmu[0x0100] = 0xDC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL C,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_C_FAIL()
    {
        _mmu[0x0100] = 0xDC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL C,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_NC_PASS()
    {
        _mmu[0x0100] = 0xD4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NC,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_NC_FAIL()
    {
        _mmu[0x0100] = 0xD4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NC,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_M_PASS()
    {
        _mmu[0x0100] = 0xFC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL M,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_M_FAIL()
    {
        _mmu[0x0100] = 0xFC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL M,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_P_PASS()
    {
        _mmu[0x0100] = 0xF4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL P,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_P_FAIL()
    {
        _mmu[0x0100] = 0xF4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL P,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_Z_PASS()
    {
        _mmu[0x0100] = 0xCC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL Z,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_Z_FAIL()
    {
        _mmu[0x0100] = 0xCC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL Z,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_NZ_PASS()
    {
        _mmu[0x0100] = 0xC4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NZ,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_NZ_FAIL()
    {
        _mmu[0x0100] = 0xC4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NZ,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_PE_PASS()
    {
        _mmu[0x0100] = 0xEC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PE,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_PE_FAIL()
    {
        _mmu[0x0100] = 0xEC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PE,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void CALL_PO_PASS()
    {
        _mmu[0x0100] = 0xE4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PO,0x0302");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void CALL_PO_FAIL()
    {
        _mmu[0x0100] = 0xE4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PO,0x0302");

        _reg.PC.ShouldBe(0x0103);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void JP()
    {
        _mmu[0x0100] = 0xC3;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP 0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_HL()
    {
        _reg.HL = 0x0110;
        _mmu[0x0100] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (HL)");

        _reg.PC.ShouldBe(0x0110);
    }

    [Test]
    public void JP_IX()
    {
        _reg.IX = 0x0110;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (IX)");

        _reg.PC.ShouldBe(0x0110);
    }

    [Test]
    public void JP_IY()
    {
        _reg.IY = 0x0110;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (IY)");

        _reg.PC.ShouldBe(0x0110);
    }

    [Test]
    public void JP_C_PASS()
    {
        _mmu[0x0100] = 0xDA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP C,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_C_FAIL()
    {
        _mmu[0x0100] = 0xDA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP C,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_NC_PASS()
    {
        _mmu[0x0100] = 0xD2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NC,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_NC_FAIL()
    {
        _mmu[0x0100] = 0xD2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NC,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_M_PASS()
    {
        _mmu[0x0100] = 0xFA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP M,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_M_FAIL()
    {
        _mmu[0x0100] = 0xFA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP M,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_P_PASS()
    {
        _mmu[0x0100] = 0xF2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP P,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_P_FAIL()
    {
        _mmu[0x0100] = 0xF2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP P,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_Z_PASS()
    {
        _mmu[0x0100] = 0xCA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP Z,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_Z_FAIL()
    {
        _mmu[0x0100] = 0xCA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP Z,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_NZ_PASS()
    {
        _mmu[0x0100] = 0xC2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NZ,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_NZ_FAIL()
    {
        _mmu[0x0100] = 0xC2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NZ,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_PE_PASS()
    {
        _mmu[0x0100] = 0xEA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PE,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_PE_FAIL()
    {
        _mmu[0x0100] = 0xEA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PE,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [Test]
    public void JP_PO_PASS()
    {
        _mmu[0x0100] = 0xE2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PO,0x0302");

        _reg.PC.ShouldBe(0x0302);
    }

    [Test]
    public void JP_PO_FAIL()
    {
        _mmu[0x0100] = 0xE2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PO,0x0302");

        _reg.PC.ShouldBe(0x0103);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x18;
        _mmu[0x0101] = (byte)relative;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR {relative}");

        _reg.PC.ShouldBe((word)expectedPC);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_C_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x38;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR C,{relative}");

        _reg.PC.ShouldBe((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_C_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x38;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR C,{relative}");

        _reg.PC.ShouldBe(0x0102);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_NC_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x30;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NC,{relative}");

        _reg.PC.ShouldBe((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_NC_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x30;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NC,{relative}");

        _reg.PC.ShouldBe(0x0102);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_Z_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x28;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR Z,{relative}");

        _reg.PC.ShouldBe((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_Z_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x28;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR Z,{relative}");

        _reg.PC.ShouldBe(0x0102);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_NZ_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x20;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NZ,{relative}");

        _reg.PC.ShouldBe((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_NZ_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x20;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NZ,{relative}");

        _reg.PC.ShouldBe(0x0102);
    }

    [Test]
    public void NOP()
    {
        _mmu[0x0100] = 0x00;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("NOP");

        _reg.PC.ShouldBe(0x0101);
    }

    [Test]
    public void RET()
    {
        _mmu[0x0100] = 0xC9;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_C_POS()
    {
        _mmu[0x0100] = 0xD8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET C");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_C_NEG()
    {
        _mmu[0x0100] = 0xD8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET C");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_NC_POS()
    {
        _mmu[0x0100] = 0xD0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET NC");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_NC_NEG()
    {
        _mmu[0x0100] = 0xD0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET NC");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_Z_POS()
    {
        _mmu[0x0100] = 0xC8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET Z");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_Z_NEG()
    {
        _mmu[0x0100] = 0xC8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET Z");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_NZ_POS()
    {
        _mmu[0x0100] = 0xC0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET NZ");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_NZ_NEG()
    {
        _mmu[0x0100] = 0xC0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET NZ");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_M_POS()
    {
        _mmu[0x0100] = 0xF8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET M");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_M_NEG()
    {
        _mmu[0x0100] = 0xF8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET M");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_P_POS()
    {
        _mmu[0x0100] = 0xF0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET P");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_P_NEG()
    {
        _mmu[0x0100] = 0xF0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET P");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_PE_POS()
    {
        _mmu[0x0100] = 0xE8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET PE");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_PE_NEG()
    {
        _mmu[0x0100] = 0xE8;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET PE");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RET_PO_POS()
    {
        _mmu[0x0100] = 0xE0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET PO");

        _reg.PC.ShouldBe(0x0302);
        _reg.SP.ShouldBe(0xFFFE);
    }

    [Test]
    public void RET_PO_NEG()
    {
        _mmu[0x0100] = 0xE0;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET PO");

        _reg.PC.ShouldBe(0x0101);
        _reg.SP.ShouldBe(0xFFFC);
    }

    [Test]
    public void RST_0()
    {
        // Place the opcode at 0x1234 so the pushed return address (0x1235)
        // has distinct MSB (0x12) and LSB (0x35), proving byte-order correctness.
        _reg.PC = 0x1234;
        _mmu[0x1234] = 0xC7;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RST 0");

        _reg.PC.ShouldBe((word)0x0000);
        _reg.SP.ShouldBe((word)0xFFFC);
        _mmu[0xFFFC].ShouldBe((byte)0x35);  // LSB of return address 0x1235 pushed at lower addr
        _mmu[0xFFFD].ShouldBe((byte)0x12);  // MSB of return address 0x1235 pushed at higher addr
    }

    [TestCase((byte)0xCF, (word)0x0008, "RST 8")]
    [TestCase((byte)0xD7, (word)0x0010, "RST 16")]
    [TestCase((byte)0xDF, (word)0x0018, "RST 24")]
    [TestCase((byte)0xE7, (word)0x0020, "RST 32")]
    [TestCase((byte)0xEF, (word)0x0028, "RST 40")]
    [TestCase((byte)0xF7, (word)0x0030, "RST 48")]
    [TestCase((byte)0xFF, (word)0x0038, "RST 56")]
    public void RST_NonZero(byte opcode, word vector, string mnemonic)
    {
        // Place the opcode at 0x1234 so the pushed return address (0x1235)
        // has distinct MSB (0x12) and LSB (0x35), proving byte-order correctness.
        _reg.PC = 0x1234;
        _mmu[0x1234] = opcode;

        _opcodeHandler.FetchVerifyAndExecuteInstruction(mnemonic);

        _reg.PC.ShouldBe(vector);
        _reg.SP.ShouldBe((word)0xFFFC);
        _mmu[0xFFFC].ShouldBe((byte)0x35);  // LSB of return address 0x1235 pushed at lower addr
        _mmu[0xFFFD].ShouldBe((byte)0x12);  // MSB of return address 0x1235 pushed at higher addr
    }

    [Test]
    public void IM_0()
    {
        _int.Mode = InterruptMode.Mode2;  // start in a non-default mode to prove it changes
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x46;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 0");

        _int.Mode.ShouldBe(InterruptMode.Mode0);
    }

    [Test]
    public void IM_1()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x56;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 1");

        _int.Mode.ShouldBe(InterruptMode.Mode1);
    }

    [Test]
    public void IM_2()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x5E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 2");

        _int.Mode.ShouldBe(InterruptMode.Mode2);
    }

    [Test]
    public void EI()
    {
        _int.IFF1 = false;
        _int.IFF2 = false;
        _int.EiPending = false;
        _mmu[0x0100] = 0xFB;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EI");

        _int.IFF1.ShouldBeTrue();
        _int.IFF2.ShouldBeTrue();
        _int.EiPending.ShouldBeTrue();
    }

    [Test]
    public void DI()
    {
        _int.IFF1 = true;
        _int.IFF2 = true;
        _int.EiPending = true;
        _mmu[0x0100] = 0xF3;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("DI");

        _int.IFF1.ShouldBeFalse();
        _int.IFF2.ShouldBeFalse();
        _int.EiPending.ShouldBeFalse();
    }

    [Test]
    public void LD_A_I_FlagPV_ReflectsIFF2_AfterEI()
    {
        // EI; LD A,I — FlagPV should be 1 because IFF2 is now true
        _mmu[0x0100] = 0xFB;        // EI
        _mmu[0x0101] = 0xED;        // LD A,I
        _mmu[0x0102] = 0x57;
        _reg.I = 0x42;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EI");
        _opcodeHandler.FetchVerifyAndExecuteInstruction("LD A,I");

        _reg.A.ShouldBe((byte)0x42);
        _reg.FlagPV.ShouldBeTrue();
    }
}
