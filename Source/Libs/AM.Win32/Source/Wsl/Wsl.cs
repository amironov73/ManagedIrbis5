// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Wsl.cs -- обертка над WSL API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Win32;

#endregion

#nullable enable

namespace AM.Win32;

/// <summary>
/// Обертка над WSL API.
/// https://docs.microsoft.com/en-us/windows/win32/api/_wsl/
/// </summary>
public static class Wsl
{
    #region Constants

    /// <summary>
    /// Имя DLL.
    /// </summary>
    public const string DllName = "Api-ms-win-wsl-api-l1-1-0.dll";

    /// <summary>
    /// Путь к реестру с настройками.
    /// </summary>
    public const string RegistryPath = @"\SOFTWARE\Microsoft\Windows\CurrentVersion\Lxss";

    /// <summary>
    /// Ubuntu 20.04.
    /// </summary>
    public const string Ubuntu2004 = "Ubuntu-20.04";

    #endregion

    #region Public methods

    /// <summary>
    /// Получение ветки регистра для WSL.
    /// </summary>
    public static RegistryKey GetRootRegistryKey() =>
        Registry.CurrentUser.OpenSubKey (RegistryPath).ThrowIfNull();

    /// <summary>
    /// Перечисление установленных дистрибутивов.
    /// </summary>
    public static WslDistribution[] ListDistributions()
    {
        var result = new List<WslDistribution>();
        using var root = GetRootRegistryKey();

        foreach (var keyName in root.GetSubKeyNames())
        {
            var subKey = root.OpenSubKey (keyName);
            if (subKey is not null)
            {
                var distribution = new WslDistribution();
                distribution.Parse (subKey);
                result.Add (distribution);
            }
        }

        return result.ToArray();
    } // method ListDistributions

    /// <summary>
    /// Modifies the behavior of a distribution registered with
    /// the Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="defaultUID">The Linux user ID to use when
    /// launching new WSL sessions for this distribution.</param>
    /// <param name="wslDistributionFlags">Flags specifying what
    /// behavior to use for this distribution.</param>
    /// <returns>Returns S_OK on success, or a failing
    /// HRESULT otherwise.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslConfigureDistribution
        (
            string distributionName,
            int defaultUID,
            WslDistributionFlags wslDistributionFlags
        );

    /// <summary>
    /// Retrieves the current configuration of a distribution
    /// registered with the Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="distributionVersion">The version of WSL
    /// for which this distribution is configured.</param>
    /// <param name="defaultUID">The default user ID used when
    /// launching new WSL sessions for this distribution.</param>
    /// <param name="wslDistributionFlags">The flags governing
    /// the behavior of this distribution.</param>
    /// <param name="defaultEnvironmentVariables">The address of
    /// a pointer to an array of default environment variable strings
    /// used when launching new WSL sessions for this distribution.
    /// </param>
    /// <param name="defaultEnvironmentVariableCount">The number
    /// of elements in pDefaultEnvironmentVariablesArray.</param>
    /// <returns></returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslGetDistributionConfiguration
        (
            string distributionName,
            out int distributionVersion,
            out int defaultUID,
            out WslDistributionFlags wslDistributionFlags,
            out IntPtr defaultEnvironmentVariables,
            out int defaultEnvironmentVariableCount
        );

    /// <summary>
    /// Determines if a distribution is registered with the
    /// Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <returns>Returns TRUE if the supplied distribution
    /// is currently registered, or FALSE otherwise.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern bool WslIsDistributionRegistered
        (
            string distributionName
        );

    /// <summary>
    /// Launches a Windows Subsystem for Linux (WSL) process
    /// in the context of a particular distribution.
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="command">Command to execute. If no command
    /// is supplied, launches the default shell.</param>
    /// <param name="useCurrentWorkingDirectory">Governs whether
    /// or not the launched process should inherit the calling
    /// process's working directory. If FALSE, the process
    /// is started in the WSL default user's home directory ("~").
    /// </param>
    /// <param name="stdIn">Handle to use for STDIN.</param>
    /// <param name="stdOut">Handle to use for STDOUT.</param>
    /// <param name="stdErr">Handle to use for STDERR.</param>
    /// <param name="process">Pointer to address to receive
    /// the process HANDLE associated with the newly-launched
    /// WSL process.</param>
    /// <returns>Returns S_OK on success, or a failing HRESULT otherwise.
    /// </returns>
    /// <remarks>Caller is responsible for calling <c>CloseHandle</c>
    /// on the value returned in <c>process</c> on success.</remarks>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslLaunch
        (
            string distributionName,
            string command,
            bool useCurrentWorkingDirectory,
            IntPtr stdIn,
            IntPtr stdOut,
            IntPtr stdErr,
            out IntPtr process
        );

    /// <summary>
    /// Launches an interactive Windows Subsystem for Linux (WSL)
    /// process in the context of a particular distribution.
    /// This differs from WslLaunch in that the end user will
    /// be able to interact with the newly-created process.
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="command">Command to execute. If no command
    /// is supplied, launches the default shell.</param>
    /// <param name="useCurrentWorkingDirectory">Governs whether
    /// or not the launched process should inherit the calling
    /// process's working directory. If FALSE, the process
    /// is started in the WSL default user's home directory ("~").
    /// </param>
    /// <param name="exitCode">Receives the exit code of the process
    /// after it exits.</param>
    /// <returns>Returns S_OK on success, or a failing HRESULT
    /// otherwise.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslLaunchInteractive
        (
            string distributionName,
            string command,
            bool useCurrentWorkingDirectory,
            out int exitCode
        );

    /// <summary>
    /// Registers a new distribution with the Windows Subsystem
    /// for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="tarGzFilename">Full path to a .tar.gz file
    /// containing the file system of the distribution to register.
    /// </param>
    /// <returns>This function can return one of the following values.
    /// Use the SUCCEEDED and FAILED macros to test the return value
    /// of this function.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslRegisterDistribution
        (
            string distributionName,
            string tarGzFilename
        );

    /// <summary>
    /// Unregisters a distribution from the Windows Subsystem
    /// for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Unique name representing
    /// a distribution (for example, "Fabrikam.Distro.10.01").
    /// </param>
    /// <returns>Returns S_OK on success, or a failing HRESULT
    /// otherwise.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslUnregisterDistribution
        (
            string distributionName
        );

    #endregion
}
