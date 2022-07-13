// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Wsl.cs -- обертка над WSL API
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Microsoft.Win32;

#endregion

#pragma warning disable CA1401 // "Метод не должен быть видимым"

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
    public static RegistryKey GetRootRegistryKey()
    {
        return Registry.CurrentUser.OpenSubKey (RegistryPath).ThrowIfNull();
    }

    /// <summary>
    /// Перечисление установленных размещений.
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
    }

    /// <summary>
    /// Модификация поведения размещения, зарегистрированного
    /// в Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Уникальное имч, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="defaultUID">Идентификатор пользователя Linux,
    /// который должен использоваться при запуске новой сессии WSL.
    /// </param>
    /// <param name="wslDistributionFlags">Флаги, определяющие
    /// поведение размещения.</param>
    /// <returns>Возвращает S_OK при успехе, в остальных случаях
    /// HRESULT с кодом ошибки.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslConfigureDistribution
        (
            string distributionName,
            int defaultUID,
            WslDistributionFlags wslDistributionFlags
        );

    /// <summary>
    /// Получение текущей конфигурации размещения, зарегистрированного
    /// в Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="distributionVersion">Версия WSL, для которой
    /// сконфигурировано размещение.</param>
    /// <param name="defaultUID">Идентификатор пользователя по
    /// умолчанию, который будет использоваться для новых сессий,
    /// запущенных WSL в контексте данного размещения.</param>
    /// <param name="wslDistributionFlags">Флаги, управляющие
    /// поведением данного размещения.</param>
    /// <param name="defaultEnvironmentVariables">Адрес указателя
    /// с массивом строк окружения, используемым по умолчанию
    /// в данном размещении. </param>
    /// <param name="defaultEnvironmentVariableCount">Количество
    /// элементов в pDefaultEnvironmentVariablesArray.</param>
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
    /// Определение, зарегистрировано ли указанное размещение в
    /// Windows Subsystem for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <returns>Возвращает TRUE, если размещение с указанным именем
    /// в настоящий момент зарегистрировано, иначе FALSE.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern bool WslIsDistributionRegistered
        (
            string distributionName
        );

    /// <summary>
    /// Запускает процесс Windows Subsystem for Linux (WSL) в контексте
    /// заданного размещения.
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="command">Команда для исполнения. Если не задана
    /// запускается командная оболочка по умолчанию.</param>
    /// <param name="useCurrentWorkingDirectory">Будет ли запускаемый
    /// процесс наследовать рабочую директорию от запускающего
    /// процесса. Если FALSE, процесс будет запущен в домашней
    /// папке пользователя по умолчанию ("~").</param>
    /// <param name="stdIn">Файл, в который перенаправляется STDIN.</param>
    /// <param name="stdOut">Файд, в который перенаправляется STDOUT.</param>
    /// <param name="stdErr">Файл, в который перенаправляется STDERR.</param>
    /// <param name="process">Адрес, по которому будет помещен
    /// HANDLE запущенного процесса WSL.</param>
    /// <returns>Возвращает S_OK при успехе, в остальных случаях
    /// HRESULT с кодом ошибки.</returns>
    /// <remarks>Вызвающий код отвечает за вызов <c>CloseHandle</c>
    /// для полученного описателя процесса.</remarks>
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
    /// Запускает интерактивную сессию  Windows Subsystem for Linux
    /// (WSL) в контексте конкретного размещения.
    /// Этот вызов отличается от WslLaunch тем, что пользователь
    /// получает возможность взаимодействия с запущенным процессом.
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="command">Команда, которая должна быть выполнена.
    /// Если никакая команда не задана, запускается командная
    /// оболочка по умолчанию.</param>
    /// <param name="useCurrentWorkingDirectory">Управляет
    /// наследованием рабочей папки от запускающего процесса
    /// к запускаемому. FALSE означает, что запускаемый процесс
    /// получит в качестве рабочей директории домашнюю папку
    /// пользователя по умолчанию ("~").</param>
    /// <param name="exitCode">Принимает код возврата по
    /// окончании сессии WSL.</param>
    /// <returns>Возвращает S_OK при успехе, в остальных случаяъ
    /// HRESULT с кодом ошибки.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslLaunchInteractive
        (
            string distributionName,
            string command,
            bool useCurrentWorkingDirectory,
            out int exitCode
        );

    /// <summary>
    /// Регистрирует новое размещение в  indows Subsystem
    /// for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <param name="tarGzFilename">Полный путь к файлу .tar.gz,
    /// содержащему размещение, подлежащее регистрации.
    /// </param>
    /// <returns>Необходимо применить макро SUCCEEDED и FAILED
    /// для анализа возвращенного значения.
    /// </returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslRegisterDistribution
        (
            string distributionName,
            string tarGzFilename
        );

    /// <summary>
    /// Отменяет регистрацию размещения в Windows Subsystem
    /// for Linux (WSL).
    /// </summary>
    /// <param name="distributionName">Уникальное имя, представляющее
    /// размещение (например, "Fabrikam.Distro.10.01").
    /// </param>
    /// <returns>Возвращает S_OK при успехе, в остальных случаях
    /// HRESULT с кодом ошибки.</returns>
    [DllImport (DllName, CharSet = CharSet.Unicode)]
    public static extern int WslUnregisterDistribution
        (
            string distributionName
        );

    #endregion
}
