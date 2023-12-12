// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* IrbisSender.cs -- отправляет данные на сервер ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

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

    private void ProcessFile
        (
            string path
        )
    {
        // TODO реализовать
    }

    /// <summary>
    /// Рабочий цикл.
    /// </summary>
    private void WorkingLoop()
    {
        while (true)
        {
            var file = GetOneFile();
            if (!string.IsNullOrEmpty (file))
            {
                ProcessFile (file);
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
