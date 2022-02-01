#!/bin/sh

rm -rf NuGet
rm -rf Publish

dotnet clean   Source/Utils/Utils.sln
dotnet clean   Source/ManagedIrbis5.sln

dotnet build                           --configuration Release                  Source/Utils/Utils.sln                                || exit
dotnet build                           --configuration Release                  Source/ManagedIrbis5.sln                              || exit
dotnet test    --no-restore --no-build --configuration Release                  Source/ManagedIrbis5.sln                              || exit
dotnet run     --no-restore --no-build --configuration Release --project        Source/Tests/PftTests/PftTests.csproj                 || exit
dotnet run     --no-restore --no-build --configuration Release --project        Source/Tests/BarsikTestRunner/BarsikTestRunner.csproj || exit
dotnet pack    --no-restore --no-build --configuration Release --output NuGet   Source/ManagedIrbis5-publish.sln
dotnet publish --no-restore --no-build --configuration Release --output Publish Source/ManagedIrbis5-publish.sln

echo ALL DONE
