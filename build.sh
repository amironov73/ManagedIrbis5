#!/bin/sh

VERSION_PREFIX=5.0.0
BUILD_NUMBER=`git rev-list --all --count`
VERSION=${VERSION_PREFIX}.${BUILD_NUMBER}

echo VERSION ${VERSION}
dotnet restore                                                                              Source/ManagedIrbis5.sln                               || exit
dotnet build --no-restore            --configuration Release -p:Version=${VERSION}          Source/ManagedIrbis5.sln                               || exit
dotnet test  --no-restore --no-build --configuration Release                                Source/ManagedIrbis5.sln                               || exit
dotnet run   --no-restore --no-build --configuration Release --project                      Source/Benchmarks/CoreBenchmarks/CoreBenchmarks.csproj || exit
dotnet pack  --no-restore --no-build --configuration Release -p:PackageVersion=${VERSION}   Source/ManagedIrbis5.sln                               || exit

echo ALL DONE
