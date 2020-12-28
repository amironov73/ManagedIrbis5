// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LoadImageFlags.cs -- flags for LoadImage function
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// Flags for LoadImage function.
    /// </summary>
    public enum LoadImageFlags
    {
        /// <summary>
        /// Loads a bitmap.
        /// </summary>
        IMAGE_BITMAP = 0,

        /// <summary>
        /// Loads an icon.
        /// </summary>
        IMAGE_ICON = 1,

        /// <summary>
        /// Loads a cursor.
        /// </summary>
        IMAGE_CURSOR = 2,

        /// <summary>
        /// Loads an enhanced metafile.
        /// </summary>
        IMAGE_ENHMETAFILE = 3

    } // enum LoadImageFlags

} // namespace AM.Win32
