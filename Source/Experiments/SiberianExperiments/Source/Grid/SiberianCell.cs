// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianCell.cs -- дефолтная реализация ячейки грида
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.WinForms.Grid;

/// <summary>
/// Дефолтная реализация ячейки грида.
/// </summary>
public class SiberianCell
    : ISiberianCell
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SiberianCell
        (
            ISiberianGrid grid,
            ISiberianColumn column,
            ISiberianRow row,
            object? data
        )
    {
        Grid = grid;
        Column = column;
        Row = row;
        Data = data;
    }

    #endregion

    #region ISiberianCell members

    /// <inheritdoc cref="ISiberianCell.Column"/>
    public ISiberianColumn Column { get; }

    /// <inheritdoc cref="ISiberianCell.Data"/>
    public object? Data { get; set; }

    /// <inheritdoc cref="ISiberianCell.Grid"/>
    public ISiberianGrid Grid { get; }

    /// <inheritdoc cref="ISiberianCell.Row"/>
    public ISiberianRow Row { get; }

    #endregion
}
