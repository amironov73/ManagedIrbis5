// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* OpenTypeDescriptor.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using PdfSharpCore.Pdf.Internal;
using PdfSharpCore.Drawing;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType
{
    /// <summary>
    /// The OpenType font descriptor.
    /// Currently the only font type PDFsharp supports.
    /// </summary>
    internal sealed class OpenTypeDescriptor : FontDescriptor
    {
        /// <summary>
        /// New...
        /// </summary>
        public OpenTypeDescriptor(string fontDescriptorKey, string name, XFontStyle stlye, OpenTypeFontface fontface, XPdfFontOptions options)
            : base(fontDescriptorKey)
        {
            FontFace = fontface;
            FontName = name;
            Initialize();
        }

        public OpenTypeDescriptor(string fontDescriptorKey, XFont font)
            : base(fontDescriptorKey)
        {
            try
            {
                FontFace = font.GlyphTypeface.Fontface;
                FontName = font.Name;
                Initialize();
            }
            catch
            {
                GetType();
                throw;
            }
        }

        internal OpenTypeDescriptor(string fontDescriptorKey, string idName, byte[] fontData)
            : base(fontDescriptorKey)
        {
            try
            {
                FontFace = new OpenTypeFontface(fontData, idName);
                // Try to get real name form name table
                if (idName.Contains("XPS-Font-") && FontFace._name != null && FontFace._name.Name.Length != 0)
                {
                    string tag = string.Empty;
                    if (idName.IndexOf('+') == 6)
                        tag = idName.Substring(0, 6);
                    idName = tag + "+" + FontFace._name.Name;
                    if (FontFace._name.Style.Length != 0)
                        idName += "," + FontFace._name.Style;
                    //idName = idName.Replace(" ", "");
                }
                FontName = idName;
                Initialize();
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        internal OpenTypeFontface FontFace;

        void Initialize()
        {
            // TODO: Respect embedding restrictions.
            //bool embeddingRestricted = fontData.os2.fsType == 0x0002;

            //fontName = image.n
            ItalicAngle = FontFace._post.italicAngle;

            XMin = FontFace._head.xMin;
            YMin = FontFace._head.yMin;
            XMax = FontFace._head.xMax;
            YMax = FontFace._head.yMax;

            UnderlinePosition = FontFace._post.underlinePosition;
            UnderlineThickness = FontFace._post.underlineThickness;
            StrikeoutPosition = FontFace._os2.yStrikeoutPosition;
            StrikeoutSize = FontFace._os2.yStrikeoutSize;

            // No documetation found how to get the set vertical stems width from the
            // TrueType tables.
            // The following formula comes from PDFlib Lite source code. Acrobat 5.0 sets
            // /StemV to 0 always. I think the value doesn't matter.
            //float weight = (float)(image.os2.usWeightClass / 65.0f);
            //stemV = (int)(50 + weight * weight);  // MAGIC
            StemV = 0;

            // PDFlib states that some Apple fonts miss the OS/2 table.
            Debug.Assert(FontFace._os2 != null, "TrueType font has no OS/2 table.");

            UnitsPerEm = FontFace._head.unitsPerEm;

            // Calculate Ascent, Descent, Leading and LineSpacing like in WPF Source Code (see FontDriver.ReadBasicMetrics)

            // OS/2 is an optional table, but we can't determine if it is existing in this font.
            bool os2SeemsToBeEmpty = FontFace._os2.sTypoAscender == 0 && FontFace._os2.sTypoDescender == 0 && FontFace._os2.sTypoLineGap == 0;
            //Debug.Assert(!os2SeemsToBeEmpty); // Are there fonts without OS/2 table?

            bool dontUseWinLineMetrics = (FontFace._os2.fsSelection & 128) != 0;
            if (!os2SeemsToBeEmpty && dontUseWinLineMetrics)
            {
                // Comment from WPF: The font specifies that the sTypoAscender, sTypoDescender, and sTypoLineGap fields are valid and
                // should be used instead of winAscent and winDescent.
                int typoAscender = FontFace._os2.sTypoAscender;
                int typoDescender = FontFace._os2.sTypoDescender;
                int typoLineGap = FontFace._os2.sTypoLineGap;

                // Comment from WPF: We include the line gap in the ascent so that white space is distributed above the line. (Note that
                // the typo line gap is a different concept than "external leading".)
                Ascender = typoAscender + typoLineGap;
                // Comment from WPF: Typo descent is a signed value where the positive direction is up. It is therefore typically negative.
                // A signed typo descent would be quite unusual as it would indicate the descender was above the baseline
                Descender = -typoDescender;
                LineSpacing = typoAscender + typoLineGap - typoDescender;
            }
            else
            {
                // Comment from WPF: get the ascender field
                int ascender = FontFace._hhea.ascender;
                // Comment from WPF: get the descender field; this is measured in the same direction as ascender and is therefore 
                // normally negative whereas we want a positive value; however some fonts get the sign wrong
                // so instead of just negating we take the absolute value.
                int descender = Math.Abs(FontFace._hhea.descender);
                // Comment from WPF: get the lineGap field and make sure it's >= 0 
                int lineGap = Math.Max((short)0, FontFace._hhea.lineGap);

                if (!os2SeemsToBeEmpty)
                {
                    // Comment from WPF: we could use sTypoAscender, sTypoDescender, and sTypoLineGap which are supposed to represent
                    // optimal typographic values not constrained by backwards compatibility; however, many fonts get 
                    // these fields wrong or get them right only for Latin text; therefore we use the more reliable 
                    // platform-specific Windows values. We take the absolute value of the win32descent in case some
                    // fonts get the sign wrong. 
                    int winAscent = FontFace._os2.usWinAscent;
                    int winDescent = Math.Abs(FontFace._os2.usWinDescent);

                    Ascender = winAscent;
                    Descender = winDescent;
                    // Comment from WPF: The following calculation for designLineSpacing is per [....]. The default line spacing 
                    // should be the sum of the Mac ascender, descender, and lineGap unless the resulting value would
                    // be less than the cell height (winAscent + winDescent) in which case we use the cell height.
                    // See also http://www.microsoft.com/typography/otspec/recom.htm.
                    // Note that in theory it's valid for the baseline-to-baseline distance to be less than the cell
                    // height. However, Windows has never allowed this for Truetype fonts, and fonts built for Windows
                    // sometimes rely on this behavior and get the hha values wrong or set them all to zero.
                    LineSpacing = Math.Max(lineGap + ascender + descender, winAscent + winDescent);
                }
                else
                {
                    Ascender = ascender;
                    Descender = descender;
                    LineSpacing = ascender + descender + lineGap;
                }
            }

            Debug.Assert(Descender >= 0);

            int cellHeight = Ascender + Descender;
            int internalLeading = cellHeight - UnitsPerEm; // Not used, only for debugging.
            int externalLeading = LineSpacing - cellHeight;
            Leading = externalLeading;

            // sCapHeight and sxHeight are only valid if Version >= 2
            if (FontFace._os2.version >= 2 && FontFace._os2.sCapHeight != 0)
                CapHeight = FontFace._os2.sCapHeight;
            else
                CapHeight = Ascender;

            if (FontFace._os2.version >= 2 && FontFace._os2.sxHeight != 0)
                XHeight = FontFace._os2.sxHeight;
            else
                XHeight = (int)(0.66 * Ascender);

            //flags = image.
            Encoding ansi = PdfEncoders.WinAnsiEncoding; // System.Text.Encoding.Default;
            Encoding unicode = Encoding.Unicode;
            byte[] bytes = new byte[256];

            bool symbol = FontFace._cmap.symbol;
            Widths = new int[256];
            for (int idx = 0; idx < 256; idx++)
            {
                bytes[idx] = (byte)idx;
                // PDFlib handles some font flaws here...
                // We wait for bug reports.

                char ch = (char)idx;
                string s = ansi.GetString(bytes, idx, 1);
                if (s.Length != 0)
                {
                    if (s[0] != ch)
                        ch = s[0];
                }

                //Debug.Assert(ch == idx);

                //int glyphIndex;
                //if (symbol)
                //{
                //    glyphIndex = idx + (FontFace.os2. usFirstCharIndex & 0xFF00);
                //    glyphIndex = CharCodeToGlyphIndex((char)glyphIndex);
                //}
                //else
                //{
                //    //Debug.Assert(idx + (fontData.os2.usFirstCharIndex & 0xFF00) == idx);
                //    //glyphIndex = CharCodeToGlyphIndex((char)idx);
                //    glyphIndex = CharCodeToGlyphIndex(ch);
                //}

                if (symbol)
                {
                    // Remap ch for symbol fonts.
                    ch = (char)(ch | (FontFace._os2.usFirstCharIndex & 0xFF00));  // @@@ refactor
                }
                int glyphIndex = CharCodeToGlyphIndex(ch);
                Widths[idx] = GlyphIndexToPdfWidth(glyphIndex);
            }
        }
        public int[] Widths;

        /// <summary>
        /// Gets a value indicating whether this instance belongs to a bold font.
        /// </summary>
        public override bool IsBoldFace
        {
            get
            {
                // usWeightClass 700 is Bold
                //Debug.Assert((fontData.os2.usWeightClass >= 700) == ((fontData.os2.fsSelection & (ushort)OS2Table.FontSelectionFlags.Bold) != 0));
                return FontFace._os2.IsBold;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance belongs to an italic font.
        /// </summary>
        public override bool IsItalicFace
        {
            get { return FontFace._os2.IsItalic; }
        }

        internal int DesignUnitsToPdf(double value)
        {
            return (int)Math.Round(value * 1000.0 / FontFace._head.unitsPerEm);
        }

        /// <summary>
        /// Maps a unicode to the index of the corresponding glyph.
        /// See OpenType spec "cmap - Character To Glyph Index Mapping Table / Format 4: Segment mapping to delta values"
        /// for details about this a little bit strange looking algorithm.
        /// </summary>
        public int CharCodeToGlyphIndex(char value)
        {
            try
            {
                CMap4 cmap4 = FontFace._cmap.cmap4;
                int segCount = cmap4.segCountX2 / 2;
                int seg;
                for (seg = 0; seg < segCount; seg++)
                {
                    if (value <= cmap4.endCount[seg])
                        break;
                }
                Debug.Assert(seg < segCount);

                if (value < cmap4.startCount[seg])
                    return 0;

                if (cmap4.idRangeOffs[seg] == 0)
                    return (value + cmap4.idDelta[seg]) & 0xFFFF;

                int idx = cmap4.idRangeOffs[seg] / 2 + (value - cmap4.startCount[seg]) - (segCount - seg);
                Debug.Assert(idx >= 0 && idx < cmap4.glyphCount);

                if (cmap4.glyphIdArray[idx] == 0)
                    return 0;

                return (cmap4.glyphIdArray[idx] + cmap4.idDelta[seg]) & 0xFFFF;
            }
            catch
            {
                GetType();
                throw;
            }
        }

        /// <summary>
        /// Converts the width of a glyph identified by its index to PDF design units.
        /// </summary>
        public int GlyphIndexToPdfWidth(int glyphIndex)
        {
            try
            {
                int numberOfHMetrics = FontFace._hhea.numberOfHMetrics;
                int unitsPerEm = FontFace._head.unitsPerEm;

                // glyphIndex >= numberOfHMetrics means the font is mono-spaced and all glyphs have the same width
                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace._hmtx.Metrics[glyphIndex].advanceWidth;

                // Sometimes the unitsPerEm is 1000, sometimes a power of 2.
                if (unitsPerEm == 1000)
                    return width;
                return width * 1000 / unitsPerEm; // normalize
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        public int PdfWidthFromCharCode(char ch)
        {
            int idx = CharCodeToGlyphIndex(ch);
            int width = GlyphIndexToPdfWidth(idx);
            return width;
        }

        /// <summary>
        ///   //Converts the width of a glyph identified by its index to PDF design units.
        /// </summary>
        public double GlyphIndexToEmfWidth(int glyphIndex, double emSize)
        {
            try
            {
                int numberOfHMetrics = FontFace._hhea.numberOfHMetrics;
                int unitsPerEm = FontFace._head.unitsPerEm;

                // glyphIndex >= numberOfHMetrics means the font is mono-spaced and all glyphs have the same width
                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace._hmtx.Metrics[glyphIndex].advanceWidth;

                return width * emSize / unitsPerEm; // normalize
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }

        /// <summary>
        ///   //Converts the width of a glyph identified by its index to PDF design units.
        /// </summary>
        public int GlyphIndexToWidth(int glyphIndex)
        {
            try
            {
                int numberOfHMetrics = FontFace._hhea.numberOfHMetrics;

                // glyphIndex >= numberOfHMetrics means the font is mono-spaced and all glyphs have the same width
                if (glyphIndex >= numberOfHMetrics)
                    glyphIndex = numberOfHMetrics - 1;

                int width = FontFace._hmtx.Metrics[glyphIndex].advanceWidth;
                return width;
            }
            catch (Exception)
            {
                GetType();
                throw;
            }
        }
    }
}
