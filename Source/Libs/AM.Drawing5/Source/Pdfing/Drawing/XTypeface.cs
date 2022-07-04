// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XTypeface.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace PdfSharpCore.Drawing
{
#if true_  // Not yet used
    /// <summary>
    /// no: Specifies a physical font face that corresponds to a font file on the disk or in memory.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay}")]
    internal class XTypeface_not_yet_used
    {
        public XTypeface_not_yet_used(XFontFamily family, XFontStyle style)
        {
            _family = family;
            _style = style;
        }

        public XFontFamily Family
        {
            get { return _family; }
        }
        XFontFamily _family;

        public XFontStyle Style
        {
            get { return _style; }
        }
        XFontStyle _style;

        public bool TryGetGlyphTypeface(out XGlyphTypeface glyphTypeface)
        {
            glyphTypeface = null;
            return false;
        }

        /// <summary>
        /// Gets the DebuggerDisplayAttribute text.
        /// </summary>
        // ReSharper disable UnusedMember.Local
        string DebuggerDisplay
        // ReSharper restore UnusedMember.Local
        {
            get { return string.Format(CultureInfo.InvariantCulture, "XTypeface"); }
        }
    }
#endif
}
