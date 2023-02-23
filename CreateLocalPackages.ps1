New-Item -Path "C:\PeterLocalPackages" -ItemType Directory -ErrorAction SilentlyContinue
dotnet nuget add source C:\PeterLocalPackages --name PeterLocal  | Out-Null
dotnet pack Peter.sln -c Debug -o C:\PeterLocalPackages --version-suffix "-local"