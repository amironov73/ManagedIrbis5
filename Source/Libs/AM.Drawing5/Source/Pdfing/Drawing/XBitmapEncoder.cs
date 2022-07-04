// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* XBitmapEncoder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace PdfSharpCore.Drawing
{
    /// <summary>
    /// Provides functionality to save a bitmap image in a specific format.
    /// </summary>
    public abstract class XBitmapEncoder
    {
        internal XBitmapEncoder()
        {
            // Prevent external deriving.
        }

        /// <summary>
        /// Gets a new instance of the PNG image encoder.
        /// </summary>
        public static XBitmapEncoder GetPngEncoder()
        {
            return new XPngBitmapEncoder();
        }

        /// <summary>
        /// Gets or sets the bitmap source to be encoded.
        /// </summary>
        public XBitmapSource Source
        {
            get { return _source; }
            set { _source = value; }
        }
        XBitmapSource _source;

        /// <summary>
        /// When overridden in a derived class saves the image on the specified stream
        /// in the respective format.
        /// </summary>
        public abstract void Save(Stream stream);
    }

    internal sealed class XPngBitmapEncoder : XBitmapEncoder
    {
        internal XPngBitmapEncoder()
        { }

        /// <summary>
        /// Saves the image on the specified stream in PNG format.
        /// </summary>
        public override void Save(Stream stream)
        {
            if (Source == null)
                throw new InvalidOperationException("No image source.");
        }
    }
}
