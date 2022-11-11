### Как поселить демона в Linux

Все действия выполняются строго из-под рута.

Пусть наш демон называется `Daemon`.

1. Наш демон будет выполняться от имени какого-нибудь достаточно безобидного пользователя, например, `www-data`. Запомним это.
2. Наш демон будет жить в директории `/usr/share/ArsMagna/Daemon`. Копируем туда все нужные файлы. Сделаем `chown -R www-data:www-data .`.
3. Наш демон будет писать в `/var/log/ArsMagna/Daemon/LogFile.txt`. Надо создать папку и сделать `chown www-data:www-data .`, затем `chmod u+w .` (на всякий случай).
4. Сочинить файл `Daemon.service` и поместить его в папку `/usr/lib/systemd/system`.

```
[Unit]
Description=Ars Magna daemon application

[Service]
WorkingDirectory=/usr/share/ArsMagna/Daemon
ExecStart=/usr/share/ArsMagna/Daemon/Daemon
Restart=always
KillSignal=SIGINT
SyslogIdentifier=magna-daemon
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target

```

5. Сказать `systemctl daemon-reload`, чтобы systemctl понял, что появился демон.
6. Сказать `systemctl enable Daemon`, чтобы демон запускался автоматически при старте системы.
7. Сказать `systemctl start Daemon`, чтобы запустить демона вручную.
8. Сказать `systemctl status Daemon`. Должно высветиться что-то вроде

```
* Daemon.service - Ars Magna daemon application
  Loaded: loaded (/lib/systemd/system/Daemon.service: enabled; vendor preset: enabled)
  Active: active (running) sinse Fri 2022-11-11 16:53:10 +08 1s ago
Main PID: 5289 (Daemon)
   Tasks: 14 (limit: 9410)
  Memory: 90.5M
  CGrohp: /system.slice/Daemon.service
          └5289 /usr/share/ArsMagna/Daemon/Daemon
```

9. Сказать `tail -f /var/log/ArsMagna/Daemon/LogFile.txt`, чтобы посмотреть, что написал демон в логах. Параметр `-f` заставляет команду tail перечитывать файл при появлении новых строк, удобно.
