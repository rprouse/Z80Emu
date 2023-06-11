# Z80Emu

A Z80 emulator/monitor program written in C#. Opcodes are generated automatically
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

Most commands are kept to one charater to make it easier to enter them
quickly. The exception is the `reg` which is used less often since the 
registers are displayed after every step or run.

| Command | Example | Description |
| ------- | ------- | ----------- |
| `h`     | `h`     | Display help |
| `s`     | `s`     | Step through the program one instruction at a time |
| `r`     | `r`     | Run the program to the next breakpoint |
| `reg`     | `reg`     | Dump the registers and flags |
| `m [<addr>]`     | `m 100`     | Dump the memory starting at the given address. If the address is not specified, defaults to 0x100 or the last memory address viewed |
| `d [<addr>]` | `d 100` | Disassemble the program starting at the given address. If no address is given, will start disassembly at the program counter. If entered again, it will continue after the last disassembled address. |
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

### OS System Calls

The emulator currently supports a small subset of the [CP/M 2.2](https://www.seasip.info/Cpm/bdos.html)
system calls. For a list of supported system calls, see [CPM22.cs](./Z80Emu.Core/OS/CPM22.cs). The plan
is to expand the supported system calls as needed and to add support for the
Agon Light [MOS API](https://github.com/breakintoprogram/agon-docs/wiki/MOS-API) system calls.
You will be able to select between CP/M and Agon MOS with a command line switch.

### Limitations

- This does not emulate undocumented opcodes
- It is not cyle accurate
- It does not set the undocumented flags
- Only a subset of system calls are supported

