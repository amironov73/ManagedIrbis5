// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ImageType.cs -- indicates whether an image is a bitmap or a metafile
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// The ImageType enumeration indicates whether
    /// an image is a bitmap or a metafile. The Image::GetType
    /// method returns an element of this enumeration.
    /// </summary>
    public enum ImageType
    {
        /// <summary>
        /// Unknown image type.
        /// </summary>
        ImageTypeUnknown = 0,

        /// <summary>
        /// Bitmap.
        /// </summary>
        ImageTypeBitmap = 1,

        /// <summary>
        /// Metafile.
        /// </summary>
        ImageTypeMetafile = 2

    } // enum ImageType

} // namespace AM.Win32
