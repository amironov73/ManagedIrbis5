@echo off

dotnet publish -r win-x64 -c Release /p:TrimLink=true /p:PublishAot=true
