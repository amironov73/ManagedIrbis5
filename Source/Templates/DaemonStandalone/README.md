### DaemonStandalone

Шаблон приложения-демона, не использующего никаких библиотек Ars Magna.

### Windows

В Windows установка (строго из-под администратора):

```sh
sc create EchoServer binpath= C:\WorkerServices\EchoServer.exe
```

параметры установки:

```
type= <own|share|interact|kernel|filesys|rec|userown|usershare>
       (по умолчанию = own)
start= <boot|system|auto|demand|disabled|delayed-auto>
       (по умолчанию = demand)
error= <normal|severe|critical|ignore>
       (по умолчанию = normal)
binPath= <путь_к_двоичному_файлу_EXE>
group= <группа_запуска>
tag= <yes|no>
depend= <зависимости (разделенные / (косой чертой))>
obj= <имя_учетной_записи|имя_объекта>
       (по умолчанию = LocalSystem)
DisplayName= <отображаемое имя>
password= <пароль>
```
Примечание. Имя параметра включает знак равенства (=). Между знаком равенства и значением параметра должен быть пробел.


Запуск службы:

```sh
sc start EchoServer
```

проверка состояния:

```sh
sc query EchoServer
```

остановка:

```sh
sc stop EchoServer
```

приостановка:

```sh
sc pause EchoServer
```

продолжение после приостановки:

```sh
sc continue EchoServer
```

удаление службы:

```sh
sc delete EchoServer
```
