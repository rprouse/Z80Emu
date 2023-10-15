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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_C_PASS()
    {
        _mmu[0x0100] = 0xDC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL C,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_C_FAIL()
    {
        _mmu[0x0100] = 0xDC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL C,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_NC_PASS()
    {
        _mmu[0x0100] = 0xD4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NC,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_NC_FAIL()
    {
        _mmu[0x0100] = 0xD4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NC,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_M_PASS()
    {
        _mmu[0x0100] = 0xFC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL M,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_M_FAIL()
    {
        _mmu[0x0100] = 0xFC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL M,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_P_PASS()
    {
        _mmu[0x0100] = 0xF4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL P,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_P_FAIL()
    {
        _mmu[0x0100] = 0xF4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL P,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_Z_PASS()
    {
        _mmu[0x0100] = 0xCC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL Z,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_Z_FAIL()
    {
        _mmu[0x0100] = 0xCC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL Z,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_NZ_PASS()
    {
        _mmu[0x0100] = 0xC4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NZ,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_NZ_FAIL()
    {
        _mmu[0x0100] = 0xC4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL NZ,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_PE_PASS()
    {
        _mmu[0x0100] = 0xEC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PE,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_PE_FAIL()
    {
        _mmu[0x0100] = 0xEC;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PE,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void CALL_PO_PASS()
    {
        _mmu[0x0100] = 0xE4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PO,0x0302");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFC);
    }

    [Test]
    public void CALL_PO_FAIL()
    {
        _mmu[0x0100] = 0xE4;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("CALL PO,0x0302");

        _reg.PC.Should().Be(0x0103);
        _reg.SP.Should().Be(0xFFFE);
    }

    [Test]
    public void JP()
    {
        _mmu[0x0100] = 0xC3;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP 0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_HL()
    {
        _reg.HL = 0x0110;
        _mmu[0x0100] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (HL)");

        _reg.PC.Should().Be(0x0110);
    }

    [Test]
    public void JP_IX()
    {
        _reg.IX = 0x0110;
        _mmu[0x0100] = 0xDD;
        _mmu[0x0101] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (IX)");

        _reg.PC.Should().Be(0x0110);
    }

    [Test]
    public void JP_IY()
    {
        _reg.IY = 0x0110;
        _mmu[0x0100] = 0xFD;
        _mmu[0x0101] = 0xE9;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP (IY)");

        _reg.PC.Should().Be(0x0110);
    }

    [Test]
    public void JP_C_PASS()
    {
        _mmu[0x0100] = 0xDA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP C,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_C_FAIL()
    {
        _mmu[0x0100] = 0xDA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP C,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_NC_PASS()
    {
        _mmu[0x0100] = 0xD2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NC,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_NC_FAIL()
    {
        _mmu[0x0100] = 0xD2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NC,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_M_PASS()
    {
        _mmu[0x0100] = 0xFA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP M,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_M_FAIL()
    {
        _mmu[0x0100] = 0xFA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP M,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_P_PASS()
    {
        _mmu[0x0100] = 0xF2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP P,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_P_FAIL()
    {
        _mmu[0x0100] = 0xF2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagS = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP P,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_Z_PASS()
    {
        _mmu[0x0100] = 0xCA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP Z,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_Z_FAIL()
    {
        _mmu[0x0100] = 0xCA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP Z,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_NZ_PASS()
    {
        _mmu[0x0100] = 0xC2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NZ,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_NZ_FAIL()
    {
        _mmu[0x0100] = 0xC2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP NZ,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_PE_PASS()
    {
        _mmu[0x0100] = 0xEA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PE,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_PE_FAIL()
    {
        _mmu[0x0100] = 0xEA;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PE,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [Test]
    public void JP_PO_PASS()
    {
        _mmu[0x0100] = 0xE2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PO,0x0302");

        _reg.PC.Should().Be(0x0302);
    }

    [Test]
    public void JP_PO_FAIL()
    {
        _mmu[0x0100] = 0xE2;
        _mmu[0x0101] = 0x02;
        _mmu[0x0102] = 0x03;
        _reg.FlagPV = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("JP PO,0x0302");

        _reg.PC.Should().Be(0x0103);
    }

    [TestCase(02, 0x104)]
    [TestCase(10, 0x010C)]
    [TestCase(-4, 0x00FE)]
    public void JR_POS(sbyte relative, int expectedPC)
    {
        _mmu[0x0100] = 0x18;
        _mmu[0x0101] = (byte)relative;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR {relative}");

        _reg.PC.Should().Be((word)expectedPC);
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

        _reg.PC.Should().Be((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_C_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x38;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR C,{relative}");

        _reg.PC.Should().Be(0x0102);
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

        _reg.PC.Should().Be((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_NC_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x30;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagC = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NC,{relative}");

        _reg.PC.Should().Be(0x0102);
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

        _reg.PC.Should().Be((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_Z_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x28;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = false;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR Z,{relative}");

        _reg.PC.Should().Be(0x0102);
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

        _reg.PC.Should().Be((word)expectedPC);
    }

    [TestCase(10)]
    [TestCase(-2)]
    public void JR_NZ_NEG(sbyte relative)
    {
        _mmu[0x0100] = 0x20;
        _mmu[0x0101] = (byte)relative;
        _reg.FlagZ = true;

        _opcodeHandler.FetchVerifyAndExecuteInstruction($"JR NZ,{relative}");

        _reg.PC.Should().Be(0x0102);
    }

    [Test]
    public void NOP()
    {
        _mmu[0x0100] = 0x00;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("NOP");

        _reg.PC.Should().Be(0x0101);
    }

    [Test]
    public void RET()
    {
        _mmu[0x0100] = 0xC9;
        _mmu[0xFFFC] = 0x02;
        _mmu[0xFFFD] = 0x03;
        _reg.SP = 0x0FFFC;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RET");

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
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

        _reg.PC.Should().Be(0x0302);
        _reg.SP.Should().Be(0xFFFE);
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

        _reg.PC.Should().Be(0x0101);
        _reg.SP.Should().Be(0xFFFC);
    }
}
