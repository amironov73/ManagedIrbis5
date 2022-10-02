// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianClickEventArgs.cs -- аргументы события щелчка по гриду
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Аргументы события щелчка по гриду.
/// </summary>
public sealed class SiberianClickEventArgs
    : CancelableEventArgs
{
    #region Properties

    /// <summary>
    /// Ячейка, в которой зафиксирован щелчок (опционально).
    /// </summary>
    public ISiberianCell? Cell { get; internal set; }

    /// <summary>
    /// Колонка, в которой зафиксирован щелчок (опционально).
    /// </summary>
    public ISiberianColumn? Column { get; internal set; }

    /// <summary>
    /// Грид, в котором зафиксирован щелчок (обязательно).
    /// </summary>
    public ISiberianGrid? Grid { get; internal set; }

    /// <summary>
    /// Строка, в которой зафиксирован щелчок (опционально).
    /// </summary>
    public ISiberianRow? Row { get; internal set; }

    #endregion
}
