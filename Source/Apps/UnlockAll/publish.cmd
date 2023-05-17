@echo off

rem dotnet publish -r win-x64 -c Release -p:RestoreLockedMode=true -p:PublishSingleFile=true --self-contained
dotnet publish -r win-x64 -c Release -p:RestoreLockedMode=true -p:PublishAot=true --self-contained
