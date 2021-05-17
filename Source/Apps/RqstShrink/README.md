### RqstShrink

Сжимает базу RQST, выбрасывая выполненные заказы.

Командная строка:

```
RqstShrink [connection-string] [-e expression]
```

Здесь:

* `connection-string` - строка подключения. По умолчанию используется строка из `appsettings.json`.
* `-e` - поисковое выражение ИРБИС для отбора годных заказов. По умолчанию используется строка из `appsettings.json`: `"I=0 + I=2"`, означающая "все невыполненные заказы плюс все забронированные издания".

Типичный вывод в консоль:

```
Reading good records .
Good records loaded: 104
Database truncated
Good records restored
Elapsed: 0.91
```
