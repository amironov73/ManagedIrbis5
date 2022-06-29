// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

/* FontFamilyAdapter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Drawing.HtmlRenderer.Adapters;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace AM.Drawing.HtmlRenderer.PdfSharp.Adapters;

/// <summary>
/// Adapter for WinForms Font object for core.
/// </summary>
internal sealed class FontFamilyAdapter
    : RFontFamily
{
    /// <summary>
    /// the underline win-forms font.
    /// </summary>
    private readonly XFontFamily _fontFamily;

    /// <summary>
    /// Init.
    /// </summary>
    public FontFamilyAdapter(XFontFamily fontFamily)
    {
        _fontFamily = fontFamily;
    }

    /// <summary>
    /// the underline win-forms font family.
    /// </summary>
    public XFontFamily FontFamily
    {
        get { return _fontFamily; }
    }

    public override string Name
    {
        get { return _fontFamily.Name; }
    }
}
