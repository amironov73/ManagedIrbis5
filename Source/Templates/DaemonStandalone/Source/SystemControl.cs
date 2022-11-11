// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* SystemControl.cs -- простая обертка над systemctl
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

#endregion

#nullable enable

namespace DaemonStandalone;

/// <summary>
/// Простая обертка над <c>systemctl</c>.
/// </summary>
public static class SystemControl
{
    #region Properties

    /// <summary>
    /// Директория, в которой хранятся описания сервисов для <c>systemd</c>.
    /// </summary>
    public static string UnitPrefix = "/lib/systemd/system";

    #endregion

    #region Private members

    /// <summary>
    /// Получение пути до исполняемого файла.
    /// </summary>
    private static string GetExecutablePath()
    {
        return Assembly.GetEntryAssembly()!.Location;
    }

    /// <summary>
    /// Запуск программы <c>sc</c>.
    /// </summary>
    /// <param name="args">Аргументы для <c>sc</c>.</param>
    private static void RunSystemCtl
        (
            params string[] args
        )
    {
        if (args.Length != 0)
        {
            Process.Start ("systemctl", args);
        }
    }

    #endregion

    #region Public method

    /// <summary>
    /// Создание файла описания для
    /// </summary>
    public static string CreateUnit
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);
        name += ".service";

        using var stream = File.CreateText (name);
        stream.WriteLine ("[Unit]");
        stream.WriteLine ("Description=Ars Magna service");
        stream.WriteLine();

        stream.WriteLine ("[Service]");
        stream.WriteLine ($"WorkingDirectory={Path.GetDirectoryName (executable)}");
        stream.WriteLine ($"ExecStart={executable}");
        stream.WriteLine ($"SyslogIdentifier={Path.GetFileNameWithoutExtension (name)}");
        stream.WriteLine ("User=www-data"); // ???
        stream.WriteLine ("Environment=ASPNETCORE_ENVIRONMENT=Production");
        stream.WriteLine ("Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false");
        stream.WriteLine();

        stream.WriteLine ("[Install]");
        stream.WriteLine ("WantedBy=multi-user.target");

        return name;
    }

    /// <summary>
    /// Регистрация сервиса в системе.
    /// </summary>
    /// <param name="name">Имя сервиса.</param>
    public static void RegisterService
        (
            string? name = null
        )
    {
        var executable = GetExecutablePath();
        name ??= Path.GetFileNameWithoutExtension (executable);

        try
        {
            var localName = name + ".service";
            var globalName = Path.Combine (UnitPrefix, localName);
            if (!File.Exists (globalName))
            {
                if (!File.Exists (localName))
                {
                    localName = CreateUnit (name);
                }

                File.Copy (localName, globalName);
            }
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception);

            return;
        }

        RunSystemCtl
            (
                "enable",
                name
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

        RunSystemCtl
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

        RunSystemCtl
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

        RunSystemCtl
            (
                "status",
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

        RunSystemCtl
            (
                "disable",
                name
            );
    }

    #endregion
}
