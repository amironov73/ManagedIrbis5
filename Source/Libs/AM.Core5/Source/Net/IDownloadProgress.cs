// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* IDownloadProgress.cs -- интерфейс для отслеживания прогресса скачивания
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Интерфейс для отслеживания прогресса скачивания.
/// </summary>
[PublicAPI]
public interface IDownloadProgress
    : IProgress<DownloadStat>
{
    /// <summary>
    /// Вызвается в начале скачивания, сразу после получения заголовков.
    /// </summary>
    void OnDownloadBegin (DownloadStat stat);

    /// <summary>
    /// Вызывается в конце скачивания.
    /// </summary>
    void OnDownloadEnd (DownloadStat stat);

    /// <summary>
    /// Вызывается при возникновении ошибки.
    /// </summary>
    void OnDownloadError (DownloadStat stat, Exception exception);
}
