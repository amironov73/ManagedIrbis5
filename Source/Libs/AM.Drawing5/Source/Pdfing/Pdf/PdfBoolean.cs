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
/// Represents a direct boolean value.
/// </summary>
public sealed class PdfBoolean
    : PdfItem
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfBoolean"/> class.
    /// </summary>
    public PdfBoolean()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PdfBoolean"/> class.
    /// </summary>
    public PdfBoolean (bool value)
    {
        Value = value;
    }

    #endregion

    /// <summary>
    /// Gets the value of this instance as boolean value.
    /// </summary>
    /// <remarks>This class must behave like a value type.
    /// Therefore it cannot be changed.</remarks>
    public bool Value { get; }

    /// <summary>
    /// A pre-defined value that represents <c>true</c>.
    /// </summary>
    public static readonly PdfBoolean True = new (true);

    /// <summary>
    /// A pre-defined value that represents <c>false</c>.
    /// </summary>
    public static readonly PdfBoolean False = new (false);

    /// <summary>
    /// Returns 'false' or 'true'.
    /// </summary>
    public override string ToString()
    {
        return Value ? bool.TrueString : bool.FalseString;
    }

    /// <summary>
    /// Writes 'true' or 'false'.
    /// </summary>
    internal override void WriteObject (PdfWriter writer)
    {
        writer.Write (this);
    }
}
