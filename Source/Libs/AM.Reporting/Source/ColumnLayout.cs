// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* ColumnLayout.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting;

/// <summary>
/// The layout of the data band columns.
/// </summary>
public enum ColumnLayout
{
    /// <summary>
    /// Print columns across then down.
    /// </summary>
    AcrossThenDown,

    /// <summary>
    /// Print columns down then across.
    /// </summary>
    DownThenAcross
}
