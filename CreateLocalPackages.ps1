$commandPath = Split-Path -Parent $PSCommandPath
Set-Location $commandPath
$path = "C:\PeterLocalPackages"
if (Test-Path $path) {
    Remove-Item -Path $path -Force -Recurse
}
New-Item -Path $path -ItemType Directory
$source = "PeterLocal"
if (dotnet nuget list source | Where-Object { $_ -like "*$source*" }) {
    dotnet nuget remove source $source
}
dotnet nuget add source $path --name $source
dotnet pack Peter.sln -c Debug --property:PackageOutputPath=$path --version-suffix "-local"