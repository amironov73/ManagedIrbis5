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

namespace PdfSharpCore.Pdf;

/// <summary>
/// Represents a indirect reference that is not in the cross reference table.
/// </summary>
public sealed class PdfNull
    : PdfItem
{
    #region Construction

    // Reference: 3.2.8  Null Object / Page 63

    PdfNull()
    {
        // пустое тело конструктора
    }

    #endregion

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return "null";
    }

    /// <inheritdoc cref="PdfItem.WriteObject"/>
    internal override void WriteObject (PdfWriter writer)
    {
        // Implementet because it must be overridden.
        writer.WriteRaw (" null ");
    }

    /// <summary>
    /// The only instance of this class.
    /// </summary>
    public static readonly PdfNull Value = new ();
}
