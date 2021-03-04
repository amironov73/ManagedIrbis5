// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ColorComboBox.cs -- выпадающий список цветов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms
{
    /// <summary>
    /// Выпадающий список, позволяющий выбрать цвет.
    /// </summary>
    [ToolboxBitmap(typeof(ColorComboBox), "Images.ColorComboBox.bmp")]
    [System.ComponentModel.DesignerCategory("Code")]
    public class ColorComboBox
        : ComboBox
    {
        #region Properties

        ///<summary>
        /// Выбранный цвет.
        ///</summary>
        public Color SelectedColor
        {
            [DebuggerStepThrough]
            get
            {
                if (SelectedIndex < 0)
                {
                    return Color.Black;
                }
                return (Color)Items[SelectedIndex];
            }
            [DebuggerStepThrough]
            set
            {
                if (!Items.Contains(value))
                {
                    var index = Items.Add(value);
                    SelectedIndex = index;
                }
                SelectedIndex = Items.IndexOf(value);
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:ColorComboBox"/> class.
        /// </summary>
        public ColorComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
            Items.AddRange(new object[]
                {
                    Color.Black,
                    Color.White,
                    Color.Red,
                    Color.Green,
                    Color.Blue,
                    Color.DarkGray,
                    Color.Gray,
                    Color.Cyan,
                    Color.Magenta,
                    Color.DarkRed,
                    Color.DarkGreen,
                    Color.DarkBlue,
                    Color.Brown
                });
        }

        #endregion

        #region Private members

        private void _DrawItem
            (
                object sender,
                DrawItemEventArgs e
            )
        {
            Graphics g = e.Graphics;
            var r = e.Bounds;
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index >= 0)
            {
                r.Inflate(-4, -2);
                using (Brush brush
                    = new SolidBrush((Color)Items[e.Index]))
                {
                    g.FillRectangle(brush, r);
                }
                g.DrawRectangle(Pens.Black, r);
            }
        }

        #endregion

    } // class ColorComboBox

} // namespace AM.Windows.Forms
