// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms.Dialogs;

/// <summary>
/// Provides data for the <see cref="TaskDialog.ExpandButtonClicked"/> event.
/// </summary>
/// <threadsafety instance="false" static="true" />
public class ExpandButtonClickedEventArgs : EventArgs
{
    private bool _expanded;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExpandButtonClickedEventArgs"/> class with the specified expanded state.
    /// </summary>
    /// <param name="expanded"><see langword="true" /> if the the expanded content on the dialog is shown; otherwise, <see langword="false" />.</param>
    public ExpandButtonClickedEventArgs (bool expanded)
    {
        _expanded = expanded;
    }

    /// <summary>
    /// Gets a value that indicates if the expanded content on the dialog is shown.
    /// </summary>
    /// <value><see langword="true" /> if the expanded content on the dialog is shown; otherwise, <see langword="false" />.</value>
    public bool Expanded
    {
        get { return _expanded; }
    }
}