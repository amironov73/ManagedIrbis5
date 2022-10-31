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
/// Class that provides data for the <see cref="AdditionalVisualStyleElements.TaskDialog.HyperlinkClicked"/> event.
/// </summary>
/// <threadsafety instance="false" static="true" />
public class HyperlinkClickedEventArgs
    : EventArgs
{
    /// <summary>
    /// Creates a new instance of the <see cref="HyperlinkClickedEventArgs"/> class with the specified URL.
    /// </summary>
    /// <param name="href">The URL of the hyperlink.</param>
    public HyperlinkClickedEventArgs
        (
            string href
        )
    {
        Href = href;
    }

    /// <summary>
    /// Gets the URL of the hyperlink that was clicked.
    /// </summary>
    /// <value>
    /// The value of the href attribute of the hyperlink.
    /// </value>
    public string Href { get; }
}
