// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HatchComboBox.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    ///
    /// </summary>
    [ToolboxBitmap (typeof(HatchComboBox), "Images.HatchComboBox.bmp")]
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class HatchComboBox
        : ComboBox
    {
        #region Properties

        /// <summary>
        /// Gets or sets the style.
        /// </summary>
        /// <value>The style.</value>
        [DefaultValue(0)]
        public HatchStyle Style
        {
            get
            {
                if (SelectedItem == null)
                {
                    return 0;
                }

                return (HatchStyle)SelectedItem;
            }
            set
            {
                var index = Items.IndexOf(value);
                SelectedIndex = index;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:HatchComboBox"/> class.
        /// </summary>
        public HatchComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (HatchStyle style in Enum.GetValues(typeof(HatchStyle)))
            {
                Items.Add(style);
            }
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
        }

        #endregion

        #region Private members

        private void _DrawItem
            (
                object? sender,
                DrawItemEventArgs e
            )
        {
            e.DrawBackground();
            if (e.Index >= 0)
            {
                var style = (HatchStyle)Items[e.Index];
                using var brush = new HatchBrush(style, e.ForeColor, e.BackColor);
                e.Graphics.FillRectangle(brush, e.Bounds);
            }
            e.DrawFocusRectangle();
        }

        #endregion
    }
}
