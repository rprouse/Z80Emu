using Z80Emu.Core.Memory;

namespace Z80Emu.Tests.Memory
{
    public class MMUTests
    {
        MMU _mmu;

        [SetUp]
        public void Setup()
        {
            _mmu = new MMU();
        }

        [Test]
        public void CanLoadProgramAtDefaultAddress()
        {
            _mmu.LoadProgram("Test.com");

            _mmu[0x0100].Should().Be(0x3A);
            _mmu[0x0101].Should().Be(0x0B);
            _mmu[0x0102].Should().Be(0x01);
            _mmu[0x0103].Should().Be(0x21);
            _mmu[0x0104].Should().Be(0x0C);
            _mmu[0x0105].Should().Be(0x01);
            // ...
            _mmu[0x010B].Should().Be(0x38);
            _mmu[0x010C].Should().Be(0x2B);
            _mmu[0x010D].Should().Be(0x00);
        }

        [Test]
        public void CanLoadProgramAtSpecifiedAddress()
        {
            _mmu.LoadProgram("Test.com", 0x0200);

            _mmu[0x0200].Should().Be(0x3A);
            _mmu[0x0201].Should().Be(0x0B);
            _mmu[0x0202].Should().Be(0x01);
            _mmu[0x0203].Should().Be(0x21);
            _mmu[0x0204].Should().Be(0x0C);
            _mmu[0x0205].Should().Be(0x01);
            // ...
            _mmu[0x020B].Should().Be(0x38);
            _mmu[0x020C].Should().Be(0x2B);
            _mmu[0x020D].Should().Be(0x00);
        }

        [Test]
        public void CanReadAndWriteByte()
        {
            _mmu[0x0100] = 0x3A;

            _mmu[0x0100].Should().Be(0x3A);
        }

        [TestCase(-1)]
        [TestCase(0x10000)]
        public void ReadFromMemoryOutOfRangeThrowsException(int addr)
        {
            Action act = () => { byte b = _mmu[addr]; };
            act.Should().Throw<IndexOutOfRangeException>();
        }

        [TestCase(-1)]
        [TestCase(0x10000)]
        public void WriteToMemoryOutOfRangeThrowsException(int addr)
        {
            Action act = () => _mmu[addr] = 0x3A;
            act.Should().Throw<IndexOutOfRangeException>();
        }
    }
}
