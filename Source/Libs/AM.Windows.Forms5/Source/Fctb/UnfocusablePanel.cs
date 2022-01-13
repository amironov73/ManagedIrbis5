// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* UnfocusablePanel.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Панель, не принимающая фокуса.
/// </summary>
[System.ComponentModel.ToolboxItem (false)]
public class UnfocusablePanel
    : Control
{
    #region Properties

    /// <summary>
    /// Цвет фона.
    /// </summary>
    public Color BackColor2 { get; set; }

    /// <summary>
    /// Цвет границы.
    /// </summary>
    public Color BorderColor { get; set; }

    /// <summary>
    /// Метка.
    /// </summary>
    public string? Label  { get; set; }

    /// <summary>
    /// Выравнивание метки.
    /// </summary>
    public StringAlignment TextAlignment { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UnfocusablePanel()
    {
        SetStyle (ControlStyles.Selectable, false);
        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint,
                true
            );
    }

    #endregion

    #region Control members

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        using (var brush = new LinearGradientBrush (ClientRectangle, BackColor2, BackColor, 90))
        {
            e.Graphics.FillRectangle (brush, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

        using (var pen = new Pen (BorderColor))
        {
            e.Graphics.DrawRectangle (pen, 0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
        }

        if (!string.IsNullOrEmpty (Label))
        {
            var format = new StringFormat();
            format.Alignment = TextAlignment;
            format.LineAlignment = StringAlignment.Center;
            using var foreBrush = new SolidBrush (ForeColor);
            e.Graphics.DrawString
                (
                    Label,
                    Font,
                    foreBrush,
                    new RectangleF (1, 1, ClientSize.Width - 2, ClientSize.Height - 2),
                    format
                );
        }
    }

    #endregion
}
