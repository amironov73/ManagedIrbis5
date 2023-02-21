# WebWorker

Шаблон веб-приложения, стартующего в Linux как демон под управлением `systemd`. Может содержать фоновые сервисы.

### Как собрать?

```sh
dotnet publish -c Release
```

Мы рассчитываем, что рантайм ASP.NET Core на целевой машине уже установлен. Работать будем из-под него.

### Как установить?

Создаем папку `/var/WebWorker`, копируем туда все файлы из `bin/Release/net7.0/publish`. Отдаем все хозяйство во владение `www-data`:

```sh
chown -R www-data /var/WebWorker
chgrp -R www-data /var/WebWorker
```

Создаем ссылку на юнит (под рутом, естественно) и запускаем сервис:

```sh
ln -s /var/WebWorker/worker.service /etc/systemd/system
systemctl start worker  # запускаем сервис
systemctl status worker # проверяем, как запустилось
systemctl enable worker # включаем автоматический запуск
```

Теперь переходим к nginx. Пусть у нас описан сайт `mysite.com`, надо добавить в него строчку `include`:

```nginx configuration
server {
        listen 80 default_server;
        listen [::]:80 default_server;

        root /var/www;
        index index.php index.html index.htm index.nginx-debian.html;
        server_name mysite.com;

        include worker.conf;
}
```

создать ссылку на конфигурационный файл и заставить nginx перечитать конфигурацию:

```sh
ln -s /etc/WebWorker/worker.conf /etc/nginx
systemctl reload nginx
```

Предварительно можно проверить конфигурацию:

```sh
nginx -t
```
