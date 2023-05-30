// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SystemFolders.cs -- пути к системным папкам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Пути к системным папкам.
/// </summary>
[PublicAPI]
public static class SystemFolders
{
    /// <summary>
    /// Системный диск, как правило, <c>"C:"</c>.
    /// </summary>
    public static string SystemDrive => Environment.ExpandEnvironmentVariables
        ("%systemdrive%");

    /// <summary>
    /// <c>"C:\\Users\\amiro\\AppData\\Local\\Temp"</c>.
    /// </summary>
    public static string Temp => Environment.ExpandEnvironmentVariables
        ("%temp%");

    /// <summary>
    /// <c>"C:\\Users\\amiro"</c>.
    /// </summary>
    public static string UserProfile => Environment.GetFolderPath
        (Environment.SpecialFolder.UserProfile);

    /// <summary>
    /// <c>"C:\\Users\\amiro\\AppData\\Local"</c>.
    /// </summary>
    public static string Local => Environment.GetFolderPath
        (Environment.SpecialFolder.UserProfile) + @"\AppData\Local";

    /// <summary>
    /// <c>"C:\\Users\\amiro\\AppData\\Roaming"</c>.
    /// </summary>
    public static string Roaming => Environment.GetFolderPath
        (Environment.SpecialFolder.UserProfile) + @"\AppData\Roaming";

    /// <summary>
    /// <c>"C:\\Users\\amiro\\Desktop"</c>.
    /// </summary>
    public static string Desktop => Environment.GetFolderPath
        (Environment.SpecialFolder.Desktop);

    /// <summary>
    /// <c>"C:\\Windows"</c>.
    /// </summary>
    public static string Windows => Environment.GetFolderPath
        (Environment.SpecialFolder.Windows);

    /// <summary>
    /// <c>"C:\\Windows\\system32"</c>.
    /// </summary>
    public static string System32 => Environment.GetFolderPath
        (Environment.SpecialFolder.System);

    /// <summary>
    /// <c>"C:\\Windows\\SysWOW64"</c>.
    /// </summary>
    public static string SysWow64 => Environment.GetFolderPath
        (Environment.SpecialFolder.SystemX86);

    /// <summary>
    /// <c>"C:\\Program Files"</c>/
    /// </summary>
    public static string ProgramFiles => Environment.GetFolderPath
        (Environment.SpecialFolder.ProgramFiles);

    /// <summary>
    /// <c>"C:\\Program Files (x86)"</c>.
    /// </summary>
    public static string ProgramFilesX86 => Environment.GetFolderPath
        (Environment.SpecialFolder.ProgramFilesX86);

    /// <summary>
    /// <c>"C:\\ProgramData"</c>.
    /// </summary>
    public static string ProgramData => Environment.GetFolderPath
        (Environment.SpecialFolder.CommonApplicationData);
}
