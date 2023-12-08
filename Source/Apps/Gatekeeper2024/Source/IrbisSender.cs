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
    public IrbisSender
        (
            WebApplication application
        )
    {
        _application = application;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Запуск рабочего цикла.
    /// </summary>
    public void StartWorkingLoop()
    {
        var thread = new Thread (WorkingLoop)
        {
            IsBackground = true
        };
        thread.Start();
    }

    #endregion

    #region Private members

    private readonly WebApplication _application;

    /// <summary>
    /// Получение одного (любого) файла, готового к отправке.
    /// </summary>
    private string? GetOneFile() =>
        Directory.EnumerateFiles ("Pool", "*.json")
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

    #endregion
}
