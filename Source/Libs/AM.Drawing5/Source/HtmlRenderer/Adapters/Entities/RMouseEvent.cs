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
    /// <summary>
    /// Is the left mouse button participated in the event
    /// </summary>
    private readonly bool _leftButton;

    /// <summary>
    /// Init.
    /// </summary>
    public RMouseEvent(bool leftButton)
    {
        _leftButton = leftButton;
    }

    /// <summary>
    /// Is the left mouse button participated in the event
    /// </summary>
    public bool LeftButton
    {
        get { return _leftButton; }
    }
}
