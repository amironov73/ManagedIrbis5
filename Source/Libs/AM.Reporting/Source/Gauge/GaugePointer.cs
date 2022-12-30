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
using System.ComponentModel;
using FastReport.Utils;
using System.Drawing.Design;

#endregion

#nullable enable

namespace FastReport.Gauge
{
    /// <summary>
    /// Represents a pointer of gauge.
    /// </summary>
#if !DEBUG
    [DesignTimeVisible(false)]
#endif
    public class GaugePointer : Component
    {
#region Fields

        private GaugeObject parent;
        private FillBase fill;
        private float borderWidth;
        private Color borderColor;

#endregion // Fields

#region Properties

        /// <summary>
        /// Gets or sets the parent gauge object.
        /// </summary>
        [Browsable(false)]
        public GaugeObject Parent
        {
            get { return parent; }
            set { parent = value; }
        }

        /// <summary>
        /// Gets or sets the color of a pointer.
        /// </summary>
        [Browsable(true)]
        [Editor("FastReport.TypeEditors.FillEditor, FastReport", typeof(UITypeEditor))]
        public FillBase Fill
        {
            get { return fill; }
            set { fill = value; }
        }

        /// <summary>
        /// Gets or sets the border width of a pointer.
        /// </summary>
        [Browsable(false)]
        public float BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; }
        }

        /// <summary>
        /// Gets or sets the border color of a pointer.
        /// </summary>
        [Browsable(true)]
        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; }
        }

#endregion // Properties

#region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GaugePointer"/> class.
        /// </summary>
        /// <param name="parent">The parent gauge object.</param>
        public GaugePointer(GaugeObject parent)
        {
            fill = new SolidFill(Color.Orange);
            borderWidth = 1.0f;
            borderColor = Color.Black;
            this.parent = parent;
        }

#endregion // Constructors

#region Public Methods

        /// <summary>
        /// Copies the contents of another GaugePointer.
        /// </summary>
        /// <param name="src">The GaugePointer instance to copy the contents from.</param>
        public virtual void Assign(GaugePointer src)
        {
            Fill = src.Fill;
            BorderWidth = src.BorderWidth;
            BorderColor = src.BorderColor;
        }

        /// <summary>
        /// Draws the gauge pointer.
        /// </summary>
        /// <param name="e">Draw event arguments.</param>
        public virtual void Draw(FRPaintEventArgs e)
        {
        }

        /// <summary>
        /// Serializes the gauge pointer.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">Gauge pointer property name.</param>
        /// <param name="diff">Another GaugePointer to compare with.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public virtual void Serialize(FRWriter writer, string prefix, GaugePointer diff)
        {
            Fill.Serialize(writer, prefix + ".Fill", diff.Fill);
            if (BorderWidth != diff.BorderWidth)
            {
                writer.WriteFloat(prefix + ".BorderWidth", BorderWidth);
            }
            if (BorderColor != diff.BorderColor)
            {
                writer.WriteValue(prefix + ".BorderColor", BorderColor);
            }
        }

#endregion // Public Methods
    }
}
