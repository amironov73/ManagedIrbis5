// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XFontFamily.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Fonts;
using PdfSharpCore.Fonts.OpenType;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Defines a group of typefaces having a similar basic design and certain variations in styles.
    /// </summary>
    public sealed class XFontFamily
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XFontFamily"/> class.
        /// </summary>
        /// <param name="familyName">The family name of a font.</param>
        public XFontFamily(string familyName)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, true);
        }

        internal XFontFamily(string familyName, bool createPlatformObjects)
        {
            FamilyInternal = FontFamilyInternal.GetOrCreateFromName(familyName, createPlatformObjects);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XFontFamily"/> class from FontFamilyInternal.
        /// </summary>
        XFontFamily(FontFamilyInternal fontFamilyInternal)
        {
            FamilyInternal = fontFamilyInternal;
        }
        internal static XFontFamily CreateFromName_not_used(string name, bool createPlatformFamily)
        {
            XFontFamily fontFamily = new XFontFamily(name);
            return fontFamily;
        }

        /// <summary>
        /// An XGlyphTypeface for a font souce that comes from a custom font resolver
        /// creates a solitary font family exclusively for it.
        /// </summary>
        internal static XFontFamily CreateSolitary(string name)
        {
            // Custom font resolver face names must not clash with platform family names.
            FontFamilyInternal fontFamilyInternal = FontFamilyCache.GetFamilyByName(name);
            if (fontFamilyInternal == null)
            {
                fontFamilyInternal = FontFamilyInternal.GetOrCreateFromName(name, false);
                fontFamilyInternal = FontFamilyCache.CacheOrGetFontFamily(fontFamilyInternal);
            }

            // Create font family and save it in cache. Do not try to create platform objects.
            return new XFontFamily(fontFamilyInternal);

            //// Custom font resolver face names must not clash with platform family names.
            //if (FontFamilyCache.GetFamilyByName(name) != null)
            //{
            //    // User must rename its font face to resolve naming confilict.
            //    throw new InvalidOperationException(String.Format("Font face name {0} clashs with existing family name.", name));
            //}

            //// Create font family and save it in cache. Do not try to create platform objects.
            //FontFamilyInternal fontFamilyInternal = FontFamilyInternal.GetOrCreateFromName(name, false);
            //fontFamilyInternal = FontFamilyCache.CacheFontFamily(fontFamilyInternal);
            //return new XFontFamily(fontFamilyInternal);
        }

        /// <summary>
        /// Gets the name of the font family.
        /// </summary>
        public string Name => FamilyInternal.Name;

#if true__
        public double LineSpacing
        {
            get
            {
                WpfFamily.FamilyTypefaces[0].UnderlineThickness
            }
        }

#endif

        /// <summary>
        /// Returns the cell ascent, in design units, of the XFontFamily object of the specified style.
        /// </summary>
        public int GetCellAscent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Ascender;
            return result;
        }

        /// <summary>
        /// Returns the cell descent, in design units, of the XFontFamily object of the specified style.
        /// </summary>
        public int GetCellDescent(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.Descender;
            return result;
        }

        /// <summary>
        /// Gets the height, in font design units, of the em square for the specified style.
        /// </summary>
        public int GetEmHeight(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.UnitsPerEm;
#if DEBUG_
            int headValue = descriptor.FontFace.head.unitsPerEm;
            Debug.Assert(headValue == result);
#endif
            return result;
        }

        /// <summary>
        /// Returns the line spacing, in design units, of the FontFamily object of the specified style.
        /// The line spacing is the vertical distance between the base lines of two consecutive lines of text.
        /// </summary>
        public int GetLineSpacing(XFontStyle style)
        {
            OpenTypeDescriptor descriptor = (OpenTypeDescriptor)FontDescriptorCache.GetOrCreateDescriptor(Name, style);
            int result = descriptor.LineSpacing;
            return result;
        }

        //public string GetName(int language);

        /// <summary>
        /// Indicates whether the specified FontStyle enumeration is available.
        /// </summary>
        public bool IsStyleAvailable(XFontStyle style)
        {
            XGdiFontStyle xStyle = ((XGdiFontStyle)style) & XGdiFontStyle.BoldItalic;
            return false;
        }

        /// <summary>
        /// The implementation sigleton of font family;
        /// </summary>
        internal FontFamilyInternal FamilyInternal;
    }
}
