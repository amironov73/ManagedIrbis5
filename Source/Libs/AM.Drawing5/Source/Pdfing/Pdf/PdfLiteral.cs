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

using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Internal;

#endregion

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents text that is written 'as it is' into the PDF stream. This class can lead to invalid PDF files.
/// E.g. strings in a literal are not encrypted when the document is saved with a password.
/// </summary>
public sealed class PdfLiteral
    : PdfItem
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfLiteral"/> class.
    /// </summary>
    public PdfLiteral()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance with the specified string.
    /// </summary>
    public PdfLiteral
        (
            string value
        )
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance with the culture invariant formatted specified arguments.
    /// </summary>
    public PdfLiteral
        (
            string format,
            params object[] args
        )
    {
        Value = PdfEncoders.Format (format, args);
    }

    /// <summary>
    /// Creates a literal from an XMatrix
    /// </summary>
    public static PdfLiteral FromMatrix (XMatrix matrix)
    {
        return new PdfLiteral ("[" + PdfEncoders.ToString (matrix) + "]");
    }

    #endregion

    /// <summary>
    /// Gets the value as litaral string.
    /// </summary>
    public string Value { get; } = string.Empty;

    /// <summary>
    /// Returns a string that represents the current value.
    /// </summary>
    public override string ToString()
    {
        return Value;
    }

    internal override void WriteObject (PdfWriter writer)
    {
        writer.Write (this);
    }
}
