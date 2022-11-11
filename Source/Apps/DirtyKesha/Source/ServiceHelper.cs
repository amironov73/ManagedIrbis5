// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* ServiceHelper.cs -- перенаправление вызовов при регистрации сервиса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Runtime.InteropServices;

#endregion

#nullable enable

namespace DirtyKesha;

/// <summary>
/// Перенаправление вызовов при регистрации сервсиа в заивисимости
/// от операционной системы.
/// </summary>
public static class ServiceHelper
{
    #region Public methods

    /// <summary>
    /// Ошибка: операционная система не поддерживается.
    /// </summary>
    public static void OperatingSystemIsNotSupported() =>
        throw new NotSupportedException ("Operating system is not supported");

    /// <summary>
    /// Регистрация демона в системе.
    /// </summary>
    public static bool Setup
        (
            string[] args
        )
    {
        if (args.Length == 0)
        {
            return false;
        }

        var command = args[0].ToLowerInvariant();
        switch (command)
        {
            case "create":
            case "install":
            case "register":
                RegisterService();
                return true;

            case "delete":
            case "uninstall":
            case "unregister":
                UnregisterService();
                return true;

            case "start":
                StartService();
                return true;

            case "stop":
                StopService();
                return true;

            case "status":
            case "query":
                QueryServiceStatus();
                return true;
        }

        return false;
    }

    /// <summary>
    /// Регистрация сервиса в системе.
    /// </summary>
    public static void RegisterService()
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            ServiceControl.RegisterService();
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            SystemControl.RegisterService();
        }
        else
        {
            OperatingSystemIsNotSupported();
        }
    }

    /// <summary>
    /// Запуск сервиса.
    /// </summary>
    public static void StartService()
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            ServiceControl.StartService();
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            SystemControl.StartService();
        }
        else
        {
            OperatingSystemIsNotSupported();
        }
    }

    /// <summary>
    /// Остановка сервиса.
    /// </summary>
    public static void StopService()
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            ServiceControl.StopService();
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            SystemControl.StopService();
        }
        else
        {
            OperatingSystemIsNotSupported();
        }
    }

    /// <summary>
    /// Запрос статуса сервиса.
    /// </summary>
    public static void QueryServiceStatus()
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            ServiceControl.QueryServiceStatus();
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            SystemControl.QueryServiceStatus();
        }
        else
        {
            OperatingSystemIsNotSupported();
        }
    }

    /// <summary>
    /// Отмена регистрации сервиса в системе.
    /// </summary>
    public static void UnregisterService()
    {
        if (RuntimeInformation.IsOSPlatform (OSPlatform.Windows))
        {
            ServiceControl.UnregisterService();
        }
        else if (RuntimeInformation.IsOSPlatform (OSPlatform.Linux))
        {
            SystemControl.UnregisterService();
        }
        else
        {
            OperatingSystemIsNotSupported();
        }
    }

    #endregion
}
