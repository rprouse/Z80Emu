# Z80Emu

A simple Z80 emulator written in C#. Opcodes are generated automatically
using the [JSON Opcode Table](https://github.com/deeptoaster/opcode-table/blob/master/opcode-table.json)
from [Z80 Opcode Table](https://clrhome.org/table/).

![CI/CD Build](https://github.com/rprouse/z80emu/actions/workflows/dotnet.yml/badge.svg)

## Usage

Pass in a compiled Z80 binary file as the first argument to the program.

```bash
z80emu <program.com>
```

This will load the program to the CP/M or Agon MOS program memory location 0x0100 
and drop you to a prompt where you can enter commands. (TODO: Allow loading to 
other memory locations)

### Commands

| Command | Example | Description |
| ------- | ------- | ----------- |
| `h`     | `h`     | Display help |
| `s`     | `s`     | Step through the program one instruction at a time |
| `r`     | `r`     | Run the program to the next breakpoint |
| `reg`     | `reg`     | Dump the registers and flags |
| `m [address]`     | `m 100`     | Dump the memory starting at the given address. If the address is not specified, defaults to 0x100 or the last memory address viewed |
| `d [<address>]` | `d 100` | Disassemble the program starting at the given address. If no address is given, will start disassembly at the program counter. If entered again, it will continue after the last disassembled address. |
| `b` | `b` | Manage breakpoints |
| `q`     | `q`     | Quit the emulator |

### Breakpoints

Breakpoints can be managed by entering the `b` command.

| Command | Description |
| ------- | ----------- |
| `add`   | Add a breakpoint. You will be prompted for an address to set a breakpoint at |
| `delete` | Delete a breakpoint. You will be shown all breakpoints and be able to select one to delete |
| `list`  | List breakpoints |
| `clear` | Clear all breakpoints |
| `quit`  | Quit the breakpoint manager |

## Limitations

- This does not emulate undocumented opcodes
- It is not cyle accurate
- It does not set the undocumented flags

