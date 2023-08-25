using System.Text;
using Z80Emu.Core.OS;

public class DosConsole : IDosConsole
{
    public byte Read() => (byte)Console.Read();

    public string ReadString()
    {
        var sb = new StringBuilder();
        while (true)
        {
            var c = Console.ReadKey();
            if (c.Key == ConsoleKey.Enter)
                break;
            if (c.Key == ConsoleKey.Backspace || c.Key == ConsoleKey.Delete)
            {
                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    Write("\b \b");
                }
                continue;
            }
            sb.Append(c.KeyChar);
        }
        return sb.ToString();
    }

    public void Write(char c) => AnsiConsole.Write(c);

    public void Write(string text) => AnsiConsole.Write(text);

    public void WriteLine(string text) => AnsiConsole.WriteLine(text);
}