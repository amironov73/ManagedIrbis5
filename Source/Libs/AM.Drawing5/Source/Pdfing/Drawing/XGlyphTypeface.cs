// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XGlyphTypeface.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;

using PdfSharpCore.Fonts;
using PdfSharpCore.Fonts.OpenType;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Specifies a physical font face that corresponds to a font file on the disk or in memory.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
internal sealed class XGlyphTypeface
{
    // Implementation Notes
    // XGlyphTypeface is the centerpiece for font management. There is a one to one relationship
    // between XFont an XGlyphTypeface.
    //
    // * Each XGlyphTypeface can belong to one or more XFont objects.
    // * An XGlyphTypeface hold an XFontFamily.
    // * XGlyphTypeface hold a reference to an OpenTypeFontface.
    // *
    //

    const string KeyPrefix = "tk:"; // "typeface key"

    public XGlyphTypeface (string key, XFontSource fontSource)
    {
        FaceName = null!;
        FamilyName = null!;
        StyleName = null!;
        DisplayName = null!;

        var familyName = fontSource.Fontface!._name!.Name;
        FontFamily = new XFontFamily (familyName, false);
        Fontface = fontSource.Fontface;
        IsBold = Fontface._os2!.IsBold;
        IsItalic = Fontface._os2.IsItalic;

        _key = key;

        //_fontFamily =xfont  FontFamilyCache.GetFamilyByName(familyName);
        FontSource = fontSource;

        Initialize();
    }

    public static XGlyphTypeface GetOrCreateFrom (string familyName, FontResolvingOptions fontResolvingOptions)
    {
        // Check cache for requested type face.
        var typefaceKey = ComputeKey (familyName, fontResolvingOptions);
        if (GlyphTypefaceCache.TryGetGlyphTypeface (typefaceKey, out var glyphTypeface))
        {
            // Just return existing one.
            return glyphTypeface;
        }

        // Resolve typeface by FontFactory.
        var fontResolverInfo = FontFactory.ResolveTypeface (familyName, fontResolvingOptions, typefaceKey);
        if (fontResolverInfo == null)
        {
            // No fallback - just stop.
            throw new InvalidOperationException ("No appropriate font found.");
        }

        // Now create the font family at the first.
        var platformFontResolverInfo = fontResolverInfo as PlatformFontResolverInfo;
        if (platformFontResolverInfo != null)
        {
        }
        else
        {
            // Create new and exclusively used font family for custom font resolver retrieved font source.
            XFontFamily.CreateSolitary (fontResolverInfo.FaceName);
        }

        // We have a valid font resolver info. That means we also have an XFontSource object loaded in the cache.
        ////XFontSource fontSource = FontFactory.GetFontSourceByTypefaceKey(fontResolverInfo.FaceName);
        var fontSource = FontFactory.GetFontSourceByFontName (fontResolverInfo.FaceName);
        Debug.Assert (fontSource != null);

        // Each font source already contains its OpenTypeFontface.
        glyphTypeface = new XGlyphTypeface (typefaceKey, fontSource);
        GlyphTypefaceCache.AddGlyphTypeface (glyphTypeface);

        return glyphTypeface;
    }

    public XFontFamily FontFamily { get; }

    internal OpenTypeFontface Fontface { get; }

    public XFontSource FontSource { get; }


    void Initialize()
    {
        FamilyName = Fontface._name!.Name;
        if (string.IsNullOrEmpty (FaceName) || FaceName.StartsWith ("?"))
        {
            FaceName = FamilyName;
        }

        StyleName = Fontface._name.Style;
        DisplayName = Fontface._name.FullFontName;
        if (string.IsNullOrEmpty (DisplayName))
        {
            DisplayName = FamilyName;
            if (string.IsNullOrEmpty (StyleName))
            {
                DisplayName += " (" + StyleName + ")";
            }
        }

        // Bold, as defined in OS/2 table.
        IsBold = Fontface._os2!.IsBold;

        // Debug.Assert(_isBold == (_fontface.os2.usWeightClass > 400), "Check font weight.");

        // Italic, as defined in OS/2 table.
        IsItalic = Fontface._os2.IsItalic;
    }

    /// <summary>
    /// Gets the name of the font face. This can be a file name, an uri, or a GUID.
    /// </summary>
    internal string FaceName { get; private set; }

    /// <summary>
    /// Gets the English family name of the font, for example "Arial".
    /// </summary>
    public string FamilyName { get; private set; }

    /// <summary>
    /// Gets the English subfamily name of the font,
    /// for example "Bold".
    /// </summary>
    public string StyleName { get; private set; }

    /// <summary>
    /// Gets the English display name of the font,
    /// for example "Arial italic".
    /// </summary>
    public string DisplayName { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the font weight is bold.
    /// </summary>
    public bool IsBold { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the font style is italic.
    /// </summary>
    public bool IsItalic { get; private set; }

    public XStyleSimulations StyleSimulations { get; }

    /// <summary>
    /// Gets the suffix of the face name in a PDF font and font descriptor.
    /// The name based on the effective value of bold and italic from the OS/2 table.
    /// </summary>
    string GetFaceNameSuffix()
    {
        // Use naming of Microsoft Word.
        if (IsBold)
        {
            return IsItalic ? ",BoldItalic" : ",Bold";
        }

        return IsItalic ? ",Italic" : "";
    }

    internal string GetBaseName()
    {
        var name = DisplayName;
        var ich = name.IndexOf ("bold", StringComparison.OrdinalIgnoreCase);
        if (ich > 0)
        {
            name = name.Substring (0, ich) + name.Substring (ich + 4, name.Length - ich - 4);
        }

        ich = name.IndexOf ("italic", StringComparison.OrdinalIgnoreCase);
        if (ich > 0)
        {
            name = name.Substring (0, ich) + name.Substring (ich + 6, name.Length - ich - 6);
        }

        //name = name.Replace(" ", "");
        name = name.Trim();
        name += GetFaceNameSuffix();
        return name;
    }

    /// <summary>
    /// Computes the bijective key for a typeface.
    /// </summary>
    internal static string ComputeKey (string familyName, FontResolvingOptions fontResolvingOptions)
    {
        // Compute a human readable key.
        var simulationSuffix = "";
        if (fontResolvingOptions.OverrideStyleSimulations)
        {
            switch (fontResolvingOptions.StyleSimulations)
            {
                case XStyleSimulations.BoldSimulation:
                    simulationSuffix = "|b+/i-";
                    break;
                case XStyleSimulations.ItalicSimulation:
                    simulationSuffix = "|b-/i+";
                    break;
                case XStyleSimulations.BoldItalicSimulation:
                    simulationSuffix = "|b+/i+";
                    break;
                case XStyleSimulations.None: break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        var key = KeyPrefix + familyName.ToLowerInvariant()
                            + (fontResolvingOptions.IsItalic ? "/i" : "/n") // normal / oblique / italic
                            + (fontResolvingOptions.IsBold ? "/700" : "/400") + "/5" // Stretch.Normal
                            + simulationSuffix;
        return key;
    }

    /// <summary>
    /// Computes the bijective key for a typeface.
    /// </summary>
    internal static string ComputeKey (string familyName, bool isBold, bool isItalic)
    {
        return ComputeKey (familyName, new FontResolvingOptions (FontHelper.CreateStyle (isBold, isItalic)));
    }

    public string Key => _key;

    readonly string _key;

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>

    // ReSharper disable UnusedMember.Local
    internal string DebuggerDisplay => string.Format (CultureInfo.InvariantCulture, "{0} - {1} ({2})", FamilyName, StyleName, FaceName); // ReSharper restore UnusedMember.Local
}
