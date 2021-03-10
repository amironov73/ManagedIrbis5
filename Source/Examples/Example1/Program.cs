// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

using System;
using System.Threading.Tasks;

using ManagedIrbis;

using static System.Console;

#nullable enable

class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            await using var connection = ConnectionFactory.Shared
                .CreateConnection();

            connection.Host = args.Length == 0
                ? "127.0.0.1"
                : args[0];
            connection.Username = "librarian";
            connection.Password = "secret";

            var success = await connection.ConnectAsync();
            if (!success)
            {
                await Error.WriteLineAsync("Can't connect");
                return 1;
            }

            WriteLine("Successfully connected");

            // Ищем все книги, автором которых является А. С. Пушкин
            // Обратите внимание на двойные кавычки в тексте запроса
            var found = await connection.SearchAsync
                (
                    "\"A=ПУШКИН$\""
                );

            WriteLine($"Найдено записей: {found.Length}");

            // Чтобы не распечатывать все найденные записи,
            // отберем только 10 первых
            foreach (var mfn in found[..10])
            {
                // Получаем запись из базы данных
                var record = await connection.ReadRecordAsync(mfn);

                if (record is not null)
                {
                    // Извлекаем из записи интересующее нас поле и подполе
                    var title = record.FM(200, 'a');
                    WriteLine($"Title: {title}");
                }

                // Форматируем запись средствами сервера
                var description = await connection.FormatRecordAsync
                    (
                        "@brief",
                        mfn
                    );
                WriteLine($"Биб. описание: {description}");

                WriteLine(); // Добавляем пустую строку
            }

            // Отключаемся от сервера
            await connection.DisposeAsync();
            WriteLine("Successfully disconnected");
        }
        catch (Exception exception)
        {
            WriteLine(exception);
            return 1;
        }

        return 0;
    }
}
