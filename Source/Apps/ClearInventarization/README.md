### ClearInventarization

Простая утилита для удаления информации о предыдущей инвентаризации из каталога.

Командная строка:

```
ClearInventarization <connectionString> <searchExpression> <placeRegex>
```

Здесь:

* **connectionString** - строка подключения;
* **searchExpression** - выражение для поиска по словарю;
* **placeRegex** - регулярное выражение для фондов, подлежащих очистке. Все фонды можно задать как `.*`

Пример:

```
ClearInventarization "server=192.168.1.2;user=librarian;password=secret;db=IBIS;" "INP=Ф404-$" "^Ф40[34]$"
```
