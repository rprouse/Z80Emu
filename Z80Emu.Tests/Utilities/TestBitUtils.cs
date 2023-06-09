using Z80Emu.Core.Processor;
using Z80Emu.Core.Utilities;

namespace Z80Emu.Tests.Utilities;

public class TestBitUtils
{
    [Test]
    public void TestLsb()
    {
        word b = 0xABCD;
        b.Lsb().Should().Be(0xCD);
    }

    [Test]
    public void TestMsb()
    {
        word b = 0xABCD;
        b.Msb().Should().Be(0xAB);
    }

    [Test]
    public void TestToWord()
    {
        BitUtils.ToWord(0xAB, 0xCD).Should().Be(0xABCD);
    }

    [TestCase(0, 0b_0000_0001, true)]
    [TestCase(1, 0b_0000_0010, true)]
    [TestCase(2, 0b_0000_0100, true)]
    [TestCase(3, 0b_0000_1000, true)]
    [TestCase(4, 0b_0001_0000, true)]
    [TestCase(5, 0b_0010_0000, true)]
    [TestCase(6, 0b_0100_0000, true)]
    [TestCase(7, 0b_1000_0000, true)]
    [TestCase(0, 0b_1111_1110, false)]
    [TestCase(1, 0b_1111_1101, false)]
    [TestCase(2, 0b_1111_1011, false)]
    [TestCase(3, 0b_1111_0111, false)]
    [TestCase(4, 0b_1110_1111, false)]
    [TestCase(5, 0b_1101_1111, false)]
    [TestCase(6, 0b_1011_1111, false)]
    [TestCase(7, 0b_0111_1111, false)]
    public void TestIsBitSet(int bit, byte b, bool expected)
    {
        b.IsBitSet(bit).Should().Be(expected);
    }

    [TestCase(0, 0b_0000_0001)]
    [TestCase(1, 0b_0000_0010)]
    [TestCase(2, 0b_0000_0100)]
    [TestCase(3, 0b_0000_1000)]
    [TestCase(4, 0b_0001_0000)]
    [TestCase(5, 0b_0010_0000)]
    [TestCase(6, 0b_0100_0000)]
    [TestCase(7, 0b_1000_0000)]
    public void TestSetBit(int bit, byte expected)
    {
        byte b = 0b_0000_0000;
        b.SetBit(bit).Should().Be(expected);
    }

    [TestCase(0, 0b_1111_1110)]
    [TestCase(1, 0b_1111_1101)]
    [TestCase(2, 0b_1111_1011)]
    [TestCase(3, 0b_1111_0111)]
    [TestCase(4, 0b_1110_1111)]
    [TestCase(5, 0b_1101_1111)]
    [TestCase(6, 0b_1011_1111)]
    [TestCase(7, 0b_0111_1111)]
    public void TestResetBit(int bit, byte expected)
    {
        byte b = 0b_1111_1111;
        b.ResetBit(bit).Should().Be(expected);
    }

    [TestCase(Registers.Flag.Carry, 0b_0000_0001, true)]
    [TestCase(Registers.Flag.Subtract, 0b_0000_0010, true)]
    [TestCase(Registers.Flag.Parity, 0b_0000_0100, true)]
    [TestCase(Registers.Flag.HalfCarry, 0b_0001_0000, true)]
    [TestCase(Registers.Flag.Zero, 0b_0100_0000, true)]
    [TestCase(Registers.Flag.Sign, 0b_1000_0000, true)]
    [TestCase(Registers.Flag.Carry, 0b_1111_1110, false)]
    [TestCase(Registers.Flag.Subtract, 0b_1111_1101, false)]
    [TestCase(Registers.Flag.Parity, 0b_1111_1011, false)]
    [TestCase(Registers.Flag.HalfCarry, 0b_1110_1111, false)]
    [TestCase(Registers.Flag.Zero, 0b_1011_1111, false)]
    [TestCase(Registers.Flag.Sign, 0b_0111_1111, false)]
    public void TestGetFlag(Registers.Flag flag, byte b, bool expected)
    {
        b.GetFlag(flag).Should().Be(expected);
    }

    [TestCase(Registers.Flag.Carry, 0b_0000_0001)]
    [TestCase(Registers.Flag.Subtract, 0b_0000_0010)]
    [TestCase(Registers.Flag.Parity, 0b_0000_0100)]
    [TestCase(Registers.Flag.HalfCarry, 0b_0001_0000)]
    [TestCase(Registers.Flag.Zero, 0b_0100_0000)]
    [TestCase(Registers.Flag.Sign, 0b_1000_0000)]
    public void TestSetFlag(Registers.Flag flag, byte expected)
    {
        byte b = 0b_0000_0000;
        b.SetFlag(flag).Should().Be(expected);
    }

    [TestCase(Registers.Flag.Carry, 0b_1111_1110)]
    [TestCase(Registers.Flag.Subtract, 0b_1111_1101)]
    [TestCase(Registers.Flag.Parity, 0b_1111_1011)]
    [TestCase(Registers.Flag.HalfCarry, 0b_1110_1111)]
    [TestCase(Registers.Flag.Zero, 0b_1011_1111)]
    [TestCase(Registers.Flag.Sign, 0b_0111_1111)]
    public void TestResetFlag(Registers.Flag flag, byte expected)
    {
        byte b = 0b_1111_1111;
        b.ResetFlag(flag).Should().Be(expected);
    }

    [TestCase(Registers.Flag.Carry, 0b_0000_0001)]
    [TestCase(Registers.Flag.Subtract, 0b_0000_0010)]
    [TestCase(Registers.Flag.Parity, 0b_0000_0100)]
    [TestCase(Registers.Flag.HalfCarry, 0b_0001_0000)]
    [TestCase(Registers.Flag.Zero, 0b_0100_0000)]
    [TestCase(Registers.Flag.Sign, 0b_1000_0000)]
    public void TestSetFlagTrue(Registers.Flag flag, byte expected)
    {
        byte b = 0b_0000_0000;
        b.SetFlag(flag, true).Should().Be(expected);
    }

    [TestCase(Registers.Flag.Carry, 0b_1111_1110)]
    [TestCase(Registers.Flag.Subtract, 0b_1111_1101)]
    [TestCase(Registers.Flag.Parity, 0b_1111_1011)]
    [TestCase(Registers.Flag.HalfCarry, 0b_1110_1111)]
    [TestCase(Registers.Flag.Zero, 0b_1011_1111)]
    [TestCase(Registers.Flag.Sign, 0b_0111_1111)]
    public void TestSetFlagFalse(Registers.Flag flag, byte expected)
    {
        byte b = 0b_1111_1111;
        b.SetFlag(flag, false).Should().Be(expected);
    }
}
