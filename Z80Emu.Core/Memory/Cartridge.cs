using System.Runtime.Intrinsics.Arm;

namespace Z80Emu.Core.Memory;

/// <summary>
/// ROM and RAM in a Cartridge
/// </summary>
/// <remarks>
/// 0x0000 - 0x3FFF 16KB ROM Bank 00 (in cartridge, fixed at bank 00)
/// 0x4000 - 0x7FFF 16KB ROM Bank 01..NN(in cartridge, switchable bank number)
/// 0xA000 - 0xBFFF 8KB External RAM(in cartridge, switchable bank, if any)
/// </remarks>
public class Cartridge
{
    private MemoryBlock _romBank0;
    private MemoryBlock _ramBank;
    private int _romBank = 0;

    public enum BankType
    {
        None,
        MBC1,
        MBC2,
        MBC3
    }

    public BankType MBC { get; }

    public Cartridge(byte[] data)
    {
        // Start simple with one ROM bank
        MBC = BankType.MBC1;
        _romBank0 = new MemoryBlock(0x0000, 0x3FFF);
        _ramBank = new MemoryBlock(0xA000, 0xBFFF);

        _romBank0.Copy(data);

        MBC = GetMbc();

        if (MBC != BankType.None) throw new ArgumentException($"Unsupported bank type {MBC}");
    }

    private BankType GetMbc() =>
        _romBank0[0x147] switch
        {
            0x00 => BankType.None,
            0x01 => BankType.MBC1,
            0x02 => BankType.MBC1,
            0x03 => BankType.MBC1,
            0x05 => BankType.MBC2,
            0x06 => BankType.MBC2,
            0x0F => BankType.MBC3,
            0x10 => BankType.MBC3,
            0x11 => BankType.MBC3,
            0x12 => BankType.MBC3,
            0x13 => BankType.MBC3,
            _ => BankType.None,
        };

    private static int GetRomBanks(int id) =>
        id switch
        {
            0 => 2,
            1 => 4,
            2 => 8,
            3 => 16,
            4 => 32,
            5 => 64,
            6 => 128,
            7 => 256,
            0x52 => 72,
            0x53 => 80,
            0x54 => 96,
            _ => throw new ArgumentException("Unsupported ROM size")
        };

    private static int GetRamBanks(int id) =>
        id switch
        {
            0 => 0,
            1 => 1,
            2 => 1,
            3 => 4,
            4 => 16,
            _ => throw new ArgumentException("Unsupported RAM size: ")
        };

    public byte this[int address]
    {
        get => Read(address);
        set => Write(address, value);
    }

    private byte Read(int address) =>
        address switch
        {
            <= 0x3FFF => _romBank0[address],
            >= 0xA000 and <= 0xBFFF => _ramBank[address],
            _ => throw new NotImplementedException(),
        };

    private void Write(int address, byte data)
    {
        if (address >= 0xA000 && address <= 0xBFFF)
        {
            _ramBank[address] = data;
        }
        else
        {
            // TODO: Handle bank switching?
        }
    }

}
