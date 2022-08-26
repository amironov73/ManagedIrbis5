// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* CodePageEnumFlags.cs -- указывает, какие кодовые страницы требуется перечислить
   Ars Magna project, http://arsmagna.ru */

#region Using directives

using System;

#endregion

namespace AM.Win32;

/// <summary>
/// Указывает, какие кодовые страницы требуется перечислить.
/// </summary>
[Flags]
public enum CodePageEnumFlags
{
    /// <summary>
    /// Только установленные кодовые страницы.
    /// </summary>
    CP_INSTALLED = 0x00000001,

    /// <summary>
    /// Все поддерживаемые кодовые страницы.
    /// </summary>
    CP_SUPPORTED = 0x00000002
}
