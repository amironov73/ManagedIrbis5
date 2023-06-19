// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement

/* ConsoleProgressReporter.cs -- рапортует прогресс
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.Net;

#endregion

#nullable enable

namespace CivitGet;

/// <summary>
/// Рапортует прогресс на консоль.
/// </summary>
internal sealed class ConsoleProgressReporter
    : IDownloadProgress,
    IProgress<ProgressInfo<int>>
{
    #region Public methods

    public void OnDownloadBegin
        (
            DownloadStat stat
        )
    {
        Console.WriteLine ($"Begin download: {stat.RequestUri}");
        Console.WriteLine ($"Content length: {stat.ContentLength}");
    }

    public void Report
        (
            DownloadStat value
        )
    {
        Console.Write ($"\r{value.Downloaded} of {value.ContentLength}");
    }

    public void OnDownloadEnd
        (
            DownloadStat stat
        )
    {
        Console.WriteLine();
        Console.WriteLine ("Download end");
    }

    public void OnDownloadError
        (
            DownloadStat stat,
            Exception exception
        )
    {
        Console.WriteLine();
        Console.WriteLine ($"ERROR: {exception.Message}");
    }

    public void Report (ProgressInfo<int> value)
    {
        Console.WriteLine ($"\rDone {value.Done} of {value.Total}");
    }

    #endregion
}
