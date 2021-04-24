// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridButtonColumn.cs
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    public class TreeGridButtonColumn
        : TreeGridColumn
    {
        #region Events

        public event EventHandler<EventArgs>? Click;

        public event EventHandler<TreeGridMouseEventArgs>? MouseClick;

        public event EventHandler<TreeGridMouseEventArgs>? MouseDoubleClick;

        #endregion

        #region Construction

        //public TreeGridButtonColumn()
        //{
        //}

        #endregion

        #region TreeGridColumn members

        protected internal override void OnDrawCell
            (
                TreeGridDrawCellEventArgs args
            )
        {
            var graphics = args.Graphics;
            if (graphics is null)
            {
                Magna.Debug("Graphics is null");
                return;
            }

            var node = args.Node;
            if (node is null)
            {
                Magna.Debug("Node is null");
                return;
            }

            var column = args.Column;
            if (column is null)
            {
                Magna.Debug("Column is null");
                return;
            }

            var text = node.Data.SafeGet(column.Index-1) as string;

            graphics.FillRectangle
                (
                    args.GetBackgroundBrush(),
                    args.Bounds
                );

            if (!string.IsNullOrEmpty(text))
            {
                ButtonRenderer.DrawButton
                    (
                        graphics,
                        args.Bounds,
                        text,
                        node.Font,
                        false,
                        PushButtonState.Normal
                    );
            }
        }

        protected internal override void OnMouseClick
            (
                TreeGridMouseEventArgs args
            )
        {

            Click?.Invoke(this, args);
            MouseClick?.Invoke(this, args);
        }

        protected internal override void OnMouseDoubleClick
            (
                TreeGridMouseEventArgs args
            )
        {
            MouseDoubleClick?.Invoke(this, args);
        }

        public override bool Editable => false;

        #endregion
    }
}
