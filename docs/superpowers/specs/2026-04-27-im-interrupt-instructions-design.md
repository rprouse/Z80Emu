# IM Interrupt Instructions — Design

**Issue:** [#21 — IM interrupt instructions](https://github.com/rprouse/z80emu/issues/21)
**Date:** 2026-04-27
**Status:** Approved, ready for implementation plan

## Context

The Z80 emulator registers `IM 0`, `IM 1`, `IM 2`, `EI`, and `DI` in the opcode table but their `Execute` lambdas in `OpcodeHandler.Methods.cs` are empty stubs. The `Interupts` class has `IFF1`/`IFF2` properties and stub `Enable()`/`Disable()` methods, but nothing actually writes to the IFFs from instruction execution. As a result:

- `LD A,I` and `LD A,R` set `FlagPV` from `IFF2` (`OpcodeHandler.Methods.cs:523,532`), but `IFF2` is permanently `false` — the flag is silently always `0`.
- `RETI` and `RETN` (`Methods.cs:841-850`) copy `IFF2 → IFF1` correctly, but since `IFF2` is never written, both reduce to "RET with extra steps".
- A program containing `IM 1; EI; HALT` runs but is observably indistinguishable from `NOP; NOP; HALT`.

There is also no interrupt source in the emulator — `Emulator.Tick()` does not check for an interrupt request, and there is no API for an external caller to assert one.

## Goals

1. Give `IM 0/1/2`, `EI`, `DI`, `RETI`, `RETN` correct semantics.
2. Add an interrupt-request API on `Interupts` and an interrupt-servicing step in `Emulator.Tick()`.
3. Expose `int` and `nmi` REPL commands in the Monitor so a user can drive the interrupt machinery interactively from the debugger.
4. Make interrupt entry observable to the user as a distinct line in `step` output (this is a learning tool — invisible state changes erode trust).

## Non-goals

- Cycle accuracy. Interrupt entry currently takes 11/13/19 T-states on real hardware; the emulator does not track T-states and will not start.
- Real interrupt sources (peripherals, timers). The latch is driven from the REPL only, until something like RomWBW (#40) needs more.
- Support for non-`0xFF` data bus values in IM 0. See decision below.
- Saving/restoring interrupt state across `Emulator.Reset()` beyond what already happens (the `Interupts` instance is rebuilt, which is correct).

## Design decisions

### D1: Synthetic interrupt-entry pseudo-opcode at *start* of `Tick()`

When an interrupt is pending and accepted, `Emulator.Tick()` performs only the entry sequence (push PC, IFF flips, jump to vector) and returns a synthetic `Opcode` with `Mnemonic = "INT"` or `"NMI"` and `Length = 0`. The next `Tick()` fetches normally at the vector address.

**Why:** The Monitor's `Step()` calls `Tick()` once and displays whatever opcode it returns. Returning a synthetic `Opcode` from `Tick()` means interrupt entry shows up as its own line in `step` output with zero new rendering code in the Monitor — the existing `ViewOpcode` path handles it. `Length = 0` keeps disassembly accounting honest (no bytes were consumed from memory).

Real hardware services interrupts at the *end* of an instruction, not before the next fetch. Because the emulator has no concept of "during instruction", the two are observationally identical, and start-of-tick is the only place where servicing can be visible as a distinct event without requiring a side channel.

### D2: IM 0 hardcoded as `RST 38h`

When IM 0 is the active mode and an interrupt is accepted, the emulator pushes `PC` and jumps to `0x0038` — equivalent to executing `RST 38h`. The `RequestData` byte is ignored in IM 0.

**Why:** Real Z80 hardware in IM 0 reads an opcode off the data bus and executes it. Without a bus, faithfully implementing this would require synthesizing a one-shot dispatch through `OpcodeHandler`, which is `PC`-driven and doesn't naturally accept a single externally-supplied byte. Hardcoding `RST 38h` matches what the overwhelming majority of production Z80 hardware actually did — undriven bus lines floated high (`0xFF`) and `0xFF` is `RST 38h` — so this is a faithful simplification, not a stub. If a user ever needs non-`0xFF` IM 0 behavior, the upgrade path is to extend `ServiceInterrupts` to dispatch a byte through a new path; nothing in this design forecloses that.

### D3: EI shadow honored

`EI` sets `IFF1=IFF2=true` AND `EiPending=true`. At the start of `Tick()`, if `EiPending` is set, it is cleared and interrupt sampling is skipped for that one tick.

**Why:** On real Z80 hardware, `EI` defers interrupt sampling by one instruction so that `EI; RET` at the end of an ISR allows `RET` to execute before any pending IRQ re-enters the ISR. Without this, contrived test programs run differently in this emulator than on real hardware, which is the kind of divergence that erodes trust in a learning tool. The Z80 reference manual specifies that NMI is also delayed by the EI shadow, so the same gate applies to both.

### D4: One-shot latch with persistence across blocked ticks

`Interupts.RaiseInterrupt(byte? data = null)` sets `IsRequested=true` and `RequestData=data`. The latch is cleared **only when serviced**, not on the next tick. If `IFF1=0` when `int` is called, the latch holds; when `EI` later runs and the EI shadow expires, the interrupt fires.

**Why:** This mirrors a peripheral asserting `/INT` and waiting for acknowledgment. It also gives users a useful debugging affordance: they can `int`, then `s` through whatever they like, and the interrupt fires when interrupts are next enabled — which is how a real ISR-aware program would experience it. NMI uses the same shape (`IsNmiRequested`), but is not gated on `IFF1`.

### D5: `Interupts.Enable()` / `Disable()` removed

The empty `Enable()` and `Disable()` methods on `Interupts` are removed. The IFF/EiPending state changes that `EI`/`DI` need are written directly into the opcode bodies in `OpcodeHandler.Methods.cs`.

**Why:** The opcode bodies are already the single point of truth for every other CPU instruction; routing two of them through stub methods on `Interupts` would just hide the logic behind an indirection that no one else uses. Direct property access keeps the same pattern as `RETI`/`RETN`, which already manipulate `_int.IFF1` and `_int.IFF2` directly.

## Architecture

### New / changed types in `Z80Emu.Core`

| File | Change |
|---|---|
| `Processor/InterruptMode.cs` | **New.** Enum `Mode0`, `Mode1`, `Mode2`. Default `Mode0` (Z80 power-on state). |
| `Processor/Interupts.cs` | Add `Mode`, `IsRequested`, `RequestData` (`byte?`), `IsNmiRequested`, `EiPending`. Add `RaiseInterrupt(byte? data = null)`, `RaiseNmi()`. Remove `Enable()`/`Disable()`. |
| `Processor/CPU.cs` | Add a helper that pushes `PC` and jumps to a given vector (used by `Emulator.ServiceInterrupts`). Exact name decided during implementation; the boundary is "anything that touches `Registers`/`MMU`/stack lives behind `CPU`/`OpcodeHandler`, never directly in `Emulator`". |
| `Emulator.cs` | New private `ServiceInterrupts()` called at the start of `Tick()`. Returns a synthetic `Opcode` if servicing happened, else `null`. |
| `Processor/Opcodes/OpcodeHandler.Methods.cs` | Fill in `IM 0/1/2`, `EI`, `DI` lambdas. |

### Changed types in `Z80Emu` (Monitor)

| File | Change |
|---|---|
| `Monitor.cs` | Add `int [<hex>]` and `nmi` to the command switch in `Run()`. Add help table rows. Print a one-line latch confirmation. The synthetic `INT`/`NMI` opcode flows through the existing `ViewOpcode` path in `Step()`/`Run()` automatically. |

## Control flow inside `Tick()`

```
Tick():
  if (Interupts.EiPending):
    Interupts.EiPending = false        # shadow expires; skip sampling this tick
  else:
    if (Interupts.IsNmiRequested):
      return EnterNmi()                # synthetic "NMI" opcode
    if (Interupts.IsRequested && Interupts.IFF1):
      return EnterMaskable()           # synthetic "INT" opcode
  opcode = CPU.Tick()                  # normal fetch+execute
  CheckOsCall()                        # existing
  return opcode

EnterMaskable():
  switch Interupts.Mode:
    Mode0, Mode1: vector = 0x0038
    Mode2:        vector = MMU.ReadWord((I << 8) | (RequestData ?? 0xFF))
  CPU.PushReturnAndJump(vector)        # push PC, PC = vector
  Interupts.IFF1 = false
  Interupts.IFF2 = false
  Interupts.IsRequested = false
  Interupts.RequestData = null
  return synthetic Opcode("INT", "Maskable interrupt accepted (IM <n>) → 0x<vector>")

EnterNmi():
  CPU.PushReturnAndJump(0x0066)
  Interupts.IFF2 = Interupts.IFF1      # save slot for RETN
  Interupts.IFF1 = false
  Interupts.IsNmiRequested = false
  return synthetic Opcode("NMI", "Non-maskable interrupt accepted → 0x0066")
```

Opcode bodies:

```
EI:    IFF1 = IFF2 = true;  EiPending = true;
DI:    IFF1 = IFF2 = false; EiPending = false;
IM 0:  Mode = InterruptMode.Mode0;
IM 1:  Mode = InterruptMode.Mode1;
IM 2:  Mode = InterruptMode.Mode2;
```

## REPL surface

| Command | Example | Description |
|---|---|---|
| `int` | `int` | Latch a maskable interrupt request, no data byte. (IM 2 falls back to vector low byte `0xFF`.) |
| `int <hex>` | `int 04` | Latch maskable interrupt with bus byte `0x04`. Used as the IM 2 vector low byte; ignored in IM 0/1. |
| `nmi` | `nmi` | Latch a non-maskable interrupt request. |

Echoes:

```
> int
[INT latched: mode=IM1]
> s
0125 INT             ; Maskable interrupt accepted (IM 1) → 0x0038, IFF1=0
   PC 0038  SP FFFC  ...
> s
0038 C3 50 00     JP 0050           ; (vector handler executes normally)
```

If `int` is called and `IFF1=0`, the latch is set but no servicing happens until the program runs `EI` and the shadow expires. NMI fires regardless of `IFF1`.

## Testing strategy

### Extend `Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs`

- `IM 0/1/2` set the corresponding `Interupts.Mode`.
- `EI` sets `IFF1`, `IFF2`, and `EiPending` to `true`.
- `DI` clears `IFF1`, `IFF2`, and `EiPending`.
- `LD A,I` after `EI` produces `FlagPV=1`. (Regression — currently impossible because `EI` is a stub.)
- `LD A,R` after `EI` produces `FlagPV=1`. (Same.)

### New `Z80Emu.Tests/Processor/InteruptsTests.cs`

Higher-level tests that drive the emulator's `Tick()` so the synthetic-opcode entry path is exercised end-to-end.

- **IM 1 entry:** `IM 1; EI; NOP; …` then `RaiseInterrupt()` — `Tick()` returns synthetic `INT`, PC=`0x0038`, original PC pushed, IFF1=IFF2=0.
- **IM 2 entry:** `I = 0x80`, MMU at `0x8004`/`0x8005` set to vector `0x1234`, `RaiseInterrupt(0x04)` — PC=`0x1234`.
- **IM 2 default byte:** `RaiseInterrupt()` (no byte) in IM 2 reads from `(I << 8) | 0xFF`.
- **IM 0 entry:** behaves identically to IM 1 (PC=`0x0038`).
- **NMI entry:** push PC, PC=`0x0066`, IFF1=0.
- **NMI copies IFF1 to IFF2:** with IFF1=1 and IFF2=0 (artificial pre-state, simulating nested NMI), trigger NMI — afterwards IFF2=1 (took IFF1's value), IFF1=0.
- **RETN restores IFF1 from IFF2:** after `EI` (IFF1=IFF2=1) → instruction → NMI (IFF1=0, IFF2=1) → `RETN` produces IFF1=1.
- **EI shadow:** after `EI`, `RaiseInterrupt()` does *not* fire on the immediately-following tick; it fires on the next one.
- **NMI EI shadow:** same as above for NMI.
- **Latch persistence:** with `IFF1=0`, `RaiseInterrupt()` then several `Tick()`s do nothing; subsequent `EI` (after shadow) services the still-latched request.
- **Latch one-shot:** after servicing, `IsRequested=false`; second `Tick()` does not re-enter.
- **NMI ignores IFF1:** with `IFF1=0`, `RaiseNmi()` services on the next tick.

### No Monitor tests

The `Z80Emu` console project has no test harness today (Spectre.Console + `Console.ReadLine`). Adding one is out of scope for this work. The `int`/`nmi` command paths are thin wrappers over `Interupts.RaiseInterrupt`/`RaiseNmi`, which are exhaustively covered by the tests above.

## Documentation deliverables

These are part of the implementation, not follow-up work:

- **`CLAUDE.md`** — new "Interrupt model" subsection under `Architecture`, summarizing: synthetic-INT-pseudo-opcode at start of tick, EI shadow, IM 0 hardcoded as `RST 38h`, latch lifecycle. Cross-link to this spec.
- **`README.md`** —
  - Add `int`, `int <hex>`, `nmi` rows to the commands table.
  - Fix the stale opening line ("Opcodes are generated automatically using…") — `ParseOpcodes` is gone; opcodes are now hand-maintained in `Initialize.cs`/`Methods.cs`.
- **`Z80Emu/Z80Emu.csproj`** — bump version `0.6.0` → `0.7.0` (minor: new feature) per the versioning rule in `CLAUDE.md`.

## Open questions / future work

- **IM 0 with arbitrary bus byte:** if a user later needs to demonstrate IM 0 executing a non-`RST 38h` opcode, extend `ServiceInterrupts` to dispatch `RequestData` through `OpcodeHandler` directly. The latch field is already there.
- **`reg` showing latch state:** would be useful for debugging but adds clutter to the register dump. Skipped for this pass; trivial to add later if requested.
- **External interrupt sources:** when peripheral emulation is needed (e.g. for #40 RomWBW), `Ports` or a future `Devices` layer can call `Interupts.RaiseInterrupt(...)`. The API shape is already correct for that.
