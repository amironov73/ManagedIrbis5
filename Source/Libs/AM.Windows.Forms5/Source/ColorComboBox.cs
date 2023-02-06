// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ColorComboBox.cs -- выпадающий список цветов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Выпадающий список, позволяющий выбрать цвет.
/// </summary>
[ToolboxBitmap (typeof (ColorComboBox), "Images.ColorComboBox.bmp")]
[System.ComponentModel.DesignerCategory ("Code")]
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

            return (Color) Items[SelectedIndex];
        }
        [DebuggerStepThrough]
        set
        {
            if (!Items.Contains (value))
            {
                var index = Items.Add (value);
                SelectedIndex = index;
            }

            SelectedIndex = Items.IndexOf (value);
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор без параметров.
    /// </summary>
    public ColorComboBox()
    {
        DropDownStyle = ComboBoxStyle.DropDownList;
        DrawMode = DrawMode.OwnerDrawFixed;
        DrawItem += _DrawItem;
        Items.AddRange (new object[]
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
            object? sender,
            DrawItemEventArgs eventArgs
        )
    {
        var graphics = eventArgs.Graphics;
        var r = eventArgs.Bounds;
        eventArgs.DrawBackground();
        eventArgs.DrawFocusRectangle();
        if (eventArgs.Index >= 0)
        {
            r.Inflate (-4, -2);
            using (Brush brush = new SolidBrush ((Color) Items[eventArgs.Index]))
            {
                graphics.FillRectangle (brush, r);
            }

            graphics.DrawRectangle (Pens.Black, r);
        }
    }

    #endregion
}
