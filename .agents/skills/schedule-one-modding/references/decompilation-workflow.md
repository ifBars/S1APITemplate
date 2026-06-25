# Decompilation Workflow

Use for focused inspection of user-owned Schedule One assemblies.

## Tools

Install/update ILSpy CLI:

```powershell
dotnet tool install --global ilspycmd
dotnet tool update --global ilspycmd
```

## Assembly locations

Mono:

```text
<game>\Schedule I_Data\Managed\Assembly-CSharp.dll
```

IL2CPP generated wrappers:

```text
<game>\MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll
```

If wrappers are missing, launch IL2CPP with MelonLoader once and inspect `Latest.log`.

## Inspection pattern

1. Start from the exact symptom, log line, stack frame, method, or class.
2. Decompile one type, not the whole game.
3. Compare Mono and IL2CPP shapes when behavior differs.
4. Look for backing fields, `SyncAccessor_*`, generated RPCs, and wrapper methods.
5. Patch the method that owns the state transition.

## Public output

Do not paste large decompiled methods or publish dumps. Record names, signatures, short snippets, and behavior summaries only. Never commit, package, attach, or redistribute assemblies, IL2CPP wrappers, decompiled folders, or whole-class dumps; ask for local paths and inspect on the user's machine.

## Validation

Mono and IL2CPP are separate evidence streams. Mono source helps with intent, but it does not prove IL2CPP wrapper names, casts, generated methods, or Harmony patchability. For dual-runtime mods, validate both:

- Mono: build against `Schedule I_Data\Managed` and inspect Mono stack traces/logs.
- IL2CPP: build against generated wrappers, avoid transpilers, inspect `Latest.log`, and verify wrapper signatures or generated names locally.
