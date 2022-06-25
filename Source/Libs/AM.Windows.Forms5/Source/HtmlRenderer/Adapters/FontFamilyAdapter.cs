// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System.Drawing;
using AM.Drawing.HtmlRenderer.Adapters;

namespace AM.Windows.Forms.HtmlRenderer.Adapters;

/// <summary>
/// Adapter for WinForms Font family object for core.
/// </summary>
internal sealed class FontFamilyAdapter : RFontFamily
{
    /// <summary>
    /// the underline win-forms font.
    /// </summary>
    private readonly FontFamily _fontFamily;

    /// <summary>
    /// Init.
    /// </summary>
    public FontFamilyAdapter(FontFamily fontFamily)
    {
        _fontFamily = fontFamily;
    }

    /// <summary>
    /// the underline win-forms font family.
    /// </summary>
    public FontFamily FontFamily
    {
        get { return _fontFamily; }
    }

    public override string Name
    {
        get { return _fontFamily.Name; }
    }
}