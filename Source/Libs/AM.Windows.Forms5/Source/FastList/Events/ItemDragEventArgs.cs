// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ItemDragEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public sealed class ItemDragEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Индексы элементов.
    /// </summary>
    public HashSet<int> ItemIndex;

    #endregion
}
