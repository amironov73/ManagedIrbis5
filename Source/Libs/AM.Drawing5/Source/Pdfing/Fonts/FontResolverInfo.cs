// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontResolverInfo.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.Globalization;

using AM;

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts;

//
// The English terms font, font family, typeface, glyph etc. are sometimes confusingly used.
// Here a short clarification by Wikipedia.
//
// Wikipedia EN -> DE
//     Font -> Schriftschnitt
//     Computer font -> Font (Informationstechnik)
//     Typeface (Font family) -> Schriftart / Schriftfamilie
//     Glyph -> Glyphe
//
// It seems that typeface and font family are synonyms in english.
// In WPF a family name is used as a term for a bunch of fonts that share the same
// characteristics, like Univers or Times New Roman.
// In WPF a fontface describes a request of a font of a particular font family, e.g.
// Univers medium bold italic.
// In WPF a glyph typeface is the result of requesting a typeface, i.e. a physical font
// plus the information whether bold and/or italic should be simulated.
//
// Wikipedia DE -> EN
//     Schriftart -> Typeface
//     Schriftschnitt -> Font
//     Schriftfamilie -> ~   (means Font family)
//     Schriftsippe -> Font superfamily
//     Font -> Computer font
//
// http://en.wikipedia.org/wiki/Font
// http://en.wikipedia.org/wiki/Computer_font
// http://en.wikipedia.org/wiki/Typeface
// http://en.wikipedia.org/wiki/Glyph
// http://en.wikipedia.org/wiki/Typographic_unit
//
// FaceName: A unique and only internally used name of a glyph typeface. In other words the name of the font data that represents a specific font.
//
//

/// <summary>
/// Describes the physical font that must be used to render a particular XFont.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
public class FontResolverInfo
{
    private const string KeyPrefix = "frik:"; // Font Resolver Info Key

    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="FontResolverInfo"/> struct.
    /// </summary>
    /// <param name="faceName">The name that uniquely identifies the fontface.</param>
    public FontResolverInfo (string faceName) :
        this (faceName, false, false, 0)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontResolverInfo"/> struct.
    /// </summary>
    /// <param name="faceName">The name that uniquely identifies the fontface.</param>
    /// <param name="mustSimulateBold">Set to <c>true</c> to simulate bold when rendered. Not implemented and must be false.</param>
    /// <param name="mustSimulateItalic">Set to <c>true</c> to simulate italic when rendered.</param>
    /// <param name="collectionNumber">Index of the font in a true type font collection.
    /// Not yet implemented and must be zero.
    /// </param>
    internal FontResolverInfo
        (
            string faceName,
            bool mustSimulateBold,
            bool mustSimulateItalic,
            int collectionNumber
        )
    {
        Sure.NotNullNorEmpty (faceName);
        Sure.AssertState
            (
                collectionNumber != 0,
                "collectionNumber is not yet implemented and must be 0."
            );

        FaceName = faceName;
        MustSimulateBold = mustSimulateBold;
        MustSimulateItalic = mustSimulateItalic;
        CollectionNumber = collectionNumber;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontResolverInfo"/> struct.
    /// </summary>
    /// <param name="faceName">The name that uniquely identifies the fontface.</param>
    /// <param name="mustSimulateBold">Set to <c>true</c> to simulate bold when rendered. Not implemented and must be false.</param>
    /// <param name="mustSimulateItalic">Set to <c>true</c> to simulate italic when rendered.</param>
    public FontResolverInfo
        (
            string faceName,
            bool mustSimulateBold,
            bool mustSimulateItalic
        )
        : this (faceName, mustSimulateBold, mustSimulateItalic, 0)
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FontResolverInfo" /> struct.
    /// </summary>
    /// <param name="faceName">The name that uniquely identifies the fontface.</param>
    /// <param name="styleSimulations">The style simulation flags.</param>
    public FontResolverInfo
        (
            string faceName,
            XStyleSimulations styleSimulations
        )
        : this
            (
                faceName,
                (styleSimulations & XStyleSimulations.BoldSimulation) == XStyleSimulations.BoldSimulation,
                (styleSimulations & XStyleSimulations.ItalicSimulation) == XStyleSimulations.ItalicSimulation,
                0
            )
    {
        // пустое тело конструктора
    }

    #endregion

    /// <summary>
    /// Gets the key for this object.
    /// </summary>
    internal string Key
    {
        get
        {
            return _key ??= KeyPrefix
                + FaceName.ToLowerInvariant()
                + '/'
                + (MustSimulateBold ? "b+" : "b-")
                + (MustSimulateItalic ? "i+" : "i-");
        }
    }

    private string? _key;

    /// <summary>
    /// A name that uniquely identifies the font (not the family), e.g. the file name of the font. PDFsharp does not use this
    /// name internally, but passes it to the GetFont function of the IFontResolver interface to retrieve the font data.
    /// </summary>
    public string FaceName { get; }

    /// <summary>
    /// Indicates whether bold must be simulated. Bold simulation is not implemented in PdfSharpCore.
    /// </summary>
    public bool MustSimulateBold { get; }

    /// <summary>
    /// Indicates whether italic must be simulated.
    /// </summary>
    public bool MustSimulateItalic { get; }

    /// <summary>
    /// Gets the style simulation flags.
    /// </summary>
    public XStyleSimulations StyleSimulations =>
        (MustSimulateBold ? XStyleSimulations.BoldSimulation : 0)
        | (MustSimulateItalic ? XStyleSimulations.ItalicSimulation : 0);

    /// <summary>
    /// The number of the font in a Truetype font collection file. The number of the first font is 0.
    /// NOT YET IMPLEMENTED. Must be zero.
    /// </summary>
    internal int CollectionNumber // TODO : Find a better name.
    {
        get;
    }

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>
    internal string DebuggerDisplay =>
        string.Format (CultureInfo.InvariantCulture, "FontResolverInfo: '{0}',{1}{2}", FaceName,
            MustSimulateBold ? " simulate Bold" : "",
            MustSimulateItalic ? " simulate Italic" : "");
}
