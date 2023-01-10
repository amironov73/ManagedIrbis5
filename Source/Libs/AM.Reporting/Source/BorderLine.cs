// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BorderLine.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting;

/// <summary>
/// Represents a single border line.
/// </summary>
[TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
public class BorderLine
{
    #region Properties

    /// <summary>
    /// Gets or sets a color of the line.
    /// </summary>
    [Editor ("AM.Reporting.TypeEditors.ColorEditor, AM.Reporting", typeof (UITypeEditor))]
    public Color Color { get; set; }

    /// <summary>
    /// Gets or sets a style of the line.
    /// </summary>
    [DefaultValue (LineStyle.Solid)]
    [Editor ("AM.Reporting.TypeEditors.LineStyleEditor, AM.Reporting", typeof (UITypeEditor))]
    public LineStyle Style { get; set; }

    /// <summary>
    /// Gets or sets a width of the line, in pixels.
    /// </summary>
    [DefaultValue (1f)]
    public float Width { get; set; }

    internal DashStyle DashStyle
    {
        get
        {
            var styles = new[]
            {
                DashStyle.Solid, DashStyle.Dash, DashStyle.Dot, DashStyle.DashDot, DashStyle.DashDotDot,
                DashStyle.Solid
            };
            return styles[(int)Style];
        }
    }

    #endregion

    #region Private Methods

    private bool ShouldSerializeColor()
    {
        return Color != Color.Black;
    }

    internal bool ShouldSerialize()
    {
        return Width != 1 || Style != LineStyle.Solid || Color != Color.Black;
    }

    #endregion

    #region Public Methods

    internal BorderLine Clone()
    {
        var result = new BorderLine();
        result.Assign (this);
        return result;
    }

    internal void Assign (BorderLine src)
    {
        Color = src.Color;
        Style = src.Style;
        Width = src.Width;
    }

    /// <inheritdoc/>
    public override bool Equals (object? obj)
    {
        var line = obj as BorderLine;
        return line != null && Width == line.Width && Color == line.Color && Style == line.Style;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Color.GetHashCode() ^ Style.GetHashCode() ^ Width.GetHashCode();
    }

    internal void Draw (PaintEventArgs e, float x, float y, float x1, float y1,
        bool reverseGaps, bool gap1, bool gap2)
    {
        var g = e.Graphics;

        var penWidth = (int)Math.Round (Width * e.ScaleX);
        if (penWidth <= 0)
        {
            penWidth = 1;
        }

        using (var pen = new Pen (Color, penWidth))
        {
            pen.DashStyle = DashStyle;
            pen.StartCap = LineCap.Square;
            pen.EndCap = LineCap.Square;
            if (pen.DashStyle != DashStyle.Solid)
            {
                float patternWidth = 0;
                foreach (var w in pen.DashPattern)
                {
                    patternWidth += w * pen.Width;
                }

                if (y == y1)
                {
                    pen.DashOffset = (x - ((int)(x / patternWidth)) * patternWidth) / pen.Width;
                }
                else
                {
                    pen.DashOffset = (y - ((int)(y / patternWidth)) * patternWidth) / pen.Width;
                }
            }

            if (Style != LineStyle.Double)
            {
                g.DrawLine (pen, x, y, x1, y1);
            }
            else
            {
                // we have to correctly draw inner and outer lines of a double line
                var g1 = gap1 ? pen.Width : 0;
                var g2 = gap2 ? pen.Width : 0;
                var g3 = -g1;
                var g4 = -g2;

                if (reverseGaps)
                {
                    g1 = -g1;
                    g2 = -g2;
                    g3 = -g3;
                    g4 = -g4;
                }

                if (x == x1)
                {
                    g.DrawLine (pen, x - pen.Width, y + g1, x1 - pen.Width, y1 - g2);
                    g.DrawLine (pen, x + pen.Width, y + g3, x1 + pen.Width, y1 - g4);
                }
                else
                {
                    g.DrawLine (pen, x + g1, y - pen.Width, x1 - g2, y1 - pen.Width);
                    g.DrawLine (pen, x + g3, y + pen.Width, x1 - g4, y1 + pen.Width);
                }
            }
        }
    }

    internal void Serialize
        (
            ReportWriter writer,
            string prefix,
            BorderLine line
        )
    {
        if (Color != line.Color)
        {
            writer.WriteValue (prefix + ".Color", Color);
        }

        if (Style != line.Style)
        {
            writer.WriteValue (prefix + ".Style", Style);
        }

        if (Width != line.Width)
        {
            writer.WriteFloat (prefix + ".Width", Width);
        }
    }

    public BorderLine()
    {
        Color = Color.Black;
        Width = 1;
    }

    #endregion
}
