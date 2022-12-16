// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType;

// TODO: Needs to be refactored #???
/// <summary>
/// Base class for all font descriptors.
/// Currently only OpenTypeDescriptor is derived from this base class.
/// </summary>
internal class FontDescriptor
{
    #region Construction

    /// <summary>
    ///
    /// </summary>
    /// <param name="key"></param>
    protected FontDescriptor (string key)
    {
        Key = key;
    }

    #endregion

    public string Key { get; }


    ///// <summary>
    /////
    ///// </summary>
    //public string FontFile
    //{
    //  get { return _fontFile; }
    //  private set { _fontFile = value; }  // BUG: never set
    //}
    //string _fontFile;

    ///// <summary>
    /////
    ///// </summary>
    //public string FontType
    //{
    //  get { return _fontType; }
    //  private set { _fontType = value; }  // BUG: never set
    //}
    //string _fontType;

    /// <summary>
    ///
    /// </summary>
    public string? FontName { get; protected set; }

    ///// <summary>
    /////
    ///// </summary>
    //public string FullName
    //{
    //    get { return _fullName; }
    //    private set { _fullName = value; }  // BUG: never set
    //}
    //string _fullName;

    ///// <summary>
    /////
    ///// </summary>
    //public string FamilyName
    //{
    //    get { return _familyName; }
    //    private set { _familyName = value; }  // BUG: never set
    //}
    //string _familyName;

    /// <summary>
    ///
    /// </summary>
    public string? Weight
    {
        get;
        private set; // BUG: never set
    }

    /// <summary>
    /// Gets a value indicating whether this instance belongs to a bold font.
    /// </summary>
    public virtual bool IsBoldFace => false;

    /// <summary>
    ///
    /// </summary>
    public float ItalicAngle { get; protected set; }

    /// <summary>
    /// Gets a value indicating whether this instance belongs to an italic font.
    /// </summary>
    public virtual bool IsItalicFace => false;

    /// <summary>
    ///
    /// </summary>
    public int XMin { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int YMin { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int XMax { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int YMax { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public bool IsFixedPitch
    {
        get => _isFixedPitch;
        private set => _isFixedPitch = value; // BUG: never set
    }

    bool _isFixedPitch;

    //Rect FontBBox;

    /// <summary>
    ///
    /// </summary>
    public int UnderlinePosition { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int UnderlineThickness { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int StrikeoutPosition { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int StrikeoutSize { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public string? Version
    {
        get => _version;
        private set => _version = value; // BUG: never set
    }

    string? _version;

    ///// <summary>
    /////
    ///// </summary>
    //public string Notice
    //{
    //  get { return Notice; }
    //}
    //protected string notice;

    /// <summary>
    ///
    /// </summary>
    public string? EncodingScheme
    {
        get => _encodingScheme;
        private set => _encodingScheme = value; // BUG: never set
    }

    private string? _encodingScheme;

    /// <summary>
    ///
    /// </summary>
    public int UnitsPerEm { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int CapHeight { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int XHeight { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int Ascender { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int Descender { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int Leading { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int Flags
    {
        get => _flags;
        private set => _flags = value; // BUG: never set
    }

    int _flags;

    /// <summary>
    ///
    /// </summary>
    public int StemV { get; protected set; }

    /// <summary>
    ///
    /// </summary>
    public int LineSpacing { get; protected set; }


    internal static string ComputeKey (XFont font)
    {
        return font.GlyphTypeface.Key;

        //return ComputeKey(font.GlyphTypeface.Fontface.FullFaceName, font.Style);
        //XGlyphTypeface glyphTypeface = font.GlyphTypeface;
        //string key = glyphTypeface.Fontface.FullFaceName.ToLowerInvariant() +
        //    (glyphTypeface.IsBold ? "/b" : "") + (glyphTypeface.IsItalic ? "/i" : "");
        //return key;
    }

    internal static string ComputeKey (string name, XFontStyle style)
    {
        return ComputeKey
            (
                name,
                (style & XFontStyle.Bold) == XFontStyle.Bold,
                (style & XFontStyle.Italic) == XFontStyle.Italic
            );
    }

    internal static string ComputeKey (string name, bool isBold, bool isItalic)
    {
        var key = name.ToLowerInvariant() + '/'
                                          + (isBold ? "b" : "") + (isItalic ? "i" : "");
        return key;
    }

    internal static string ComputeKey (string name)
    {
        var key = name.ToLowerInvariant();
        return key;
    }
}
