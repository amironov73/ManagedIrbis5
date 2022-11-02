// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* RMouseEvent.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Core;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.Adapters.Entities;

/// <summary>
/// Even class for handling keyboard events in <see cref="HtmlContainerInt"/>.
/// </summary>
public sealed class RMouseEvent
{
    #region Properties

    /// <summary>
    /// Is the left mouse button participated in the event
    /// </summary>
    public bool LeftButton { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RMouseEvent (bool leftButton)
    {
        LeftButton = leftButton;
    }

    #endregion
}
