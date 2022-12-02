@echo off
dotnet publish --runtime win-x64 --configuration Release --self-contained /p:PublishAot=true
