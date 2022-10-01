// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable EventNeverSubscribedTo.Global
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ISiberianCell.cs -- интерфейс ячейки грида
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Интерфейс ячейки грида.
/// </summary>
public interface ISiberianCell
{
    #region Properties

    /// <summary>
    /// Колонка, в которой находится ячейка.
    /// </summary>
    ISiberianColumn Column { get; }

    /// <summary>
    /// Данные, отображаемые в ячейке (опционально).
    /// </summary>
    object? Data { get; set; }

    /// <summary>
    /// Грид, которому принадлежит ячейка.
    /// </summary>
    ISiberianGrid Grid { get; }

    /// <summary>
    /// Строка, которой принадлежит ячейка.
    /// </summary>
    ISiberianRow Row { get; }

    #endregion
}
