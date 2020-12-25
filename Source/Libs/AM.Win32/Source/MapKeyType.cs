﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

/* MapKeyType.cs -- translation type for MapVirtualKey function
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32
{
    /// <summary>
    /// Specifies the translation type for MapVirtualKey function.
    /// </summary>
    [Serializable]
    public enum MapKeyType
    {
        /// <summary>
        /// uCode is a virtual-key code and is translated into a scan code.
        /// If it is a virtual-key code that does not distinguish between left-
        /// and right-hand keys, the left-hand scan code is returned. If there
        /// is no translation, the function returns 0.
        /// </summary>
        VirtualKeyToScanCode = 0,

        /// <summary>
        /// uCode is a scan code and is translated into a virtual-key code that
        /// does not distinguish between left- and right-hand keys. If there is
        /// no translation, the function returns 0.
        /// </summary>
        ScanCodeToVirtualKey = 1,

        /// <summary>
        /// uCode is a virtual-key code and is translated into an unshifted
        /// character value in the low-order word of the return value. Dead
        /// keys (diacritics) are indicated by setting the top bit of the
        /// return value. If there is no translation, the function returns 0.
        /// </summary>
        VirtualKeyToUnshiftedKey = 2,

        /// <summary>
        /// Windows NT/2000/XP: uCode is a scan code and is translated into
        /// a virtual-key code that distinguishes between left- and right-hand
        /// keys. If there is no translation, the function returns 0.
        /// </summary>
        VirtualKeyToScanCodeRL = 3

    } // enum MapKeyType

} // namespace AM.Win32
