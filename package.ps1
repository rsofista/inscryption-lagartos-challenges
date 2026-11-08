<#
.SYNOPSIS
    Builds the mod and creates the Thunderstore upload zip.
.DESCRIPTION
    Runs the tests, builds LagartosChallenges.dll in Release, then zips manifest.json, icon.png, README.md,
    CHANGELOG.md and plugins/LagartosChallenges.dll into dist/<name>-<version>.zip.
    Upload that zip at https://thunderstore.io/package/create/
#>
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$ts = Join-Path $root 'thunderstore'

dotnet test (Join-Path $root 'LagartosChallenges.sln')
if ($LASTEXITCODE -ne 0) { throw 'Tests failed.' }
dotnet build (Join-Path $root 'LagartosChallenges\LagartosChallenges.csproj') -c Release
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }

$manifest = Get-Content (Join-Path $ts 'manifest.json') -Raw | ConvertFrom-Json
$stage = Join-Path $root 'dist\stage'
if (Test-Path $stage) { Remove-Item $stage -Recurse -Force }
New-Item -ItemType Directory -Force (Join-Path $stage 'plugins') | Out-Null

Copy-Item (Join-Path $ts 'manifest.json'), (Join-Path $ts 'icon.png'), (Join-Path $ts 'README.md'), (Join-Path $ts 'CHANGELOG.md') $stage
Copy-Item (Join-Path $root 'LagartosChallenges\bin\Release\netstandard2.0\LagartosChallenges.dll') (Join-Path $stage 'plugins')

$zip = Join-Path $root ("dist\{0}-{1}.zip" -f $manifest.name, $manifest.version_number)
if (Test-Path $zip) { Remove-Item $zip -Force }
# Build the zip entry by entry: Windows PowerShell's Compress-Archive writes backslash paths,
# which Thunderstore/r2modman do not treat as folders.
Add-Type -AssemblyName System.IO.Compression, System.IO.Compression.FileSystem
$archive = [System.IO.Compression.ZipFile]::Open($zip, 'Create')
try {
    Get-ChildItem $stage -Recurse -File | ForEach-Object {
        $entry = $_.FullName.Substring($stage.Length + 1).Replace('\', '/')
        [System.IO.Compression.ZipFileExtensions]::CreateEntryFromFile($archive, $_.FullName, $entry) | Out-Null
    }
} finally {
    $archive.Dispose()
}
Remove-Item $stage -Recurse -Force
Write-Host "Created $zip"
