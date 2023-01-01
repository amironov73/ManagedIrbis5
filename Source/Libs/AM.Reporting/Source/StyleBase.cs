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

using AM.Reporting.Utils;

using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents the base class for the report style or the highlight condition.
    /// </summary>
    public partial class StyleBase : IFRSerializable
    {
        #region Private Fields

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets or sets a value determines that the border must be applied.
        /// </summary>
        public bool ApplyBorder { get; set; }

        /// <summary>
        /// Gets or sets a value determines that the fill must be applied.
        /// </summary>
        public bool ApplyFill { get; set; }

        /// <summary>
        /// Gets or sets a value determines that the font must be applied.
        /// </summary>
        public bool ApplyFont { get; set; }

        /// <summary>
        /// Gets or sets a value determines that the text fill must be applied.
        /// </summary>
        public bool ApplyTextFill { get; set; }

        /// <summary>
        /// Gets or sets a border.
        /// </summary>
        public Border Border { get; set; }

        /// <summary>
        /// Gets or sets a fill.
        /// </summary>
        public FillBase Fill { get; set; }

        /// <summary>
        /// Gets or sets a font.
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Gets or sets a text fill.
        /// </summary>
        public FillBase TextFill { get; set; }

        #endregion Public Properties

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StyleBase"/> class with default settings.
        /// </summary>
        public StyleBase()
        {
            Border = new Border();
            Fill = new SolidFill();
            TextFill = new SolidFill (Color.Black);
            Font = GetDefaultFontInternal();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Assigns values from another source.
        /// </summary>
        /// <param name="source">Source to assign from.</param>
        public virtual void Assign (StyleBase source)
        {
            Border = source.Border.Clone();
            Fill = source.Fill.Clone();
            TextFill = source.TextFill.Clone();
            Font = source.Font;
            ApplyBorder = source.ApplyBorder;
            ApplyFill = source.ApplyFill;
            ApplyTextFill = source.ApplyTextFill;
            ApplyFont = source.ApplyFont;
        }

        /// <summary>
        /// Deserializes the style.
        /// </summary>
        /// <param name="reader">Reader object.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public void Deserialize (FRReader reader)
        {
            reader.ReadProperties (this);
            Fill.Deserialize (reader, "Fill");
            TextFill.Deserialize (reader, "TextFill");
        }

        /// <summary>
        /// Serializes the style.
        /// </summary>
        /// <param name="writer">Writer object.</param>
        /// <remarks>
        /// This method is for internal use only.
        /// </remarks>
        public virtual void Serialize (ReportWriter writer)
        {
            var c = writer.DiffObject as StyleBase;

            Border.Serialize (writer, "Border", c.Border);
            Fill.Serialize (writer, "Fill", c.Fill);
            TextFill.Serialize (writer, "TextFill", c.TextFill);
            if ((writer.SerializeTo != SerializeTo.Preview || !Font.Equals (c.Font)) && writer.ItemName != "inherited")
            {
                writer.WriteValue ("Font", Font);
            }

            if (ApplyBorder != c.ApplyBorder)
            {
                writer.WriteBool ("ApplyBorder", ApplyBorder);
            }

            if (ApplyFill != c.ApplyFill)
            {
                writer.WriteBool ("ApplyFill", ApplyFill);
            }

            if (ApplyTextFill != c.ApplyTextFill)
            {
                writer.WriteBool ("ApplyTextFill", ApplyTextFill);
            }

            if (ApplyFont != c.ApplyFont)
            {
                writer.WriteBool ("ApplyFont", ApplyFont);
            }
        }

        #endregion Public Methods
    }
}
