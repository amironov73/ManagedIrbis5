#echo off

del packages.lock.json
dotnet publish -r win-x64 -c Release /p:RestoreLockedMode=true /p:TrimLink=true --self-contained