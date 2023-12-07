@echo off

set DOTNET_CLI_UI_LANGUAGE=en

del packages.lock.json 2> nul
dotnet publish -r win-x64 -c Release /p:RestoreLockedMode=true /p:PublishSingleFile=true --self-contained
