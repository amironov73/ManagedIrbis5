// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontHelper.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

using AM;

using PdfSharpCore.Fonts;
using PdfSharpCore.Fonts.OpenType;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Bunch of functions that do not have a better place.
    /// </summary>
    static class FontHelper
    {
        /// <summary>
        /// Measure string directly from font data.
        /// </summary>
        public static XSize MeasureString
            (
                string text,
                XFont font,
                XStringFormat stringFormat
            )
        {
            var size = new XSize();

            var descriptor = FontDescriptorCache.GetOrCreateDescriptorFor (font) as OpenTypeDescriptor;
            if (descriptor != null)
            {
                // Height is the sum of ascender and descender.
                var singleLineHeight = (descriptor.Ascender + descriptor.Descender) * font.Size / font.UnitsPerEm;
                var lineGapHeight = (descriptor.LineSpacing - descriptor.Ascender - descriptor.Descender) * font.Size /
                                    font.UnitsPerEm;

                Debug.Assert (descriptor.Ascender > 0);

                var symbol = descriptor.FontFace.cmap.symbol;
                var length = text.Length;
                var adjustedLength = length;
                var height = singleLineHeight;
                var maxWidth = 0;
                var width = 0;
                for (var idx = 0; idx < length; idx++)
                {
                    var ch = text[idx];

                    // Handle line feed ( \n)
                    if (ch == 10)
                    {
                        adjustedLength--;
                        if (idx < (length - 1))
                        {
                            maxWidth = Math.Max (maxWidth, width);
                            width = 0;
                            height += lineGapHeight + singleLineHeight;
                        }

                        continue;
                    }

                    // HACK: Handle tabulator sign as space (\t)
                    if (ch == 9)
                    {
                        ch = ' ';
                    }

                    // HACK: Unclear what to do here.
                    if (ch < 32)
                    {
                        adjustedLength--;

                        continue;
                    }

                    if (symbol)
                    {
                        // Remap ch for symbol fonts.
                        ch = (char)(ch | (descriptor.FontFace.os2.usFirstCharIndex & 0xFF00)); // @@@ refactor

                        // Used | instead of + because of: http://PdfSharpCore.codeplex.com/workitem/15954
                    }

                    var glyphIndex = descriptor.CharCodeToGlyphIndex (ch);
                    width += descriptor.GlyphIndexToWidth (glyphIndex);
                }

                maxWidth = Math.Max (maxWidth, width);

                // What? size.Width = maxWidth * font.Size * (font.Italic ? 1 : 1) / descriptor.UnitsPerEm;
                size.Width = maxWidth * font.Size / descriptor.UnitsPerEm;
                size.Height = height;

                // Adjust bold simulation.
                if ((font.GlyphTypeface.StyleSimulations & XStyleSimulations.BoldSimulation) ==
                    XStyleSimulations.BoldSimulation)
                {
                    // Add 2% of the em-size for each character.
                    // Unsure how to deal with white space. Currently count as regular character.
                    size.Width += adjustedLength * font.Size * Const.BoldEmphasis;
                }
            }

            Debug.Assert (descriptor != null, "No OpenTypeDescriptor.");

            return size;
        }

        /// <summary>
        /// Calculates an Adler32 checksum combined with the buffer length
        /// in a 64 bit unsigned integer.
        /// </summary>
        public static ulong CalcChecksum
            (
                byte[] buffer
            )
        {
            Sure.NotNull (buffer);

            const uint prime = 65521; // largest prime smaller than 65536
            uint s1 = 0;
            uint s2 = 0;
            var length = buffer.Length;
            var offset = 0;
            while (length > 0)
            {
                var n = 3800;
                if (n > length)
                    n = length;
                length -= n;
                while (--n >= 0)
                {
                    s1 += buffer[offset++];
                    s2 = s2 + s1;
                }

                s1 %= prime;
                s2 %= prime;
            }

            var ul1 = (ulong)s2 << 16;
            ul1 = ul1 | s1;
            var ul2 = (ulong)buffer.Length;
            return (ul1 << 32) | ul2;
        }

        public static XFontStyle CreateStyle (bool isBold, bool isItalic)
        {
            return (isBold ? XFontStyle.Bold : 0) | (isItalic ? XFontStyle.Italic : 0);
        }
    }
}
