// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* AreaDefinition.cs -- описание области грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace GridExtra.Avalonia;

/// <summary>
/// Описание области грида.
/// </summary>
public class AreaDefinition
{
    #region Properties

    /// <summary>
    /// Номер строки.
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Номер колонки.
    /// </summary>
    public int Column { get; set; }

    /// <summary>
    /// Количество дополнительных строк.
    /// </summary>
    public int RowSpan { get; set; }

    /// <summary>
    /// Количество дополнительных колонок.
    /// </summary>
    public int ColumnSpan { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AreaDefinition
        (
            int row,
            int column,
            int rowSpan,
            int columnSpan
        )
    {
        Sure.NonNegative (row);
        Sure.NonNegative (column);
        Sure.NonNegative (rowSpan);
        Sure.NonNegative (columnSpan);

        Row = row;
        Column = column;
        RowSpan = rowSpan;
        ColumnSpan = columnSpan;
    }

    #endregion
}
