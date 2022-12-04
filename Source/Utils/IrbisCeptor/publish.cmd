@echo off

rem Публикация нативного образа под Windows
rem ВНИМАНИЕ: нативная кросс-компиляция не поддерживается!

dotnet publish --runtime win-x64 --configuration Release /p:PublishAot=true
