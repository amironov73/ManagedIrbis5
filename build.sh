#!/bin/sh

dotnet restore                                                         Source/ManagedIrbis5.sln                               || exit
dotnet build --no-restore            --configuration Release           Source/ManagedIrbis5.sln                               || exit
dotnet test  --no-restore --no-build --configuration Release           Source/ManagedIrbis5.sln                               || exit
dotnet run   --no-restore --no-build --configuration Release --project Source/Benchmarks/CoreBenchmarks/CoreBenchmarks.csproj || exit
dotnet pack  --no-restore --no-build --configuration Release           Source/ManagedIrbis5.sln                               || exit

echo ALL DONE
