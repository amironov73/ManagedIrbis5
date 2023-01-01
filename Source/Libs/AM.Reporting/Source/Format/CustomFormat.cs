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
using System.Globalization;
using System.Text;

using AM.Reporting.Utils;

#endregion

#nullable enable

namespace AM.Reporting.Format
{
    /// <summary>
    /// Represents a format that uses the <b>Format</b> string to display values.
    /// </summary>
    public class CustomFormat : FormatBase
    {
        #region Fields

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a format string.
        /// </summary>
        /// <remarks>
        /// Default format is "G". For example, if you want to format a date, use the following
        /// format string: "MM/dd/yyyy". See the <b>System.String.Format</b> method for list
        /// of possible format strings.
        /// </remarks>
        public string Format { get; set; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public override FormatBase Clone()
        {
            var result = new CustomFormat
            {
                Format = Format
            };
            return result;
        }

        /// <inheritdoc/>
        public override bool Equals (object obj)
        {
            var f = obj as CustomFormat;
            return f != null && Format == f.Format;
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

            return string.Format ("{0:" + Format + "}", value);
        }

        internal override string GetSampleValue()
        {
            return "";
        }

        internal override void Serialize (ReportWriter writer, string prefix, FormatBase format)
        {
            base.Serialize (writer, prefix, format);

            if (format is not CustomFormat c || Format != c.Format)
            {
                writer.WriteStr (prefix + "Format", Format);
            }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <b>CustomFormat</b> class with default settings.
        /// </summary>
        public CustomFormat()
        {
            Format = "G";
        }
    }
}
