# S1APITemplate

A beginner-friendly Schedule I mod template for [S1API.Forked](https://www.nuget.org/packages/S1API.Forked), MelonLoader, and Harmony. The project currently pins S1API.Forked 3.1.4.

The default path is `CrossCompat`: one mod assembly that stays on S1API/MelonLoader abstractions and avoids direct `ScheduleOne` or `Il2CppScheduleOne` game types. Use `Mono` or `Il2cpp` only when your mod intentionally needs runtime-specific game assemblies.

## Quick Start

1. Clone this repository, or install it as a local .NET template and create a project:

```powershell
dotnet new install .
dotnet new s1api -n MyScheduleOneMod
Set-Location .\MyScheduleOneMod
```

2. Run the setup script once from the generated project:

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
.\setup.ps1 -MonoPath "<path-to-mono-install>" -Il2CppPath "<path-to-il2cpp-install>" -Force
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

## Developing Against a Local S1API Checkout

The default package reference is recommended for released mods. To compile against a sibling S1API checkout, build the matching S1API target and enable the local reference in `local.build.props`:

```powershell
dotnet build ..\S1API\S1API.sln -c MonoMelon -p:AutomateLocalDeployment=false
dotnet build ..\S1API\S1API.sln -c Il2CppMelon -p:AutomateLocalDeployment=false
```

```xml
<UseLocalS1APIForked>true</UseLocalS1APIForked>
<LocalS1APIRoot>..\S1API</LocalS1APIRoot>
```

`CrossCompat` and `Mono` consume the local `MonoMelon` build; `Il2cpp` consumes the local `Il2CppMelon` build. Override `LocalS1APIForkedPath` only for a non-standard output layout.

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
|-- .agents/
|   `-- skills/
|       |-- schedule-one-modding/
|       `-- schedule-one-custom-npcs/
|-- Core.cs
|-- Utils/
|   `-- Constants.cs
|-- example.build.props
|-- setup.ps1
|-- S1APITemplate.csproj
`-- S1APITemplate.sln
```

## Included Agent Skills

This template includes repo-local Codex/agent skills under `.agents/skills/`:

- `schedule-one-modding`: general Schedule I modding guidance for Mono, IL2CPP, CrossCompat, Harmony, S1API, MAPI, SteamNetworkLib, local game inspection, and packaging safety.
- `schedule-one-custom-npcs`: focused S1API custom NPC guidance for prefab configuration, appearance, schedules, dialogue, customer/dealer behavior, and save/load lifecycle.

They are development guidance only. They are not compiled into the mod DLL and should not include game assemblies, generated IL2CPP wrappers, decompiled dumps, AssetRipper exports, private logs, or local machine paths.

## Where To Put Code

- Use `GameLifecycle.OnPreLoad` for stable content definitions that save deserialization must resolve.
- Use `GameLifecycle.OnLoadComplete` for systems that need the loaded world, managers, or player state.
- Put IDs, version strings, config names, and log tags in `Utils/Constants.cs`.
- Keep CrossCompat code on S1API wrappers and public abstractions. If a file needs direct game types, guard it with `#if MONO` / `#if IL2CPP` or keep it out of `CrossCompat`.

## Useful Links

- [S1API GitHub](https://github.com/ifBars/S1API)
- [S1API.Forked on NuGet](https://www.nuget.org/packages/S1API.Forked)
- [S1API Documentation](https://ifbars.github.io/S1API-docs/)
