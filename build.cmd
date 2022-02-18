@echo off

if exist NuGet   rmdir /s /q NuGet
if exist Publish rmdir /s /q Publish

dotnet clean   --configuration Release Source\Utils\Utils-windows.sln
dotnet clean   --configuration Release Source\ManagedIrbis5-windows.sln
dotnet clean   --configuration Release Source\TinyClient.sln

dotnet build   --configuration Release Source\Utils\Utils-windows.sln
dotnet build   --configuration Release Source\ManagedIrbis5-windows.sln
dotnet build   --configuration Release Source\TinyClient.sln

dotnet pack    --no-restore --no-build --configuration Release --output NuGet   Source\ManagedIrbis5-publish.sln
dotnet pack    --no-restore --no-build --configuration Release --output NuGet   Source\Libs\TinyClient\TinyClient.csproj

dotnet publish --no-restore --no-build --configuration Release --output Publish Source\ManagedIrbis5-publish.sln
dotnet publish --no-restore --no-build --configuration Release --output Publish Source\Libs\TinyClient\TinyClient.csproj

dotnet test    --no-restore --no-build --configuration Release                  Source\ManagedIrbis5-windows.sln
dotnet run     --no-restore --no-build --configuration Release --project        Source\Tests\PftTests\PftTests.csproj

echo ALL DONE
