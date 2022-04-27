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
// ReSharper disable UnusedType.Global

/* ServiceNegotiator.cs -- установщик/запускальщик сервисов под systemd
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;

using AM.IO;

#endregion

#nullable enable

namespace AM.Linux;

/// <summary>
/// Установщик/запускальщик сервисов под systemd.
/// </summary>
[SupportedOSPlatform("linux")]
public static class ServiceNegotiator
{
    #region Constants

    /// <summary>
    /// Директория, в которой хранятся юнит-файлы systemd.
    /// </summary>
    public const string ServiceCatalog = "/etc/systemd/system";

    /// <summary>
    /// Расширение, характерное для
    /// </summary>
    public const string ServiceExtension = ".service";

    #endregion

    #region Private members



    #endregion

    #region Public methods

    /// <summary>
    /// Запрет запуска указанного сервиса.
    /// </summary>
    public static void DisableService
        (
            string serviceName
        )
    {
        Sure.NotNullNorEmpty (serviceName);

        SystemControl ($"disable {serviceName}");
    }

    /// <summary>
    /// Разрешение запуска указанного сервиса.
    /// </summary>
    public static void EnableService
        (
            string serviceName
        )
    {
        Sure.NotNullNorEmpty (serviceName);

        SystemControl ($"enable {serviceName}");
    }

    /// <summary>
    /// Установка указанного сервиса.
    /// </summary>
    public static void InstallService
        (
            string serviceName,
            ServiceUnit unit
        )
    {
        Sure.NotNullNorEmpty (serviceName);
        Sure.VerifyNotNull (unit);

        var fileName = Path.Combine (ServiceCatalog, serviceName);
        if (!fileName.EndsWith (ServiceExtension))
        {
            fileName += ServiceExtension;
        }

        FileUtility.DeleteIfExists (fileName);
        unit.WriteTo (fileName);
    }

    /// <summary>
    /// Выполнение команды <c>systemctl</c>
    /// с указанными аргументами.
    /// </summary>
    public static void SystemControl
        (
            string arguments
        )
    {
        Sure.NotNullNorEmpty (arguments);

        using var process = Process.Start
            (
                "systemctl",
                arguments
            );
        process.WaitForExit();
    }

    /// <summary>
    /// Запуск сервиса с указанным именем.
    /// </summary>
    public static void StartService
        (
            string serviceName
        )
    {
        Sure.NotNullNorEmpty (serviceName);

        SystemControl ($"start {serviceName}");
    }

    /// <summary>
    /// Остановка сервиса с указанным именем.
    /// </summary>
    public static void StopService
        (
            string serviceName
        )
    {
        Sure.NotNullNorEmpty (serviceName);

        SystemControl ($"stop {serviceName}");
    }

    /// <summary>
    /// Удаление (деинсталляция) указанного сервиса.
    /// </summary>
    public static void UninstallService
        (
            string serviceName
        )
    {
        Sure.NotNullNorEmpty (serviceName);


        var fileName = Path.Combine (ServiceCatalog, serviceName);
        if (File.Exists (fileName))
        {
            File.Delete (fileName);
        }
        else
        {
            if (!fileName.EndsWith (ServiceExtension))
            {
                fileName += ServiceExtension;
            }

            if (File.Exists (fileName))
            {
                File.Delete (fileName);
            }
        }
    }

    #endregion
}
