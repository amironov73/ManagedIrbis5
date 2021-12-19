@echo off

set BUILD_VERSION=5.0.0.556

if exist NuGet   rmdir /s /q NuGet
if exist Publish rmdir /s /q Publish

dotnet clean                                        Source/Utils/Utils.sln
dotnet restore                                      Source/Utils/Utils.sln
dotnet build   --no-restore --configuration Release Source/Utils/Utils.sln

echo VERSION %BUILD_VERSION%
dotnet run --no-restore --no-build --configuration Release --project Source\Utils\PatchVersion\PatchVersion.csproj Source\Common\ArsMagna.targets %BUILD_VERSION%
type Source\Common\ArsMagna.targets

dotnet clean   Source/ManagedIrbis5-windows.sln
dotnet clean   Source/TinyClient.sln

dotnet restore Source/ManagedIrbis5-windows.sln
dotnet restore Source/TinyClient.sln

dotnet build   --no-restore            --configuration Release -p:Version=%BUILD_VERSION%       Source/ManagedIrbis5-windows.sln
dotnet build   --no-restore            --configuration Release -p:Version=%BUILD_VERSION%       Source/TinyClient.sln

dotnet pack    --no-restore --no-build --configuration Release -p:PackageVersion=%BUILD_VERSION% --include-symbols --include-source --output NuGet Source/ManagedIrbis5-publish.sln
dotnet pack    --no-restore --no-build --configuration Release -p:PackageVersion=%BUILD_VERSION% --include-symbols --include-source --output NuGet Source/Libs/TinyClient/TinyClient.csproj

dotnet publish --no-restore --no-build --configuration Release --output Publish Source/ManagedIrbis5-publish.sln
dotnet publish --no-restore --no-build --configuration Release --output Publish Source/Libs/TinyClient/TinyClient.csproj

dotnet test    --no-restore --no-build --configuration Release                  Source/ManagedIrbis5-windows.sln
dotnet run     --no-restore --no-build --configuration Release --project        Source/Tests/PftTests/PftTests.csproj

echo ALL DONE
