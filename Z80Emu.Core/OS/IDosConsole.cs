namespace Z80Emu.Core.OS;

public interface IDosConsole
{
    void Write(char c);
    void Write(string text);
    void WriteLine(string text);
    byte Read();
    string ReadString();
}
