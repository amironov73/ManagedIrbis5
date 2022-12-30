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
using System.Text;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Format
{
    /// <summary>
    /// Defines how boolean values are formatted and displayed.
    /// </summary>
    public class BooleanFormat : FormatBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a string that will be displayed if value is <b>false</b>.
        /// </summary>
        public string FalseText { get; set; }

        /// <summary>
        /// Gets or sets a string that will be displayed if value is <b>true</b>.
        /// </summary>
        public string TrueText { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override FormatBase Clone()
        {
            var result = new BooleanFormat
            {
                FalseText = FalseText,
                TrueText = TrueText
            };
            return result;
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            var f = obj as BooleanFormat;
            return f != null && FalseText == f.FalseText && TrueText == f.TrueText;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string FormatValue (object value)
        {
            if (value is Variant variant)
            {
                value = variant.Value;
            }

            if (value is bool b && b == false)
            {
                return FalseText;
            }

            if (value is bool value1 && value1 == true)
            {
                return TrueText;
            }

            return value.ToString();
        }

        internal override string GetSampleValue()
        {
            return FormatValue (false);
        }

        internal override void Serialize (FRWriter writer, string prefix, FormatBase format)
        {
            base.Serialize (writer, prefix, format);
            var c = format as BooleanFormat;

            if (c == null || TrueText != c.TrueText)
            {
                writer.WriteStr (prefix + "TrueText", TrueText);
            }

            if (c == null || FalseText != c.FalseText)
            {
                writer.WriteStr (prefix + "FalseText", FalseText);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>BooleanFormat</b> class with default settings.
        /// </summary>
        public BooleanFormat()
        {
            FalseText = "False";
            TrueText = "True";
        }
    }
}
