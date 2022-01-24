// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// Represents the type of a task dialog button.
/// </summary>
[SuppressMessage ("Microsoft.Design", "CA1027:MarkEnumsWithFlags")]
public enum ButtonType
{
    /// <summary>
    /// The button is a custom button.
    /// </summary>
    Custom = 0,

    /// <summary>
    /// The button is the common OK button.
    /// </summary>
    Ok = 1,

    /// <summary>
    /// The button is the common Yes button.
    /// </summary>
    Yes = 6,

    /// <summary>
    /// The button is the common No button.
    /// </summary>
    No = 7,

    /// <summary>
    /// The button is the common Cancel button.
    /// </summary>
    Cancel = 2,

    /// <summary>
    /// The button is the common Retry button.
    /// </summary>
    Retry = 4,

    /// <summary>
    /// The button is the common Close button.
    /// </summary>
    Close = 8
}