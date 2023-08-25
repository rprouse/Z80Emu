using System.Text;
using Z80Emu.Core;
using Z80Emu.Core.OS;
using Z80Emu.Core.Processor;

namespace Z80Emu.Tests.OS;

internal class MockDosConsole : IDosConsole
{
    public StringBuilder Output { get; } = new StringBuilder();

    public byte Read() => 0xDE;

    public string ReadString() => "Hello";

    public void Write(char c) => Output.Append(c);

    public void Write(string text) => Output.Append(text);

    public void WriteLine(string text) => Output.AppendLine(text);
}

public class CPM22Tests
{
    MockDosConsole _console;
    Emulator _emulator;
    CPM22 _cpm;

    [SetUp] 
    public void Setup()
    {
        _console = new MockDosConsole();
        _cpm = new CPM22(_console);
        _emulator = new Emulator(_cpm);
        
        // Set up the stack for the return address
        _emulator.Memory[0xFFFC] = 0x02;
        _emulator.Memory[0xFFFD] = 0x03;
        _emulator.CPU.Registers.SP = 0x0FFFC;
    }

    [Test]
    public void NameIsCorrect()
    {
        _cpm.Name.Should().Be("CP/M 2.2");
    }

    [Test]
    public void HasCorrectCallVectors()
    {
        _cpm.CallVectors.Should().BeEquivalentTo(new ushort[] { 0x0000, 0x0005 });
    }

    [Test]
    public void SetsZeroPageMemory()
    {
        // Emulator initializes CPM22
        _emulator.Memory[0x0000].Should().Be(0xC3); // JP to warm boot
        _emulator.Memory[0x005C].Should().Be(0x01); // FCB
        _emulator.Memory[0x0081].Should().Be((byte)'A'); // Command tail
    }

    [Test]
    public void ExecuteThrowsInvalidOperationExceptionWithIncorrectCallVector()
    {
        _emulator.CPU.Registers.PC = 0x0010;
        Action action = () => _cpm.Execute(_emulator);
        action.Should().Throw<InvalidOperationException>().WithMessage("Invalid CP/M Call");
    }

    [Test]
    public void CallsWarmBoot()
    {
        _emulator.CPU.Registers.PC = 0x0000;    // Warm Boot Vector

        _cpm.Execute(_emulator);

        _emulator.WarmBoot.Should().BeTrue();
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void UnsupportedBDOSCallWritesToConsole()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.P_TERMCPM;       // Unsupported BDOS Call

        _cpm.Execute(_emulator);

        _console.Output.ToString().Should().Be("BDOS call P_TERMCPM not implemented\r\n");
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void C_READ_ReadsFromConsole()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.C_READ;

        _cpm.Execute(_emulator);

        _emulator.CPU.Registers.A.Should().Be(0xDE);
        _emulator.CPU.Registers.L.Should().Be(0xDE);
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void C_WRITE_WritesToConsole()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.C_WRITE;
        _emulator.CPU.Registers.E = (byte)'A';  // Character to write

        _cpm.Execute(_emulator);

        _console.Output.ToString().Should().Be("A");
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void C_WRITESTR_WritesStringToConsole()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.C_WRITESTR;
        _emulator.CPU.Registers.DE = 0x0200;    // String address

        _emulator.Memory[0x0200] = (byte)'H';
        _emulator.Memory[0x0201] = (byte)'e';
        _emulator.Memory[0x0202] = (byte)'l';
        _emulator.Memory[0x0203] = (byte)'l';
        _emulator.Memory[0x0204] = (byte)'o';
        _emulator.Memory[0x0205] = (byte)'$';   // CP/M string terminator

        _cpm.Execute(_emulator);

        _console.Output.ToString().Should().Be("Hello");
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void C_READSTR_ReadsStringFromConsole()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.C_READSTR;
        _emulator.CPU.Registers.DE = 0x0200;    // String address
        _emulator.Memory[0x0200] = 0xFF;        // Buffer length

        _cpm.Execute(_emulator);

        _emulator.Memory[0x0200].Should().Be(5);
        _emulator.Memory[0x0201].Should().Be((byte)'H');
        _emulator.Memory[0x0202].Should().Be((byte)'e');
        _emulator.Memory[0x0203].Should().Be((byte)'l');
        _emulator.Memory[0x0204].Should().Be((byte)'l');
        _emulator.Memory[0x0205].Should().Be((byte)'o');
        _emulator.Memory[0x0206].Should().Be(0x00);
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }

    [Test]
    public void C_READSTR_ReadsStringFromConsole_RespoectsBufferLength()
    {
        _emulator.CPU.Registers.PC = 0x0005;    // BDOS Call Vector
        _emulator.CPU.Registers.C = (byte)CPM22.SystemCalls.C_READSTR;
        _emulator.CPU.Registers.DE = 0x0200;    // String address
        _emulator.Memory[0x0200] = 0x03;        // Buffer length

        _cpm.Execute(_emulator);

        _emulator.Memory[0x0200].Should().Be(3);
        _emulator.Memory[0x0201].Should().Be((byte)'H');
        _emulator.Memory[0x0202].Should().Be((byte)'e');
        _emulator.Memory[0x0203].Should().Be((byte)'l');
        _emulator.Memory[0x0204].Should().Be(0x00);
        _emulator.CPU.Registers.PC.Should().Be(0x0302);
    }
}
