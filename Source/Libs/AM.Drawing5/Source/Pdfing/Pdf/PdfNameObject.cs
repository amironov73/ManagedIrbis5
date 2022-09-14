// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using PdfSharpCore.Pdf.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents an indirect name value. This type is not used by PdfSharpCore. If it is imported from
/// an external PDF file, the value is converted into a direct object. Acrobat sometime uses indirect
/// names to save space, because an indirect reference to a name may be shorter than a long name.
/// </summary>
[DebuggerDisplay ("({Value})")]
public sealed class PdfNameObject
    : PdfObject
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNameObject"/> class.
    /// </summary>
    public PdfNameObject()
    {
        Value = "/"; // Empty name.
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfNameObject"/> class.
    /// </summary>
    /// <param name="document">The document.</param>
    /// <param name="value">The value.</param>
    public PdfNameObject (PdfDocument document, string value)
        : base (document)
    {
        if (value == null)
        {
            throw new ArgumentNullException (nameof (value));
        }

        if (value.Length == 0 || value[0] != '/')
        {
            throw new ArgumentException (PSSR.NameMustStartWithSlash);
        }

        Value = value;
    }

    #endregion

    /// <inheritdoc cref="object.Equals(object?)"/>
    public override bool Equals (object? obj)
    {
        return Value.Equals (obj);
    }

    /// <inheritdoc cref="object.GetHashCode"/>
    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    /// <summary>
    /// Gets or sets the name value.
    /// </summary>
    public string Value { get; set; }

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        // TODO: Encode characters.
        return Value;
    }

    /// <summary>
    /// Determines whether a name is equal to a string.
    /// </summary>
    public static bool operator == (PdfNameObject? name, string? str)
    {
        return name?.Value == str;
    }

    /// <summary>
    /// Determines whether a name is not equal to a string.
    /// </summary>
    public static bool operator != (PdfNameObject? name, string? str)
    {
        return name?.Value != str;
    }

    /// <summary>
    /// Writes the name including the leading slash.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.WriteBeginObject (this);
        writer.Write (new PdfName (Value));
        writer.WriteEndObject();
    }
}
