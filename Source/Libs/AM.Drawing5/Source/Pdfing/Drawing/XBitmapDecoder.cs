// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XBitmapDecoder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Provides functionality to load a bitmap image encoded in a specific format.
/// </summary>
public class XBitmapDecoder
{
    internal XBitmapDecoder()
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Gets a new instance of the PNG image decoder.
    /// </summary>
    public static XBitmapDecoder GetPngDecoder()
    {
        return new XPngBitmapDecoder();
    }
}

internal sealed class XPngBitmapDecoder
    : XBitmapDecoder
{
    internal XPngBitmapDecoder()
    {
        // пустое тело конструктора
    }
}
