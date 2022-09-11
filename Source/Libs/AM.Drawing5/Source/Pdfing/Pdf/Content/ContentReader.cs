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

using System.IO;

using PdfSharpCore.Pdf.Content.Objects;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Content;

/// <summary>
/// Represents the functionality for reading PDF content streams.
/// </summary>
public static class ContentReader
{
    /// <summary>
    /// Reads the content stream(s) of the specified page.
    /// </summary>
    /// <param name="page">The page.</param>
    public static CSequence ReadContent (PdfPage page)
    {
        var parser = new CParser (page);
        var sequence = parser.ReadContent();

        return sequence;
    }

    /// <summary>
    /// Reads the specified content.
    /// </summary>
    /// <param name="content">The content.</param>
    public static CSequence ReadContent (byte[] content)
    {
        var parser = new CParser (content);
        var sequence = parser.ReadContent();
        return sequence;
    }

    /// <summary>
    /// Reads the specified content.
    /// </summary>
    /// <param name="content">The content.</param>
    public static CSequence ReadContent (MemoryStream content)
    {
        var parser = new CParser (content);
        var sequence = parser.ReadContent();
        return sequence;
    }
}
