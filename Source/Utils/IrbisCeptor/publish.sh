#!/bin/sh

# Публикация нативного образа под Linux
# ВНИМАНИЕ: нативная кросс-компиляция не поддерживается!

dotnet publish --runtime linux-x64 --configuration Release /p:PublishAot=true
