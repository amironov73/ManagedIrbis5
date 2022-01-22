// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Sytle.cs -- стиль символов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System;
using System.Drawing.Drawing2D;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Стиль символов.
/// </summary>
/// <remarks>This is base class for all text and design renderers</remarks>
public abstract class Style
    : IDisposable
{
    /// <summary>
    /// This style is exported to outer formats (HTML for example)
    /// </summary>
    public virtual bool IsExportable { get; set; }

    /// <summary>
    /// Occurs when user click on StyleVisualMarker joined to this style
    /// </summary>
    public event EventHandler<VisualMarkerEventArgs> VisualMarkerClick;

    #region Construction

    /// <summary>
    /// Constructor
    /// </summary>
    protected Style()
    {
        IsExportable = true;
    }

    #endregion

    /// <summary>
    /// Renders given range of text
    /// </summary>
    /// <param name="graphics">Graphics object</param>
    /// <param name="position">Position of the range in absolute control coordinates</param>
    /// <param name="range">Rendering range of text</param>
    public abstract void Draw (Graphics graphics, Point position, TextRange range);

    /// <summary>
    /// Occurs when user click on StyleVisualMarker joined to this style
    /// </summary>
    public virtual void OnVisualMarkerClick (SyntaxTextBox tb, VisualMarkerEventArgs args)
    {
        if (VisualMarkerClick != null)
        {
            VisualMarkerClick (tb, args);
        }
    }

    /// <summary>
    /// Shows VisualMarker
    /// Call this method in Draw method, when you need to show VisualMarker for your style
    /// </summary>
    protected virtual void AddVisualMarker (SyntaxTextBox tb, StyleVisualMarker marker)
    {
        tb.AddVisualMarker (marker);
    }

    public static Size GetSizeOfRange (TextRange range)
    {
        return new Size ((range.End.Column - range.Start.Column) * range._textBox.CharWidth, range._textBox.CharHeight);
    }

    public static GraphicsPath GetRoundedRectangle (Rectangle rect, int d)
    {
        var gp = new GraphicsPath();

        gp.AddArc (rect.X, rect.Y, d, d, 180, 90);
        gp.AddArc (rect.X + rect.Width - d, rect.Y, d, d, 270, 90);
        gp.AddArc (rect.X + rect.Width - d, rect.Y + rect.Height - d, d, d, 0, 90);
        gp.AddArc (rect.X, rect.Y + rect.Height - d, d, d, 90, 90);
        gp.AddLine (rect.X, rect.Y + rect.Height - d, rect.X, rect.Y + d / 2);

        return gp;
    }

    public virtual void Dispose()
    {
        ;
    }

    /// <summary>
    /// Returns CSS for export to HTML
    /// </summary>
    /// <returns></returns>
    public virtual string GetCSS()
    {
        return "";
    }

    /// <summary>
    /// Returns RTF descriptor for export to RTF
    /// </summary>
    /// <returns></returns>
    public virtual RtfStyleDescriptor GetRTF()
    {
        return new RtfStyleDescriptor();
    }
}
