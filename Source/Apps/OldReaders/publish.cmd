@echo off

dotnet publish --runtime win-x64 --configuration Release /p:PublishAot=true OldReaders.csproj
