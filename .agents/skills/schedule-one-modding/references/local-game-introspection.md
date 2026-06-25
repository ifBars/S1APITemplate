# Local Game Introspection

Use for setup checks and local evidence: paths, logs, IL2CPP wrappers, saves, runtime state, or tool availability.

## Requirements

Agent-only/fresh machine checklist:

- Check `dotnet --info`, `dotnet --list-sdks`, `dotnet tool list -g`.
- Minimum CLI setup: .NET SDK + `ilspycmd`.
- IDEs are optional; agent-driven repo work can start with CLI tools.
- AssetRipper/Unity are optional for prefab/resource/scene inspection.
- Ask before installing. Offer agent-run checks/installs or official manual links.
- Install tools into user/system tool locations, not the Schedule One game folder.

Links: .NET SDK `https://dotnet.microsoft.com/download/dotnet`, ILSpy `https://github.com/icsharpcode/ILSpy`, AssetRipper `https://github.com/AssetRipper/AssetRipper/releases`, Visual Studio `https://visualstudio.microsoft.com/downloads/`.

## Probe

Run the read-only probe when PowerShell is available:

```powershell
.\scripts\Invoke-S1LocalProbe.ps1 -MonoGamePath "<mono-game>" -Il2CppGamePath "<il2cpp-game>" -Json
```

It is read-only and checks `dotnet`, `ilspycmd`, optional AssetRipper path, `Mods`, `Latest.log`, Mono assemblies, and IL2CPP wrappers.

## Ask for the smallest useful input

- Steam branch/backend.
- Mono `Schedule I_Data\Managed`.
- IL2CPP `MelonLoader\Il2CppAssemblies`.
- `Latest.log` for loader/patch/startup failures.
- `Player.log` for Unity runtime failures.
- Save path only when persistence is relevant.
- AssetRipper export path only for prefab/resource inspection.

## Workflow

1. Start from the exact symptom.
2. Pick one evidence source: `ilspycmd`, logs, AssetRipper/runtime inspection, or saves.
3. Inspect the smallest target.
4. Summarize findings without proprietary code/assets.
5. Implement at the owning lifecycle hook, patch point, service, adapter, or save/network path.

## Safety

Store user paths only in ignored local config, env vars, or CLI overrides. Do not commit paths, private logs, assemblies, wrappers, decompiled dumps, or asset exports. Keep tools/dumps/wrappers/exports outside the game folder in ignored workspace/tool dirs.

If `cpp2il_out\Assembly-CSharp.dll` is locked, check for duplicate game launches or stale locks before blaming mod code.
