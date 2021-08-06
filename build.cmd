@echo off

if exist NuGet   rmdir /s /q NuGet 
if exist Publish rmdir /s /q Publish
                                                                               
dotnet clean                                                                    Source/Utils/Utils.sln
dotnet restore                                                                  Source/Utils/Utils.sln
dotnet build   --no-restore            --configuration Release                  Source/Utils/Utils.sln

dotnet clean                                                                    Source/ManagedIrbis5-windows.sln
dotnet clean                                                                    Source/TinyClient.sln

dotnet restore                                                                  Source/ManagedIrbis5-windows.sln
dotnet restore                                                                  Source/TinyClient.sln

dotnet build   --no-restore            --configuration Release                  Source/ManagedIrbis5-windows.sln
dotnet build   --no-restore            --configuration Release                  Source/TinyClient.sln

dotnet pack    --no-restore --no-build --configuration Release --output NuGet   Source/ManagedIrbis5-publish.sln
dotnet pack    --no-restore --no-build --configuration Release --output NuGet   Source/Libs/TinyClient/TinyClient.csproj

dotnet publish --no-restore --no-build --configuration Release --output Publish Source/ManagedIrbis5-publish.sln
dotnet publish --no-restore --no-build --configuration Release --output Publish Source/Libs/TinyClient/TinyClient.csproj

dotnet test    --no-restore --no-build --configuration Release                  Source/ManagedIrbis5-windows.sln
dotnet run     --no-restore --no-build --configuration Release --project        Source/Tests/PftTests/PftTests.csproj