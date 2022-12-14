// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Measures.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Docking;

/// <summary>
///
/// </summary>
public class Measures
{
    /// <summary>
    ///
    /// </summary>
    public int SplitterSize = 4;

    /// <summary>
    ///
    /// </summary>
    public int AutoHideSplitterSize = 4;

    /// <summary>
    ///
    /// </summary>
    public int AutoHideTabLineWidth = 6;

    /// <summary>
    ///
    /// </summary>
    public int DockPadding { get; set; }
}

internal static class MeasurePane
{
    public const int MinSize = 24;
}
