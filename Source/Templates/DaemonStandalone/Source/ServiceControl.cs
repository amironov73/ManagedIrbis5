// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ServiceControl.cs -- управление сервисами под Windows
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace DaemonStandalone;

/// <summary>
/// Управление сервисами под Windows.
/// </summary>
public static class ServiceControl
{
    #region Private members

    private static string GetExecutablePath()
    {
        return Assembly.GetEntryAssembly()!.Location;
    }

    /// <summary>
    /// Запуск программы <c>sc</c>.
    /// </summary>
    /// <param name="args">Аргументы для <c>sc</c>.</param>
    private static void RunSC
        (
            params string[] args
        )
    {
        if (args.Length != 0)
        {
            Process.Start ("sc", args);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Регистрация сервиса в системе.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    /// <param name="start">Тип запуска.</param>
    /// <param name="account">Учетная запись.</param>
    public static void RegisterService
        (
            string? name = null,
            string start = "delayed-auto",
            string account = "NT Authority\\NetworkService"
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        RunSC
            (
                "create",
                name,
                "binpath=",
                executable,
                "start=",
                start,
                "obj=",
                account
            );
    }

    /// <summary>
    /// Запуск сервиса.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    public static void StartService
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        RunSC
            (
                "start",
                name
            );
    }

    /// <summary>
    /// Остановка сервиса.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    public static void StopService
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        RunSC
            (
                "stop",
                name
            );
    }

    /// <summary>
    /// Запрос статуса сервиса.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    public static void QueryServiceStatus
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        RunSC
            (
                "query",
                name
            );
    }

    /// <summary>
    /// Отмена регистрации сервиса в системе.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    public static void UnregisterService
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        RunSC
            (
                "delete",
                name
            );
    }

    #endregion
}
