// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* OpenTypeFontTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts.OpenType
{
    // TODO: Create a font driver for reading and writing OpenType font files.

    /// <summary>
    /// Base class for all OpenType tables used in PdfSharpCore.
    /// </summary>
    internal class OpenTypeFontTable : ICloneable
    {
        public OpenTypeFontTable(OpenTypeFontface fontData, string tag)
        {
            _fontData = fontData;
            if (fontData != null && fontData.TableDictionary.ContainsKey(tag))
                DirectoryEntry = fontData.TableDictionary[tag];
            else
                DirectoryEntry = new TableDirectoryEntry(tag);
            DirectoryEntry.FontTable = this;
        }

        /// <summary>
        /// Creates a deep copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return DeepCopy();
        }

        protected virtual OpenTypeFontTable DeepCopy()
        {
            OpenTypeFontTable fontTable = (OpenTypeFontTable)MemberwiseClone();
            fontTable.DirectoryEntry.Offset = 0;
            fontTable.DirectoryEntry.FontTable = fontTable;
            return fontTable;
        }

        /// <summary>
        /// Gets the font image the table belongs to.
        /// </summary>
        public OpenTypeFontface FontData
        {
            get { return _fontData; }
        }
        internal OpenTypeFontface _fontData;

        public TableDirectoryEntry DirectoryEntry;

        /// <summary>
        /// When overridden in a derived class, prepares the font table to be compiled into its binary representation.
        /// </summary>
        public virtual void PrepareForCompilation()
        { }

        /// <summary>
        /// When overridden in a derived class, converts the font into its binary representation.
        /// </summary>
        public virtual void Write(OpenTypeFontWriter writer)
        { }

        /// <summary>
        /// Calculates the checksum of a table represented by its bytes.
        /// </summary>
        public static uint CalcChecksum(byte[] bytes)
        {
            Debug.Assert((bytes.Length & 3) == 0);
            // Cannot use Buffer.BlockCopy because 32-bit values are Big-endian in fonts.
            uint byte3, byte2, byte1, byte0;
            byte3 = byte2 = byte1 = byte0 = 0;
            int length = bytes.Length;
            for (int idx = 0; idx < length;)
            {
                byte3 += bytes[idx++];
                byte2 += bytes[idx++];
                byte1 += bytes[idx++];
                byte0 += bytes[idx++];
            }
            return (byte3 << 24) + (byte2 << 16) + (byte1 << 8) + byte0;
        }
    }
}
