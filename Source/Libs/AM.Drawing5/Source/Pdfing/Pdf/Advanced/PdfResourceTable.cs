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

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

/// <summary>
/// Base class for FontTable, ImageTable, FormXObjectTable etc.
/// </summary>
public class PdfResourceTable
{
    /// <summary>
    /// Base class for document wide resource tables.
    /// </summary>
    public PdfResourceTable
        (
            PdfDocument owner
        )
    {
        Sure.NotNull (owner);

        Owner = owner;
    }

    /// <summary>
    /// Gets the owning document of this resource table.
    /// </summary>
    protected PdfDocument Owner { get; }
}
