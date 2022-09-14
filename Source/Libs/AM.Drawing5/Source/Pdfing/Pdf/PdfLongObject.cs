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

using System.Diagnostics;
using System.Globalization;

using PdfSharpCore.Pdf.IO;

#endregion

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents an indirect long value. This type is not used by PdfSharpCore. If it is imported from
/// an external PDF file, the value is converted into a direct object.
/// </summary>
[DebuggerDisplay ("({Value})")]
public sealed class PdfLongObject
    : PdfNumberObject
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfLongObject"/> class.
    /// </summary>
    public PdfLongObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfLongObject"/> class.
    /// </summary>
    public PdfLongObject
        (
            long value
        )
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfLongObject"/> class.
    /// </summary>
    public PdfLongObject
        (
            PdfDocument document,
            long value
        )
        : base (document)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    /// Gets the value as long.
    /// </summary>
    public long Value { get; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value.ToString (CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Writes the long literal.
    /// </summary>
    internal override void WriteObject
        (
            PdfWriter writer
        )
    {
        writer.WriteBeginObject (this);
        writer.Write (Value);
        writer.WriteEndObject();
    }
}
