@echo off

dotnet restore                                                         Source/ManagedIrbis5-windows.sln
dotnet build --no-restore            --configuration Release           Source/ManagedIrbis5-windows.sln
dotnet test  --no-restore --no-build --configuration Release           Source/ManagedIrbis5-windows.sln
dotnet run   --no-restore --no-build --configuration Release --project Source/Benchmarks/CoreBenchmarks/CoreBenchmarks.csproj
dotnet pack  --no-restore --no-build --configuration Release           Source/ManagedIrbis5-windows.sln
