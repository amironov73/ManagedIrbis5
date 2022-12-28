### Encryptor

Простейший шифровальщик для паролей и прочего

Командная строка:

```
Encrypt <text> <passwprd>
```

Здесь:

* `text` - строка, подлежащая шифрованию.
* `password` - произвольный пароль.

Результат выводится в стандартный выходной поток.

Пример:

```
$ Encryptor "host=1.2.3.4;user=librarian;password=secret;" "irbis"

4kRO2z2kgSqSjnaMql9cBdSSdVOq52M1UWQoJG+MQFdGhbsLqT969SoFe+lTAdmt
```
