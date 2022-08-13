// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* VirtualData.cs -- порция данных для виртуального грида
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Порция данных для виртуального грида.
/// </summary>
public class VirtualData
{
    #region Properties

    /// <summary>
    /// Номер первой строчки.
    /// </summary>
    public int FirstLine { get; set; }

    /// <summary>
    /// Количество строк.
    /// </summary>
    public int LineCount { get; set; }

    /// <summary>
    /// Общее количество строк.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Строки с данными.
    /// </summary>
    public object?[]? Lines { get; set; }

    #endregion
}
