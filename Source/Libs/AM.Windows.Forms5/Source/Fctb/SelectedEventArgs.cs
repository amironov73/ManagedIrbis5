// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SelectedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
///
/// </summary>
public sealed class SelectedEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    ///
    /// </summary>
    public AutocompleteItem? Item { get; internal set; }

    /// <summary>
    ///
    /// </summary>
    public SyntaxTextBox? Tb { get; set; }

    #endregion
}
