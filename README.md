# Z80Emu

A simple Z80 emulator written in C#. Opcodes are generated automatically
using the [JSON Opcode Table](https://github.com/deeptoaster/opcode-table/blob/master/opcode-table.json)
from [Z80 Opcode Table](https://clrhome.org/table/).

![CI/CD Build](https://github.com/rprouse/z80emu/actions/workflows/dotnet.yml/badge.svg)

## Limitations

- This does not emulate undocumented opcodes
- It is not cyle accurate
- It does not set the undocumented flags

