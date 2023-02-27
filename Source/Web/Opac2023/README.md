# Opac2023

Электронный каталог НТБ ИРНИТУ, версия `2023.03`.

### Как собрать?

```sh
dotnet publish -c Release
```

Мы рассчитываем, что рантайм ASP.NET Core на целевой машине уже установлен. Работать будем из-под него.

### Как установить?

Создаем папку `/var/Opac2023`, копируем туда все файлы из `bin/Release/net7.0/publish`. Отдаем все хозяйство во владение `www-data`:

```sh
chown -R www-data /var/Opac2023
chgrp -R www-data /var/Opac2023
```

Создаем ссылку на юнит (под рутом, естественно) и запускаем сервис:

```sh
ln -s /var/Opac2023/opac.service /etc/systemd/system
systemctl start opac  # запускаем сервис
systemctl status opac # проверяем, как запустилось
systemctl enable opac # включаем автоматический запуск
```

Теперь переходим к nginx. Пусть у нас описан сайт `mysite.com`, надо добавить в него строчку `include`:

```nginx configuration
server {
        listen 80 default_server;
        listen [::]:80 default_server;

        root /var/www;
        index index.php index.html index.htm index.nginx-debian.html;
        server_name mysite.com;

        include opac.conf;
}
```

создать ссылку на конфигурационный файл и заставить nginx перечитать конфигурацию:

```sh
ln -s /etc/Opac2023/opac.conf /etc/nginx
systemctl reload nginx
```

Предварительно можно проверить конфигурацию:

```sh
nginx -t
```
