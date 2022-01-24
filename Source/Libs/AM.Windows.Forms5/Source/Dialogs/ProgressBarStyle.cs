// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// Indicates the type of progress on a task dialog.
/// </summary>
public enum ProgressBarStyle
{
    /// <summary>
    /// No progress bar is displayed on the dialog.
    /// </summary>
    None,

    /// <summary>
    /// A regular progress bar is displayed on the dialog.
    /// </summary>
    ProgressBar,

    /// <summary>
    /// A marquee progress bar is displayed on the dialog. Use this value for operations
    /// that cannot report concrete progress information.
    /// </summary>
    MarqueeProgressBar
}