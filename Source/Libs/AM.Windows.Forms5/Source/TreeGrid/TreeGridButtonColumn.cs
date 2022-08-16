// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TreeGridButtonColumn.cs -- колонка, содержащая кнопки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Колонка, содержащая кнопки.
/// </summary>
public class TreeGridButtonColumn
    : TreeGridColumn
{
    #region Events

    /// <summary>
    /// Событие, возникающее при клике по кнопке.
    /// </summary>
    public event EventHandler<EventArgs>? Click;

    /// <summary>
    /// Событие, возникающее при клике мышкой по кнопке.
    /// </summary>
    public event EventHandler<TreeGridMouseEventArgs>? MouseClick;

    /// <summary>
    /// Событие, возникающее при двойном клике мышкой по кнопке.
    /// </summary>
    public event EventHandler<TreeGridMouseEventArgs>? MouseDoubleClick;

    #endregion

    #region TreeGridColumn members

    /// <inheritdoc cref="TreeGridColumn.OnDrawCell"/>
    protected internal override void OnDrawCell
        (
            TreeGridDrawCellEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        var graphics = eventArgs.Graphics;
        if (graphics is null)
        {
            Magna.Logger.LogDebug (nameof (OnDrawCell) + ": graphics is null");
            return;
        }

        var node = eventArgs.Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (OnDrawCell) + ": node is null");
            return;
        }

        var column = eventArgs.Column;
        if (column is null)
        {
            Magna.Logger.LogDebug (nameof (OnDrawCell) + ": column is null");
            return;
        }

        var text = node.Data.SafeGet (column.Index - 1) as string;

        graphics.FillRectangle
            (
                eventArgs.GetBackgroundBrush(),
                eventArgs.Bounds
            );

        if (!string.IsNullOrEmpty (text))
        {
            ButtonRenderer.DrawButton
                (
                    graphics,
                    eventArgs.Bounds,
                    text,
                    node.Font,
                    false,
                    PushButtonState.Normal
                );
        }
    }

    /// <inheritdoc cref="TreeGridColumn.OnMouseClick"/>
    protected internal override void OnMouseClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        Click?.Invoke (this, eventArgs);
        MouseClick?.Invoke (this, eventArgs);
    }

    /// <inheritdoc cref="TreeGridColumn.OnMouseDoubleClick"/>
    protected internal override void OnMouseDoubleClick
        (
            TreeGridMouseEventArgs args
        )
    {
        Sure.NotNull (args);

        MouseDoubleClick?.Invoke (this, args);
    }

    /// <inheritdoc cref="TreeGridColumn.Editable"/>
    public override bool Editable => false;

    #endregion
}
