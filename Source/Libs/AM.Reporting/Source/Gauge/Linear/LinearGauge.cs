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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Gauge.Linear
{
    /// <summary>
    /// Represents a linear gauge.
    /// </summary>
    public partial class LinearGauge : GaugeObject
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the value that specifies inverted gauge or not.
        /// </summary>
        [Category ("Appearance")]
        public bool Inverted { get; set; }

        /// <summary>
        /// Gets or sets gauge label.
        /// </summary>
        [Browsable (false)]
        public override GaugeLabel Label
        {
            get => base.Label;
            set => base.Label = value;
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LinearGauge"/> class.
        /// </summary>
        public LinearGauge() : base()
        {
            InitializeComponent();
            Scale = new LinearScale (this);
            Pointer = new LinearPointer (this);
            Height = 2.0f * Units.Centimeters;
            Width = 8.0f * Units.Centimeters;
            Inverted = false;
        }

        #endregion // Constructors

        #region Public Methods

        /// <inheritdoc/>
        public override void Assign (Base source)
        {
            base.Assign (source);

            var src = source as LinearGauge;
            Inverted = src.Inverted;
        }

        /// <inheritdoc/>
        public override void Draw (FRPaintEventArgs e)
        {
            var g = e.Graphics;
            if (Report != null && Report.SmoothGraphics)
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.AntiAlias;
            }

            base.Draw (e);
            Scale.Draw (e);
            Pointer.Draw (e);
            Border.Draw (e, new RectangleF (AbsLeft, AbsTop, Width, Height));
        }

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as LinearGauge;
            base.Serialize (writer);

            if (Inverted != c.Inverted)
            {
                writer.WriteBool ("Inverted", Inverted);
            }
        }

        #endregion // Public Methods
    }
}
