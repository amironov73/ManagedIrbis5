// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TableLocation.cs -- хранит информацию о размещении контрола в таблице
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Хранит информацию о размещении контрола <see cref="Control"/>
/// в таблице <see cref="TableLayoutPanel"/>.
/// </summary>
public sealed class TableLocation
{
    #region Properties

    /// <summary>
    /// Номер колонки.
    /// </summary>
    public int Column { get; }

    /// <summary>
    /// Номер строки.
    /// </summary>
    public int Row { get; }

    /// <summary>
    /// Количество занимаемых колонок.
    /// </summary>
    public int ColumnSpan { get; }

    /// <summary>
    /// Количество занимаемых строк.
    /// </summary>
    public int RowSpan { get; }

    /// <summary>
    /// Размещенный в ячейке контрол.
    /// </summary>
    public Control Control { get; }

    /// <summary>
    /// Занимает более одной ячейки?
    /// </summary>
    public bool HasSpans { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TableLocation
        (
            Control control,
            int column,
            int row = 1,
            int columnSpan = 1,
            int rowSpan = 1
        )
    {
        Sure.NotNull (control);
        Sure.Positive (column);
        Sure.Positive (row);
        Sure.Positive (columnSpan);
        Sure.Positive (rowSpan);

        Column = column;
        Row = row;
        ColumnSpan = columnSpan;
        RowSpan = rowSpan;
        Control = control;
        HasSpans = RowSpan > 1 || ColumnSpan > 1;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return $"[{Row}, {Column}] {Control}";
    }

    #endregion
}
