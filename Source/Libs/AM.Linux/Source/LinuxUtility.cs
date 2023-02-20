// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* LinuxUtility.cs -- полезные методы для Linux
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Versioning;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Полезные методы для Linux.
/// </summary>
[SupportedOSPlatform ("linux")]
public static class LinuxUtility
{
    #region Public methods

    /// <summary>
    /// Проверка, исполняется ли наш процесс от имени суперпользователя.
    /// </summary>
    public static bool IsRoot()
    {
        return Mono.Unix.Native.Syscall.geteuid() == 0;
    }

    #endregion
}
