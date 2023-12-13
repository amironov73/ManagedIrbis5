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
    public EventUploader()
    {
        _queueDirectory = Utility.GetQueueDirectory();
    }

    #endregion

    #region Private members

    private readonly string _queueDirectory;

    /// <summary>
    /// Получение одного (любого) файла, готового к отправке.
    /// </summary>
    private string? GetOneFile() =>
        Directory.EnumerateFiles (_queueDirectory, "*.json")
            .FirstOrDefault();

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
            GlobalState.Logger.LogInformation ("Delete file {Path}", path);
        }
        catch (Exception exception)
        {
            GlobalState.Logger.LogError (exception, "Can't delete file {Path}", path);
        }
    }

    private ReaderInfo? GetReader
        (
            ISyncProvider connection,
            string readerId,
            string path
        )
    {
        var readers = Utility.SearchForReader (readerId);
        if (readers is null)
        {
            // не удалось связаться с сервером
            // не удаляем файл, вдруг удастся связаться в будущем
            return null;
        }

        if (readers.Length == 0)
        {
            GlobalState.Logger.LogError ("No reader with ticket {Ticket}", readerId);
            DeleteFile (path);
            return null;
        }

        if (readers.Length != 1)
        {
            GlobalState.Logger.LogError ("Many readers with ticket {Ticket}", readerId);
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
        // TODO реализовать

        var readerId = passEvent.Id;
        if (string.IsNullOrEmpty (readerId))
        {
            GlobalState.Logger.LogError ("Empty reader ID in file {Path}", path);
            DeleteFile (path);
            return;
        }

        using var connection = Utility.ConnectToIrbis();
        if (connection is null)
        {
            return;
        }

        var reader = GetReader (connection, readerId, path);
        if (reader is null)
        {
            // файл не удаляем, это делает GetReader при необходимости
            return;
        }

        var record = reader.Record;
        if (record is null)
        {
            GlobalState.Logger.LogError ("Strange thing: reader.Record is null: {Event}", passEvent);
            DeleteFile (path);
            return;
        }

        var eventData = Utility.GetArrivalField (passEvent.Moment);
        record.Add (40, eventData);

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
            GlobalState.Logger.LogInformation
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
        var result = passEvent.Type is 1 or 2
               && !string.IsNullOrEmpty (passEvent.Id);

        if (!result)
        {
            GlobalState.Logger.LogError ("Bad PassEvent: {Event}", passEvent);
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
        // TODO реализовать

        // при успешном окончании удаляем файл
        DeleteFile (path);
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
            GlobalState.Logger.LogError ("Bad event file {Path}", path);
            DeleteFile (path);
            return;
        }

        switch (passEvent.Type)
        {
            case 1:
                ProcessArrival (path, passEvent);
                break;

            case 2:
                ProcessDeparture (path, passEvent);
                break;

            default:
                GlobalState.Logger.LogError ("Bad event file {Path}", path);
                break;
        }
    }

    /// <summary>
    /// Проверка подключения к серверу ИРБИС64.
    /// </summary>
    private void CheckIrbisConnection()
    {
        try
        {
            var connection = Utility.ConnectToIrbis();

            if (connection is null)
            {
                GlobalState.Instance.HasError = true;
                GlobalState.SetMessageWithTimestamp ("Тестовое подключение к серверу ИРБИС64: ОШИБКА");
            }
            else
            {
                GlobalState.Instance.HasError = false;
                GlobalState.SetMessageWithTimestamp ("Тестовое подключение к серверу ИРБИС64 выполнено успешно");
                connection.Dispose();
            }
        }
        catch (Exception exception)
        {
            GlobalState.Logger.LogError (exception, "Error during CheckIrbisConnection");
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
            GlobalState.Logger.LogError
                (
                    exception,
                    "Can't create queue directory {Directory}", _queueDirectory
                );
            return false;
        }

        return true;
    }

    private void MainLoop ()
    {
        if (!CreateQueueDirectory())
        {
            return;
        }

        CheckIrbisConnection();

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
                    GlobalState.Logger.LogError
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
