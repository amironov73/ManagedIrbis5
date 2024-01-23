// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* LinuxSystemInfo.cs -- информация об операционной системе Linux
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.Versioning;

using JetBrains.Annotations;

#endregion

namespace AM.Linux;

/// <summary>
/// Информация об операционной системе Linux.
/// </summary>
[PublicAPI]
[SupportedOSPlatform ("linux")]
public sealed class LinuxSystemInfo
{
    #region Properties

    /// <summary>
    /// Дистрибутив, например, "debian".
    /// </summary>
    public required string Distribution { get; init; }

    /// <summary>
    /// Версия дистрибутива, например, "11.6".
    /// </summary>
    public required Version DistributionVersion { get; init; }

    /// <summary>
    /// Имя компьютера.
    /// </summary>
    public required string NodeName { get; init; }

    /// <summary>
    /// Версия ядра, например, "6.1.12".
    /// </summary>
    public required Version KernelVersion { get; init; }

    /// <summary>
    /// Архитектура, например, "x86_64".
    /// </summary>
    public required string Architecture { get; init; }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение информации об операционной системе.
    /// </summary>
    public static LinuxSystemInfo GetSystemInfo()
    {
        var uname = LinuxUtility.Uname().ThrowIfNull();
        var osRelease = OsRelease.ReadFile().ThrowIfNull();

        var result = new LinuxSystemInfo
        {
            Distribution = osRelease.Id.ThrowIfNullOrEmpty(),
            DistributionVersion = Version.Parse (osRelease.VersionId.ThrowIfNullOrEmpty()),
            NodeName = uname.nodename,
            KernelVersion = Version.Parse (uname.release),
            Architecture = uname.machine
        };

        return result;
    }

    #endregion
}
