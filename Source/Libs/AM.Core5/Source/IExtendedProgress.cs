// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* IExtendedProgress.cs -- расширенный интерфейс прогресса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM;

/// <summary>
/// Расширенный интерфейс прогресса длительного процесса.
/// </summary>
public interface IExtendedProgress<in T>
    : IProgress<T>
{
    /// <summary>
    /// Установка ожидаемого максимального значения.
    /// </summary>
    void SetMaximum (T maximum);

    /// <summary>
    /// Сообщение о продвижении прогресса.
    /// </summary>
    void ExtendedReport (T value, string? message);

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    void ReportError (T value, string? message);
}
