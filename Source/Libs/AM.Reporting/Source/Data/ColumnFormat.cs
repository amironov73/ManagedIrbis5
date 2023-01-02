// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ColumnFormat.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Data;

/// <summary>
/// Specifies the format for the column value.
/// </summary>
public enum ColumnFormat
{
    /// <summary>
    /// The format will be determined automatically depending on the column's DataType.
    /// </summary>
    Auto,

    /// <summary>
    /// Specifies the General format (no formatting).
    /// </summary>
    General,

    /// <summary>
    /// Specifies the Number format.
    /// </summary>
    Number,

    /// <summary>
    /// Specifies the Currency format.
    /// </summary>
    Currency,

    /// <summary>
    /// Specifies the Date format.
    /// </summary>
    Date,

    /// <summary>
    /// Specifies the Time format.
    /// </summary>
    Time,

    /// <summary>
    /// Specifies the Percent format.
    /// </summary>
    Percent,

    /// <summary>
    /// Specifies the Boolean format.
    /// </summary>
    Boolean
}
