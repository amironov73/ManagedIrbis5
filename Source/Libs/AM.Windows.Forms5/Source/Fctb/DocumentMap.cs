// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* DocumentMap.cs -- карта документа
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
/// Карта документа.
/// </summary>
public class DocumentMap
    : Control
{
    #region Events

    /// <summary>
    /// <see cref="Target"/> поменялся.
    /// </summary>
    public event EventHandler? TargetChanged;

    #endregion

    #region Properties

    /// <summary>
    /// Целевой текст-бокс.
    /// </summary>
    public SyntaxTextBox? Target
    {
        get => _target;
        set
        {
            if (_target != null)
                UnSubscribe (_target);

            _target = value;
            if (value != null)
            {
                Subscribe (_target);
            }

            OnTargetChanged();
        }
    }

    /// <summary>
    /// Масштаб.
    /// </summary>
    [Description ("Scale")]
    [DefaultValue (0.3f)]
    public float Scale
    {
        get => _scale;
        set
        {
            _scale = value;
            NeedRepaint();
        }
    }

    /// <summary>
    /// Scrollbar visibility
    /// </summary>
    [DefaultValue (true)]
    public bool ScrollbarVisible
    {
        get => _scrollbarVisible;
        set
        {
            _scrollbarVisible = value;
            NeedRepaint();
        }
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public DocumentMap()
    {
        ForeColor = Color.Maroon;
        SetStyle (
            ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint |
            ControlStyles.ResizeRedraw, true);
        Application.Idle += Application_Idle;
    }

    #endregion

    #region Private members

    private SyntaxTextBox? _target;
    private float _scale = 0.3f;
    private bool _needRepaint = true;
    private Place _startPlace = Place.Empty;
    private bool _scrollbarVisible = true;

    #endregion

    private void Application_Idle (object sender, EventArgs e)
    {
        if (_needRepaint)
            Invalidate();
    }

    protected virtual void OnTargetChanged()
    {
        NeedRepaint();

        if (TargetChanged != null)
            TargetChanged (this, EventArgs.Empty);
    }

    protected virtual void UnSubscribe (SyntaxTextBox target)
    {
        target.Scroll -= new ScrollEventHandler (Target_Scroll);
        target.SelectionChangedDelayed -= new EventHandler (Target_SelectionChanged);
        target.VisibleRangeChanged -= new EventHandler (Target_VisibleRangeChanged);
    }

    protected virtual void Subscribe (SyntaxTextBox target)
    {
        target.Scroll += new ScrollEventHandler (Target_Scroll);
        target.SelectionChangedDelayed += new EventHandler (Target_SelectionChanged);
        target.VisibleRangeChanged += new EventHandler (Target_VisibleRangeChanged);
    }

    protected virtual void Target_VisibleRangeChanged (object sender, EventArgs e)
    {
        NeedRepaint();
    }

    protected virtual void Target_SelectionChanged (object sender, EventArgs e)
    {
        NeedRepaint();
    }

    protected virtual void Target_Scroll (object sender, ScrollEventArgs e)
    {
        NeedRepaint();
    }

    protected override void OnResize (EventArgs e)
    {
        base.OnResize (e);
        NeedRepaint();
    }

    public void NeedRepaint()
    {
        _needRepaint = true;
    }

    protected override void OnPaint (PaintEventArgs e)
    {
        if (_target == null)
            return;

        var zoom = this.Scale * 100 / _target.Zoom;

        if (zoom <= float.Epsilon)
            return;

        //calc startPlace
        var r = _target.VisibleRange;
        if (_startPlace.Line > r.Start.Line)
            _startPlace.Line = r.Start.Line;
        else
        {
            var endP = _target.PlaceToPoint (r.End);
            endP.Offset (0, -(int)(ClientSize.Height / zoom) + _target.CharHeight);
            var pp = _target.PointToPlace (endP);
            if (pp.Line > _startPlace.Line)
                _startPlace.Line = pp.Line;
        }

        _startPlace.Column = 0;

        //calc scroll pos
        var linesCount = _target.Lines.Count;
        var sp1 = (float)r.Start.Line / linesCount;
        var sp2 = (float)r.End.Line / linesCount;

        //scale graphics
        e.Graphics.ScaleTransform (zoom, zoom);

        //draw text
        var size = new SizeF (ClientSize.Width / zoom, ClientSize.Height / zoom);
        _target.DrawText (e.Graphics, _startPlace, size.ToSize());

        //draw visible rect
        var p0 = _target.PlaceToPoint (_startPlace);
        var p1 = _target.PlaceToPoint (r.Start);
        var p2 = _target.PlaceToPoint (r.End);
        var y1 = p1.Y - p0.Y;
        var y2 = p2.Y + _target.CharHeight - p0.Y;

        e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

        using (var brush = new SolidBrush (Color.FromArgb (50, ForeColor)))
        using (var pen = new Pen (brush, 1 / zoom))
        {
            var rect = new Rectangle (0, y1, (int)((ClientSize.Width - 1) / zoom), y2 - y1);
            e.Graphics.FillRectangle (brush, rect);
            e.Graphics.DrawRectangle (pen, rect);
        }

        //draw scrollbar
        if (_scrollbarVisible)
        {
            e.Graphics.ResetTransform();
            e.Graphics.SmoothingMode = SmoothingMode.None;

            using (var brush = new SolidBrush (Color.FromArgb (200, ForeColor)))
            {
                var rect = new RectangleF (ClientSize.Width - 3, ClientSize.Height * sp1, 2,
                    ClientSize.Height * (sp2 - sp1));
                e.Graphics.FillRectangle (brush, rect);
            }
        }

        _needRepaint = false;
    }

    protected override void OnMouseDown (MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            Scroll (e.Location);
        base.OnMouseDown (e);
    }

    protected override void OnMouseMove (MouseEventArgs e)
    {
        if (e.Button == System.Windows.Forms.MouseButtons.Left)
            Scroll (e.Location);
        base.OnMouseMove (e);
    }

    private void Scroll (Point point)
    {
        if (_target is null)
        {
            return;
        }

        var zoom = this.Scale * 100 / _target.Zoom;

        if (zoom <= float.Epsilon)
        {
            return;
        }

        var p0 = _target.PlaceToPoint (_startPlace);
        p0 = new Point (0, p0.Y + (int)(point.Y / zoom));
        var pp = _target.PointToPlace (p0);
        _target.DoRangeVisible (new TextRange (_target, pp, pp), true);
        BeginInvoke ((MethodInvoker)OnScroll);
    }

    private void OnScroll()
    {
        Refresh();
        _target.Refresh();
    }

    /// <inheritdoc cref="Control.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        if (disposing)
        {
            Application.Idle -= Application_Idle;
            if (_target != null)
            {
                UnSubscribe (_target);
            }
        }

        base.Dispose (disposing);
    }
}
