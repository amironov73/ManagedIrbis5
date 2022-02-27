// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using ManagedIrbis;

#endregion

#nullable enable

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
