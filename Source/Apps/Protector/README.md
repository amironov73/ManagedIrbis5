### Protector

Простейший инструмент для защиты строки подключения и других чувствительных данных. Осуществляет несложное шифрование, так что внешне строка начинает выглядеть непонятно. Например, *"Hello, world"* превращается в

```
AQAAANCMnd8BFdERjHoAwE/Cl+sBAAAAmfQrHwl6gUSdX3oc1eHHCAQAAAACAAAAAAAQZgAAAAEAACAAAAAKjpiYAuNQFiUTbfKJ0p/oKa/d4oOPihy9mr47/saQ2AAAAAAOgAAAAAIAACAAAAAAaQsGdbuVWg5sF/XtIKQP6a7f6audmMvsLRqHmU7RjSAAAAByFIycQuHgrulmvOfvTk8mXN7HST5tXitbAm7c1zzvzUAAAABsMa7djZdciULEOHBxqxTRU+1RNt7YbYuTLs/O0ANqw6lxarQUTLkLrRUVTJvX00x3VM0H3aXLG7tsgljGwvoK
```

"Мамкиных хакеров" это отпугивает, а от остальных защищаться лучше с помощью серьезных инструментов.

#### Командная строка

```
Protector.exe <строка-для-шифрования>
```

Результат выводится в stdout.

#### Обратная расшифровка

Для расшифровки защищенной строки достаточно вызвать метод `AM.Configuration.ConfigurationUtility.Unprotect`.

