﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ListBoxEx.cs -- ListBox with drag-drop support
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// <see cref="ListBox"/> with drag-drop support
    /// </summary>
    [System.ComponentModel.DesignerCategory("Code")]
    public sealed class ListBoxEx
        : ListBox
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ListBoxEx()
        {
            AllowDrop = true;
        }

        #endregion

        #region Control members

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.Control.DragDrop"/>
        /// event.
        /// </summary>
        protected override void OnDragDrop
            (
                DragEventArgs dragEvent
            )
        {
            base.OnDragDrop(dragEvent);
            if (Items.Count == 0)
            {
                return;
            }
            ArrayList sel = new ArrayList(SelectedItems);
            Point pt = PointToClient(new Point(dragEvent.X, dragEvent.Y));
            int index = IndexFromPoint(pt);
            if (index < 0)
            {
                index = Items.Count - 1;
            }
            foreach (object obj in sel)
            {
                Items.Remove(obj);
            }
            foreach (object obj in sel)
            {
                //Items.Add ( obj );
                Items.Insert(index, obj);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.DragEnter"/>
        /// event.
        /// </summary>
        protected override void OnDragEnter
            (
                DragEventArgs dragEvent
            )
        {
            base.OnDragEnter(dragEvent);
            object obj = dragEvent.Data.GetData(typeof(ListBoxEx));
            if (obj == this)
            {
                dragEvent.Effect = DragDropEffects.Move;
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/>
        /// event.
        /// </summary>
        /// <param name="e">A <see cref="MouseEventArgs"/>
        /// that contains the event data.</param>
        protected override void OnMouseDown
            (
                MouseEventArgs e
            )
        {
            base.OnMouseDown(e);
            DoDragDrop(this, DragDropEffects.Move);
        }

        #endregion
    }
}
