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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a zip code object.
    /// </summary>
    /// <remarks>
    /// This object is mainly used in Russia to print postal index on envelopes. It complies with the
    /// GOST R 51506-99.
    /// </remarks>
    public partial class ZipCodeObject : ReportComponentBase
    {
        #region Fields

        private static List<Point[]> FDigits;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the width of a single zipcode segment, in pixels.
        /// </summary>
        [Category ("Layout")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float SegmentWidth { get; set; }

        /// <summary>
        /// Gets or sets the height of a single zipcode segment, in pixels.
        /// </summary>
        [Category ("Layout")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float SegmentHeight { get; set; }

        /// <summary>
        /// Gets or sets the spacing between origins of segments, in pixels.
        /// </summary>
        [Category ("Layout")]
        [TypeConverter ("AM.Reporting.TypeConverters.UnitsConverter, AM.Reporting")]
        public float Spacing { get; set; }

        /// <summary>
        /// Gets or sets the number of segments in zipcode.
        /// </summary>
        [Category ("Layout")]
        [DefaultValue (6)]
        public int SegmentCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the reference markers should be drawn.
        /// </summary>
        /// <remarks>
        /// Reference markers are used by postal service to automatically read the zipcode.
        /// </remarks>
        [Category ("Behavior")]
        [DefaultValue (true)]
        public bool ShowMarkers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the segment grid should be drawn.
        /// </summary>
        [Category ("Behavior")]
        [DefaultValue (true)]
        public bool ShowGrid { get; set; }

        /// <summary>
        /// Gets or sets a data column name bound to this control.
        /// </summary>
        /// <remarks>
        /// Value must be in the form "Datasource.Column".
        /// </remarks>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.DataColumnEditor, AM.Reporting", typeof (UITypeEditor))]
        public string DataColumn { get; set; }

        /// <summary>
        /// Gets or sets an expression that contains the zip code.
        /// </summary>
        [Category ("Data")]
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the zip code.
        /// </summary>
        [Category ("Data")]
        public string Text { get; set; }

        #endregion

        #region Private Methods

        private void DrawSegmentGrid (FRPaintEventArgs e, float offsetX, float offsetY)
        {
            var g = e.Graphics;
            var saveSmoothing = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Brush b = e.Cache.GetBrush (Border.Color);

            var grid = new int[]
                { 111111, 110001, 101001, 100101, 100011, 111111, 110001, 101001, 100101, 100011, 111111 };
            var ratioX = SegmentWidth / (Units.Centimeters * 0.5f);
            var ratioY = SegmentHeight / (Units.Centimeters * 1);
            var pointSize = Units.Millimeters * 0.25f;
            var y = AbsTop;

            foreach (var gridRow in grid)
            {
                var row = gridRow;
                var x = AbsLeft;

                while (row > 0)
                {
                    if (row % 10 == 1)
                    {
                        g.FillEllipse (b, (x + offsetX - pointSize / 2) * e.ScaleX,
                            (y + offsetY - pointSize / 2) * e.ScaleY,
                            pointSize * e.ScaleX, pointSize * e.ScaleY);
                    }

                    row /= 10;

                    x += Units.Millimeters * 1 * ratioX;
                }

                y += Units.Millimeters * 1 * ratioY;
            }

            g.SmoothingMode = saveSmoothing;
        }

        private void DrawReferenceLine (FRPaintEventArgs e, float offsetX)
        {
            var g = e.Graphics;
            Brush b = e.Cache.GetBrush (Border.Color);

            g.FillRectangle (b,
                new RectangleF ((AbsLeft + offsetX) * e.ScaleX, AbsTop * e.ScaleY,
                    Units.Millimeters * 7 * e.ScaleX, Units.Millimeters * 2 * e.ScaleY));

            // draw start line
            if (offsetX == 0)
            {
                g.FillRectangle (b,
                    new RectangleF ((AbsLeft + offsetX) * e.ScaleX, (AbsTop + Units.Millimeters * 3) * e.ScaleY,
                        Units.Millimeters * 7 * e.ScaleX, Units.Millimeters * 1 * e.ScaleY));
            }
        }

        private void DrawSegment (FRPaintEventArgs e, int symbol, float offsetX)
        {
            var g = e.Graphics;
            float offsetY = 0;

            // draw marker
            if (ShowMarkers)
            {
                DrawReferenceLine (e, offsetX);
                if (offsetX == 0)
                {
                    return;
                }

                offsetX += Units.Millimeters * 1;
                offsetY = Units.Millimeters * 4;
            }
            else
            {
                // draw inside the object's area - important when you export the object
                offsetX += Border.Width / 2;
                offsetY += Border.Width / 2;
            }

            // draw grid
            if (ShowGrid)
            {
                DrawSegmentGrid (e, offsetX, offsetY);
            }

            // draw symbol
            if (symbol != -1)
            {
                var digit = FDigits[symbol];
                var path = new PointF[digit.Length];
                var ratioX = SegmentWidth / (Units.Centimeters * 0.5f);
                var ratioY = SegmentHeight / (Units.Centimeters * 1);

                for (var i = 0; i < digit.Length; i++)
                {
                    path[i] = new PointF ((AbsLeft + digit[i].X * Units.Millimeters * ratioX + offsetX) * e.ScaleX,
                        (AbsTop + digit[i].Y * Units.Millimeters * ratioY + offsetY) * e.ScaleY);
                }

                using (var pen = new Pen (Border.Color, Border.Width * e.ScaleX))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;
                    pen.LineJoin = LineJoin.Round;
                    g.DrawLines (pen, path);
                }
            }
        }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as ZipCodeObject;

            SegmentWidth = src.SegmentWidth;
            SegmentHeight = src.SegmentHeight;
            Spacing = src.Spacing;
            SegmentCount = src.SegmentCount;
            ShowMarkers = src.ShowMarkers;
            ShowGrid = src.ShowGrid;

            DataColumn = src.DataColumn;
            Expression = src.Expression;
            Text = src.Text;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            Width = ((ShowMarkers ? 1 : 0) + SegmentCount) * Spacing;
            Height = (ShowMarkers ? SegmentHeight + Units.Millimeters * 4 : SegmentHeight) + Border.Width;

            base.Draw (e);

            float offsetX = 0;
            if (ShowMarkers)
            {
                // draw starting marker
                DrawSegment (e, -1, 0);
                offsetX += Spacing;
            }

            var text = Text.PadLeft (SegmentCount, '0');
            text = text.Substring (0, SegmentCount);

            foreach (var ch in text)
            {
                var symbol = -1;
                if (ch >= '0' && ch <= '9')
                {
                    symbol = (int)ch - (int)'0';
                }

                DrawSegment (e, symbol, offsetX);
                offsetX += Spacing;
            }
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as ZipCodeObject;
            Border.SimpleBorder = true;
            base.Serialize (writer);

            if (FloatDiff (SegmentWidth, c.SegmentWidth))
            {
                writer.WriteFloat ("SegmentWidth", SegmentWidth);
            }

            if (FloatDiff (SegmentHeight, c.SegmentHeight))
            {
                writer.WriteFloat ("SegmentHeight", SegmentHeight);
            }

            if (FloatDiff (Spacing, c.Spacing))
            {
                writer.WriteFloat ("Spacing", Spacing);
            }

            if (SegmentCount != c.SegmentCount)
            {
                writer.WriteInt ("SegmentCount", SegmentCount);
            }

            if (ShowMarkers != c.ShowMarkers)
            {
                writer.WriteBool ("ShowMarkers", ShowMarkers);
            }

            if (ShowGrid != c.ShowGrid)
            {
                writer.WriteBool ("ShowGrid", ShowGrid);
            }

            if (DataColumn != c.DataColumn)
            {
                writer.WriteStr ("DataColumn", DataColumn);
            }

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }

            if (Text != c.Text)
            {
                writer.WriteStr ("Text", Text);
            }
        }

        #endregion

        #region Report Engine

        /// <inheritdoc/>
        public override string[] GetExpressions()
        {
            List<string> expressions = new List<string>();
            expressions.AddRange (base.GetExpressions());

            if (!string.IsNullOrEmpty (DataColumn))
            {
                expressions.Add (DataColumn);
            }

            if (!string.IsNullOrEmpty (Expression))
            {
                expressions.Add (Expression);
            }

            return expressions.ToArray();
        }

        /// <inheritdoc/>
        public override void GetData()
        {
            base.GetData();
            if (!string.IsNullOrEmpty (DataColumn))
            {
                var value = Report.GetColumnValue (DataColumn);
                Text = value == null ? "" : value.ToString();
            }
            else if (!string.IsNullOrEmpty (Expression))
            {
                var value = Report.Calc (Expression);
                Text = value == null ? "" : value.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ZipCodeObject"/> with the default settings.
        /// </summary>
        public ZipCodeObject()
        {
            SegmentWidth = Units.Centimeters * 0.5f;
            SegmentHeight = Units.Centimeters * 1;
            Spacing = Units.Centimeters * 0.9f;
            SegmentCount = 6;
            ShowMarkers = true;
            ShowGrid = true;

            Text = "123456";
            DataColumn = "";
            Expression = "";

            FlagSimpleBorder = true;
            Border.Width = 3;
            SetFlags (Flags.HasSmartTag, true);
        }

        static ZipCodeObject()
        {
            FDigits = new List<Point[]>();
            FDigits.Add (new Point[]
                { new Point (0, 0), new Point (5, 0), new Point (5, 10), new Point (0, 10), new Point (0, 0) });
            FDigits.Add (new Point[] { new Point (0, 5), new Point (5, 0), new Point (5, 10) });
            FDigits.Add (new Point[]
                { new Point (0, 0), new Point (5, 0), new Point (5, 5), new Point (0, 10), new Point (5, 10) });
            FDigits.Add (new Point[]
                { new Point (0, 0), new Point (5, 0), new Point (0, 5), new Point (5, 5), new Point (0, 10) });
            FDigits.Add (new Point[]
                { new Point (0, 0), new Point (0, 5), new Point (5, 5), new Point (5, 0), new Point (5, 10) });
            FDigits.Add (new Point[]
            {
                new Point (5, 0), new Point (0, 0), new Point (0, 5), new Point (5, 5), new Point (5, 10),
                new Point (0, 10)
            });
            FDigits.Add (new Point[]
            {
                new Point (5, 0), new Point (0, 5), new Point (0, 10), new Point (5, 10), new Point (5, 5),
                new Point (0, 5)
            });
            FDigits.Add (new Point[] { new Point (0, 0), new Point (5, 0), new Point (0, 5), new Point (0, 10) });
            FDigits.Add (new Point[]
            {
                new Point (0, 5), new Point (0, 0), new Point (5, 0), new Point (5, 10), new Point (0, 10),
                new Point (0, 5), new Point (5, 5)
            });
            FDigits.Add (new Point[]
            {
                new Point (5, 5), new Point (0, 5), new Point (0, 0), new Point (5, 0), new Point (5, 5),
                new Point (0, 10)
            });
        }
    }
}
