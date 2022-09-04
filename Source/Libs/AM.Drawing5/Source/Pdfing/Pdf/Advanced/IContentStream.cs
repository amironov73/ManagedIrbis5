// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* IContentStream.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Advanced;

internal interface IContentStream
{
    PdfResources? Resources { get; }

    string? GetFontName (XFont font, out PdfFont? pdfFont);

    string? GetFontName (string idName, byte[] fontData, out PdfFont? pdfFont);

    string GetImageName (XImage image);

    string GetFormName (XForm form);
}
