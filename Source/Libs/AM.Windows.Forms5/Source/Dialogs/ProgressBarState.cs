// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// Represents the state of the progress bar on the task dialog.
/// </summary>
public enum ProgressBarState
{
    /// <summary>
    /// Normal state.
    /// </summary>
    Normal,

    /// <summary>
    /// Error state
    /// </summary>
    Error,

    /// <summary>
    /// Paused state
    /// </summary>
    Paused
}