# Build Configuration

Use when target frameworks, references, local paths, or validation commands are in scope.

## Runtime map

```text
none/beta = IL2CPP
alternate/alternate-beta = Mono
```

- Mono: `netstandard2.1`, MelonLoader `net35`, assemblies from `Schedule I_Data\Managed`.
- IL2CPP: `net6.0`, MelonLoader `net6`, assemblies from `MelonLoader\Il2CppAssemblies`.

## Local paths

Keep user paths in ignored files such as `local.build.props`, `Directory.Build.user.props`, environment variables, or command-line `-p:` overrides. Do not commit local paths.

## Validation

Use existing project configs:

```powershell
dotnet build -c Mono -p:AutomateLocalDeployment=false
dotnet build -c Il2cpp -p:AutomateLocalDeployment=false
dotnet build -c Mono_Server -p:AutomateLocalDeployment=false
dotnet build -c Il2cpp_Server -p:AutomateLocalDeployment=false
```

If IL2CPP assemblies are missing, launch the IL2CPP game with MelonLoader once and inspect `Latest.log`.
