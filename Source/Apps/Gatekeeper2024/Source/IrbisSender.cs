// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisSender.cs -- отправляет данные на сервер ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Gatekeeper2024;

/// <summary>
/// Отправляет данные на сервер ИРБИС64.
/// </summary>
internal sealed class IrbisSender
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IrbisSender()
    {
        _queueDirectory = Path.Combine
            (
                AppContext.BaseDirectory,
                "Queue"
            );
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

    private void DeleteFile
        (
            string path
        )
    {
        try
        {
            File.Delete (path);
        }
        catch (Exception exception)
        {
            GlobalState.Logger.LogError (exception, "Can't delete file {Path}", path);
        }

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

        // при успешном окончании удаляем файл
        DeleteFile (path);
    }

    // TODO реализовать
    private bool VerifyEvent (PassEvent passEvent) => true;

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
        var content = File.ReadAllText (path);
        var passEvent = JsonSerializer.Deserialize<PassEvent> (content);
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
    /// Рабочий цикл.
    /// </summary>
    [DoesNotReturn]
    private void WorkingLoop()
    {
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

            // засыпаем на одну секунду
            Thread.Sleep (TimeSpan.FromSeconds (1));
        }
    }

    private void _CheckIrbisConnection()
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

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка подключения к серверу ИРБИС64.
    /// </summary>
    public void CheckIrbisConnection()
    {
        new Thread(_CheckIrbisConnection)
        {
            IsBackground = true
        }
        .Start();
    }

    /// <summary>
    /// Запуск рабочего цикла.
    /// </summary>
    public void StartWorkingLoop()
    {
        Directory.CreateDirectory (_queueDirectory);

        var thread = new Thread (WorkingLoop)
        {
            IsBackground = true
        };
        thread.Start();
    }

    #endregion
}
