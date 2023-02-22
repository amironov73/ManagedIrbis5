// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* LinuxUtility.cs -- полезные методы для Linux
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Runtime.Versioning;

using JetBrains.Annotations;

using Mono.Unix.Native;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Полезные методы для Linux.
/// </summary>
[PublicAPI]
[SupportedOSPlatform ("linux")]
public static class LinuxUtility
{
    #region Public methods

    /// <summary>
    /// Проверка, исполняется ли наш процесс от имени суперпользователя.
    /// </summary>
    public static bool IsRoot()
    {
        return Syscall.geteuid() == 0;
    }


    /// <summary>
    /// Выяснение версии операционной системы.
    /// </summary>
    public static Utsname? Uname()
    {
        if (Syscall.uname (out var uname) < 0)
        {
            return null;
        }

        return uname;
    }
    #endregion
}
