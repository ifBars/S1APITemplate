param(
    [string]$MonoGamePath,
    [string]$Il2CppGamePath,
    [string]$AssetRipperPath,
    [switch]$Json
)

$ErrorActionPreference = "Stop"

function Test-File {
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path)) {
        return $false
    }

    return Test-Path -LiteralPath $Path -PathType Leaf
}

function Test-Directory {
    param([string]$Path)

    if ([string]::IsNullOrWhiteSpace($Path)) {
        return $false
    }

    return Test-Path -LiteralPath $Path -PathType Container
}

function Get-CommandStatus {
    param([string]$Name)

    $command = Get-Command $Name -ErrorAction SilentlyContinue
    if ($null -eq $command) {
        return [ordered]@{
            available = $false
            path = $null
        }
    }

    return [ordered]@{
        available = $true
        path = $command.Source
    }
}

function Get-GameProbe {
    param(
        [string]$GamePath,
        [string]$Backend
    )

    $managedPath = Join-Path $GamePath 'Schedule I_Data\Managed'
    $il2cppPath = Join-Path $GamePath 'MelonLoader\Il2CppAssemblies'
    $melonLogPath = Join-Path $GamePath 'MelonLoader\Latest.log'
    $modsPath = Join-Path $GamePath 'Mods'

    return [ordered]@{
        backend = $Backend
        gamePath = $GamePath
        gamePathExists = Test-Directory $GamePath
        modsPathExists = Test-Directory $modsPath
        latestLogExists = Test-File $melonLogPath
        mono = [ordered]@{
            managedPathExists = Test-Directory $managedPath
            assemblyCSharpExists = Test-File (Join-Path $managedPath 'Assembly-CSharp.dll')
            unityCoreModuleExists = Test-File (Join-Path $managedPath 'UnityEngine.CoreModule.dll')
        }
        il2cpp = [ordered]@{
            generatedPathExists = Test-Directory $il2cppPath
            assemblyCSharpExists = Test-File (Join-Path $il2cppPath 'Assembly-CSharp.dll')
            unityCoreModuleExists = Test-File (Join-Path $il2cppPath 'UnityEngine.CoreModule.dll')
            fishNetExists = Test-File (Join-Path $il2cppPath 'Il2CppFishNet.Runtime.dll')
        }
    }
}

$result = [ordered]@{
    generatedAt = (Get-Date).ToString("o")
    tools = [ordered]@{
        dotnet = Get-CommandStatus 'dotnet'
        ilspycmd = Get-CommandStatus 'ilspycmd'
        assetRipper = [ordered]@{
            providedPath = $AssetRipperPath
            providedPathExists = Test-File $AssetRipperPath
        }
    }
    paths = [ordered]@{
        mono = if ($MonoGamePath) { Get-GameProbe -GamePath $MonoGamePath -Backend 'Mono' } else { $null }
        il2cpp = if ($Il2CppGamePath) { Get-GameProbe -GamePath $Il2CppGamePath -Backend 'IL2CPP' } else { $null }
    }
    reminders = @(
        'This script is read-only.',
        'Do not commit game assemblies, generated IL2CPP assemblies, logs with private data, or AssetRipper exports.',
        'If IL2CPP generated assemblies are missing, launch the IL2CPP game with MelonLoader once and inspect Latest.log.'
    )
}

if ($Json) {
    $result | ConvertTo-Json -Depth 8
    return
}

Write-Host "Schedule One local probe"
Write-Host "dotnet:   $($result.tools.dotnet.available) $($result.tools.dotnet.path)"
Write-Host "ilspycmd: $($result.tools.ilspycmd.available) $($result.tools.ilspycmd.path)"

if ($AssetRipperPath) {
    Write-Host "AssetRipper path exists: $($result.tools.assetRipper.providedPathExists) $AssetRipperPath"
}

foreach ($name in @('mono', 'il2cpp')) {
    $probe = $result.paths[$name]
    if ($null -eq $probe) {
        continue
    }

    Write-Host ""
    Write-Host "$($probe.backend): $($probe.gamePath)"
    Write-Host "  game path:      $($probe.gamePathExists)"
    Write-Host "  Mods:           $($probe.modsPathExists)"
    Write-Host "  Latest.log:     $($probe.latestLogExists)"
    Write-Host "  Managed asm:    $($probe.mono.assemblyCSharpExists)"
    Write-Host "  IL2CPP asm:     $($probe.il2cpp.assemblyCSharpExists)"
    Write-Host "  IL2CPP FishNet: $($probe.il2cpp.fishNetExists)"
}
