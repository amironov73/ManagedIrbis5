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

using AM.Reporting.Gauge.Radial;
using AM.Reporting.Utils;

using System;
using System.ComponentModel;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.Gauge.Simple.Progress
{
    /// <inheritdoc />
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class SimpleProgressLabel : GaugeLabel
    {
        private int decimals;

        /// <summary>
        /// Gets or sets the number of fractional digits
        /// </summary>
        public int Decimals
        {
            get => decimals;
            set
            {
                if (value < 0)
                {
                    decimals = 0;
                }
                else if (value > 15)
                {
                    decimals = 15;
                }
                else
                {
                    decimals = value;
                }
            }
        }

        /// <inheritdoc />
        [Browsable (false)]
        public override string Text
        {
            get => base.Text;
            set => base.Text = value;
        }

        /// <inheritdoc />
        public SimpleProgressLabel (GaugeObject parent) : base (parent)
        {
            Parent = parent as SimpleProgressGauge;
            decimals = 0;
        }

        /// <inheritdoc />
        public override void Draw (FRPaintEventArgs e)
        {
            base.Draw (e);
            var x = (Parent.AbsLeft + Parent.Border.Width / 2) * e.ScaleX;
            var y = (Parent.AbsTop + Parent.Border.Width / 2) * e.ScaleY;
            var dx = (Parent.Width - Parent.Border.Width) * e.ScaleX;
            var dy = (Parent.Height - Parent.Border.Width) * e.ScaleY;

            var lblPt = new PointF (x + dx / 2, y + dy / 2);
            var txtSize = RadialUtils.GetStringSize (e, Parent, Font, Text);
            var font = RadialUtils.GetFont (e, Parent, Font);
            Brush brush = e.Cache.GetBrush (Color);
            Text = Math.Round ((Parent.Value - Parent.Minimum) / (Parent.Maximum - Parent.Minimum) * 100, decimals) +
                   "%";
            e.Graphics.DrawString (Text, font, brush, lblPt.X - txtSize.Width / 2, lblPt.Y - txtSize.Height / 2);
        }
    }
}
