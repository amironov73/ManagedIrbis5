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

using System.Drawing;
using System.Drawing.Drawing2D;
using FastReport.Utils;
using System.ComponentModel;

#endregion

#nullable enable

namespace FastReport.Gauge.Simple
{
    /// <summary>
    /// Represents a simple gauge.
    /// </summary>
    public partial class SimpleGauge : GaugeObject
    {
        /// <summary>
        /// Gets or sets gauge label.
        /// </summary>
        [Browsable(false)]
        public override GaugeLabel Label
        {
            get { return base.Label; }
            set { base.Label = value; }
        }

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleGauge"/> class.
        /// </summary>
        public SimpleGauge() : base()
        {
            InitializeComponent();
            Value = 75;
            Scale = new SimpleScale(this);
            Pointer = new SimplePointer(this);
            Height = 2.0f * Units.Centimeters;
            Width = 8.0f * Units.Centimeters;
        }

        #endregion // Constructors

        #region Public Methods

        /// <inheritdoc/>
        public override void Draw(FRPaintEventArgs e)
        {
            base.Draw(e);
            Scale.Draw(e);
            Pointer.Draw(e);
            Border.Draw(e, new RectangleF(AbsLeft, AbsTop, Width, Height));
            IGraphics g = e.Graphics;

            if (Report != null && Report.SmoothGraphics)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            Scale.Draw(e);
            Pointer.Draw(e);
            Border.Draw(e, new RectangleF(AbsLeft, AbsTop, Width, Height));
        }

        #endregion // Public Methods
    }
}
