// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* GenericFontTable.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Fonts.OpenType
{
#if true_
    /// <summary>
    /// Generic font table. Not yet used
    /// </summary>
    internal class GenericFontTable : OpenTypeFontTable
    {
        public GenericFontTable(OpenTypeFontTable fontTable)
          : base(null, "xxxx")
        {
            DirectoryEntry.Tag = fontTable.DirectoryEntry.Tag;
            int length = fontTable.DirectoryEntry.Length;
            if (length > 0)
            {
                _table = new byte[length];
                Buffer.BlockCopy(fontTable.FontData.Data, fontTable.DirectoryEntry.Offset, _table, 0, length);
            }
        }

        public GenericFontTable(OpenTypeFontface fontData, string tag)
          : base(fontData, tag)
        {
            _fontData = fontData;
        }

        protected override OpenTypeFontTable DeepCopy()
        {
            GenericFontTable fontTable = (GenericFontTable)base.DeepCopy();
            fontTable._table = (byte[])_table.Clone();
            return fontTable;
        }

        byte[] _table;
    }
#endif
}
