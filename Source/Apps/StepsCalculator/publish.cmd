@echo off

set DOTNET_CLI_UI_LANGUAGE=en

del packages.lock.json 2> nul
rem dotnet publish StepsCalculator.csproj -r win-x64 -c Release /p:RestoreLockedMode=true /p:PublishSingleFile=true --self-contained
dotnet publish StepsCalculator.csproj -r win-x64 -c Release --self-contained