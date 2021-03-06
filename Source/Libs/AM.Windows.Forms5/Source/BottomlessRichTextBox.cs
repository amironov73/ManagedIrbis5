﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BottomlessRichEdit.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// RichTextBox that adjusts its height according to height
    /// entered text.
    /// </summary>
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class BottomlessRichTextBox
        : RichTextBox
    {
        #region Construction

        /// <summary>
        /// Default constructor.
        /// </summary>
        public BottomlessRichTextBox()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Multiline = true;
            WordWrap = true;
        }

        #endregion

        #region Protected members

        /// <summary>
        /// Raises the
        /// <see cref="E:System.Windows.Forms.RichTextBox.ContentsResized" />
        /// event.
        /// </summary>
        /// <param name="e">A
        /// <see cref="T:System.Windows.Forms.ContentsResizedEventArgs" />
        /// that contains the event data.</param>
        protected override void OnContentsResized
            (
                ContentsResizedEventArgs e
            )
        {
            base.OnContentsResized(e);
            Size newSize = e.NewRectangle.Size;
            int prevWidth = Width;
            if (newSize.Height < FontHeight)
            {
                newSize.Height = FontHeight;
            }
            if (!MaximumSize.IsEmpty
                && (newSize.Height >= MaximumSize.Height))
            {
                newSize.Height = MaximumSize.Height;
            }
            if (Parent != null)
            {
                int maxHeight = Parent.ClientSize.Height - Location.Y;
                if (newSize.Height >= maxHeight)
                {
                    newSize.Height = maxHeight;
                }
            }
            newSize.Width = ClientSize.Width;
            ClientSize = newSize;
            Width = prevWidth;
        }

        #endregion

        #region TextBoxBase members

        /// <summary>
        /// Gets or sets value indicating whether this
        /// is multiline RichTextBox.
        /// </summary>
        [DefaultValue(true)]
        public override bool Multiline
        {
            get => base.Multiline;
            set => base.Multiline = value;
        }

        #endregion

    } // class BottomlessRichTextBox

} // namespace AM.Windows.Forms
