@echo off
dotnet publish  -r win-x64 -c Release --self-contained /p:RestoreLockedMode=true /p:TrimLink=true /p:PublishSingleFile=true
