param(
    [string]$MonoPath,
    [string]$Il2CppPath,
    [switch]$EnableDeployment,
    [switch]$Force
)

$ErrorActionPreference = "Stop"

function Get-SteamLibraryFolders {
    $folders = New-Object System.Collections.Generic.List[string]

    $defaultSteam = Join-Path ${env:ProgramFiles(x86)} "Steam"
    if (Test-Path -LiteralPath $defaultSteam) {
        $folders.Add($defaultSteam)
    }

    $libraryFiles = @(
        Join-Path $defaultSteam "steamapps\libraryfolders.vdf"
    )

    foreach ($libraryFile in $libraryFiles) {
        if (-not (Test-Path -LiteralPath $libraryFile)) {
            continue
        }

        foreach ($line in Get-Content -LiteralPath $libraryFile) {
            if ($line -match '"path"\s+"(?<path>[^"]+)"') {
                $folders.Add($matches.path.Replace("\\", "\"))
            }
        }
    }

    $folders |
        Where-Object { $_ -and (Test-Path -LiteralPath $_) } |
        Select-Object -Unique
}

function Get-ScheduleOneInstalls {
    Get-SteamLibraryFolders |
        ForEach-Object { Join-Path $_ "steamapps\common" } |
        Where-Object { Test-Path -LiteralPath $_ } |
        ForEach-Object {
            Get-ChildItem -LiteralPath $_ -Directory -Filter "Schedule I*" |
                Where-Object { Test-Path -LiteralPath (Join-Path $_.FullName "Schedule I.exe") }
        } |
        Select-Object -ExpandProperty FullName -Unique
}

function Select-InstallPath {
    param(
        [string[]]$Candidates,
        [string[]]$PreferredNamePatterns
    )

    foreach ($pattern in $PreferredNamePatterns) {
        $match = $Candidates | Where-Object { (Split-Path $_ -Leaf) -like $pattern } | Select-Object -First 1
        if ($match) {
            return $match
        }
    }

    return $Candidates | Select-Object -First 1
}

function Convert-ToXmlText {
    param([string]$Value)
    return [System.Security.SecurityElement]::Escape($Value)
}

$propsPath = Join-Path $PSScriptRoot "local.build.props"
if ((Test-Path -LiteralPath $propsPath) -and -not $Force) {
    Write-Host "local.build.props already exists. Re-run with -Force to overwrite it."
    exit 0
}

$installs = @(Get-ScheduleOneInstalls)

if (-not $MonoPath) {
    $MonoPath = Select-InstallPath -Candidates $installs -PreferredNamePatterns @("*alternate*", "Schedule I")
}

if (-not $Il2CppPath) {
    $Il2CppPath = Select-InstallPath -Candidates $installs -PreferredNamePatterns @("*public*", "Schedule I")
}

if (-not $MonoPath -and -not $Il2CppPath) {
    throw "Could not find a Schedule I install. Pass -MonoPath and/or -Il2CppPath explicitly."
}

$deployment = if ($EnableDeployment) { "true" } else { "false" }
$monoValue = if ($MonoPath) { $MonoPath } else { "" }
$il2cppValue = if ($Il2CppPath) { $Il2CppPath } else { "" }
$monoXml = Convert-ToXmlText $monoValue
$il2cppXml = Convert-ToXmlText $il2cppValue

$content = @"
<Project>
  <PropertyGroup>
    <AutomateLocalDeployment>$deployment</AutomateLocalDeployment>

    <LocalMonoDeploymentPath>$monoXml</LocalMonoDeploymentPath>
    <LocalIl2CppDeploymentPath>$il2cppXml</LocalIl2CppDeploymentPath>

    <MonoAssembliesPath>`$(LocalMonoDeploymentPath)\Schedule I_Data\Managed</MonoAssembliesPath>
    <Il2CppAssembliesPath>`$(LocalIl2CppDeploymentPath)\MelonLoader\Il2CppAssemblies</Il2CppAssembliesPath>
    <MelonLoaderMonoAssembliesPath>`$(LocalMonoDeploymentPath)\MelonLoader\net35</MelonLoaderMonoAssembliesPath>
    <MelonLoaderAssembliesPath>`$(LocalIl2CppDeploymentPath)\MelonLoader\net6</MelonLoaderAssembliesPath>

    <UseLocalS1APIForked>false</UseLocalS1APIForked>
    <LocalS1APIForkedPath>..\S1API\S1API\bin\CrossCompat\netstandard2.1\S1API.dll</LocalS1APIForkedPath>
  </PropertyGroup>
</Project>
"@

Set-Content -LiteralPath $propsPath -Value $content -Encoding UTF8

Write-Host "Wrote $propsPath"
if ($MonoPath) {
    Write-Host "Mono/CrossCompat install: $MonoPath"
}
if ($Il2CppPath) {
    Write-Host "Il2Cpp install: $Il2CppPath"
}
