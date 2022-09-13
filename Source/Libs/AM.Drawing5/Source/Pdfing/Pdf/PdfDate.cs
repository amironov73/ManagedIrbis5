// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using PdfSharpCore.Pdf.IO;

#endregion

namespace PdfSharpCore.Pdf
{
    /// <summary>
    /// Represents a direct date value.
    /// </summary>
    [DebuggerDisplay("({Value})")]
    public sealed class PdfDate : PdfItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfDate"/> class.
        /// </summary>
        public PdfDate(string value)
        {
            _value = Parser.ParseDateTime(value, DateTime.MinValue);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfDate"/> class.
        /// </summary>
        public PdfDate(DateTime value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value as DateTime.
        /// </summary>
        public DateTime Value =>
            // This class must behave like a value type. Therefore it cannot be changed (like System.String).
            _value;

        readonly DateTime _value;

        /// <summary>
        /// Returns the value in the PDF date format.
        /// </summary>
        public override string ToString()
        {
            var delta = _value.ToString("zzz").Replace(':', '\'');
            return $"D:{_value:yyyyMMddHHmmss}{delta}'";
        }

        /// <summary>
        /// Writes the value in the PDF date format.
        /// </summary>
        internal override void WriteObject(PdfWriter writer)
        {
            writer.WriteDocString(ToString());
        }
    }
}
