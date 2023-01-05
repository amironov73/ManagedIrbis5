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
using System.Drawing.Design;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Gauge
{
    /// <summary>
    /// Represents a scale of a gauge.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class GaugeScale : Component
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets major ticks of scale.
        /// </summary>
        [Browsable (true)]
        public ScaleTicks MajorTicks { get; set; }

        /// <summary>
        /// Gets or sets minor ticks of scale.
        /// </summary>
        [Browsable (true)]
        public ScaleTicks MinorTicks { get; set; }

        /// <summary>
        /// Gets or sets the parent gauge object.
        /// </summary>
        [Browsable (false)]
        public GaugeObject Parent { get; set; }

        /// <summary>
        /// Gets or sets the font of scale.
        /// </summary>
        [Browsable (true)]
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets the scale font color
        /// </summary>
        [Editor ("AM.Reporting.TypeEditors.FillEditor, AM.Reporting", typeof (UITypeEditor))]
        public FillBase TextFill { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeScale"/> class.
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        public GaugeScale (GaugeObject parent)
        {
            this.Parent = parent;
            Font = new Font ("Arial", 8.0f);
            TextFill = new SolidFill (Color.Black);
            MajorTicks = new ScaleTicks();
            MinorTicks = new ScaleTicks();
        }

        #endregion // Constructors

        #region Public Methods

        /// <summary>
        /// Copies the contents of another GaugeScale.
        /// </summary>
        /// <param name="src">The GaugeScale instance to copy the contents from.</param>
        public virtual void Assign (GaugeScale src)
        {
            Font = src.Font;
            TextFill = src.TextFill;
        }

        /// <summary>
        /// Draws the scale of gauge.
        /// </summary>
        /// <param name="e">Draw event arguments.</param>
        public virtual void Draw (PaintEventArgs e)
        {
        }

        /// <summary>
        /// Serializes the gauge scale.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">Scale property name.</param>
        /// <param name="diff">Another GaugeScale to compare with.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public virtual void Serialize (ReportWriter writer, string prefix, GaugeScale diff)
        {
            TextFill.Serialize (writer, prefix + ".TextFill", diff.TextFill);
            if ((writer.SerializeTo != SerializeTo.Preview || !Font.Equals (diff.Font)) &&
                writer.ItemName != "inherited")
            {
                writer.WriteValue (prefix + ".Font", Font);
            }
        }

        #endregion // Public Methods
    }

    /// <summary>
    /// Represents a scale ticks.
    /// </summary>
    [ToolboxItem (false)]
    public class ScaleTicks : Component
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets the length of ticks.
        /// </summary>
        [Browsable (false)]
        public float Length { get; set; }

        /// <summary>
        /// Gets or sets the width of ticks.
        /// </summary>
        [Browsable (true)]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the color of ticks.
        /// </summary>
        [Browsable (true)]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the count of ticks
        /// </summary>
        [Browsable (false)]
        public int Count { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTicks"/> class.
        /// </summary>
        public ScaleTicks()
        {
            Length = 8.0f;
            Width = 1;
            Color = Color.Black;
            Count = 6;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTicks"/> class.
        /// </summary>
        /// <param name="length">Ticks length.</param>
        /// <param name="width">Ticks width.</param>
        /// <param name="color">Ticks color.</param>
        public ScaleTicks (float length, int width, Color color)
        {
            this.Length = length;
            this.Width = width;
            this.Color = color;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScaleTicks"/> class.
        /// </summary>
        /// <param name="length">Ticks length.</param>
        /// <param name="width">Ticks width.</param>
        /// <param name="color">Ticks color.</param>
        /// <param name="count">Ticks count.</param>
        public ScaleTicks (float length, int width, Color color, int count)
        {
            this.Length = length;
            this.Width = width;
            this.Color = color;
            this.Count = count;
        }

        #endregion // Constructors

        #region Public Methods

        /// <summary>
        /// Copies the contents of another ScaleTicks.
        /// </summary>
        /// <param name="src">The ScaleTicks instance to copy the contents from.</param>
        public virtual void Assign (ScaleTicks src)
        {
            Length = src.Length;
            Width = src.Width;
            Color = src.Color;
        }

        /// <summary>
        /// Serializes the scale ticks.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">Scale ticks property name.</param>
        /// <param name="diff">Another ScaleTicks to compare with.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public virtual void Serialize (ReportWriter writer, string prefix, ScaleTicks diff)
        {
            if (Length != diff.Length)
            {
                writer.WriteFloat (prefix + ".Length", Length);
            }

            if (Width != diff.Width)
            {
                writer.WriteInt (prefix + ".Width", Width);
            }

            if (Color != diff.Color)
            {
                writer.WriteValue (prefix + ".Color", Color);
            }
        }

        #endregion // Public Methods
    }
}
