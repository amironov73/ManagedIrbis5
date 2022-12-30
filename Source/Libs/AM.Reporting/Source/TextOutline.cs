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
using System.ComponentModel;

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents text outline.
    /// </summary>
    [ToolboxItem (false)]
    [TypeConverter (typeof (TypeConverters.FRExpandableObjectConverter))]
    public class TextOutline // : Component
    {
        #region Fields

        #endregion // Fields

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating that outline is enabled.
        /// </summary>
        [DefaultValue (false)]
        [Browsable (true)]
        public bool Enabled { get; set; }

        /// <summary>
        /// Enable or disable draw the outline behind of text.
        /// </summary>
        [DefaultValue (false)]
        [Browsable (true)]
        public bool DrawBehind { get; set; }

        /// <summary>
        /// Gets or sets the outline color.
        /// </summary>
        [Editor ("AM.Reporting.TypeEditors.ColorEditor, AM.Reporting", typeof (UITypeEditor))]
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the outline width.
        /// </summary>
        [DefaultValue (1.0f)]
        [Browsable (true)]
        public float Width { get; set; }

        /// <summary>
        /// Specifies the style of an outline.
        /// </summary>
        [DefaultValue (DashStyle.Solid)]
        [Browsable (true)]
        public DashStyle Style { get; set; }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOutline"/> class.
        /// </summary>
        public TextOutline()
        {
            Enabled = false;
            Color = Color.Black;
            Width = 1.0f;
            Style = DashStyle.Solid;
            DrawBehind = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextOutline"/> class with specified parameters.
        /// </summary>
        /// <param name="enabled">True if outline enabled.</param>
        /// <param name="color">Outline color.</param>
        /// <param name="width">Outline width.</param>
        /// <param name="style">Outline style.</param>
        /// <param name="drawbehind">True if outline should be drawn behind text.</param>
        public TextOutline (bool enabled, Color color, float width, DashStyle style, bool drawbehind)
        {
            this.Enabled = enabled;
            this.Color = color;
            this.Width = width;
            this.Style = style;
            this.DrawBehind = drawbehind;
        }

        #endregion // Constructors

        #region Public Methods

        /// <summary>
        /// Copies the content of another TextOutline.
        /// </summary>
        /// <param name="src">The TextOutline instance to copy the contents from.</param>
        public void Assign (TextOutline src)
        {
            Enabled = src.Enabled;
            Color = src.Color;
            Width = src.Width;
            Style = src.Style;
            DrawBehind = src.DrawBehind;
        }

        /// <summary>
        /// Creates the exact copy of this outline.
        /// </summary>
        /// <returns>Copy of this outline.</returns>
        public TextOutline Clone()
        {
            return new TextOutline (Enabled, Color, Width, Style, DrawBehind);
        }

        /// <summary>
        /// Serializes the TextOutline.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <param name="prefix">TextOutline property name.</param>
        /// <param name="diff">Another TextOutline to compare with.</param>
        public void Serialize (FRWriter writer, string prefix, TextOutline diff)
        {
            if (Enabled != diff.Enabled)
            {
                writer.WriteBool (prefix + ".Enabled", Enabled);
            }

            if (Color != diff.Color)
            {
                writer.WriteValue (prefix + ".Color", Color);
            }

            if (Width != diff.Width)
            {
                writer.WriteFloat (prefix + ".Width", Width);
            }

            if (Style != diff.Style)
            {
                writer.WriteValue (prefix + ".Style", Style);
            }

            if (DrawBehind != diff.DrawBehind)
            {
                writer.WriteBool (prefix + ".DrawBehind", DrawBehind);
            }
        }

        #endregion // Public Methods
    }
}
