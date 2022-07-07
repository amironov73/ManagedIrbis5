// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable VirtualMemberCallInConstructor
// ReSharper disable VirtualMemberNeverOverridden.Global

/* IrbisApplication.cs -- класс-приложение, работающее с ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using AM;
using AM.AppServices;
using AM.IO;

using ManagedIrbis.Client;
using ManagedIrbis.CommandLine;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.AppServices;

/// <summary>
/// Класс-приложение, работающее с ИРБИС64
/// </summary>
public class IrbisApplication
    : MagnaApplication
{
    #region Properties

    /// <summary>
    /// Настройки подключения.
    /// </summary>
    [AllowNull]
    public ConnectionSettings Settings { get; set; }

    /// <summary>
    /// Действующее подключение к серверу ИРБИС64.
    /// </summary>
    [AllowNull]
    public ISyncProvider Connection { get; set; }

    /// <summary>
    /// Фабрика, использущаяся для создания подключений.
    /// </summary>
    public virtual ConnectionFactory Factory => ConnectionFactory.Shared;

    /// <summary>
    /// INI-файл.
    /// </summary>
    [AllowNull]
    public LocalCatalogerIniFile IniFile { get; protected set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    public IrbisApplication
        (
            string[] args
        )
        : base (args)
    {
        // пустое тело конструктора
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение настроек подключения.
    /// </summary>
    /// <remarks>
    /// Метод обязан вернуть корректные настройки подключения
    /// либо выбросить исключение.
    /// </remarks>
    protected internal virtual bool BuildConnectionSettings()
    {
        // сначала берем настройки из INI-файла, если он есть
        var connectionString = IniFile.BuildConnectionString();
        if (!string.IsNullOrEmpty (connectionString))
        {
            Settings.ParseConnectionString (connectionString);

            // достаточно INI-файла
            return !Settings.Verify (false);
        }

        // сначала берем настройки из стандартного JSON-файла конфигурации
        connectionString = ConnectionUtility.GetConfiguredConnectionString (Configuration)
            ?? ConnectionUtility.GetStandardConnectionString();

        if (!string.IsNullOrEmpty (connectionString))
        {
            Settings.ParseConnectionString (connectionString);
        }

        // затем из опционального файла с настройками подключения
        connectionString = ConnectionUtility.GetConnectionStringFromFile();
        if (!string.IsNullOrEmpty (connectionString))
        {
            Settings.ParseConnectionString (connectionString);
        }

        // затем из переменных окружения
        CommandLineUtility.ConfigureConnectionFromEnvironment (Settings);

        // наконец, из командной строки
        // TODO: сделать по-умному
        // CommandLineUtility.ConfigureConnectionFromCommandLine (Settings, Args);

        // Применяем настройки по умолчанию, если соответствующие элементы не заданы
        Settings.ApplyDefaults();

        // Logger.LogInformation($"Using connection settings: {Settings}");

        if (!Settings.Verify (false))
        {
            throw new IrbisException ("Can't build connection settings");
        }

        return false;
    }

    /// <summary>
    /// Конфигурирование подключения к серверу.
    /// </summary>
    protected virtual bool ConfigureConnection()
    {
        Settings.ThrowIfNull().Apply (Connection);

        return true;
    }

    /// <summary>
    /// Создание подключения.
    /// </summary>
    /// <returns></returns>
    protected virtual ISyncProvider CreateConnection()
    {
        return Factory.CreateSyncConnection();
    }

    /// <summary>
    /// Получение имени INI-файла.
    /// </summary>
    protected virtual string GetIniFileName() => "cirbisc.ini";

    /// <summary>
    /// Загрузка INI-файла.
    /// </summary>
    protected virtual bool LoadIniFile()
    {
        var iniName = GetIniFileName();
        if (string.IsNullOrEmpty (iniName))
        {
            return true;
        }

        var iniPath = Path.IsPathFullyQualified (iniName)
            ? iniName
            : Path.Combine (AppContext.BaseDirectory, iniName);
        if (File.Exists (iniPath))
        {
            IniFile = LocalCatalogerIniFile.Load (iniPath);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Подключение к серверу.
    /// </summary>
    protected virtual bool ConnectToServer()
    {
        Connection.Connect();
        if (!Connection.Connected)
        {
            Logger.LogError ("Can't connect");
            Logger.LogInformation
                (
                    "Last error: {LastError}",
                    IrbisException.GetErrorDescription (Connection.LastError)
                );

            return false;
        }
        return true;
    }

    #endregion

    #region MagnaApplication members

    /// <inheritdoc cref="MagnaApplication.EarlyInitialization"/>
    protected override void EarlyInitialization()
    {
        base.EarlyInitialization();

        Connection = new NullProvider(); // временно
        IniFile = new LocalCatalogerIniFile (new IniFile()); // временно
        Settings = new ConnectionSettings(); // пусть пока будут по умолчанию
        ConfigureServices (services => services.RegisterIrbisProviders());
    }

    /// <inheritdoc cref="MagnaApplication.FinalInitialization"/>
    protected override bool FinalInitialization()
    {
        return base.FinalInitialization()
               && LoadIniFile()
               && BuildConnectionSettings()
               && ConfigureConnection ()
               && ConnectToServer();
    }

    /// <inheritdoc cref="MagnaApplication.Cleanup"/>
    protected override void Cleanup()
    {
        Connection.Disconnect();
        Logger.LogInformation ("Disconnected from IRBIS server");

        base.Cleanup();
    }

    #endregion
}
