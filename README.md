# ManagedIrbis5

Client framework for IRBIS64 system ported to .NET 6

Currently supports:

* Windows 7/10/11 x64
* MacOS 10.14
* .NET runtime 6.0.0 and higher
* .NET SDK 6.0.100 and higher

```c#
using System;                                   
using ManagedIrbis;
using static System.Console;

try
{
    await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();

    connection.Host = args.Length == 0 ? "127.0.0.1" : args[0];
    connection.Username = "librarian";
    connection.Password = "secret";

    var success = await connection.ConnectAsync();
    if (!success)
    {
        // не получилось подключиться, жалуемся и завершаемся
        await Error.WriteLineAsync ("Can't connect");
        await Error.WriteLineAsync (IrbisException.GetErrorDescription (connection.LastError));
        return 1;
    }

    await Out.WriteLineAsync ("Successfully connected");

    // Ищем все книги, автором которых является А. С. Пушкин
    // Обратите внимание на двойные кавычки в тексте запроса
    var found = await connection.SearchAsync
        (
            "\"A=ПУШКИН$\""
        );

    await Out.WriteLineAsync ($"Найдено записей: {found.Length}");

    // Чтобы не распечатывать все найденные записи,
    // отберем только 10 первых
    foreach (var mfn in found[..10])
    {
        // Получаем запись из базы данных
        var record = await connection.ReadRecordAsync (mfn);

        if (record is not null)
        {
            // Извлекаем из записи интересующее нас поле и подполе
            var title = record.FM (200, 'a');
            await Out.WriteLineAsync ($"Title: {title}");
        }

        // Форматируем запись средствами сервера
        var description = await connection.FormatRecordAsync
            (
                "@brief",
                mfn
            );
        await Out.WriteLineAsync ($"Биб. описание: {description}");

        await Out.WriteLineAsync(); // Добавляем пустую строку
    }

    // Отключаемся от сервера
    await connection.DisposeAsync();
    await Out.WriteLineAsync ("Successfully disconnected");
}
catch (Exception exception)
{
    await Error.WriteLineAsync (exception.ToString());
    return 1;
}

return 0;
```

### Build status

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/7a2fc9c9cff946c89f7f4f6adcb567c3)](https://app.codacy.com/gh/amironov73/ManagedIrbis5?utm_source=github.com&utm_medium=referral&utm_content=amironov73/ManagedIrbis5&utm_campaign=Badge_Grade_Settings)
[![Build status](https://img.shields.io/appveyor/ci/AlexeyMironov/managedirbis5.svg)](https://ci.appveyor.com/project/AlexeyMironov/managedirbis5/)
[![Build status](https://api.travis-ci.org/amironov73/ManagedIrbis5.svg)](https://travis-ci.org/amironov73/ManagedIrbis5/)
[![GitHub Action](https://github.com/amironov73/ManagedIrbis5/workflows/CI/badge.svg)](https://github.com/amironov73/ManagedIrbis5/actions)
[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5.svg?type=shield)](https://app.fossa.com/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5?ref=badge_shield)
[![Maintainability](https://api.codeclimate.com/v1/badges/50cc8f9ee8ebc972e037/maintainability)](https://codeclimate.com/github/amironov73/ManagedIrbis5/maintainability)
[![CodeFactor](https://www.codefactor.io/repository/github/amironov73/managedirbis5/badge)](https://www.codefactor.io/repository/github/amironov73/managedirbis5)

### License

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2Famironov73%2FManagedIrbis5?ref=badge_large)

### Documentation (in russian)

[![Badge](https://readthedocs.org/projects/managedirbis5/badge/)](https://managedirbis5.readthedocs.io/) 

