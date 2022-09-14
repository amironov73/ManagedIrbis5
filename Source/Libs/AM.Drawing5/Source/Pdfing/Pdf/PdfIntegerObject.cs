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

using AM;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents an indirect integer value.
/// This type is not used by PdfSharpCore. If it is imported from
/// an external PDF file, the value is converted into a direct object.
/// </summary>
public sealed class PdfIntegerObject
    : PdfNumberObject
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfIntegerObject"/> class.
    /// </summary>
    public PdfIntegerObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfIntegerObject"/> class.
    /// </summary>
    public PdfIntegerObject
        (
            int value
        )
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfIntegerObject"/> class.
    /// </summary>
    public PdfIntegerObject
        (
            PdfDocument document,
            int value
        )
        : base (document)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    /// Gets the value as integer.
    /// </summary>
    public int Value { get; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return Value.ToInvariantString();
    }

    /// <summary>
    /// Writes the integer literal.
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
