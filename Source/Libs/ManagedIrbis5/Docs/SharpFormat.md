### C#-скрипты для форматирования записей

Стандартный (полный) вид скрипта форматирования:

```c#
using System;
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Scripting;

sealed class HelloScript : ScriptContext
{
    public HelloScript (ISyncProvider provider, TextWriter output)
       : base (provider, output) {}

    // выполняется после расформатирования всех записей
    public override void AfterAll()
    {
        WriteLine ("В конце");
    }

    // выполняется перед расформатированием всех записей
    public override void BeforeAll()
    {
        WriteLine ("В начале");
    }

    // собственно форматирование записей
    public override void FormatRecord()
    {
        WriteLine (FM (200, 'a'));
    }
}
```

Скрипт, разложенный по фрагментам:

```c#
<references>
Microsoft.Extensions.Caching.Memory
</references>

<using>
using ManagedIrbis;
using ManagedIrbis.Scripting;
</using>

<before>
</before>

<format>
WriteLine (FM (200, 'a'));
</format>

<after>
WriteLine ("В конце");
</after>
```

Сокращенный вариант скрипта (функционально идентичный, кроме `BeforeAll` и `AfterAll`):

```c#
WriteLine (FM (200, 'a'));
```

Предоставляемые функции:

* **int Count (int tag)** - число повторений поля с указанной меткой.
* **string? FM (int tag)** - получение текста поля до разделителя.
* **string? FM (int tag, int occurrence)** - получение текста поля до разделителя (заданное повторение).
* **string? FM (int tag, char code)** - текст первого подполя с указанным тегом и кодом.
* **string? FM (int tag, int occurrence, char code)** - текст первого подполя с указанным тегом и кодом (заданное повторение).
* **string[] FMA (int tag)** - текст до разделителя всех повторений поля (пустые строки исключаются).
* **string[] FMA (int tag, char code)** - текст всех подполей с указанной меткой и полем.
* **bool HaveField (int tag)** - проверка, есть ли в записи поле с указанной меткой.
* **bool V (int tag, char? code = null, string? prefix = null, string? before = null, string? after = null, string? suffix = null, bool skipFirst = false, bool skipLast = false)** - вывод значения поля до первого разделителя или подполя с заданным кодом (с учетом повторяющихся групп).
* **void Write (string text)** - вывод текста.
* **void WriteLine()** - переход на новую строку.
* **void WriteLine (string text)** - вывод текста с последующим переходом на новую строку.
