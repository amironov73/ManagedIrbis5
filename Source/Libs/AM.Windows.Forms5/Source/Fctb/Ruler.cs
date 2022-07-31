// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* Ruler.cs -- линейка
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

namespace Fctb;

/// <summary>
/// Линейка.
/// </summary>
public partial class Ruler
    : UserControl
{
    #region Events

    /// <summary>
    /// Цель поменялась.
    /// </summary>
    public event EventHandler? TargetChanged;

    #endregion

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (typeof (Color), "ControlLight")]
    public Color BackColor2 { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (typeof (Color), "DarkGray")]
    public Color TickColor { get; set; }

    /// <summary>
    ///
    /// </summary>
    [DefaultValue (typeof (Color), "Black")]
    public Color CaretTickColor { get; set; }

    private SyntaxTextBox? _target;

    /// <summary>
    ///
    /// </summary>
    [Description ("Target FastColoredTextBox")]
    public SyntaxTextBox? Target
    {
        get => _target;
        set
        {
            if (_target != null)
            {
                UnSubscribe (_target);
            }

            _target = value;
            Subscribe (_target);
            OnTargetChanged();
        }
    }

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Ruler()
    {
        InitializeComponent();

        SetStyle
            (
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint,
                true
            );
        MinimumSize = new Size (0, 24);
        MaximumSize = new Size (int.MaxValue / 2, 24);

        BackColor2 = SystemColors.ControlLight;
        TickColor = Color.DarkGray;
        CaretTickColor = Color.Black;
    }

    #endregion

    #region Control members

    /// <summary>
    ///
    /// </summary>
    protected virtual void OnTargetChanged()
    {
        TargetChanged?.Invoke (this, EventArgs.Empty);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    protected virtual void UnSubscribe
        (
            SyntaxTextBox target
        )
    {
        target.Scroll -= target_Scroll;
        target.SelectionChanged -= target_SelectionChanged;
        target.VisibleRangeChanged -= target_VisibleRangeChanged;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="target"></param>
    protected virtual void Subscribe
        (
            SyntaxTextBox target
        )
    {
        target.Scroll += target_Scroll;
        target.SelectionChanged += target_SelectionChanged;
        target.VisibleRangeChanged += target_VisibleRangeChanged;
    }

    private void target_VisibleRangeChanged
        (
            object? sender,
            EventArgs e
        )
    {
        Invalidate();
    }

    private void target_SelectionChanged
        (
            object? sender,
            EventArgs e
        )
    {
        Invalidate();
    }

    /// <summary>
    ///
    /// </summary>
    protected virtual void target_Scroll
        (
            object? sender,
            ScrollEventArgs e
        )
    {
        Invalidate();
    }

    /// <inheritdoc cref="Control.OnResize"/>
    protected override void OnResize
        (
            EventArgs e
        )
    {
        base.OnResize (e);
        Invalidate();
    }

    /// <inheritdoc cref="Control.OnPaint"/>
    protected override void OnPaint
        (
            PaintEventArgs e
        )
    {
        if (_target is null)
        {
            return;
        }

        var car = PointToClient (_target.PointToScreen (_target.PlaceToPoint (_target.Selection.Start)));

        var fontSize = TextRenderer.MeasureText ("W", Font);

        var column = 0;
        e.Graphics.FillRectangle
            (
                new LinearGradientBrush (new Rectangle (0, 0, Width, Height), BackColor, BackColor2, 270),
                new Rectangle (0, 0, Width, Height)
            );

        float columnWidth = _target.CharWidth;
        var sf = new StringFormat();
        sf.Alignment = StringAlignment.Center;
        sf.LineAlignment = StringAlignment.Near;

        var zeroPoint = _target.PositionToPoint (0);
        zeroPoint = PointToClient (_target.PointToScreen (zeroPoint));

        using (var pen = new Pen (TickColor))
        using (var textBrush = new SolidBrush (ForeColor))
        {
            for (float x = zeroPoint.X; x < Right; x += columnWidth, ++column)
            {
                if (column % 10 == 0)
                {
                    e.Graphics.DrawString (column.ToString(), Font, textBrush, x, 0f, sf);
                }

                e.Graphics.DrawLine (pen, (int)x, fontSize.Height + (column % 5 == 0 ? 1 : 3), (int)x, Height - 4);
            }
        }

        using (var pen = new Pen (TickColor))
        {
            e.Graphics.DrawLine (pen, new Point (car.X - 3, Height - 3), new Point (car.X + 3, Height - 3));
        }

        using (var pen = new Pen (CaretTickColor))
        {
            e.Graphics.DrawLine (pen, new Point (car.X - 2, fontSize.Height + 3), new Point (car.X - 2, Height - 4));
            e.Graphics.DrawLine (pen, new Point (car.X, fontSize.Height + 1), new Point (car.X, Height - 4));
            e.Graphics.DrawLine (pen, new Point (car.X + 2, fontSize.Height + 3), new Point (car.X + 2, Height - 4));
        }
    }

    #endregion
}
