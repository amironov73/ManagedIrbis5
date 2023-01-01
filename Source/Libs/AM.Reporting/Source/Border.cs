// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Specifies the style of a border line.
    /// </summary>
    public enum LineStyle
    {
        /// <summary>
        /// Specifies a solid line.
        /// </summary>
        Solid,

        /// <summary>
        /// Specifies a line consisting of dashes.
        /// </summary>
        Dash,

        /// <summary>
        /// Specifies a line consisting of dots.
        /// </summary>
        Dot,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot.
        /// </summary>
        DashDot,

        /// <summary>
        /// Specifies a line consisting of a repeating pattern of dash-dot-dot.
        /// </summary>
        DashDotDot,

        /// <summary>
        /// Specifies a double line.
        /// </summary>
        Double,

        /// <summary>
        /// Specifies a custom line.
        /// </summary>
        Custom
    }

    /// <summary>
    /// Specifies the sides of a border.
    /// </summary>
    [Flags]
    public enum BorderLines
    {
        /// <summary>
        /// Specifies no border lines.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies the left border line.
        /// </summary>
        Left = 1,

        /// <summary>
        /// Specifies the right border line.
        /// </summary>
        Right = 2,

        /// <summary>
        /// Specifies the top border line.
        /// </summary>
        Top = 4,

        /// <summary>
        /// Specifies the bottom border line.
        /// </summary>
        Bottom = 8,

        /// <summary>
        /// Specifies all border lines.
        /// </summary>
        All = 15
    }

    /// <summary>
    /// Represents a single border line.
    /// </summary>
    [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
    public class BorderLine
    {
        #region Fields

        #endregion

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
                var styles = new DashStyle[]
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
        public override bool Equals (object obj)
        {
            var line = obj as BorderLine;
            return line != null && Width == line.Width && Color == line.Color && Style == line.Style;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Color.GetHashCode() ^ Style.GetHashCode() ^ Width.GetHashCode();
        }

        internal void Draw (FRPaintEventArgs e, float x, float y, float x1, float y1,
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

        internal void Serialize (ReportWriter writer, string prefix, BorderLine c)
        {
            if (Color != c.Color)
            {
                writer.WriteValue (prefix + ".Color", Color);
            }

            if (Style != c.Style)
            {
                writer.WriteValue (prefix + ".Style", Style);
            }

            if (Width != c.Width)
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

    /// <summary>
    /// Represents a border around the report object.
    /// </summary>
    /// <remarks>
    /// Border consists of four lines. Each line has own color, style and width. Lines are accessible through
    /// <see cref="LeftLine"/>, <see cref="RightLine"/>, <see cref="TopLine"/>, <see cref="BottomLine"/> properties.
    /// <para/>
    /// To turn on and off the lines, use the <see cref="Lines"/> property. To set the same color, style or width
    /// for each line, use <see cref="Color"/>, <see cref="Style"/>, <see cref="Width"/> properties of the <b>Border</b>.
    /// </remarks>
    [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
    [Editor ("AM.Reporting.TypeEditors.BorderEditor, AM.Reporting", typeof (UITypeEditor))]
    public class Border
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a color of the border.
        /// </summary>
        /// <remarks>
        /// This property actually returns a color of the <see cref="LeftLine"/>. When you assign a value
        /// to this property, the value will be set to each border line.
        /// </remarks>
        [Editor ("AM.Reporting.TypeEditors.ColorEditor, AM.Reporting", typeof (UITypeEditor))]
        public Color Color
        {
            get => LeftLine.Color;
            set
            {
                LeftLine.Color = value;
                RightLine.Color = value;
                TopLine.Color = value;
                BottomLine.Color = value;
            }
        }

        /// <summary>
        /// Gets or sets a value determines whether to draw a shadow.
        /// </summary>
        [DefaultValue (false)]
        public bool Shadow { get; set; }

        /// <summary>
        /// Gets or sets a shadow width, in pixels.
        /// </summary>
        [DefaultValue (4f)]
        public float ShadowWidth { get; set; }

        /// <summary>
        /// Gets or sets a shadow color.
        /// </summary>
        [Editor ("AM.Reporting.TypeEditors.ColorEditor, AM.Reporting", typeof (UITypeEditor))]
        public Color ShadowColor { get; set; }

        /// <summary>
        /// Gets or sets a style of the border.
        /// </summary>
        /// <remarks>
        /// This property actually returns a style of the <see cref="LeftLine"/>. When you assign a value
        /// to this property, the value will be set to each border line.
        /// </remarks>
        [DefaultValue (LineStyle.Solid)]
        [Editor ("AM.Reporting.TypeEditors.LineStyleEditor, AM.Reporting", typeof (UITypeEditor))]
        public LineStyle Style
        {
            get => LeftLine.Style;
            set
            {
                LeftLine.Style = value;
                RightLine.Style = value;
                TopLine.Style = value;
                BottomLine.Style = value;
            }
        }

        /// <summary>
        /// Gets or sets a visible lines of a border.
        /// </summary>
        [DefaultValue (BorderLines.None)]
        [Editor ("AM.Reporting.TypeEditors.BorderLinesEditor, AM.Reporting", typeof (UITypeEditor))]
        public BorderLines Lines { get; set; }

        /// <summary>
        /// Gets or sets a width of the border, in pixels.
        /// </summary>
        /// <remarks>
        /// This property actually returns a width of the <see cref="LeftLine"/>. When you assign a value
        /// to this property, the value will be set to each border line.
        /// </remarks>
        [DefaultValue (1f)]
        public float Width
        {
            get => LeftLine.Width;
            set
            {
                LeftLine.Width = value;
                RightLine.Width = value;
                TopLine.Width = value;
                BottomLine.Width = value;
            }
        }

        /// <summary>
        /// Gets or sets the left line of the border.
        /// </summary>
        public BorderLine LeftLine { get; set; }

        /// <summary>
        /// Gets or sets the top line of the border.
        /// </summary>
        public BorderLine TopLine { get; set; }

        /// <summary>
        /// Gets or sets the right line of the border.
        /// </summary>
        public BorderLine RightLine { get; set; }

        /// <summary>
        /// Gets or sets the bottom line of the border.
        /// </summary>
        public BorderLine BottomLine { get; set; }

        /// <summary>
        /// Gets or sets a value determines that <b>Border</b> must serialize only one line.
        /// </summary>
        /// <remarks>
        /// This property is for internal use only.
        /// </remarks>
        [Browsable (false)]
        public bool SimpleBorder { get; set; }

        internal DashStyle DashStyle => LeftLine.DashStyle;

        #endregion

        #region Private Methods

        private bool ShouldSerializeLeftLine()
        {
            return LeftLine.ShouldSerialize();
        }

        private bool ShouldSerializeTopLine()
        {
            return TopLine.ShouldSerialize();
        }

        private bool ShouldSerializeRightLine()
        {
            return RightLine.ShouldSerialize();
        }

        private bool ShouldSerializeBottomLine()
        {
            return BottomLine.ShouldSerialize();
        }

        private bool ShouldSerializeColor()
        {
            return Color != Color.Black;
        }

        private bool ShouldSerializeShadowColor()
        {
            return ShadowColor != Color.Black;
        }

        #endregion

        #region Internal Methods

        internal void ZoomBorder (float zoom)
        {
            LeftLine.Width *= zoom;
            if (LeftLine.Width is > 0 and < 1)
            {
                LeftLine.Width = 1;
            }

            RightLine.Width *= zoom;
            if (RightLine.Width is > 0 and < 1)
            {
                RightLine.Width = 1;
            }

            TopLine.Width *= zoom;
            if (TopLine.Width is > 0 and < 1)
            {
                TopLine.Width = 1;
            }

            BottomLine.Width *= zoom;
            if (BottomLine.Width is > 0 and < 1)
            {
                BottomLine.Width = 1;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates the exact copy of this <b>Border</b>.
        /// </summary>
        /// <returns>A copy of this border.</returns>
        public Border Clone()
        {
            var result = new Border (this);
            return result;
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            var b = obj as Border;
            return b != null && Lines == b.Lines &&
                   LeftLine.Equals (b.LeftLine) && TopLine.Equals (b.TopLine) &&
                   RightLine.Equals (b.RightLine) && BottomLine.Equals (b.BottomLine) &&
                   Shadow == b.Shadow && ShadowColor == b.ShadowColor && ShadowWidth == b.ShadowWidth;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Lines.GetHashCode() ^ (Shadow.GetHashCode() + 16) ^ ShadowColor.GetHashCode() ^
                   (ShadowWidth.GetHashCode() + 32) ^ (LeftLine.GetHashCode() << 1) ^ (TopLine.GetHashCode() << 2) ^
                   (RightLine.GetHashCode() << 3) ^ (BottomLine.GetHashCode() << 4);
        }

        /// <summary>
        /// Serializes the border.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">Border property name.</param>
        /// <param name="c">Another Border to compare with.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public void Serialize (ReportWriter writer, string prefix, Border c)
        {
            if (Shadow != c.Shadow)
            {
                writer.WriteBool (prefix + ".Shadow", Shadow);
            }

            if (ShadowWidth != c.ShadowWidth)
            {
                writer.WriteFloat (prefix + ".ShadowWidth", ShadowWidth);
            }

            if (ShadowColor != c.ShadowColor)
            {
                writer.WriteValue (prefix + ".ShadowColor", ShadowColor);
            }

            if (!SimpleBorder)
            {
                if (Lines != c.Lines)
                {
                    writer.WriteValue (prefix + ".Lines", Lines);
                }

                if (Lines != BorderLines.None || Color != Color.Black)
                {
                    if (LeftLine.Equals (RightLine) && LeftLine.Equals (TopLine) && LeftLine.Equals (BottomLine) &&
                        c.LeftLine.Equals (c.RightLine) && c.LeftLine.Equals (c.TopLine) &&
                        c.LeftLine.Equals (c.BottomLine))
                    {
                        LeftLine.Serialize (writer, prefix, c.LeftLine);
                    }
                    else
                    {
                        LeftLine.Serialize (writer, prefix + ".LeftLine", c.LeftLine);
                        TopLine.Serialize (writer, prefix + ".TopLine", c.TopLine);
                        RightLine.Serialize (writer, prefix + ".RightLine", c.RightLine);
                        BottomLine.Serialize (writer, prefix + ".BottomLine", c.BottomLine);
                    }
                }
            }
            else
            {
                LeftLine.Serialize (writer, prefix, c.LeftLine);
            }
        }

        /// <summary>
        /// Draw the border using draw event arguments and specified bounding rectangle.
        /// </summary>
        /// <param name="e">Draw event arguments.</param>
        /// <param name="rect">Bounding rectangle.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public void Draw (FRPaintEventArgs e, RectangleF rect)
        {
            var g = e.Graphics;
            rect.X *= e.ScaleX;
            rect.Y *= e.ScaleY;
            rect.Width *= e.ScaleX;
            rect.Height *= e.ScaleY;

            if (Shadow)
            {
                //int d = (int)Math.Round(ShadowWidth * e.ScaleX);
                //Pen pen = e.Cache.GetPen(ShadowColor, d, DashStyle.Solid);
                //g.DrawLine(pen, rect.Right + d / 2, rect.Top + d, rect.Right + d / 2, rect.Bottom);
                //g.DrawLine(pen, rect.Left + d, rect.Bottom + d / 2, rect.Right + d, rect.Bottom + d / 2);

                var d = ShadowWidth * e.ScaleX;
                Brush brush = e.Cache.GetBrush (ShadowColor);
                g.FillRectangle (brush, rect.Left + d, rect.Bottom, rect.Width, d);
                g.FillRectangle (brush, rect.Right, rect.Top + d, d, rect.Height);
            }

            if (Lines != BorderLines.None)
            {
                // draw full frame as a rectangle with solid line only. Non-solid lines
                // should be drawn separately to avoid overlapping effect
                if (Lines == BorderLines.All && LeftLine.Equals (TopLine) && LeftLine.Equals (RightLine) &&
                    LeftLine.Equals (BottomLine) && LeftLine.Style == LineStyle.Solid)
                {
                    var pen = e.Cache.GetPen (LeftLine.Color, (int)Math.Round (LeftLine.Width * e.ScaleX),
                        LeftLine.DashStyle);
                    g.DrawRectangle (pen, rect.Left, rect.Top, rect.Width, rect.Height);
                }
                else
                {
                    if ((Lines & BorderLines.Left) != 0)
                    {
                        LeftLine.Draw (e, rect.Left, rect.Top, rect.Left, rect.Bottom,
                            true, (Lines & BorderLines.Top) != 0, (Lines & BorderLines.Bottom) != 0);
                    }

                    if ((Lines & BorderLines.Right) != 0)
                    {
                        RightLine.Draw (e, rect.Right, rect.Top, rect.Right, rect.Bottom,
                            false, (Lines & BorderLines.Top) != 0, (Lines & BorderLines.Bottom) != 0);
                    }

                    if ((Lines & BorderLines.Top) != 0)
                    {
                        TopLine.Draw (e, rect.Left, rect.Top, rect.Right, rect.Top,
                            true, (Lines & BorderLines.Left) != 0, (Lines & BorderLines.Right) != 0);
                    }

                    if ((Lines & BorderLines.Bottom) != 0)
                    {
                        BottomLine.Draw (e, rect.Left, rect.Bottom, rect.Right, rect.Bottom,
                            false, (Lines & BorderLines.Left) != 0, (Lines & BorderLines.Right) != 0);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class with default settings.
        /// </summary>
        public Border()
        {
            LeftLine = new BorderLine();
            TopLine = new BorderLine();
            RightLine = new BorderLine();
            BottomLine = new BorderLine();
            ShadowWidth = 4;
            ShadowColor = Color.Black;
        }

        private Border (Border src)
        {
            Lines = src.Lines;
            Shadow = src.Shadow;
            ShadowColor = src.ShadowColor;
            ShadowWidth = src.ShadowWidth;
            LeftLine = src.LeftLine.Clone();
            TopLine = src.TopLine.Clone();
            RightLine = src.RightLine.Clone();
            BottomLine = src.BottomLine.Clone();

            //Fix 1513
            //Width = src.Width;
            //1513
        }
    }
}
