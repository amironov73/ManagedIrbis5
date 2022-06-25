// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System.Drawing;
using AM.Drawing.HtmlRenderer.Adapters.Entities;
using AM.Drawing.HtmlRenderer.Adapters;

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms pens objects for core.
/// </summary>
internal sealed class PenAdapter : RPen
{
    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    private readonly Pen _pen;

    /// <summary>
    /// Init.
    /// </summary>
    public PenAdapter(Pen pen)
    {
        _pen = pen;
    }

    /// <summary>
    /// The actual WinForms brush instance.
    /// </summary>
    public Pen Pen
    {
        get { return _pen; }
    }

    public override double Width
    {
        get { return _pen.Width; }
        set { _pen.Width = (float)value; }
    }

    public override RDashStyle DashStyle
    {
        set
        {
            switch (value)
            {
                case RDashStyle.Solid:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
                case RDashStyle.Dash:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                    if (Width < 2)
                        _pen.DashPattern = new[] { 4, 4f }; // better looking
                    break;
                case RDashStyle.Dot:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    break;
                case RDashStyle.DashDot:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDot;
                    break;
                case RDashStyle.DashDotDot:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.DashDotDot;
                    break;
                case RDashStyle.Custom:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Custom;
                    break;
                default:
                    _pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                    break;
            }
        }
    }
}