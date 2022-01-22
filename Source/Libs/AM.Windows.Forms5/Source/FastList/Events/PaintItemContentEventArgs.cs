// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PaintItemContentEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
///
/// </summary>
public sealed class PaintItemContentEventArgs
    : EventArgs
{
    #region Properties

    /// <summary>
    /// Канва для отрисовки.
    /// </summary>
    public Graphics Graphics;

    /// <summary>
    /// Информация об элементе.
    /// </summary>
    public FastListBase.VisibleItemInfo Info;

    #endregion
}
