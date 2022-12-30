// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Reporting.Utils
{
    /// <summary>
    /// A wrapper around PrivateFontCollection.
    /// </summary>
    public class FRPrivateFontCollection
    {
        private PrivateFontCollection collection = TypeConverters.FontConverter.PrivateFontCollection;
        private Dictionary<string, string> FontFiles = new Dictionary<string, string>();
        private Dictionary<string, MemoryFont> MemoryFonts = new Dictionary<string, MemoryFont>();

        /// <summary>
        /// Gets the array of FontFamily objects associated with this collection.
        /// </summary>
        public FontFamily[] Families => collection.Families;

        /// <summary>
        /// Checks if the font name is contained in this collection.
        /// </summary>
        /// <param name="fontName">The name of the font.</param>
        /// <returns>true if the font is contained in this collection.</returns>
        public bool HasFont (string fontName)
        {
            return FontFiles.ContainsKey (fontName) || MemoryFonts.ContainsKey (fontName);
        }

        /// <summary>
        /// Returns the font's stream.
        /// </summary>
        /// <param name="fontName">The name of the font.</param>
        /// <returns>Either FileStream or MemoryStream containing font data.</returns>
        public Stream GetFontStream (string fontName)
        {
            if (FontFiles.ContainsKey (fontName))
            {
                return new FileStream (FontFiles[fontName], FileMode.Open, FileAccess.Read);
            }
            else if (MemoryFonts.ContainsKey (fontName))
            {
                var font = MemoryFonts[fontName];
                var buffer = new byte[font.Length];
                Marshal.Copy (font.Memory, buffer, 0, font.Length);
                return new MemoryStream (buffer);
            }

            return null;
        }

        /// <summary>
        /// Adds a font from the specified file to this collection.
        /// </summary>
        /// <param name="filename">A System.String that contains the file name of the font to add.</param>
        public void AddFontFile (string filename)
        {
            collection.AddFontFile (filename);
            var fontName = Families[Families.Length - 1].Name;
            if (!FontFiles.ContainsKey (fontName))
            {
                FontFiles.Add (fontName, filename);
            }
        }

        /// <summary>
        /// Adds a font contained in system memory to this collection.
        /// </summary>
        /// <param name="memory">The memory address of the font to add.</param>
        /// <param name="length">The memory length of the font to add.</param>
        public void AddMemoryFont (nint memory, int length)
        {
            collection.AddMemoryFont (memory, length);
            var fontName = Families[Families.Length - 1].Name;
            if (!FontFiles.ContainsKey (fontName))
            {
                MemoryFonts.Add (fontName, new MemoryFont (memory, length));
            }
        }

        private struct MemoryFont
        {
            public nint Memory;
            public int Length;

            public MemoryFont (nint memory, int length)
            {
                Memory = memory;
                Length = length;
            }
        }
    }
}
