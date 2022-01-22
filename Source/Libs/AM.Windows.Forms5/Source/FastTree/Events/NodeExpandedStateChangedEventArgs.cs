// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NodeExpandedStateChangedEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public sealed class NodeExpandedStateChangedEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Узел.
    /// </summary>
    public object Node;

    /// <summary>
    /// Состояние: узер развернут?
    /// </summary>
    public bool Expanded;

    #endregion
}
