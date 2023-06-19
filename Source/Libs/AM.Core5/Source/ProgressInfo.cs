// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ProgressInfo.cs -- информация о прогрессе длительной операции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Информация о прогрессе длительной операции.
/// </summary>
[PublicAPI]
public sealed class ProgressInfo<TValue>
    where TValue: struct
{
    #region Properties

    /// <summary>
    /// Момент, когда операция была начата.
    /// </summary>
    public DateTime StartedAt { get; set; }

    /// <summary>
    /// Вызвано первый раз?
    /// </summary>
    public bool FirstTime { get; set; }

    /// <summary>
    /// Заголовок задачи.
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Общее количество элементов, подлежащих обработке
    /// (или байт, подлежащих скачиванию).
    /// </summary>
    public TValue Total { get; set; }

    /// <summary>
    /// Количество успешно выполненных элементов
    /// (или успешно скачанных байт).
    /// </summary>
    public TValue Done { get; set; }

    /// <summary>
    /// Количество ошибок.
    /// </summary>
    public TValue Errors { get; set; }

    /// <summary>
    /// Количество пропущенных элементов.
    /// </summary>
    public TValue Skipped { get; set; }

    /// <summary>
    /// Последняя ошибка (если есть).
    /// </summary>
    public Exception? LastError { get; set; }

    /// <summary>
    /// Произвольная пользовательская информация.
    /// </summary>
    public object? ExtraInfo { get; set; }

    #endregion
}
