// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FontComboBox.cs -- выпадающий список шрифтов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms
{
    /// <summary>
    /// Комбинированный список, позволяющий выбрать
    /// шрифт из списка.
    /// </summary>
    [ToolboxBitmap(typeof(FontComboBox), "Images.FontComboBox.bmp")]
    // ReSharper disable RedundantNameQualifier
    [System.ComponentModel.DesignerCategory("Code")]
    // ReSharper restore RedundantNameQualifier
    public class FontComboBox
        : ComboBox
    {
        #region Properties

        ///<summary>
        /// Имя выбранного шрифта.
        ///</summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string? SelectedFontName
        {
            [DebuggerStepThrough]
            get
            {
                var selected = SelectedItem;
                if (selected is null)
                {
                    return null;
                }

                var font = (FontFamily) selected;

                return font.Name;
            }
            [DebuggerStepThrough]
            set
            {
                if (value is not null)
                {
                    for (var i = 0; i < Items.Count; i++)
                    {
                        if (((FontFamily)Items[i]).Name == value)
                        {
                            SelectedIndex = i;
                            return;
                        }
                    }
                    //throw new ArgumentException ( "No such font: " + value );
                }
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:FontComboBox"/> class.
        /// </summary>
        public FontComboBox()
        {
            DropDownStyle = ComboBoxStyle.DropDownList;
            DrawMode = DrawMode.OwnerDrawFixed;
            DrawItem += _DrawItem;
            var families = FontFamily.Families;
            foreach (var family in families)
            {
                Items.Add(family);
            }
        }

        #endregion

        #region Private members

        private void _DrawItem
            (
                object? sender,
                DrawItemEventArgs e
            )
        {
            var g = e.Graphics;
            e.DrawBackground();
            e.DrawFocusRectangle();
            if (e.Index >= 0)
            {
                var family = (FontFamily)Items[e.Index];
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center
                };
                var r = e.Bounds;
                r.Inflate(-4, 0);
                var height = (r.Height * 7) / 10;
                var style = FontStyle.Regular;

                if (!family.IsStyleAvailable(style))
                {
                    style = FontStyle.Bold;
                }

                if (!family.IsStyleAvailable(style))
                {
                    style = FontStyle.Italic;
                }

                if (!family.IsStyleAvailable(style))
                {
                    family = FontFamily.GenericSerif;
                    style = FontStyle.Regular;
                }

                var brush = (e.State & DrawItemState.Selected) != 0
                    ? SystemBrushes.HighlightText
                    : SystemBrushes.WindowText;

                using var font = new Font(family, height, style);
                g.DrawString(family.Name, font, brush, r, format);
            }
        }

        #endregion

        #region ComboBox members

        /// <summary>
        /// Gets an object representing the collection of the items contained in this <see cref="T:System.Windows.Forms.ComboBox"></see>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public new ObjectCollection Items => base.Items;

        #endregion

    } // class FontComboBox

} // namespace AM.Windows.Forms
