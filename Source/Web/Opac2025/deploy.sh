#!/bin/bash

# ========================================================
# Скрипт для деплоймента приложения ASP.NET с демонизацией
# ========================================================

INSTALL_ROOT=/var/Opac2025
PUBLISH_DIR="$PWD/publish"
USER=www-data
SERVICE=opac

# скрипт надо запускать от рута
if [ "$EUID" -ne 0 ]
  then
    echo "Please run as root"
    exit 1
fi

if ! command -v dotnet &> /dev/null
then
    echo "dotnet could not be found"
    exit
fi

# собираем проект
rm -Rf "$PUBLISH_DIR"
if ! dotnet publish -c Release --property:PublishDir="$PUBLISH_DIR"
then
  echo "Publish failed"
  exit 1
fi

# останавливаем и запрещаем сервис, если есть
systemctl stop "$SERVICE"                    2> /dev/null
systemctl disable "$SERVICE"                 2> /dev/null
rm -f "/etc/systemd/system/$SERVICE.service" 2> /dev/null

# копируем в директорию назначения
rm    -Rf "$INSTALL_ROOT"
mkdir -p  "$INSTALL_ROOT"
cp    -R  "$PUBLISH_DIR"/* "$INSTALL_ROOT"

# отдаем пользователю www-data
chown -R $USER "$INSTALL_ROOT"
chgrp -R $USER "$INSTALL_ROOT"

# создаем сервис
ln -s "$INSTALL_ROOT/$SERVICE.service" /etc/systemd/system
systemctl enable "$SERVICE" # включаем автоматический запуск

# создаем конфигурацию NGINX
rm -f "/etc/nginx/$SERVICE.conf" 2> /dev/null
ln -s "$INSTALL_ROOT/$SERVICE.conf" /etc/nginx
if ! nginx -t
then
    echo "Problem with NGINX configuration"
    exit 1
fi

# запускаем сервис и заставляем NGINX перечитать конфигурацию
systemctl start "$SERVICE"
systemctl reload nginx

echo SUCCESS
