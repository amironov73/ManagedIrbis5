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

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts;

/// <summary>
/// Parameters that affect font selection.
/// </summary>
class FontResolvingOptions
{
    #region Construction

    public FontResolvingOptions (XFontStyle fontStyle)
    {
        FontStyle = fontStyle;
    }

    public FontResolvingOptions (XFontStyle fontStyle, XStyleSimulations styleSimulations)
    {
        FontStyle = fontStyle;
        OverrideStyleSimulations = true;
        StyleSimulations = styleSimulations;
    }

    #endregion

    public bool IsBold => (FontStyle & XFontStyle.Bold) == XFontStyle.Bold;

    public bool IsItalic => (FontStyle & XFontStyle.Italic) == XFontStyle.Italic;

    public bool IsBoldItalic => (FontStyle & XFontStyle.BoldItalic) == XFontStyle.BoldItalic;

    public bool MustSimulateBold => (StyleSimulations & XStyleSimulations.BoldSimulation) == XStyleSimulations.BoldSimulation;

    public bool MustSimulateItalic => (StyleSimulations & XStyleSimulations.ItalicSimulation) == XStyleSimulations.ItalicSimulation;

    public XFontStyle FontStyle;

    public bool OverrideStyleSimulations;

    public XStyleSimulations StyleSimulations;
}
