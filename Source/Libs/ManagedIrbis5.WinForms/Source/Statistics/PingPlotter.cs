// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PingPlotter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using ManagedIrbis.Statistics;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Statistics
{
    /// <summary>
    ///
    /// </summary>
    public sealed partial class PingPlotter
        : Control
    {
        #region Properties

        /// <summary>
        /// Statistics.
        /// </summary>
        public PingStatistics? Statistics { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public PingPlotter()
        {
            BackColor = Color.White;
            DoubleBuffered = true;
            TabStop = false;
            SetStyle
                (
                    ControlStyles.DoubleBuffer
                    | ControlStyles.OptimizedDoubleBuffer
                    | ControlStyles.ResizeRedraw,
                    true
                );
            SetStyle
                (
                    ControlStyles.Selectable,
                    false
                );

            InitializeComponent();
        }

        #endregion

        #region Control members

        /// <inheritdoc cref="Control.OnPaint" />
        protected override void OnPaint
            (
                PaintEventArgs e
            )
        {
            base.OnPaint(e);

            if (ReferenceEquals(Statistics, null))
            {
                return;
            }

            var data = Statistics.Data.ToArray();
            if (data.Length == 0)
            {
                return;
            }

            var maxTime = Statistics.MaxTime;
            if (maxTime == 0)
            {
                maxTime = 1;
            }
            maxTime += maxTime / 7;
            var workArea = ClientRectangle;
            var height = workArea.Height;
            var width = workArea.Width;
            var top = workArea.Top;
            var bottom = workArea.Bottom;

            var delta = 50;
            while (delta * 10 < maxTime)
            {
                delta *= 2;
            }
            //maxTime = delta * 10;
            var scale = (double)height / maxTime;
            var graphics = e.Graphics;

            using (var linePen = new Pen(Color.LightBlue))
            {
                var time = delta;
                while (time < maxTime)
                {
                    var y = (int)(bottom - time * scale);
                    graphics.DrawLine
                        (
                            linePen,
                            workArea.Left,
                            y,
                            workArea.Right,
                            y
                        );

                    time += delta;
                }
            }

            using (var linePen = new Pen(Color.Blue))
            using (var errorPen = new Pen(Color.Red))
            using (var endPen = new Pen(Color.LightGreen))
            {
                var i = Math.Max(0, data.Length - width);
                var x1 = workArea.Left;
                var x2 = x1 + 1;
                var y1 = (int)(bottom - data[i].RoundTripTime * scale);
                var right = workArea.Right;

                while (x2 < right && i < data.Length)
                {
                    if (data[i].Success)
                    {
                        var y2 = (int)(bottom - data[i].RoundTripTime * scale);
                        graphics.DrawLine(linePen, x1, y1, x2, y2);
                        y1 = y2;
                    }
                    else
                    {
                        graphics.DrawLine
                            (
                                errorPen,
                                x1, y1, x1, bottom
                            );
                    }

                    x1++;
                    x2++;
                    i++;
                }

                graphics.DrawLine
                    (
                        endPen,
                        x1, top, x1, bottom
                    );
            }

            var backColor = Color.FromArgb(200, 255, 255, 255);
            using (Brush foreBrush = new SolidBrush(Color.Black))
            using (Brush backBrush = new SolidBrush(backColor))
            using (var font = new Font(FontFamily.GenericSansSerif, 8.0f))
            {
                var text = string.Format
                    (
                        "Min: {1} ms{0}Max: {2} ms{0}Avg: {3} ms",
                        Environment.NewLine,
                        Statistics.MinTime,
                        Statistics.MaxTime,
                        Statistics.AverageTime
                    );
                var point = new PointF(10, 10);
                var size = graphics.MeasureString(text, font);
                var r = new RectangleF(point, size);
                r.Inflate(10, 10);
                graphics.FillRectangle
                    (
                        backBrush,
                        r
                    );
                graphics.DrawString
                    (
                        text,
                        font,
                        foreBrush,
                        point
                    );
            }
        }

        #endregion
    }
}
