// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* TableTagNames.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace PdfSharpCore.Fonts.OpenType
{
    /// <summary>
    /// TrueType font table names.
    /// </summary>
    static class TableTagNames
    {
        // --- Required Tables ---

        /// <summary>
        /// Character to glyph mapping.
        /// </summary>
        public const string CMap = "cmap";

        /// <summary>
        /// Font header .
        /// </summary>
        public const string Head = "head";

        /// <summary>
        /// Horizontal header.
        /// </summary>
        public const string HHea = "hhea";

        /// <summary>
        /// Horizontal Metrics.
        /// </summary>
        public const string HMtx = "hmtx";

        /// <summary>
        /// Maximum profile.
        /// </summary>
        public const string MaxP = "maxp";

        /// <summary>
        /// Naming table.
        /// </summary>
        public const string Name = "name";

        /// <summary>
        /// OS/2 and Windows specific Metrics.
        /// </summary>
        public const string OS2 = "OS/2";

        /// <summary>
        /// PostScript information.
        /// </summary>
        public const string Post = "post";

        // --- Tables Related to TrueType Outlines ---

        /// <summary>
        /// Control Value Table.
        /// </summary>
        public const string Cvt = "cvt ";

        /// <summary>
        /// Font program.
        /// </summary>
        public const string Fpgm = "fpgm";

        /// <summary>
        /// Glyph data.
        /// </summary>
        public const string Glyf = "glyf";

        /// <summary>
        /// Index to location.
        /// </summary>
        public const string Loca = "loca";

        /// <summary>
        /// CVT Program.
        /// </summary>
        public const string Prep = "prep";

        // --- Tables Related to PostScript Outlines ---

        /// <summary>
        /// PostScript font program (compact font format).
        /// </summary>
        public const string Cff = "CFF";

        /// <summary>
        /// Vertical Origin.
        /// </summary>
        public const string VOrg = "VORG";

        // --- Tables Related to Bitmap Glyphs ---

        /// <summary>
        /// Embedded bitmap data.
        /// </summary>
        public const string EBDT = "EBDT";

        /// <summary>
        /// Embedded bitmap location data.
        /// </summary>
        public const string EBLC = "EBLC";

        /// <summary>
        /// Embedded bitmap scaling data.
        /// </summary>
        public const string EBSC = "EBSC";

        // --- Advanced Typographic Tables ---

        /// <summary>
        /// Baseline data.
        /// </summary>
        public const string BASE = "BASE";

        /// <summary>
        /// Glyph definition data.
        /// </summary>
        public const string GDEF = "GDEF";

        /// <summary>
        /// Glyph positioning data.
        /// </summary>
        public const string GPOS = "GPOS";

        /// <summary>
        /// Glyph substitution data.
        /// </summary>
        public const string GSUB = "GSUB";

        /// <summary>
        /// Justification data.
        /// </summary>
        public const string JSTF = "JSTF";

        // --- Other OpenType Tables ---

        /// <summary>
        /// Digital signature.
        /// </summary>
        public const string DSIG = "DSIG";

        /// <summary>
        /// Grid-fitting/Scan-conversion.
        /// </summary>
        public const string Gasp = "gasp";

        /// <summary>
        /// Horizontal device Metrics.
        /// </summary>
        public const string Hdmx = "hdmx";

        /// <summary>
        /// Kerning.
        /// </summary>
        public const string Kern = "kern";

        /// <summary>
        /// Linear threshold data.
        /// </summary>
        public const string LTSH = "LTSH";

        /// <summary>
        /// PCL 5 data.
        /// </summary>
        public const string PCLT = "PCLT";

        /// <summary>
        /// Vertical device Metrics.
        /// </summary>
        public const string VDMX = "VDMX";

        /// <summary>
        /// Vertical Header.
        /// </summary>
        public const string VHea = "vhea";

        /// <summary>
        /// Vertical Metrics.
        /// </summary>
        public const string VMtx = "vmtx";
    }
}