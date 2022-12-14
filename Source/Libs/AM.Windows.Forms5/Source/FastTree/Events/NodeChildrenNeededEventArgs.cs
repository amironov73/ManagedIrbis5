// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* NodeChildrenNeededEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public class NodeChildrenNeededEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Узел.
    /// </summary>
    public object? Node { get; internal set; }

    /// <summary>
    /// Потомки.
    /// </summary>
    public IEnumerable? Children { get; set; }

    #endregion
}
