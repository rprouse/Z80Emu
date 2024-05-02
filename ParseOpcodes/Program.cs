using System.Text.Json;
using ParseOpcodes;

var url = "https://raw.githubusercontent.com/deeptoaster/opcode-table/master/opcode-table.json";
string json = await new HttpClient().GetStringAsync(url);
var opcodes = JsonSerializer.Deserialize<List<Opcode>>(json) ?? Enumerable.Empty<Opcode>();

var results = opcodes.Where(o => !o.undocumented && !o.z180)
    .SelectMany(o => o.GenerateOpcodes())
    .ToList();

Console.WriteLine("Generating OpcodeHandler.Initialize.cs");
using (var file = new StreamWriter("OpcodeHandler.Initialize.cs", false))
{
    file.WriteLine("namespace Z80Emu.Core.Processor.Opcodes;");
    file.WriteLine();
    file.WriteLine("public partial class OpcodeHandler");
    file.WriteLine("{");
    file.WriteLine("    private void InitializeOpcodes()");
    file.WriteLine("    {");
    foreach (var result in results)
    {
        file.WriteLine($"        Add({result.ToCodeString()});");
    }
    file.WriteLine("    }");
    file.WriteLine("}");
}


Console.WriteLine("Generating OpcodeHandler.Methods.cs");
using (var file = new StreamWriter("OpcodeHandler.Methods.cs", false))
{
    file.WriteLine("namespace Z80Emu.Core.Processor.Opcodes;");
    file.WriteLine();
    file.WriteLine("public partial class OpcodeHandler");
    file.WriteLine("{");
    file.WriteLine("    private void InitializeMethods()");
    file.WriteLine("    {");
    foreach (var result in results)
    {
        file.WriteLine($"        _opcodes[\"{result.Id}\"].Execute = () => {{ }};");
    }
    file.WriteLine("    }");
    file.WriteLine("}");
}