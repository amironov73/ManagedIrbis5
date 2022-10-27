// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* XFont.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Globalization;
using System.ComponentModel;

using PdfSharpCore.Fonts;
using PdfSharpCore.Fonts.OpenType;
using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing;

/// <summary>
/// Defines an object used to draw text.
/// </summary>
[DebuggerDisplay ("{DebuggerDisplay}")]
public sealed class XFont
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="XFont"/> class.
    /// </summary>
    /// <param name="familyName">Name of the font family.</param>
    /// <param name="emSize">The em size.</param>
    public XFont
        (
            string familyName,
            double emSize
        )
        : this
            (
                familyName,
                emSize,
                XFontStyle.Regular,
                new XPdfFontOptions (GlobalFontSettings.DefaultFontEncoding)
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XFont"/> class.
    /// </summary>
    /// <param name="familyName">Name of the font family.</param>
    /// <param name="emSize">The em size.</param>
    /// <param name="style">The font style.</param>
    public XFont
        (
            string familyName,
            double emSize,
            XFontStyle style
        )
        : this
            (
                familyName,
                emSize,
                style,
                new XPdfFontOptions (GlobalFontSettings.DefaultFontEncoding)
            )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XFont"/> class.
    /// </summary>
    /// <param name="familyName">Name of the font family.</param>
    /// <param name="emSize">The em size.</param>
    /// <param name="style">The font style.</param>
    /// <param name="pdfOptions">Additional PDF options.</param>
    public XFont
        (
            string familyName,
            double emSize,
            XFontStyle style,
            XPdfFontOptions pdfOptions
        )
    {
        FamilyName = familyName;
        _emSize = emSize;
        _style = style;
        _pdfOptions = pdfOptions;
        Initialize();
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="familyName"></param>
    /// <param name="emSize"></param>
    /// <param name="style"></param>
    /// <param name="pdfOptions"></param>
    /// <param name="styleSimulations"></param>
    internal XFont
        (
            string familyName,
            double emSize,
            XFontStyle style,
            XPdfFontOptions pdfOptions,
            XStyleSimulations styleSimulations
        )
    {
        FamilyName = familyName;
        _emSize = emSize;
        _style = style;
        _pdfOptions = pdfOptions;
        OverrideStyleSimulations = true;
        StyleSimulations = styleSimulations;
        Initialize();
    }

    #endregion

    /// <summary>
    /// Initializes this instance by computing the glyph typeface, font family, font source and TrueType fontface.
    /// (PDFsharp currently only deals with TrueType fonts.)
    /// </summary>
    void Initialize()
    {
#if DEBUG
        if (FamilyName == "Segoe UI Semilight" && (_style & XFontStyle.BoldItalic) == XFontStyle.Italic)
        {
            GetType();
        }
#endif

        var fontResolvingOptions = OverrideStyleSimulations
            ? new FontResolvingOptions (_style, StyleSimulations)
            : new FontResolvingOptions (_style);

        // HACK: 'PlatformDefault' is used in unit test code.
        if (StringComparer.OrdinalIgnoreCase.Compare (FamilyName, GlobalFontSettings.DefaultFontName) == 0)
        {
        }

        // In principle an XFont is an XGlyphTypeface plus an em-size.
        GlyphTypeface = XGlyphTypeface.GetOrCreateFrom (FamilyName, fontResolvingOptions);
        CreateDescriptorAndInitializeFontMetrics();
    }

    /// <summary>
    /// Code separated from Metric getter to make code easier to debug.
    /// (Setup properties in their getters caused side effects during debugging because Visual Studio calls a getter
    /// to early to show its value in a debugger window.)
    /// </summary>
    void CreateDescriptorAndInitializeFontMetrics() // TODO: refactor
    {
        Debug.Assert (_fontMetrics == null, "InitializeFontMetrics() was already called.");
        Descriptor =
            (OpenTypeDescriptor)FontDescriptorCache
                .GetOrCreateDescriptorFor (this); //_familyName, _style, _glyphTypeface.Fontface);
        _fontMetrics = new XFontMetrics (Descriptor.FontName, Descriptor.UnitsPerEm, Descriptor.Ascender,
            Descriptor.Descender,
            Descriptor.Leading, Descriptor.LineSpacing, Descriptor.CapHeight, Descriptor.XHeight, Descriptor.StemV,
            0, 0, 0,
            Descriptor.UnderlinePosition, Descriptor.UnderlineThickness, Descriptor.StrikeoutPosition,
            Descriptor.StrikeoutSize);

        var fm = Metrics;

        // Already done in CreateDescriptorAndInitializeFontMetrics.
        //if (_descriptor == null)
        //    _descriptor = (OpenTypeDescriptor)FontDescriptorStock.Global.CreateDescriptor(this);  //(Name, (XGdiFontStyle)Font.Style);

        UnitsPerEm = Descriptor.UnitsPerEm;
        CellAscent = Descriptor.Ascender;
        CellDescent = Descriptor.Descender;
        CellSpace = Descriptor.LineSpacing;
        Debug.Assert (fm.UnitsPerEm == Descriptor.UnitsPerEm);
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    /// <summary>
    /// Gets the XFontFamily object associated with this XFont object.
    /// </summary>
    [Browsable (false)]
    public XFontFamily FontFamily
    {
        get { return GlyphTypeface.FontFamily; }
    }

    /// <summary>
    /// WRONG: Gets the face name of this Font object.
    /// Indeed it returns the font family name.
    /// </summary>

    // [Obsolete("This function returns the font family name, not the face name. Use xxx.FontFamily.Name or xxx.FaceName")]
    public string Name
    {
        get { return GlyphTypeface.FontFamily.Name; }
    }

    internal string FaceName
    {
        get { return GlyphTypeface.FaceName; }
    }

    /// <summary>
    /// Gets the em-size of this font measured in the unit of this font object.
    /// </summary>
    public double Size
    {
        get { return _emSize; }
    }

    readonly double _emSize;

    /// <summary>
    /// Gets style information for this Font object.
    /// </summary>
    [Browsable (false)]
    public XFontStyle Style
    {
        get { return _style; }
    }

    readonly XFontStyle _style;

    /// <summary>
    /// Indicates whether this XFont object is bold.
    /// </summary>
    public bool Bold
    {
        get { return (_style & XFontStyle.Bold) == XFontStyle.Bold; }
    }

    /// <summary>
    /// Indicates whether this XFont object is italic.
    /// </summary>
    public bool Italic
    {
        get { return (_style & XFontStyle.Italic) == XFontStyle.Italic; }
    }

    /// <summary>
    /// Indicates whether this XFont object is stroke out.
    /// </summary>
    public bool Strikeout
    {
        get { return (_style & XFontStyle.Strikeout) == XFontStyle.Strikeout; }
    }

    /// <summary>
    /// Indicates whether this XFont object is underlined.
    /// </summary>
    public bool Underline
    {
        get { return (_style & XFontStyle.Underline) == XFontStyle.Underline; }
    }

    /// <summary>
    /// Temporary HACK for XPS to PDF converter.
    /// </summary>
    internal bool IsVertical { get; set; }


    /// <summary>
    /// Gets the PDF options of the font.
    /// </summary>
    public XPdfFontOptions PdfOptions
    {
        get { return _pdfOptions ??= new (); }
    }

    private XPdfFontOptions? _pdfOptions;

    /// <summary>
    /// Indicates whether this XFont is encoded as Unicode.
    /// </summary>
    internal bool Unicode
    {
        get { return _pdfOptions != null && _pdfOptions.FontEncoding == PdfFontEncoding.Unicode; }
    }

    /// <summary>
    /// Gets the cell space for the font. The CellSpace is the line spacing, the sum of CellAscent and CellDescent and optionally some extra space.
    /// </summary>
    public int CellSpace { get; internal set; }

    /// <summary>
    /// Gets the cell ascent, the area above the base line that is used by the font.
    /// </summary>
    public int CellAscent { get; internal set; }

    /// <summary>
    /// Gets the cell descent, the area below the base line that is used by the font.
    /// </summary>
    public int CellDescent { get; internal set; }

    /// <summary>
    /// Gets the font metrics.
    /// </summary>
    /// <value>The metrics.</value>
    public XFontMetrics Metrics
    {
        get
        {
            // Code moved to InitializeFontMetrics().
            //if (_fontMetrics == null)
            //{
            //    FontDescriptor descriptor = FontDescriptorStock.Global.CreateDescriptor(this);
            //    _fontMetrics = new XFontMetrics(descriptor.FontName, descriptor.UnitsPerEm, descriptor.Ascender, descriptor.Descender,
            //        descriptor.Leading, descriptor.LineSpacing, descriptor.CapHeight, descriptor.XHeight, descriptor.StemV, 0, 0, 0);
            //}
            Debug.Assert (_fontMetrics != null, "InitializeFontMetrics() not yet called.");
            return _fontMetrics;
        }
    }

    XFontMetrics _fontMetrics;

    /// <summary>
    /// Returns the line spacing, in pixels, of this font. The line spacing is the vertical distance
    /// between the base lines of two consecutive lines of text. Thus, the line spacing includes the
    /// blank space between lines along with the height of the character itself.
    /// </summary>
    public double GetHeight()
    {
        var value = CellSpace * _emSize / UnitsPerEm;
        return value;
    }

    /// <summary>
    /// Gets the line spacing of this font.
    /// </summary>
    [Browsable (false)]
    public int Height
    {
        // Implementation from System.Drawing.Font.cs
        get { return (int)Math.Ceiling (GetHeight()); }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    internal XGlyphTypeface GlyphTypeface { get; private set; }


    internal OpenTypeDescriptor Descriptor { get; private set; }


    internal string FamilyName { get; }


    internal int UnitsPerEm { get; set; }

    /// <summary>
    /// Override style simulations by using the value of StyleSimulations.
    /// </summary>
    internal bool OverrideStyleSimulations;

    /// <summary>
    /// Used to enforce style simulations by renderer. For development purposes only.
    /// </summary>
    internal XStyleSimulations StyleSimulations;

    /// <summary>
    /// Cache PdfFontTable.FontSelector to speed up finding the right PdfFont
    /// if this font is used more than once.
    /// </summary>
    internal string? Selector { get; set; }

    /// <summary>
    /// Gets the DebuggerDisplayAttribute text.
    /// </summary>

    // ReSharper disable UnusedMember.Local
    string DebuggerDisplay

        // ReSharper restore UnusedMember.Local
    {
        get { return string.Format (CultureInfo.InvariantCulture, "font=('{0}' {1:0.##})", Name, Size); }
    }
}
