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

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
    [MaybeNull]
    public ConnectionSettings Settings { get; set; }

    /// <summary>
    /// Подключение к серверу ИРБИС64.
    /// </summary>
    [MaybeNull]
    public ISyncProvider Connection { get; set; }

    /// <summary>
    /// Фабрика подключений.
    /// </summary>
    public virtual ConnectionFactory Factory => ConnectionFactory.Shared;

    /// <summary>
    /// INI-файл.
    /// </summary>
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
        IniFile = new LocalCatalogerIniFile (new IniFile());
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
    protected internal virtual void BuildConnectionSettings()
    {
        Settings = new ConnectionSettings();

        // сначала берем настройки из INI-файла, если он есть
        var connectionString = IniFile.BuildConnectionString();
        if (!string.IsNullOrEmpty (connectionString))
        {
            Settings.ParseConnectionString (connectionString);
            if (Settings.Verify (false))
            {
                return;
            }
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
    }

    /// <summary>
    /// Конфигурирование подключения к серверу.
    /// </summary>
    /// <param name="context">Контекст.</param>
    /// <param name="services">Сервисы.</param>
    protected virtual void ConfigureConnection
        (
            HostBuilderContext context,
            IServiceCollection services
        )
    {
        LoadIniFile();
        BuildConnectionSettings();
        Connection = Factory.CreateSyncConnection();
        Settings.ThrowIfNull().Apply (Connection);
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
            return false;
        }

        var iniPath = Path.Combine (AppContext.BaseDirectory, iniName);
        if (File.Exists (iniPath))
        {
            IniFile = LocalCatalogerIniFile.Load (iniPath);

            return true;
        }

        return false;
    }

    #endregion

    #region MagnaApplication members

    /// <inheritdoc cref="MagnaApplication.ConfigureServices"/>
    protected override void ConfigureServices
        (
            HostBuilderContext context,
            IServiceCollection services
        )
    {
        base.ConfigureServices (context, services);

        services.RegisterIrbisProviders();
        ConfigureConnection (context, services);
    }

    /// <inheritdoc cref="MagnaApplication.Run"/>
    public override int Run()
    {
        try
        {
            PreRun();

            using var host = Magna.Host;
            using var connection = Connection.ThrowIfNull();
            connection.Connect();
            if (!connection.Connected)
            {
                Logger.LogError ("Can't connect");
                Logger.LogInformation
                    (
                        "Last error: {LastError}",
                        IrbisException.GetErrorDescription (connection.LastError)
                    );

                return 1;
            }

            Logger.LogInformation ("Successfully connected");

            Magna.Host.Start();

            var result = ActualRun();

            Logger.LogInformation ("Disconnecting");

            return result;
        }
        catch (Exception exception)
        {
            Logger.LogError
                (
                    exception,
                    nameof (IrbisApplication) + "::" + nameof (Run)
                    + ": {Type}: {Message}",
                    exception.GetType(),
                    exception.Message
                );
            Console.Error.WriteLine (exception);
        }

        return 1;
    }

    #endregion
}
