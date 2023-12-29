// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* EventUploader.cs -- отправляет данные о проходах читателей на сервер ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Readers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Caching.Memory;

#endregion

namespace Gatekeeper2024;

/// <summary>
/// Отправляет данные о событиях прохода читателей на сервер ИРБИС64.
/// </summary>
internal sealed class EventUploader
    : IHostedService
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public EventUploader
        (
            IMemoryCache cache
        )
    {
        _cache = cache;
        _queueDirectory = Utility.GetQueueDirectory();
    }

    #endregion

    #region Private members

    private readonly string _queueDirectory;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Получение одного (первого в хронологическом порядке) файла,
    /// готового к отправке.
    /// </summary>
    private string? GetOneFile()
    {
        var files = Directory.GetFiles (_queueDirectory, "*.json");

        // надо отсортировать, потому что файловая система
        // может выдать список файлов в произвольном порядке,
        // мы ведь кроссплатформенное приложение
        Array.Sort (files);

        // учитывая, что имена файлов строятся по схеме "yyyy-MM-dd-HH-mm-ss-ff",
        // первый элемент отсортированного массива будет и хронологически первым

        return files.FirstOrDefault();
    }

    /// <summary>
    /// Удаление отработанного либо битого файла.
    /// </summary>
    private void DeleteFile
        (
            string path
        )
    {
        try
        {
            File.Delete (path);
            Program.Logger.LogInformation ("Delete file {Path}", path);
        }
        catch (Exception exception)
        {
            Program.Logger.LogError (exception, "Can't delete file {Path}", path);
        }
    }

    /// <summary>
    /// Поиск читателя по его идентификатору.
    /// </summary>
    private ReaderInfo? GetReader
        (
            ISyncProvider connection,
            string readerId,
            string path,
            PassEvent passEvent
        )
    {
        var readers = Utility.SearchForReader (connection, readerId);
        if (readers is null)
        {
            // не удалось связаться с сервером
            // не удаляем файл, вдруг удастся связаться в будущем
            return null;
        }

        if (readers.Length == 0)
        {
            Program.Logger.LogError ("No reader with ticket {Ticket}", readerId);
            LogPassError (passEvent);
            DeleteFile (path);
            return null;
        }

        if (readers.Length != 1)
        {
            Program.Logger.LogError ("Many readers with ticket {Ticket}", readerId);
            LogPassError (passEvent);
            DeleteFile (path);
            return null;
        }

        return readers[0];
    }

    /// <summary>
    /// Обработка входа читателя.
    /// </summary>
    private void ProcessArrival
        (
            string path,
            PassEvent passEvent
        )
    {
        var readerId = passEvent.Id;
        if (string.IsNullOrEmpty (readerId))
        {
            Program.Logger.LogError ("Empty reader ID in file {Path}", path);
            DeleteFile (path);
            return;
        }

        using var connection = Utility.ConnectToIrbis();
        if (connection is null)
        {
            return;
        }

        var reader = GetReader (connection, readerId, path, passEvent);
        if (reader is null)
        {
            // файл не удаляем, т. к. это делает GetReader при необходимости
            return;
        }

        var record = reader.Record;
        if (record is null)
        {
            Program.Logger.LogError ("Strange thing: reader.Record is null: {Event}", passEvent);
            LogPassError (passEvent);
            DeleteFile (path);
            return;
        }

        // проверяем, не зафиксировалось ли посещение с прошлой попытки
        var found = false;
        var today = Utility.FormatDateTime ("{date}", passEvent.Moment);
        var now = Utility.FormatDateTime ("{time}", passEvent.Moment);
        var department = Utility.GetDepartment();
        var person = Utility.GetPerson();
        var description = Utility.GetEvent();
        foreach (var field in record.EnumerateField (VisitInfo.Tag))
        {
            var visit = VisitInfo.Parse (field);
            if (
                    visit is { IsVisit: true, IsReturned: false }
                    && visit.DateGivenString == today
                    && visit.TimeIn == now
                    && visit.Department == department
                    && visit.Description == description
                    && visit.Responsible == person
                )
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            // посещение уже зафиксировано, просто мы об этом не в курсе
            // возможно, ответ от сервера ИРБИС64 до нас не дошел
            Program.Logger.LogInformation
                (
                    "Arrival already registered: {Ticket}, {Moment}",
                    readerId,
                    passEvent.Moment
                );

            DeleteFile (path);
            return;
        }

        // минимальный промежуток времени между последовательными
        // проходами читателя, минуты
        var minimumTimeSpan = Utility.GetTimeSpan();
        if (minimumTimeSpan > 0)
        {
            string? lastVisit = null;
            foreach (var field in record.EnumerateField (VisitInfo.Tag))
            {
                var visit = VisitInfo.Parse (field);
                if (
                        visit is { IsVisit: true }
                        && visit.DateGivenString == today
                        && visit.Department == department
                        && visit.Description == description
                        && visit.Responsible == person
                    )
                {
                    var candidate = visit.TimeIn;
                    if (string.CompareOrdinal (candidate, lastVisit) > 0)
                    {
                        lastVisit = candidate;
                    }
                }
            }

            if (lastVisit is not null)
            {
                // было предыдущее посещение, сравниваем моменты времени

                // мы не берем DateTimeOffset.Now, т. к. имеем дело
                // с отложенной записью посещений, т. е. реально
                // посетитель мог пройти час назад, а дело до записи
                // дошло только сейчас
                var previousTime = Utility.ParseTime (lastVisit);
                var currentTime = Utility.ParseTime (now);
                var delta = (int) Math.Ceiling ((currentTime - previousTime).TotalMinutes);
                if (delta < minimumTimeSpan)
                {
                    // кладем в кеш, чтобы потом можно было взять его
                    // при обработке выхода читателя
                    _cache.Set
                        (
                            readerId,
                            passEvent,
                            new MemoryCacheEntryOptions
                            {
                                Size = 1,
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes (minimumTimeSpan)
                            }
                        );

                    // прошло слишком мало времени, это посещение можно не регистрировать
                    Program.Logger.LogInformation
                        (
                            "Last arrival was recently: {Ticket}, {Previous}, {Current}",
                            readerId,
                            lastVisit,
                            passEvent.Moment
                        );

                    DeleteFile (path);
                    return;
                }
            }
        }

        var eventData = Utility.GetArrivalField (passEvent.Moment);
        record.Add (40, eventData);

        // Добавление поля 999 нужно для формирования словаря "VIS="
        // (читатели в библиотеке)
        // в RDR.FST есть строчка
        // 999 0 mhl,"VIS="v999
        record.SetValue (999, "1");

        // на всякий случай удаляем событие из кэша, раз уж мы добрались сюда
        _cache.Remove (readerId);

        // отправляем модифицированную запись на сервер
        var parameters = new WriteRecordParameters
        {
            Record = record,
            Lock = false,
            Actualize = true,
            DontParse = true
        };

        if (connection.WriteRecord (parameters))
        {
            Program.Logger.LogInformation
                (
                    "Arrival to Irbis success: {Ticket}, {EventData}",
                    readerId,
                    eventData
                );

            // при успешном окончании удаляем файл
            DeleteFile (path);
        }
    }

    /// <summary>
    /// Проверка, что с данным событием можно иметь дело.
    /// </summary>
    private bool VerifyEvent
        (
            PassEvent passEvent
        )
    {
        var arrivalPoint = Utility.GetArrivalPoint();
        var departurePoint = Utility.GetDeparturePoint();

        var result = (passEvent.Point == arrivalPoint
                      || passEvent.Point == departurePoint)
                     && !string.IsNullOrEmpty (passEvent.Id);

        if (!result)
        {
            Program.Logger.LogError ("Bad PassEvent: {Event}", passEvent);
        }

        return result;
    }

    /// <summary>
    /// Обработка выхода читателя.
    /// </summary>
    private void ProcessDeparture
        (
            string path,
            PassEvent passEvent
        )
    {
        var readerId = passEvent.Id;
        if (string.IsNullOrEmpty (readerId))
        {
            Program.Logger.LogError ("Empty reader ID in file {Path}", path);
            DeleteFile (path);
            return;
        }

        if (_cache.TryGetValue (readerId, out _))
        {
            // событие входа не было зарегистрировано,
            // т. к. произошло слишком рано после предыдущего
            _cache.Remove (readerId);
            Program.Logger.LogError ("Arrival was skipped: {Reader}", readerId);
            DeleteFile (path);
            return;
        }

        using var connection = Utility.ConnectToIrbis();
        if (connection is null)
        {
            return;
        }

        var reader = GetReader (connection, readerId, path, passEvent);
        if (reader is null)
        {
            // файл не удаляем, т. к. это делает GetReader при необходимости
            return;
        }

        var record = reader.Record;
        if (record is null)
        {
            Program.Logger.LogError ("Strange thing: reader.Record is null: {Event}", passEvent);
            LogPassError (passEvent);
            DeleteFile (path);
            return;
        }

        // проверяем, не зафиксировался ли выход с прошлой попытки
        var found = false;
        var date = Utility.FormatDateTime ("{date}", passEvent.Moment);
        var time = Utility.FormatDateTime ("{time}", passEvent.Moment);
        var department = Utility.GetDepartment();
        var person = Utility.GetPerson();
        var description = Utility.GetEvent();
        foreach (var field in record.EnumerateField (VisitInfo.Tag))
        {
            var visit = VisitInfo.Parse (field);
            if (
                    visit is { IsVisit: true }
                    && visit.DateReturnedString == date
                    && visit.TimeOut == time
                    && visit.Department == department
                    && visit.Description == description
                    && visit.Responsible == person
                )
            {
                found = true;
                break;
            }
        }

        if (found)
        {
            // выход уже зафиксирован, просто мы об этом не в курсе
            // возможно, ответ от сервера ИРБИС64 до нас не дошел
            Program.Logger.LogInformation
                (
                    "Departure already registered: {Ticket}, {Moment}",
                    readerId,
                    passEvent.Moment
                );

            DeleteFile (path);
            return;
        }

        // далее проверяем, есть ли соответствующие события входа
        var counter = 0;
        foreach (var field in record.EnumerateField (VisitInfo.Tag))
        {
            var visit = VisitInfo.Parse (field);
            if (
                    visit is { IsVisit: true, IsReturned: false }
                    && visit.DateGivenString == date
                    && visit.Department == department
                    && visit.Description == description
                    && visit.Responsible == person
                    && string.IsNullOrEmpty (visit.DateReturnedString)
                    && string.IsNullOrEmpty (visit.TimeOut)
                )
            {
                // событие входа есть, просто добавляем в поле
                // информацию о выходе посетителя
                var departureText = Utility.GetDepartureField (passEvent.Moment);
                field.Append (departureText);
                counter++;
            }
        }

        // не заносите eventData под if
        var eventData = Utility.GetArrivalField (passEvent.Moment)
            + Utility.GetDepartureField (DateTimeOffset.Now);
        if (counter is 0)
        {
            // минимальный промежуток времени между последовательными
            // проходами читателя, минуты
            var minimumTimeSpan = Utility.GetTimeSpan();
            if (minimumTimeSpan > 0)
            {
                // TODO реализовать проверку, насколько давно произошел выход

                Program.Logger.LogInformation
                    (
                        "Departure was recently: {Ticket}, {Moment}",
                        readerId,
                        passEvent.Moment
                    );

                DeleteFile (path);
                return;
            }

            // события входа почему-то нет, ничего страшного,
            // формируем собыитие входа-выхода
            record.Add (40, eventData);
        }

        // удаляем поле 999, чтобы убрать читателя из словаря
        // "VIS=" (читатели в библиотеке)
        record.RemoveField (999);

        // отправляем модифицированную запись на сервер
        var parameters = new WriteRecordParameters
        {
            Record = record,
            Lock = false,
            Actualize = true,
            DontParse = true
        };

        if (connection.WriteRecord (parameters))
        {
            Program.Logger.LogInformation
                (
                    "Departure to Irbis success: {Ticket}, {EventData}",
                    readerId,
                    eventData
                );

            // при успешном окончании удаляем файл
            DeleteFile (path);
        }
    }

    private void ProcessFile
        (
            string path
        )
    {
        var content = File.ReadAllBytes (path);
        var stream = new MemoryStream (content);

        // синхронно нельзя - лается и бросается исключениями
        // TODO разобраться с предупреждением
        var passEvent = JsonSerializer.DeserializeAsync<PassEvent> (stream)
            .GetAwaiter().GetResult();

        if (passEvent is null || !VerifyEvent (passEvent))
        {
            Program.Logger.LogError ("Bad event file {Path}", path);
            DeleteFile (path);
            return;
        }

        var arrivalPoint = Utility.GetArrivalPoint();
        var departurePoint = Utility.GetDeparturePoint();
        if (passEvent.Moment < DateTimeOffset.Now.AddDays (-1))
        {
            // событие слишком старое, просто удаляем его
            Program.Logger.LogInformation
                (
                    "Event is too old {Path}: {Moment}",
                    path,
                    passEvent.Moment
                );

            LogPassError (passEvent);
            DeleteFile (path);
            return;
        }

        if (passEvent.Point == arrivalPoint)
        {
            ProcessArrival (path, passEvent);
        }
        else if (passEvent.Point == departurePoint)
        {
            ProcessDeparture (path, passEvent);
        }
        else
        {
            Program.Logger.LogError ("Bad event file {Path}", path);
        }
    }

    private bool CreateQueueDirectory()
    {
        try
        {
            Directory.CreateDirectory (_queueDirectory);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine($"Ошибка при создании директории {_queueDirectory}");
            Program.Logger.LogError
                (
                    exception,
                    "Can't create queue directory {Directory}", _queueDirectory
                );
            return false;
        }

        return true;
    }

    /// <summary>
    /// Запись в журнал ошибки.
    /// </summary>
    private void LogPassError
        (
            PassEvent passEvent
        )
    {
        var arrivalPoint = Utility.GetArrivalPoint();
        var departurePoint = Utility.GetDeparturePoint();

        if (passEvent.Point == arrivalPoint)
        {
            LogArrivalError (passEvent);
        }

        if (passEvent.Point == departurePoint)
        {
            LogDepartureError (passEvent);
        }
    }

    /// <summary>
    /// Запись в журнал ошибки входа.
    /// </summary>
    private void LogArrivalError
        (
            PassEvent passEvent
        )
    {
        try
        {
            var logDirectory = Utility.GetRequiredString ("log-directory");
            var fileName = Path.Combine (logDirectory, "(loginerror.txt");
            var entry = $"#10: {passEvent.Id}\n"
                + Utility.FormatDateTime ("#40: ^d{date}^1{time}\n*****", passEvent.Moment);
            using var stream = File.CreateText (fileName);
            stream.WriteLine(entry);
        }
        catch (Exception exception)
        {
            Program.Logger.LogError (exception, "Can't log LogoutError");
        }
    }

    /// <summary>
    /// Запись в журнал ошибки выхода.
    /// </summary>
    private void LogDepartureError
        (
            PassEvent passEvent
        )
    {
        try
        {
            var logDirectory = Utility.GetRequiredString ("log-directory");
            var fileName = Path.Combine (logDirectory, "(logouterror.txt");
            var entry = $"#10: {passEvent.Id}\n"
                + Utility.FormatDateTime ("#40: ^f{date}^2{time}\n*****", passEvent.Moment);
            using var stream = File.CreateText (fileName);
            stream.WriteLine(entry);
        }
        catch (Exception exception)
        {
            Program.Logger.LogError (exception, "Can't log LogoutError");
        }
    }

    private void MainLoop ()
    {
        if (!CreateQueueDirectory())
        {
            return;
        }

        Utility.TestIrbisConnection();

        while (true)
        {
            var file = GetOneFile();
            if (!string.IsNullOrEmpty (file))
            {
                try
                {
                    ProcessFile (file);
                }
                catch (Exception exception)
                {
                    Program.Logger.LogError
                        (
                            exception,
                            "Error during processing file {File}",
                            file
                        );
                }
            }

            // засыпаем на секунду
            Thread.Sleep (1000);
        }
    }

    #endregion

    #region IHostedService members

    public Task StartAsync
        (
            CancellationToken cancellationToken
        )
    {
        var thread = new Thread (MainLoop)
        {
            IsBackground = true,
            Priority = ThreadPriority.BelowNormal
        };
        thread.Start();

        return Task.CompletedTask;
    }

    public Task StopAsync
        (
            CancellationToken cancellationToken
        )
    {
        // TODO implement

        return Task.CompletedTask;
    }

    #endregion
}
