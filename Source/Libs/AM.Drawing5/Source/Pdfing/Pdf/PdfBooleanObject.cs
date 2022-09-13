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
/// Represents an indirect boolean value.
/// This type is not used by PdfSharpCore. If it is imported from
/// an external PDF file, the value is converted into a direct object.
/// </summary>
public sealed class PdfBooleanObject
    : PdfObject
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfBooleanObject"/> class.
    /// </summary>
    public PdfBooleanObject()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfBooleanObject"/> class.
    /// </summary>
    public PdfBooleanObject (bool value)
    {
        Value = value;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfBooleanObject"/> class.
    /// </summary>
    public PdfBooleanObject (PdfDocument document, bool value)
        : base (document)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    /// Gets the value of this instance as boolean value.
    /// </summary>
    public bool Value { get; }

    /// <summary>
    /// Returns "false" or "true".
    /// </summary>
    public override string ToString()
    {
        return Value ? bool.TrueString : bool.FalseString;
    }

    /// <summary>
    /// Writes the keyword �false� or �true�.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.WriteBeginObject (this);
        writer.Write (Value);
        writer.WriteEndObject();
    }
}
