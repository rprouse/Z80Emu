using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Z80Emu.Core.Processor.Opcodes;

public partial class EdOpcodeHandler
{
    protected override Dictionary<byte, Opcode> Initialize() => new Dictionary<byte, Opcode>
    {
    };
}
