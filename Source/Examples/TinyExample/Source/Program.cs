// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.Linq;

using ManagedIrbis;

#endregion

internal sealed class Program
{
    static int Main (string[] args)
    {
        try
        {
            using (var connection = new SyncConnection())
            {

                connection.Host = args.Length == 0
                    ? "127.0.0.1"
                    : args[0];
                connection.Username = "librarian";
                connection.Password = "secret";

                var success = connection.Connect();
                if (!success)
                {
                    Console.Error.WriteLineAsync("Can't connect");
                    return 1;
                }

                Console.WriteLine("Successfully connected");

                // Ищем все книги, автором которых является А. С. Пушкин
                // Обратите внимание на двойные кавычки в тексте запроса
                var found = connection.Search
                (
                    "\"A=ПУШКИН$\""
                );

                Console.WriteLine($"Найдено записей: {found.Length}");

                // Чтобы не распечатывать все найденные записи,
                // отберем только 10 первых
                foreach (var mfn in found.Take(10))
                {
                    // Получаем запись из базы данных
                    var record = connection.ReadRecord(mfn);

                    if (record != null)
                    {
                        // Извлекаем из записи интересующее нас поле и подполе
                        var title = record.FM(200, 'a');
                        Console.WriteLine($"Title: {title}");
                    }

                    // Форматируем запись средствами сервера
                    var description = connection.FormatRecord
                    (
                        "@brief",
                        mfn
                    );
                    Console.WriteLine($"Биб. описание: {description}");

                    Console.WriteLine(); // Добавляем пустую строку
                }

                // Отключаемся от сервера
                connection.Dispose();
                Console.WriteLine("Successfully disconnected");
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            return 1;
        }

        return 0;
    }
}
