$commandPath = Split-Path -Parent $PSCommandPath
Set-Location $commandPath
$path = "C:\PeterLocalPackages"
if (-not (Test-Path $path)) {
    New-Item -Path $path -ItemType Directory
}
$source = "PeterLocal"
if (dotnet nuget list source | Where-Object { $_ -like "*$source*" }) {
    dotnet nuget remove source $source
}
dotnet nuget add source $path --name $source
dotnet pack Peter.sln -c Debug -o $path --version-suffix "-local"