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

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents an indirect null value.
/// This type is not used by PDFsharp, but at least
/// one tool from Adobe creates PDF files with a null object.
/// </summary>
public sealed class PdfNullObject
    : PdfObject
{
    #region Construction

    // Reference: 3.2.8  Null Object / Page 63

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNullObject"/> class.
    /// </summary>
    public PdfNullObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNullObject"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    public PdfNullObject (PdfDocument document)
        : base (document)
    {
        // пустое тело конструктора
    }

    #endregion

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return "null";
    }

    /// <summary>
    /// Writes the keyword �null�.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.WriteBeginObject (this);
        writer.WriteRaw (" null ");
        writer.WriteEndObject();
    }
}
