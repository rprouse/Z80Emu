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

            _mmu[0x0100].ShouldBe(0x3A);
            _mmu[0x0101].ShouldBe(0x0B);
            _mmu[0x0102].ShouldBe(0x01);
            _mmu[0x0103].ShouldBe(0x21);
            _mmu[0x0104].ShouldBe(0x0C);
            _mmu[0x0105].ShouldBe(0x01);
            // ...
            _mmu[0x010B].ShouldBe(0x38);
            _mmu[0x010C].ShouldBe(0x2B);
            _mmu[0x010D].ShouldBe(0x00);
        }

        [Test]
        public void CanLoadProgramAtSpecifiedAddress()
        {
            _mmu.LoadProgram("Test.com", 0x0200);

            _mmu[0x0200].ShouldBe(0x3A);
            _mmu[0x0201].ShouldBe(0x0B);
            _mmu[0x0202].ShouldBe(0x01);
            _mmu[0x0203].ShouldBe(0x21);
            _mmu[0x0204].ShouldBe(0x0C);
            _mmu[0x0205].ShouldBe(0x01);
            // ...
            _mmu[0x020B].ShouldBe(0x38);
            _mmu[0x020C].ShouldBe(0x2B);
            _mmu[0x020D].ShouldBe(0x00);
        }

        [Test]
        public void CanReadAndWriteByte()
        {
            _mmu[0x0100] = 0x3A;

            _mmu[0x0100].ShouldBe(0x3A);
        }

        [TestCase(-1)]
        [TestCase(0x10000)]
        public void ReadFromMemoryOutOfRangeThrowsException(int addr)
        {
            Action act = () => { byte b = _mmu[addr]; };
            Should.Throw<IndexOutOfRangeException>(act);
        }

        [TestCase(-1)]
        [TestCase(0x10000)]
        public void WriteToMemoryOutOfRangeThrowsException(int addr)
        {
            Action act = () => _mmu[addr] = 0x3A;
            Should.Throw<IndexOutOfRangeException>(act);
        }
    }
}
