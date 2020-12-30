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

/* DeviceOrientation.cs -- the orientation at which images should be presented
   Ars Magna project, http://arsmagna.ru */

namespace AM.Win32
{
    /// <summary>
    /// The orientation at which images should be presented
    /// or orientation of the paper.
    /// </summary>
    public enum DeviceOrientation
    {
        /// <summary>
        /// Portrait orientation.
        /// </summary>
        DMORIENT_PORTRAIT = 1,

        /// <summary>
        /// Landscape orientation.
        /// </summary>
        DMORIENT_LANDSCAPE = 2,

        /// <summary>
        /// The display orientation is the natural orientation
        /// of the display device; it should be used as the default.
        /// </summary>
        DMDO_DEFAULT = 0,

        /// <summary>
        /// The display orientation is rotated 90 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_90 = 1,

        /// <summary>
        /// The display orientation is rotated 180 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_180 = 2,

        /// <summary>
        /// The display orientation is rotated 270 degrees
        /// (measured clockwise) from DMDO_DEFAULT.
        /// </summary>
        DMDO_270 = 3

    } // enum DeviceModeOritentation

} // namespace AM.Win32
