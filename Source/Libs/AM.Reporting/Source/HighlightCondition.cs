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

using AM.Reporting.Utils;

using System.Drawing.Design;

#endregion

#nullable enable

namespace AM.Reporting
{
    /// <summary>
    /// Represents a single highlight condition used by the <see cref="TextObject.Highlight"/> property
    /// of the <see cref="TextObject"/>.
    /// </summary>
    public class HighlightCondition : StyleBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a highlight expression.
        /// </summary>
        /// <remarks>
        /// This property can contain any valid boolean expression. If value of this expression is <b>true</b>,
        /// the fill and font settings will be applied to the <b>TextObject</b>.
        /// </remarks>
        [Editor ("AM.Reporting.TypeEditors.ExpressionEditor, AM.Reporting", typeof (UITypeEditor))]
        public string Expression { get; set; }

        /// <summary>
        /// Gets or sets the visibility flag.
        /// </summary>
        /// <remarks>
        /// If this property is set to <b>false</b>, the Text object will be hidden if the
        /// condition is met.
        /// </remarks>
        public bool Visible { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override void Serialize (FRWriter writer)
        {
            var c = writer.DiffObject as HighlightCondition;
            writer.ItemName = "Condition";

            if (Expression != c.Expression)
            {
                writer.WriteStr ("Expression", Expression);
            }

            if (Visible != c.Visible)
            {
                writer.WriteBool ("Visible", Visible);
            }

            base.Serialize (writer);
        }

        /// <inheritdoc/>
        public override void Assign (StyleBase source)
        {
            base.Assign (source);
            Expression = (source as HighlightCondition).Expression;
            Visible = (source as HighlightCondition).Visible;
        }

        /// <summary>
        /// Creates exact copy of this condition.
        /// </summary>
        /// <returns>A copy of this condition.</returns>
        public HighlightCondition Clone()
        {
            var result = new HighlightCondition();
            result.Assign (this);
            return result;
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            var c = obj as HighlightCondition;
            return c != null && Expression == c.Expression && Border.Equals (c.Border) && Fill.Equals (c.Fill) &&
                   TextFill.Equals (c.TextFill) && Font.Equals (c.Font) && Visible == c.Visible &&
                   ApplyBorder == c.ApplyBorder && ApplyFill == c.ApplyFill && ApplyTextFill == c.ApplyTextFill &&
                   ApplyFont == c.ApplyFont;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="HighlightCondition"/> class with default settings.
        /// </summary>
        public HighlightCondition()
        {
            Expression = "";
            TextFill = new SolidFill (Color.Red);
            Visible = true;
            ApplyBorder = false;
            ApplyFill = false;
            ApplyTextFill = true;
            ApplyFont = false;
        }
    }
}
