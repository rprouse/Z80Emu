# IM Interrupt Instructions Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Wire up `IM 0/1/2`, `EI`, `DI`, and full interrupt servicing in `Emulator.Tick()` with `int`/`nmi` REPL commands, closing GitHub issue #21.

**Architecture:** Synthetic `INT`/`NMI` pseudo-opcodes returned from `Emulator.Tick()` at start-of-tick (so interrupt entry shows up as a distinct line in the Monitor). `Interupts` owns mode/latch/IFF state. `CPU.AcceptInterrupt(vector)` performs the push-PC-and-jump primitive (via the now-public `OpcodeHandler.RST` helper). `Emulator.ServiceInterrupts()` orchestrates: reads mode + latch from `Interupts`, computes the vector (with IM 2 reading from `MMU` using the `I` register), calls `CPU.AcceptInterrupt`, then updates `IFF1/IFF2` and clears the latch.

**Tech Stack:** C# / .NET 10, NUnit + Shouldly for tests, Spectre.Console for the Monitor REPL.

**Spec:** `docs/superpowers/specs/2026-04-27-im-interrupt-instructions-design.md`

---

## File Structure

**Create:**
- `Z80Emu.Core/Processor/InterruptMode.cs` — new enum.
- `Z80Emu.Tests/Processor/InteruptsTests.cs` — high-level interrupt tests that drive `Emulator.Tick()` end-to-end.

**Modify:**
- `Z80Emu.Core/Processor/Interupts.cs` — new fields/methods, remove dead stubs.
- `Z80Emu.Core/Processor/CPU.cs` — expose `AcceptInterrupt(word)`.
- `Z80Emu.Core/Processor/Opcodes/OpcodeHandler.cs` — refactor broken `RST` to public `void RST(word)`.
- `Z80Emu.Core/Processor/Opcodes/OpcodeHandler.Methods.cs` — fill `IM 0/1/2`, `EI`, `DI` lambdas.
- `Z80Emu.Core/Emulator.cs` — add `ServiceInterrupts()` step in `Tick()`.
- `Z80Emu/Monitor.cs` — add `int [<hex>]` and `nmi` commands plus help.
- `Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs` — add `RST`, `IM`, `EI`, `DI`, regression tests for `LD A,I/R`.
- `README.md` — commands table rows + fix stale opcode-table line.
- `CLAUDE.md` — new "Interrupt model" subsection under Architecture.
- `Z80Emu/Z80Emu.csproj` — version bump `0.6.0` → `0.7.0`.

---

## Task 1: Fix the broken `RST` opcode primitive

**Background:** `OpcodeHandler.cs:509-515` declares `RST` as `Action RST(word) => () => { ... }` (returns an Action). The bindings in `OpcodeHandler.Methods.cs:913-920` are written as `_opcodes["C7 RST 0"].Execute = () => RST(0x00);` — when `Execute()` runs, it calls `RST(0x00)`, gets back an `Action`, and discards it. The push/jump never happens. There are zero tests for any RST opcode today, so this is silently broken. We need a working push-PC-and-jump primitive for interrupt entry, so we'll fix RST first and reuse it.

**Files:**
- Test: `Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs`
- Modify: `Z80Emu.Core/Processor/Opcodes/OpcodeHandler.cs:509-515`

- [ ] **Step 1.1: Write a failing test for `RST 0`**

Append to `ControlOpcodeTests.cs` (before the closing `}` of the class):

```csharp
    [Test]
    public void RST_0()
    {
        _mmu[0x0100] = 0xC7;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("RST 0");

        _reg.PC.ShouldBe((word)0x0000);
        _reg.SP.ShouldBe((word)0xFFFC);
        _mmu[0xFFFC].ShouldBe((byte)0x01);  // PC.Lsb() pushed at lower addr
        _mmu[0xFFFD].ShouldBe((byte)0x01);  // PC.Msb() pushed at higher addr
    }
```

Note: PC was `0x0100` at start, advanced to `0x0101` by `FetchInstruction` (RST is a 1-byte opcode), so the pushed return address is `0x0101`.

- [ ] **Step 1.2: Run the test and verify it fails**

```bash
dotnet test --filter "Name=RST_0"
```

Expected: FAIL. The Execute lambda calls `RST(0x00)` and discards the returned Action, so PC stays at `0x0101` and SP stays at `0xFFFE`.

- [ ] **Step 1.3: Refactor `RST` to a void method and make it public**

Replace lines 509-515 in `OpcodeHandler.cs` (the existing `Action RST(word address) => () => { ... };` block):

```csharp
    // This is public so interrupt entry can push PC and jump to a vector.
    public void RST(word address)
    {
        _mmu[--_reg.SP] = _reg.PC.Msb();
        _mmu[--_reg.SP] = _reg.PC.Lsb();
        _reg.PC = address;
    }
```

The 8 callers in `OpcodeHandler.Methods.cs:913-920` already have the correct shape (`() => RST(0x00)` etc.) — they were always meant to call a void method.

- [ ] **Step 1.4: Run the test and verify it passes**

```bash
dotnet test --filter "Name=RST_0"
```

Expected: PASS.

- [ ] **Step 1.5: Add tests for the remaining 7 RST opcodes**

Append to `ControlOpcodeTests.cs`:

```csharp
    [TestCase((byte)0xCF, (word)0x0008, "RST 8")]
    [TestCase((byte)0xD7, (word)0x0010, "RST 16")]
    [TestCase((byte)0xDF, (word)0x0018, "RST 24")]
    [TestCase((byte)0xE7, (word)0x0020, "RST 32")]
    [TestCase((byte)0xEF, (word)0x0028, "RST 40")]
    [TestCase((byte)0xF7, (word)0x0030, "RST 48")]
    [TestCase((byte)0xFF, (word)0x0038, "RST 56")]
    public void RST_NonZero(byte opcode, word vector, string mnemonic)
    {
        _mmu[0x0100] = opcode;

        _opcodeHandler.FetchVerifyAndExecuteInstruction(mnemonic);

        _reg.PC.ShouldBe(vector);
        _reg.SP.ShouldBe((word)0xFFFC);
        _mmu[0xFFFC].ShouldBe((byte)0x01);
        _mmu[0xFFFD].ShouldBe((byte)0x01);
    }
```

- [ ] **Step 1.6: Run the new tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~ControlOpcodeTests.RST"
```

Expected: 8 tests pass (`RST_0` plus 7 `RST_NonZero` cases).

- [ ] **Step 1.7: Run the full test suite to check for regressions**

```bash
dotnet test
```

Expected: all tests pass.

- [ ] **Step 1.8: Commit**

```bash
git add Z80Emu.Core/Processor/Opcodes/OpcodeHandler.cs Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs
git commit -m "Fix broken RST opcode and add tests

The RST helper was declared as 'Action RST(word) => () => { ... }', so
the bindings 'Execute = () => RST(addr)' called RST and discarded the
returned Action — the push+jump never happened. No RST tests existed
to catch this. Refactor to 'public void RST(word)' so the existing
bindings work, and add tests for all 8 RST opcodes.

Required for #21 interrupt entry, which reuses RST as the push-PC-
and-jump primitive."
```

---

## Task 2: Add `InterruptMode` enum

**Files:**
- Create: `Z80Emu.Core/Processor/InterruptMode.cs`

- [ ] **Step 2.1: Create the enum file**

Create `Z80Emu.Core/Processor/InterruptMode.cs`:

```csharp
namespace Z80Emu.Core.Processor;

public enum InterruptMode
{
    Mode0,
    Mode1,
    Mode2,
}
```

`Mode0` is first so it's the default value — matches the Z80 power-on state.

- [ ] **Step 2.2: Verify build**

```bash
dotnet build
```

Expected: Build succeeds.

- [ ] **Step 2.3: Commit**

```bash
git add Z80Emu.Core/Processor/InterruptMode.cs
git commit -m "Add InterruptMode enum"
```

---

## Task 3: Extend `Interupts` with new fields and remove dead stubs

Adds `Mode`, `EiPending`, `IsRequested`, `RequestData`, `IsNmiRequested` fields plus `RaiseInterrupt`/`RaiseNmi` methods. Removes the empty `Enable()`/`Disable()` methods (the spec moves their would-be logic into the `EI`/`DI` opcode bodies in Task 5).

**Files:**
- Modify: `Z80Emu.Core/Processor/Interupts.cs`

- [ ] **Step 3.1: Verify nothing references `Enable()` / `Disable()`**

```bash
```

Use Grep tool with pattern `Interupts\.(Enable|Disable)|_int\.(Enable|Disable)` across the whole repo.

Expected: zero matches outside `Interupts.cs` itself.

- [ ] **Step 3.2: Replace `Interupts.cs` with the extended version**

Replace the entire contents of `Z80Emu.Core/Processor/Interupts.cs` with:

```csharp
using Z80Emu.Core.Memory;

namespace Z80Emu.Core.Processor;

public class Interupts
{
    private readonly MMU _mmu;

    public bool IFF1 { get; set; }
    public bool IFF2 { get; set; }

    public InterruptMode Mode { get; set; }

    /// <summary>
    /// True after EI for one tick — interrupt sampling is suppressed for that
    /// tick so an EI immediately followed by RET runs the RET before any
    /// pending IRQ re-enters the ISR.
    /// </summary>
    public bool EiPending { get; set; }

    public bool IsRequested { get; private set; }
    public byte? RequestData { get; private set; }
    public bool IsNmiRequested { get; private set; }

    public Interupts(MMU mmu)
    {
        _mmu = mmu;
    }

    /// <summary>
    /// Latch a maskable interrupt request. The optional data byte is used
    /// as the IM 2 vector low byte; ignored in IM 0 / IM 1.
    /// </summary>
    public void RaiseInterrupt(byte? data = null)
    {
        IsRequested = true;
        RequestData = data;
    }

    /// <summary>
    /// Latch a non-maskable interrupt request.
    /// </summary>
    public void RaiseNmi()
    {
        IsNmiRequested = true;
    }

    /// <summary>
    /// Called by Emulator.ServiceInterrupts after a maskable INT is taken.
    /// </summary>
    public void ConsumeInterrupt()
    {
        IsRequested = false;
        RequestData = null;
    }

    /// <summary>
    /// Called by Emulator.ServiceInterrupts after an NMI is taken.
    /// </summary>
    public void ConsumeNmi()
    {
        IsNmiRequested = false;
    }
}
```

`Consume*` methods exist so `Emulator` can clear the latch without making the setters public.

- [ ] **Step 3.3: Verify build**

```bash
dotnet build
```

Expected: Build succeeds.

- [ ] **Step 3.4: Run all tests to confirm no regressions**

```bash
dotnet test
```

Expected: all tests pass (existing tests don't use Mode/Raise/Consume; they only touch `IFF1`/`IFF2`).

- [ ] **Step 3.5: Commit**

```bash
git add Z80Emu.Core/Processor/Interupts.cs
git commit -m "Extend Interupts with mode, EI shadow, and request latches

Adds Mode, EiPending, IsRequested, RequestData, IsNmiRequested.
RaiseInterrupt/RaiseNmi are the public latching API; ConsumeInterrupt/
ConsumeNmi are called by Emulator after servicing.

Removes the empty Enable()/Disable() stubs; EI/DI will manipulate the
IFF properties directly in a later commit, matching the pattern used
by RETI/RETN."
```

---

## Task 4: Wire `IM 0/1/2` opcode bodies (TDD)

**Files:**
- Test: `Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs`
- Modify: `Z80Emu.Core/Processor/Opcodes/OpcodeHandler.Methods.cs:322-324`

- [ ] **Step 4.1: Write the failing tests**

Append to `ControlOpcodeTests.cs`:

```csharp
    [Test]
    public void IM_0()
    {
        _int.Mode = InterruptMode.Mode2;  // start in a non-default mode to prove it changes
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x46;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 0");

        _int.Mode.ShouldBe(InterruptMode.Mode0);
    }

    [Test]
    public void IM_1()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x56;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 1");

        _int.Mode.ShouldBe(InterruptMode.Mode1);
    }

    [Test]
    public void IM_2()
    {
        _mmu[0x0100] = 0xED;
        _mmu[0x0101] = 0x5E;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("IM 2");

        _int.Mode.ShouldBe(InterruptMode.Mode2);
    }
```

- [ ] **Step 4.2: Run the tests and verify they fail**

```bash
dotnet test --filter "FullyQualifiedName~ControlOpcodeTests.IM_"
```

Expected: 3 tests fail. The IM opcodes are empty stubs (`() => { }`), so `_int.Mode` doesn't change.

- [ ] **Step 4.3: Implement the lambdas**

In `OpcodeHandler.Methods.cs`, replace lines 322-324:

```csharp
        _opcodes["ED IM 0"].Execute = () => { };
        _opcodes["ED IM 1"].Execute = () => { };
        _opcodes["ED IM 2"].Execute = () => { };
```

With:

```csharp
        _opcodes["ED IM 0"].Execute = () => _int.Mode = InterruptMode.Mode0;
        _opcodes["ED IM 1"].Execute = () => _int.Mode = InterruptMode.Mode1;
        _opcodes["ED IM 2"].Execute = () => _int.Mode = InterruptMode.Mode2;
```

- [ ] **Step 4.4: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~ControlOpcodeTests.IM_"
```

Expected: 3 tests pass.

- [ ] **Step 4.5: Commit**

```bash
git add Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs Z80Emu.Core/Processor/Opcodes/OpcodeHandler.Methods.cs
git commit -m "Wire IM 0/1/2 opcode bodies"
```

---

## Task 5: Wire `EI` / `DI` opcode bodies (TDD)

`EI` sets `IFF1=IFF2=true` and `EiPending=true`. `DI` clears all three. Also adds a regression test for `LD A,I` — its `FlagPV` reflects `IFF2`, which was previously always `false` because nothing wrote it.

**Files:**
- Test: `Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs`
- Modify: `Z80Emu.Core/Processor/Opcodes/OpcodeHandler.Methods.cs` (find `_opcodes["FB EI"]` and `_opcodes["F3 DI"]` — they will be empty stubs like the IM opcodes)

- [ ] **Step 5.1: Write failing tests for EI / DI / LD A,I-after-EI**

Append to `ControlOpcodeTests.cs`:

```csharp
    [Test]
    public void EI()
    {
        _int.IFF1 = false;
        _int.IFF2 = false;
        _int.EiPending = false;
        _mmu[0x0100] = 0xFB;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EI");

        _int.IFF1.ShouldBeTrue();
        _int.IFF2.ShouldBeTrue();
        _int.EiPending.ShouldBeTrue();
    }

    [Test]
    public void DI()
    {
        _int.IFF1 = true;
        _int.IFF2 = true;
        _int.EiPending = true;
        _mmu[0x0100] = 0xF3;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("DI");

        _int.IFF1.ShouldBeFalse();
        _int.IFF2.ShouldBeFalse();
        _int.EiPending.ShouldBeFalse();
    }

    [Test]
    public void LD_A_I_FlagPV_ReflectsIFF2_AfterEI()
    {
        // EI; LD A,I — FlagPV should be 1 because IFF2 is now true
        _mmu[0x0100] = 0xFB;        // EI
        _mmu[0x0101] = 0xED;        // LD A,I
        _mmu[0x0102] = 0x57;
        _reg.I = 0x42;

        _opcodeHandler.FetchVerifyAndExecuteInstruction("EI");
        _opcodeHandler.FetchVerifyAndExecuteInstruction("LD A,I");

        _reg.A.ShouldBe((byte)0x42);
        _reg.FlagPV.ShouldBeTrue();
    }
```

- [ ] **Step 5.2: Run the tests and verify they fail**

```bash
dotnet test --filter "FullyQualifiedName~ControlOpcodeTests.EI|FullyQualifiedName~ControlOpcodeTests.DI|FullyQualifiedName~ControlOpcodeTests.LD_A_I_FlagPV_ReflectsIFF2_AfterEI"
```

Expected: 3 tests fail (EI/DI bodies are empty stubs).

- [ ] **Step 5.3: Implement the lambdas**

In `OpcodeHandler.Methods.cs`, locate the existing lines for `FB EI` and `F3 DI` (they will be empty `() => { };` stubs). Replace them with:

```csharp
        _opcodes["FB EI"].Execute = () =>
        {
            _int.IFF1 = true;
            _int.IFF2 = true;
            _int.EiPending = true;
        };
        _opcodes["F3 DI"].Execute = () =>
        {
            _int.IFF1 = false;
            _int.IFF2 = false;
            _int.EiPending = false;
        };
```

- [ ] **Step 5.4: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~ControlOpcodeTests.EI|FullyQualifiedName~ControlOpcodeTests.DI|FullyQualifiedName~ControlOpcodeTests.LD_A_I_FlagPV_ReflectsIFF2_AfterEI"
```

Expected: 3 tests pass.

- [ ] **Step 5.5: Run the full test suite for regressions**

```bash
dotnet test
```

Expected: all pass.

- [ ] **Step 5.6: Commit**

```bash
git add Z80Emu.Tests/Processor/Opcodes/ControlOpcodeTests.cs Z80Emu.Core/Processor/Opcodes/OpcodeHandler.Methods.cs
git commit -m "Wire EI/DI opcode bodies and add EI shadow latch

Also adds a regression test for LD A,I — its FlagPV mirror of IFF2
was previously always 0 because nothing in the emulator wrote IFF2."
```

---

## Task 6: Expose `CPU.AcceptInterrupt(word vector)`

Thin pass-through to `OpcodeHandler.RST` (now public after Task 1). Keeps interrupt entry layered: `Emulator.ServiceInterrupts` calls `CPU.AcceptInterrupt`, never `OpcodeHandler` directly.

**Files:**
- Modify: `Z80Emu.Core/Processor/CPU.cs:48-51`

- [ ] **Step 6.1: Add `AcceptInterrupt` after `Return()`**

In `CPU.cs`, after the existing `public void Return() => _opcodeHandler.RET();` (around line 51), add:

```csharp
    /// <summary>
    /// Push PC onto the stack and jump to the given vector. Used by
    /// Emulator.ServiceInterrupts for INT and NMI entry.
    /// </summary>
    public void AcceptInterrupt(word vector) => _opcodeHandler.RST(vector);
```

- [ ] **Step 6.2: Verify build**

```bash
dotnet build
```

Expected: succeeds.

- [ ] **Step 6.3: Commit**

```bash
git add Z80Emu.Core/Processor/CPU.cs
git commit -m "Expose CPU.AcceptInterrupt for interrupt entry"
```

---

## Task 7: Implement NMI servicing in `Emulator.Tick()`

Start with NMI because it's the simplest path (always taken if requested, fixed vector, no mode lookup). Maskable INT and IM 2 vectoring follow.

**Files:**
- Create: `Z80Emu.Tests/Processor/InteruptsTests.cs`
- Modify: `Z80Emu.Core/Emulator.cs`

- [ ] **Step 7.1: Create the new test file with the first NMI test**

Create `Z80Emu.Tests/Processor/InteruptsTests.cs`:

```csharp
using Z80Emu.Core;
using Z80Emu.Core.Processor;
using Z80Emu.Tests;  // MockOperatingSytem lives in the parent namespace

namespace Z80Emu.Tests.Processor;

public class InteruptsTests
{
    Emulator _emulator;

    [SetUp]
    public void Setup()
    {
        _emulator = new Emulator(new MockOperatingSytem());
        // Put a NOP at PC so any normal Tick advances by one and stays in range.
        _emulator.Memory[0x0100] = 0x00;
        _emulator.Memory[0x0101] = 0x00;
        _emulator.Memory[0x0102] = 0x00;
    }

    [Test]
    public void Nmi_PushesPC_JumpsTo0x0066_ClearsLatch()
    {
        word originalPC = _emulator.CPU.Registers.PC;
        word originalSP = _emulator.CPU.Registers.SP;

        _emulator.Interupts.RaiseNmi();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NMI");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0066);
        _emulator.CPU.Registers.SP.ShouldBe((word)(originalSP - 2));
        _emulator.Memory[(word)(originalSP - 2)].ShouldBe(originalPC.Lsb());
        _emulator.Memory[(word)(originalSP - 1)].ShouldBe(originalPC.Msb());
        _emulator.Interupts.IsNmiRequested.ShouldBeFalse();
    }

    [Test]
    public void Nmi_CopiesIFF1_ToIFF2_AndClearsIFF1()
    {
        // Artificial pre-state: IFF1=1, IFF2=0 (simulates a nested NMI scenario)
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = false;

        _emulator.Interupts.RaiseNmi();
        _emulator.Tick();

        _emulator.Interupts.IFF2.ShouldBeTrue();   // took IFF1's value
        _emulator.Interupts.IFF1.ShouldBeFalse();  // cleared
    }
}
```

`MockOperatingSytem` is already declared `internal` in `EmulatorTests.cs` (in namespace `Z80Emu.Tests`), so the explicit `using Z80Emu.Tests;` brings it into scope from the child namespace. Note the typo in the type name (`Sytem` not `System`) is pre-existing in the codebase — match it.

The `originalPC.Lsb()` / `Msb()` helpers exist as extension methods on `word` — they're used throughout `OpcodeHandler.cs` (e.g., line 512). Same namespace import will not be needed because they're already pulled in via global usings.

- [ ] **Step 7.2: Run the tests and verify they fail**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests"
```

Expected: 2 tests fail. `Tick()` doesn't service interrupts; PC stays at `0x0101` (after fetching the NOP at `0x0100`).

- [ ] **Step 7.3: Add `ServiceInterrupts` to `Emulator.cs`**

Modify `Z80Emu.Core/Emulator.cs`. Add `using Z80Emu.Core.Utilities;` at the top if `BitUtils` isn't already in scope (it is via the existing imports in this project — check by trying to build first; if compiler complains add the using).

Replace the existing `Tick()` method:

```csharp
    public Opcode Tick()
    {
        Opcode opcode = CPU.Tick();

        // If we are using an OS, check if we need to make a system call
        if (OperatingSystem != null && OperatingSystem.CallVectors.Contains(CPU.Registers.PC))
            OperatingSystem.Execute(this);

        return opcode;
    }
```

With:

```csharp
    public Opcode Tick()
    {
        Opcode? interruptOpcode = ServiceInterrupts();
        if (interruptOpcode != null)
            return interruptOpcode;

        Opcode opcode = CPU.Tick();

        // If we are using an OS, check if we need to make a system call
        if (OperatingSystem != null && OperatingSystem.CallVectors.Contains(CPU.Registers.PC))
            OperatingSystem.Execute(this);

        return opcode;
    }

    private Opcode? ServiceInterrupts()
    {
        if (Interupts.EiPending)
        {
            // EI shadow expires — skip sampling for this tick.
            Interupts.EiPending = false;
            return null;
        }

        if (Interupts.IsNmiRequested)
        {
            CPU.AcceptInterrupt(0x0066);
            Interupts.IFF2 = Interupts.IFF1;
            Interupts.IFF1 = false;
            Interupts.ConsumeNmi();
            return new Opcode("NMI", new string[0], "0", "Non-maskable interrupt accepted -> 0x0066");
        }

        return null;
    }
```

The maskable-INT branch will be added in Task 8.

- [ ] **Step 7.4: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests"
```

Expected: 2 tests pass.

- [ ] **Step 7.5: Run the full test suite for regressions**

```bash
dotnet test
```

Expected: all pass.

- [ ] **Step 7.6: Commit**

```bash
git add Z80Emu.Tests/Processor/InteruptsTests.cs Z80Emu.Core/Emulator.cs
git commit -m "Implement NMI servicing in Emulator.Tick()

Adds ServiceInterrupts() called at start of Tick. NMI path: push PC,
jump to 0x0066, copy IFF1 to IFF2 (save slot for RETN), clear IFF1.
Returns a synthetic Opcode so the Monitor displays the entry as a
distinct line."
```

---

## Task 8: Implement maskable INT servicing for IM 0 / IM 1

IM 0 hardcoded as RST 38h (per spec D2). IM 1 jumps to 0x0038. Both push PC, clear IFF1 and IFF2, clear the latch.

**Files:**
- Modify: `Z80Emu.Tests/Processor/InteruptsTests.cs`
- Modify: `Z80Emu.Core/Emulator.cs:ServiceInterrupts`

- [ ] **Step 8.1: Add IM 0 and IM 1 entry tests**

Append to `InteruptsTests.cs` (before the closing `}`):

```csharp
    [Test]
    public void Im1_Entry_PushesPC_JumpsTo0x0038_ClearsIffAndLatch()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;
        word originalPC = _emulator.CPU.Registers.PC;
        word originalSP = _emulator.CPU.Registers.SP;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
        _emulator.CPU.Registers.SP.ShouldBe((word)(originalSP - 2));
        _emulator.Memory[(word)(originalSP - 2)].ShouldBe(originalPC.Lsb());
        _emulator.Memory[(word)(originalSP - 1)].ShouldBe(originalPC.Msb());
        _emulator.Interupts.IFF1.ShouldBeFalse();
        _emulator.Interupts.IFF2.ShouldBeFalse();
        _emulator.Interupts.IsRequested.ShouldBeFalse();
    }

    [Test]
    public void Im0_Entry_BehavesLikeRst38h()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode0;
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
        _emulator.Interupts.IFF1.ShouldBeFalse();
    }
```

- [ ] **Step 8.2: Run the tests and verify they fail**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests.Im1_Entry|FullyQualifiedName~InteruptsTests.Im0_Entry"
```

Expected: 2 tests fail (`ServiceInterrupts` has no maskable-INT branch yet).

- [ ] **Step 8.3: Add the maskable-INT branch to `ServiceInterrupts`**

In `Emulator.cs`, in the `ServiceInterrupts` method, after the `IsNmiRequested` block and before `return null;`, add:

```csharp
        if (Interupts.IsRequested && Interupts.IFF1)
        {
            // IM 0 is hardcoded as RST 38h; IM 1 also jumps to 0x0038.
            // IM 2 lookup is added in a later task.
            word vector = 0x0038;
            string desc = Interupts.Mode switch
            {
                InterruptMode.Mode0 => "Maskable interrupt accepted (IM 0) -> 0x0038 (RST 38h)",
                InterruptMode.Mode1 => "Maskable interrupt accepted (IM 1) -> 0x0038",
                _                   => "Maskable interrupt accepted -> 0x0038",
            };
            CPU.AcceptInterrupt(vector);
            Interupts.IFF1 = false;
            Interupts.IFF2 = false;
            Interupts.ConsumeInterrupt();
            return new Opcode("INT", new string[0], "0", desc);
        }
```

- [ ] **Step 8.4: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests.Im1_Entry|FullyQualifiedName~InteruptsTests.Im0_Entry"
```

Expected: 2 tests pass.

- [ ] **Step 8.5: Commit**

```bash
git add Z80Emu.Tests/Processor/InteruptsTests.cs Z80Emu.Core/Emulator.cs
git commit -m "Implement maskable INT servicing for IM 0 / IM 1"
```

---

## Task 9: Implement IM 2 vectored servicing

IM 2 forms a vector pointer from `(I << 8) | RequestData` (or `0xFF` if no data byte was supplied), reads the 16-bit vector address (little-endian) from memory at that pointer, and jumps there.

**Files:**
- Modify: `Z80Emu.Tests/Processor/InteruptsTests.cs`
- Modify: `Z80Emu.Core/Emulator.cs:ServiceInterrupts`

- [ ] **Step 9.1: Add IM 2 entry tests**

Append to `InteruptsTests.cs`:

```csharp
    [Test]
    public void Im2_Entry_ReadsVectorFromTable()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode2;
        _emulator.Interupts.IFF1 = true;
        _emulator.CPU.Registers.I = 0x80;
        // Vector table at 0x80NN — entry 0x04 contains 0x1234 (little-endian).
        _emulator.Memory[0x8004] = 0x34;
        _emulator.Memory[0x8005] = 0x12;

        _emulator.Interupts.RaiseInterrupt(0x04);
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x1234);
    }

    [Test]
    public void Im2_Entry_DefaultByteIs0xFF()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode2;
        _emulator.Interupts.IFF1 = true;
        _emulator.CPU.Registers.I = 0x80;
        // No data byte supplied — should read from 0x80FF / 0x8100.
        _emulator.Memory[0x80FF] = 0xCD;
        _emulator.Memory[0x8100] = 0xAB;

        _emulator.Interupts.RaiseInterrupt();
        _emulator.Tick();

        _emulator.CPU.Registers.PC.ShouldBe((word)0xABCD);
    }
```

- [ ] **Step 9.2: Run the tests and verify they fail**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests.Im2_Entry"
```

Expected: 2 tests fail. The current `ServiceInterrupts` always uses `vector = 0x0038`, so IM 2 jumps to the wrong place.

- [ ] **Step 9.3: Add IM 2 vector resolution to `ServiceInterrupts`**

In `Emulator.cs`, replace the maskable-INT branch (the block beginning `if (Interupts.IsRequested && Interupts.IFF1)`) with:

```csharp
        if (Interupts.IsRequested && Interupts.IFF1)
        {
            word vector;
            string desc;
            switch (Interupts.Mode)
            {
                case InterruptMode.Mode2:
                    {
                        word vectorPtr = (word)((CPU.Registers.I << 8) | (Interupts.RequestData ?? 0xFF));
                        byte lo = Memory[vectorPtr];
                        byte hi = Memory[(word)(vectorPtr + 1)];
                        vector = BitUtils.ToWord(hi, lo);
                        desc = $"Maskable interrupt accepted (IM 2) -> 0x{vector:X4}";
                        break;
                    }
                case InterruptMode.Mode1:
                    vector = 0x0038;
                    desc = "Maskable interrupt accepted (IM 1) -> 0x0038";
                    break;
                default: // Mode0
                    vector = 0x0038;
                    desc = "Maskable interrupt accepted (IM 0) -> 0x0038 (RST 38h)";
                    break;
            }
            CPU.AcceptInterrupt(vector);
            Interupts.IFF1 = false;
            Interupts.IFF2 = false;
            Interupts.ConsumeInterrupt();
            return new Opcode("INT", new string[0], "0", desc);
        }
```

If the compiler complains about `BitUtils`, add `using Z80Emu.Core.Utilities;` at the top of `Emulator.cs`.

- [ ] **Step 9.4: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests"
```

Expected: all 6 InteruptsTests pass (the 2 new IM 2 tests plus the 4 from earlier tasks).

- [ ] **Step 9.5: Commit**

```bash
git add Z80Emu.Tests/Processor/InteruptsTests.cs Z80Emu.Core/Emulator.cs
git commit -m "Implement IM 2 vectored interrupt servicing

IM 2 forms a 16-bit pointer from (I << 8) | requestData (default 0xFF),
reads the vector address at that pointer (little-endian), and jumps."
```

---

## Task 10: Verify IFF1 gating and one-shot latch behavior

These behaviors should already work given the code in Tasks 7-9. We add tests to lock them in.

**Files:**
- Modify: `Z80Emu.Tests/Processor/InteruptsTests.cs`

- [ ] **Step 10.1: Add latch-persistence and one-shot tests**

Append to `InteruptsTests.cs`:

```csharp
    [Test]
    public void MaskableInt_NotServiced_When_IFF1_False()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = false;

        _emulator.Interupts.RaiseInterrupt();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NOP");                          // executed memory, not INT
        _emulator.Interupts.IsRequested.ShouldBeTrue();       // latch still set
        _emulator.CPU.Registers.PC.ShouldNotBe((word)0x0038);
    }

    [Test]
    public void MaskableInt_OneShot_DoesNotReFire()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        _emulator.Interupts.IFF1 = true;

        _emulator.Interupts.RaiseInterrupt();
        _emulator.Tick();   // services the interrupt
        // Put a NOP at the vector so the next tick has something to fetch.
        _emulator.Memory[0x0038] = 0x00;
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NOP");                          // not another INT
    }

    [Test]
    public void Nmi_NotGatedBy_IFF1()
    {
        _emulator.Interupts.IFF1 = false;

        _emulator.Interupts.RaiseNmi();
        var op = _emulator.Tick();

        op.Mnemonic.ShouldBe("NMI");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0066);
    }
```

- [ ] **Step 10.2: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests"
```

Expected: all pass — the gating and one-shot behavior fall out naturally from the existing implementation.

- [ ] **Step 10.3: Commit**

```bash
git add Z80Emu.Tests/Processor/InteruptsTests.cs
git commit -m "Lock down IFF1 gating and one-shot latch behavior with tests"
```

---

## Task 11: Lock down EI shadow and RETN-after-NMI with tests

`EI` followed by `RaiseInterrupt` must not fire on the very next tick — it fires on the tick after that. Also adds a test for `RETN` restoring `IFF1` from the saved `IFF2` after an NMI (the existing `RETN` body already does `IFF1 = IFF2` — this test exercises the full save/restore cycle end-to-end).

**Files:**
- Modify: `Z80Emu.Tests/Processor/InteruptsTests.cs`

- [ ] **Step 11.1: Add EI shadow tests**

Append to `InteruptsTests.cs`:

```csharp
    [Test]
    public void EiShadow_DefersInterruptByOneInstruction()
    {
        _emulator.Interupts.Mode = InterruptMode.Mode1;
        // Program: EI ; NOP ; NOP
        _emulator.Memory[0x0100] = 0xFB;  // EI
        _emulator.Memory[0x0101] = 0x00;  // NOP
        _emulator.Memory[0x0102] = 0x00;  // NOP

        var op1 = _emulator.Tick();       // executes EI; sets IFF1=IFF2=true, EiPending=true
        op1.Mnemonic.ShouldBe("EI");

        _emulator.Interupts.RaiseInterrupt();

        var op2 = _emulator.Tick();       // EI shadow active — should run NOP, not INT
        op2.Mnemonic.ShouldBe("NOP");
        _emulator.Interupts.IsRequested.ShouldBeTrue();   // still latched

        var op3 = _emulator.Tick();       // shadow expired — should service now
        op3.Mnemonic.ShouldBe("INT");
        _emulator.CPU.Registers.PC.ShouldBe((word)0x0038);
    }

    [Test]
    public void EiShadow_AlsoDefersNmi()
    {
        _emulator.Memory[0x0100] = 0xFB;  // EI
        _emulator.Memory[0x0101] = 0x00;  // NOP

        _emulator.Tick();                 // EI

        _emulator.Interupts.RaiseNmi();

        var op2 = _emulator.Tick();       // EI shadow defers NMI too
        op2.Mnemonic.ShouldBe("NOP");
        _emulator.Interupts.IsNmiRequested.ShouldBeTrue();

        _emulator.Memory[0x0102] = 0x00;
        var op3 = _emulator.Tick();
        op3.Mnemonic.ShouldBe("NMI");
    }

    [Test]
    public void Retn_RestoresIFF1_FromIFF2_AfterNmi()
    {
        // Pre-state: IFF1=IFF2=true (as if EI had run earlier)
        _emulator.Interupts.IFF1 = true;
        _emulator.Interupts.IFF2 = true;
        // Place RETN at the NMI vector so the second tick will execute it.
        _emulator.Memory[0x0066] = 0xED;
        _emulator.Memory[0x0067] = 0x45;

        _emulator.Interupts.RaiseNmi();
        _emulator.Tick();   // NMI entry: IFF2 takes IFF1's value (still true), IFF1 cleared
        _emulator.Interupts.IFF1.ShouldBeFalse();
        _emulator.Interupts.IFF2.ShouldBeTrue();

        var op2 = _emulator.Tick();   // executes RETN at 0x0066
        op2.Mnemonic.ShouldBe("RETN");
        _emulator.Interupts.IFF1.ShouldBeTrue();   // restored from IFF2
    }
```

- [ ] **Step 11.2: Run the tests and verify they pass**

```bash
dotnet test --filter "FullyQualifiedName~InteruptsTests.EiShadow|FullyQualifiedName~InteruptsTests.Retn_"
```

Expected: all 3 pass — the EI shadow logic was implemented in Task 7 (`if (Interupts.EiPending) { Interupts.EiPending = false; return null; }`) and `RETN`'s `IFF1 = IFF2` body was already correct in the existing codebase (`OpcodeHandler.Methods.cs:846-850`).

- [ ] **Step 11.3: Run the full test suite**

```bash
dotnet test
```

Expected: all pass.

- [ ] **Step 11.4: Commit**

```bash
git add Z80Emu.Tests/Processor/InteruptsTests.cs
git commit -m "Lock down EI shadow and RETN-after-NMI restore with tests"
```

---

## Task 12: Add `int` and `nmi` REPL commands to the Monitor

**Files:**
- Modify: `Z80Emu/Monitor.cs`

- [ ] **Step 12.1: Add the command cases to `Run()`**

In `Z80Emu/Monitor.cs`, locate the switch in `Run()` (currently around line 49). Add two new cases before `case "h":`:

```csharp
                case "int":
                    HandleInterruptCommand(parts);
                    break;
                case "nmi":
                    _emulator.Interupts.RaiseNmi();
                    AnsiConsole.MarkupLine("[yellow][[NMI latched]][/]");
                    break;
```

- [ ] **Step 12.2: Add the `HandleInterruptCommand` helper**

Add a new private method anywhere in the `Monitor` class (e.g., after `Step()` / `Run()`):

```csharp
    void HandleInterruptCommand(string[] parts)
    {
        byte? data = null;
        if (parts.Length >= 2)
        {
            if (!parts[1].TryParseHexByte(out byte parsed))
            {
                AnsiConsole.MarkupLine("[red]Invalid byte (use hex, e.g. 'int 04')[/]");
                return;
            }
            data = parsed;
        }
        _emulator.Interupts.RaiseInterrupt(data);
        string modeLabel = _emulator.Interupts.Mode switch
        {
            InterruptMode.Mode0 => "IM0",
            InterruptMode.Mode1 => "IM1",
            InterruptMode.Mode2 => "IM2",
            _ => "?",
        };
        string suffix = data.HasValue ? $", byte=0x{data.Value:X2}" : "";
        AnsiConsole.MarkupLine($"[yellow][[INT latched: mode={modeLabel}{suffix}]][/]");
    }
```

You'll need to add `using Z80Emu.Core.Processor;` at the top of `Monitor.cs` if it's not already there (check the existing imports).

- [ ] **Step 12.3: Update `ViewHelp()` with the two new lines**

In `Monitor.cs`, locate `ViewHelp()` (currently around line 199). Add two new lines before the `quit` line:

```csharp
        AnsiConsole.MarkupLine("[blue]int[/] [yellow][[byte]][/]");
        AnsiConsole.MarkupLine("[blue]nmi[/]");
```

- [ ] **Step 12.4: Verify build and run a manual smoke test**

```bash
dotnet build
dotnet test --filter "FullyQualifiedName~ResetClearsWarmBoot"
dotnet run --project Z80Emu -- Z80Emu.Tests/bin/Debug/net10.0/Test.com
```

The test invocation is to ensure `Test.com` has been copied into the test bin directory; the path then resolves regardless of where dotnet run sets the working directory. (If you have any `.com` Z80 binary on disk, you can use that instead.)

In the REPL: type `int` then `s`, then `int 04` then `s`, then `nmi` then `s`. Verify each shows the latch confirmation and that the next `s` shows the synthetic `INT`/`NMI` line with PC jumping to `0x0038`/`0x0066`. Type `q` to quit.

(Note: Test.com starts with `Mode=Mode0` and `IFF1=false`, so `int` won't service until you run `EI`. For a quick exercise, type `q` when done — the unit tests cover all the servicing logic.)

- [ ] **Step 12.5: Commit**

```bash
git add Z80Emu/Monitor.cs
git commit -m "Add int and nmi REPL commands to the Monitor

int        — latch a maskable interrupt
int <hex>  — same with a data byte (used as IM 2 vector low byte)
nmi        — latch a non-maskable interrupt

The synthetic INT/NMI opcode returned from Emulator.Tick() flows
through the existing ViewOpcode path so each interrupt entry shows
up as its own line in step output."
```

---

## Task 13: Update README.md

Add the new commands to the table and fix the stale opening line about auto-generated opcodes (the `ParseOpcodes` generator was removed earlier).

**Files:**
- Modify: `README.md`

- [ ] **Step 13.1: Replace the opening paragraph**

Replace the first paragraph (lines 3-5):

```markdown
A Z80 emulator/monitor program written in C#. Opcodes are generated automatically
using the [JSON Opcode Table](https://github.com/deeptoaster/opcode-table/blob/master/opcode-table.json)
from [Z80 Opcode Table](https://clrhome.org/table/).
```

With:

```markdown
A Z80 emulator/monitor program written in C#. The opcode table was originally
seeded from the [JSON Opcode Table](https://github.com/deeptoaster/opcode-table/blob/master/opcode-table.json)
([Z80 Opcode Table](https://clrhome.org/table/)) and is now hand-maintained in
`Z80Emu.Core/Processor/Opcodes/`.
```

- [ ] **Step 13.2: Add command rows to the commands table**

In the commands table (currently lines 27-38), insert these rows before the `b` row:

```markdown
| `int` | `int` | Latch a maskable interrupt request (uses current `IM` mode and `I` register) |
| `int <byte>` | `int 04` | Latch a maskable interrupt with a bus byte. Used as the IM 2 vector low byte; ignored in IM 0 / IM 1 |
| `nmi` | `nmi` | Latch a non-maskable interrupt request |
```

- [ ] **Step 13.3: Commit**

```bash
git add README.md
git commit -m "Document int/nmi commands and refresh opcode-table preamble"
```

---

## Task 14: Update CLAUDE.md with the interrupt model

**Files:**
- Modify: `CLAUDE.md`

- [ ] **Step 14.1: Add a new "Interrupt model" subsection**

In `CLAUDE.md`, locate the `### Memory, ports, OS` heading (currently around line 66 — comes after `### Opcode dispatch` under `## Architecture`). Insert a new subsection **before** it:

```markdown
### Interrupt model

Interrupts are sampled at instruction boundaries the same way real Z80 hardware does, but the servicing is implemented as a synthetic pseudo-opcode at the **start** of `Emulator.Tick()` rather than the end. When `Interupts.IsRequested` (or `IsNmiRequested`) is set and not gated, `ServiceInterrupts()` performs only the entry sequence (push PC, IFF flips, jump to vector) and returns a synthetic `Opcode { Mnemonic = "INT"|"NMI", Length = 0 }`. The next `Tick()` fetches normally at the vector. This shape is deliberate: it lets the Monitor display interrupt entry as its own line in `step` output without any new rendering code (`Step()` already prints whatever `Tick()` returned). See `docs/superpowers/specs/2026-04-27-im-interrupt-instructions-design.md` for the full design rationale.

Specifics worth knowing when modifying this code:
- **IM 0 is hardcoded as `RST 38h`** (push PC, jump to `0x0038`). Real hardware reads an opcode off the data bus; we don't emulate the bus, and `0xFF`/`RST 38h` is what most production Z80 hardware actually drove. The `RequestData` byte is ignored in IM 0.
- **EI shadow:** `EI` sets `IFF1=IFF2=true` AND `EiPending=true`. `ServiceInterrupts` skips sampling for one tick when `EiPending` is set so that `EI; RET` lets `RET` run before any pending IRQ re-enters the ISR. NMI is also delayed by the EI shadow.
- **NMI semantics:** copies `IFF1 → IFF2` (the save slot for `RETN`) then clears `IFF1`. NMI is not gated by `IFF1`.
- **Latch lifecycle:** `RaiseInterrupt()`/`RaiseNmi()` set the latches. `Emulator.ServiceInterrupts` calls `Interupts.ConsumeInterrupt()`/`ConsumeNmi()` to clear them once the entry sequence completes. While `IFF1=0`, a maskable INT latch persists until `EI` re-enables sampling.
- **Don't add direct IFF mutation outside opcode bodies and `ServiceInterrupts`.** The existing pattern is: `EI`/`DI`/`RETI`/`RETN` opcode bodies write `_int.IFF1`/`IFF2` directly. Keep it that way — `Interupts` is a state container, not a method-rich abstraction.
```

- [ ] **Step 14.2: Commit**

```bash
git add CLAUDE.md
git commit -m "Document the interrupt model in CLAUDE.md"
```

---

## Task 15: Bump version

**Files:**
- Modify: `Z80Emu/Z80Emu.csproj:8`

- [ ] **Step 15.1: Bump the version**

In `Z80Emu/Z80Emu.csproj`, change line 8:

```xml
    <Version>0.6.0</Version>
```

To:

```xml
    <Version>0.7.0</Version>
```

This is a minor bump — new feature (interrupt support), no breaking changes.

- [ ] **Step 15.2: Final build and full test run**

```bash
dotnet build
dotnet test
```

Expected: build succeeds, all tests pass.

- [ ] **Step 15.3: Commit**

```bash
git add Z80Emu/Z80Emu.csproj
git commit -m "Bump version to 0.7.0 for interrupt support"
```

- [ ] **Step 15.4: Push to origin (PowerShell on Windows)**

Use the **PowerShell tool**, not Bash, per the project rule in `CLAUDE.md`:

```powershell
git push
```

Expected: all commits pushed to `origin/main`.

---

## Verification checklist

Before declaring done:

- [ ] `dotnet build` succeeds with no warnings introduced.
- [ ] `dotnet test` reports zero failures and the new IM/EI/DI/RST/Interrupt tests are visible in the output.
- [ ] `dotnet run --project Z80Emu -- Z80Emu.Tests/bin/Debug/net10.0/Test.com` enters the REPL; `int` then `s` shows an `INT` line and PC=`0x0038`; `q` exits cleanly.
- [ ] `git log --oneline` shows ~15 small commits for this feature, each describing one concern.
- [ ] GitHub issue #21 can be closed referencing the merged commits.
