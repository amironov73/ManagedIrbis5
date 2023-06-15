// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DownloadStat.cs -- сведения о скачивании из сети
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Сведения о скачивании из сети
/// </summary>
[PublicAPI]
public sealed class DownloadStat
{
    #region Properties

    /// <summary>
    /// URI запрошенного ресурса.
    /// </summary>
    public Uri? RequestUri { get; init; }

    /// <summary>
    /// Длина контента (доступна не всегда).
    /// </summary>
    public long? ContentLength { get; init; }

    /// <summary>
    /// Количество успешно скачанных байт.
    /// </summary>
    public long Downloaded { get; set; }

    #endregion
}
