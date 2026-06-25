# S1APITemplate

A beginner-friendly Schedule I mod template for [S1API.Forked](https://www.nuget.org/packages/S1API.Forked), MelonLoader, and Harmony.

The default path is `CrossCompat`: one mod assembly that stays on S1API/MelonLoader abstractions and avoids direct `ScheduleOne` or `Il2CppScheduleOne` game types. Use `Mono` or `Il2cpp` only when your mod intentionally needs runtime-specific game assemblies.

## Quick Start

1. Install the template or clone/copy this folder.
2. Run the setup script once:

```powershell
.\setup.ps1
```

3. Build the recommended starter configuration:

```powershell
dotnet build .\S1APITemplate.sln -c CrossCompat
```

The script writes `local.build.props` with your local Schedule I install paths. That file is ignored by git.

If detection picks the wrong installs, pass paths explicitly:

```powershell
.\setup.ps1 -MonoPath "D:\SteamLibrary\steamapps\common\Schedule I_alternate" -Il2CppPath "D:\SteamLibrary\steamapps\common\Schedule I_public" -Force
```

To copy successful builds into the target `Mods` folder, generate props with deployment enabled:

```powershell
.\setup.ps1 -EnableDeployment -Force
```

## Build Configurations

`CrossCompat` is the recommended default for S1API mods.

```powershell
dotnet build .\S1APITemplate.sln -c CrossCompat
```

`Mono` targets the Mono/alternate branch and includes direct Mono game assembly references.

```powershell
dotnet build .\S1APITemplate.sln -c Mono
```

`Il2cpp` targets the IL2CPP/default branch and includes generated Il2Cpp assembly references.

```powershell
dotnet build .\S1APITemplate.sln -c Il2cpp
```

Players still need MelonLoader and S1API installed in the game at runtime.

## Included References

The project includes the common references that S1API mods usually need so new modders do not have to add Unity assemblies manually:

- `S1API.Forked`, `LavaGang.MelonLoader`, and `HarmonyX` from NuGet for compile-time access.
- `Newtonsoft.Json`, TextMeshPro, and common `UnityEngine.*` modules from the selected game install when `local.build.props` is configured.
- `Assembly-CSharp` and `Assembly-CSharp-firstpass` for `Mono` and `Il2cpp` only.
- `Il2CppInterop.Runtime` for `Il2cpp`.

If your mod needs a less common Unity, FishNet, Steamworks, or Schedule One assembly, add it near the matching reference group in `S1APITemplate.csproj`.

## Project Layout

```text
S1APITemplate/
|-- Core.cs
|-- Integrations/
|   `-- HarmonyPatches.cs
|-- Utils/
|   `-- Constants.cs
|-- example.build.props
|-- setup.ps1
|-- S1APITemplate.csproj
`-- S1APITemplate.sln
```

## Where To Put Code

- Register S1API items, NPCs, quests, saveables, and shop data from `GameLifecycle.OnPreLoad`.
- Put Harmony patch classes under `Integrations/`.
- Put IDs, version strings, config names, and log tags in `Utils/Constants.cs`.
- Keep CrossCompat code on S1API wrappers and public abstractions. If a file needs direct game types, guard it with `#if MONO` / `#if IL2CPP` or keep it out of `CrossCompat`.

## Useful Links

- [S1API GitHub](https://github.com/ifBars/S1API)
- [S1API.Forked on NuGet](https://www.nuget.org/packages/S1API.Forked)
- [S1API Documentation](https://ifbars.github.io/S1API-docs/)
