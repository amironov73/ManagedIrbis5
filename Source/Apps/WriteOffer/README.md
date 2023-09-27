### WriteOffer

Списание книг скопом.

Параметры командной строки:

```shell
WriteOffer <connection-string> <book-list> [act-number]
```

здесь

* **connection-string** - строка подключения (может быть зашифрованной),
* **book-list** - имя файла, содержащего инвентарные номера, подлежащие списанию (одна строка - один номер, кодировка UTF-8),
* **act-number** - номер акта списания (опционально).

Пример запуска:

```shell
WriteOffer "host=127.0.0.1;port=6666;user=librarian;password=secret;arm=C;db=IBIS" books.txt "ООБУ-000004"
```

Пример вывода:

```
805333: not found
1) 823433: OK
2) 839539: OK
3) 962138: OK
4) 1080528: OK
1753261: not found
5) 1766171: OK
1779180: not found
6) 1783470: OK
1789840: not found
7) CD1967: OK
8) CD2440: OK
ALL DONE
```
