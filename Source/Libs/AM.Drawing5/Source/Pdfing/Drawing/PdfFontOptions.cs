// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* PdfFontOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using PdfSharpCore.Pdf;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Specifies details about how the font is used in PDF creation.
    /// </summary>
    public class XPdfFontOptions
    {
        internal XPdfFontOptions() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XPdfFontOptions"/> class.
        /// </summary>
        public XPdfFontOptions(PdfFontEncoding encoding)
        {
            _fontEncoding = encoding;
        }

        /// <summary>
        /// Gets a value indicating how the font is encoded.
        /// </summary>
        public PdfFontEncoding FontEncoding
        {
            get { return _fontEncoding; }
        }
        readonly PdfFontEncoding _fontEncoding;

        /// <summary>
        /// Gets the default options with WinAnsi encoding and always font embedding.
        /// </summary>
        public static XPdfFontOptions WinAnsiDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.WinAnsi); }
        }

        /// <summary>
        /// Gets the default options with Unicode encoding and always font embedding.
        /// </summary>
        public static XPdfFontOptions UnicodeDefault
        {
            get { return new XPdfFontOptions(PdfFontEncoding.Unicode); }
        }
    }
}
