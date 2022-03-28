// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TaskContext.cs -- контекст одной задачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using ManagedIrbis;

#endregion

#nullable enable

namespace Minigun.Source;

/// <summary>
/// Контекст одной задачи.
/// </summary>
internal sealed class TaskContext
{
    #region Properties

    /// <summary>
    /// Идентификатор задачи.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Подключение, используемое в данной задаче.
    /// </summary>
    public SyncConnection? Connection { get; set; }

    /// <summary>
    /// Количество выполненных операций.
    /// </summary>
    public int Counter { get; set; }

    /// <summary>
    /// Общая успешность выполнения операций.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Исключение (если было).
    /// </summary>
    public Exception? Exception { get; set; }

    #endregion
}
