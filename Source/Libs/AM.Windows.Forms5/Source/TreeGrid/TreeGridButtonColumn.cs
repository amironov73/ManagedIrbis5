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
            var text = args.Node.Data.SafeGet(args.Column.Index-1) as string;

            args.Graphics.FillRectangle
                (
                    args.GetBackgroundBrush(),
                    args.Bounds
                );

            if (!string.IsNullOrEmpty(text))
            {
                ButtonRenderer.DrawButton
                    (
                        args.Graphics,
                        args.Bounds,
                        text,
                        args.Node.Font,
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
