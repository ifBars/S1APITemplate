# S1APITemplate

A starter project for Schedule I mods that use [S1API.Forked](https://www.nuget.org/packages/S1API.Forked) and MelonLoader.

This template is aligned with S1API `3.0.5`, the latest published NuGet package at the time this template was refreshed.

## What This Includes

- A minimal `MelonMod` entry point.
- A placeholder Harmony patch class.
- A pinned `S1API.Forked` package reference.
- `CrossCompat`, `MonoMelon`, and `Il2CppMelon` build configurations.
- `example.build.props` for local MelonLoader and deployment paths.
- Optional post-build copying into the target game's `Mods` folder.

## Project Layout

```text
S1APITemplate/
|-- Core.cs
|-- Integrations/
|   `-- HarmonyPatches.cs
|-- Utils/
|   `-- Constants.cs
|-- example.build.props
|-- S1APITemplate.csproj
`-- S1APITemplate.sln
```

## Setup

1. Copy `example.build.props` to `local.build.props`.
2. Update the paths in `local.build.props` for your Schedule I install.
3. Keep `AutomateLocalDeployment` as `false` until you want builds to copy the DLL into `Mods`.
4. Restore packages:

```powershell
dotnet restore .\S1APITemplate.sln -p:Configuration=CrossCompat
```

## Build Configurations

`CrossCompat` is the recommended starting point. It builds one `netstandard2.1` mod assembly and expects your mod code to stay on S1API and MelonLoader abstractions instead of direct `ScheduleOne` or `Il2CppScheduleOne` game assembly types.

```powershell
dotnet build .\S1APITemplate.sln -c CrossCompat
```

`MonoMelon` is for mods that intentionally target the Mono MelonLoader runtime.

```powershell
dotnet build .\S1APITemplate.sln -c MonoMelon
```

`Il2CppMelon` is for mods that intentionally target the IL2CPP MelonLoader runtime.

```powershell
dotnet restore .\S1APITemplate.sln -p:Configuration=Il2CppMelon
dotnet build .\S1APITemplate.sln -c Il2CppMelon
```

## Local Paths

The project imports `local.build.props` when it exists. This keeps machine-specific install paths out of git.

Required properties:

- `MelonLoaderMonoAssembliesPath`: usually `...\Schedule I\MelonLoader\net35`.
- `MelonLoaderAssembliesPath`: usually `...\Schedule I\MelonLoader\net6`.
- `LocalMonoDeploymentPath`: the Mono game install root.
- `LocalIl2CppDeploymentPath`: the IL2CPP game install root.

## Development Notes

- S1API is installed through NuGet for compile-time access. Players still need S1API installed in the game at runtime.
- Prefer public S1API namespaces such as `S1API.Lifecycle`, `S1API.Items`, `S1API.Quests`, and `S1API.Entities`.
- Avoid depending on `S1API.Internal` from mod code unless you are intentionally accepting internal API churn.
- If you directly reference game assemblies, keep those references scoped to runtime-specific configurations and expect to maintain Mono/IL2CPP differences yourself.

## Useful Links

- [S1API GitHub](https://github.com/ifBars/S1API)
- [S1API.Forked on NuGet](https://www.nuget.org/packages/S1API.Forked)
- [S1API Documentation](https://ifbars.github.io/S1API-docs/)
