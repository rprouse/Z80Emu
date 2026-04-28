# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project overview

A Z80 CPU emulator + interactive monitor written in C#. Loads a binary (typically a CP/M `.com` file) into emulated memory at `0x0100` and exposes a stepping/disassembly/breakpoint REPL. Currently emulates a documented-only Z80 (no undocumented opcodes, no cycle accuracy, no undocumented flags) and a small subset of CP/M 2.2 BDOS calls. Agon Light MOS support is planned but not yet wired up.

## Solution layout

Three projects in `Z80Emu.sln`, all targeting **.NET 10**. Both GitHub Actions workflows (`.github/workflows/dotnet.yml`, `release.yml`) install the matching `10.0.x` SDK and publish from `bin/Release/net10.0/...`.

- **`Z80Emu.Core/`** — pure emulator library. No console / UI dependencies. This is where CPU, memory, opcode dispatch, ports, interrupts, and OS-call abstractions live.
- **`Z80Emu/`** — console front-end ("Monitor"). Uses Spectre.Console for output and references `Z80Emu.Core`. `Program.cs` constructs `CPM22 → Emulator → Monitor` and dispatches commands.
- **`Z80Emu.Tests/`** — NUnit + Shouldly tests. Tests live under folders that mirror `Z80Emu.Core` (`Processor/`, `Memory/`, `OS/`, `Utilities/`). Opcode tests are grouped by category (`ArithmeticOpcodeTests.cs`, `BitOpcodeTests.cs`, etc.).

## Common commands

```bash
# Restore + build everything
dotnet build

# Run all tests
dotnet test

# Run tests in a single file / fixture
dotnet test --filter "FullyQualifiedName~ArithmeticOpcodeTests"
dotnet test --filter "Name=ResetClearsWarmBoot"

# Run the emulator against a CP/M binary (default load address 0x0100)
dotnet run --project Z80Emu -- path/to/program.com
dotnet run --project Z80Emu -- path/to/program.com 0x0100
```

A test binary `Z80Emu.Tests/Test.com` (built from `Test.asm`) is copied to test output and used by integration-style tests.

## Architecture

### Core execution flow

`Emulator` (`Z80Emu.Core/Emulator.cs`) is the composition root. It owns an `MMU`, `Ports`, `Interupts`, a `CPU`, and an injected `IDos`. Each call to `Emulator.Tick()`:

1. `CPU.Tick()` → `OpcodeHandler.FetchInstruction()` matches bytes at `PC` against the opcode table, advances `PC` past the opcode bytes (operands are consumed by the lambda), and invokes the opcode's `Execute` delegate, which mutates `Registers` / `MMU` / `Ports`.
2. The emulator then checks whether `PC` matches any `IDos.CallVectors` (e.g. `0x0005` for CP/M BDOS) and, if so, dispatches to `IDos.Execute(this)`. The OS implementation reads register state, performs host-side I/O via `IDosConsole`, and calls `CPU.Return()` to pop a return address — this is how "system calls" are simulated without actually executing CP/M code.

`Emulator.Reset()` rebuilds `MMU` / `Ports` / `Interupts` / `CPU` from scratch (preserving the loaded program filename + base address so it gets reloaded), then calls `IDos.Initialize`. This is genuine state reset, not a soft reset.

### Opcode dispatch

`OpcodeHandler` is a `partial class` split across three files:

- `OpcodeHandler.cs` — fetch/peek logic, helpers (`NextByte`, `NextWord`, `AddSubtractByte`, etc.), the `_opcodes` dictionary keyed by `"<first-byte> <mnemonic>"`.
- `OpcodeHandler.Initialize.cs` — ~1500 `Add(new Opcode(...))` calls registering the table.
- `OpcodeHandler.Methods.cs` — assigns the `Execute` lambda for each registered opcode (e.g. `_opcodes["8E ADC A,(HL)"].Execute = () => ADC(_mmu[_reg.HL]);`).

`Opcode.Match` linear-scans the dictionary on every fetch — there is no prefix tree / jump table. Acceptable because the table is fixed-size and the project isn't aiming for cycle accuracy. `Opcode.Bytes` contains literal hex strings plus placeholders `"n"` (immediate byte), `"d"` (signed displacement), `"nn"` (immediate word) which get resolved by `SetSubstitutions` for display and consumed by the `Execute` lambda via `NextByte`/`NextWord`.

### Memory, ports, OS

- `MMU` is a thin facade over an array of `MemoryBlock`s (currently a single 64K RAM block at `0x0000–0xFFFF`); the structure is set up to support memory-mapped regions later. Indexer-style access: `_mmu[addr]`.
- `Ports` exposes a 256-entry array with an `OnPortChanged` event the monitor subscribes to so port writes are echoed to the user.
- `IDos` (`Z80Emu.Core/OS/IDos.cs`) abstracts the host OS layer. `CPM22` implements CP/M 2.2 BDOS: BDOS dispatch is via vector `0x0005`, system call number is in `C`, args are in `DE`, results returned in `A`/`HL`. Console I/O is pushed through `IDosConsole` so tests can fake it. Add new BDOS calls by extending the `SystemCalls` enum and the dispatch in `CPM22.Execute`.

### Type conventions

- `global using word = System.UInt16;` is declared in every project's `Usings.cs` (with `#pragma warning disable CS8981` because lowercase type names are reserved-ish). Treat `word` as the canonical 16-bit-address/value type and prefer it over `ushort` for Z80-semantic values.
- `byte` is used for 8-bit register values; `sbyte` casts handle signed displacements (`d`).
- `Z80Emu.Core` has nullable enabled; tests have nullable disabled.

## Testing patterns

- NUnit with `Shouldly` (globally imported in `Z80Emu.Tests/Usings.cs`). Use `.ShouldBe(...)`.
- `Z80Emu.Tests/ShouldlyByteWordExtensions.cs` adds `ShouldBe` overloads for `byte` and `ushort` — Shouldly only ships native overloads for `int`/`long`/`decimal`/etc., so without the shim a `byte`/`ushort` receiver with an `int` literal expected (e.g. `_reg.A.ShouldBe(0x3A)`) fails overload resolution. Don't delete the file; if you add tests that assert on a new narrow numeric type and hit the same error, extend the shim rather than casting at every call site.
- `TestHelpers.FlagsShouldBe(s, z, h, pv, n, c)` is the canonical way to assert the flag register after an opcode — use it instead of asserting individual flags so failures show the full mismatch.
- Opcode tests typically construct a `CPU` directly, write opcode bytes into the `MMU`, call `Tick()`, then assert register/flag state. See `OpcodeTestExtensions.cs` for the shared helpers.
- A `MockOperatingSytem` (see `EmulatorTests.cs`) implements `IDos` for tests that need to verify the OS-call dispatch path without engaging full CP/M behavior.

## When adding a new Z80 opcode

1. Add an `Add(new Opcode(...))` row in `OpcodeHandler.Initialize.cs` next to neighbouring opcodes.
2. Implement the lambda in `OpcodeHandler.Methods.cs` — follow neighbours; arithmetic ops route through `ADC` / `ADD` / `AddSubtractByte`, bit ops have their own helpers, etc.
3. Add tests under `Z80Emu.Tests/Processor/Opcodes/<Category>OpcodeTests.cs` covering both the result and `FlagsShouldBe(...)`.
