// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridTreeColumn.cs -- колонка, отрисовывающая собственно дерево
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Колонка, отрисовывающая собственно дерево.
/// </summary>
public class TreeGridTreeColumn
    : TreeGridColumn
{
    #region TreeGridColumn members

    /// <inheritdoc cref="TreeGridColumn.Editable"/>
    public override bool Editable => false;

    /// <inheritdoc cref="TreeGridColumn.OnMouseClick"/>
    protected internal override void OnMouseClick
        (
            TreeGridMouseEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        //base.OnMouseClick(args);
        var node = eventArgs.Node;
        if (node != null)
        {
            var layout = MakeLayout (node, node.Bounds);
            var clickKind = layout.DetermineClickKind (eventArgs.Location);
            switch (clickKind)
            {
                case TreeGridClickKind.Expand:
                    node.Expanded = !node.Expanded;
                    break;

                case TreeGridClickKind.Check:
                    node.Checked = !node.Checked;
                    break;

                case TreeGridClickKind.Icon:
                    // Do not know what to do
                    break;

                case TreeGridClickKind.Text:
                    if (node.Checkable)
                    {
                        node.Checked = !node.Checked;
                    }
                    else
                    {
                        node.Expanded = !node.Expanded;
                    }

                    break;
            }
        }
    }

    /// <summary>
    /// Создание лэйаута.
    /// </summary>
    public virtual TreeGridDrawLayout MakeLayout
        (
            TreeGridNode node,
            Rectangle bounds
        )
    {
        Sure.NotNull (node);

        var result = new TreeGridDrawLayout();

        //TreeGridNode node = args.Node;
        //Rectangle bounds = args.Bounds;
        var left = 0;

        left += node.Level * 16;

        if (node.HasChildren)
        {
            result.Expand = new Rectangle
                (
                    left,
                    bounds.Top,
                    16,
                    bounds.Height
                );
            left += result.Expand.Width;
        }

        if (node.Checkable)
        {
            result.Check = new Rectangle
                (
                    left,
                    bounds.Top,
                    16,
                    bounds.Height
                );

            left += result.Check.Width;
        }

        if (node.Icon != null)
        {
            result.Icon = new Rectangle
                (
                    left,
                    bounds.Top,
                    node.Icon.Width + 2,
                    bounds.Height
                );
            left += result.Icon.Width;
        }

        result.Text = new Rectangle
            (
                left,
                bounds.Top,
                bounds.Width - left,
                bounds.Height
            );

        return result;
    }

    /// <inheritdoc cref="TreeGridColumn.OnDrawCell"/>
    protected internal override void OnDrawCell
        (
            TreeGridDrawCellEventArgs eventArgs
        )
    {
        Sure.NotNull (eventArgs);

        var node = eventArgs.Node;
        if (node is null)
        {
            Magna.Logger.LogDebug (nameof (OnDrawCell) + ": node is null");
            return;
        }

        var bounds = eventArgs.Bounds;
        var layout = MakeLayout (node, bounds);
        TreeGridUtilities.DrawTreeCell
            (
                eventArgs,
                layout
            );
    }

    #endregion
}
