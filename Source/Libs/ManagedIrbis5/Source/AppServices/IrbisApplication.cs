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

/* IrbisApplication.cs -- класс-приложение, работающее с ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

using AM;
using AM.AppServices;

using ManagedIrbis.CommandLine;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

#endregion

#nullable enable

namespace ManagedIrbis.AppServices
{
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
            : base(args)
        {
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Построение настроек подключения.
        /// </summary>
        /// <remarks>
        /// Метод обязан вернуть корректные настройки подключения
        /// либо выбросить исключение.
        /// </remarks>
        protected virtual void BuildConnectionSettings()
        {
            // сначала берем настройки из стандартного JSON-файла конфигурации
            var connectionString = ConnectionUtility.GetConfiguredConnectionString(Configuration)
                ?? ConnectionUtility.GetStandardConnectionString();

            Settings = new ConnectionSettings();
            if (!string.IsNullOrEmpty(connectionString))
            {
                Settings.ParseConnectionString(connectionString);
            }

            // затем из опционального файла с настройками подключения
            connectionString = ConnectionUtility.GetConnectionStringFromFile();
            if (!string.IsNullOrEmpty(connectionString))
            {
                Settings.ParseConnectionString(connectionString);
            }

            // затем из переменных окружения
            CommandLineUtility.ConfigureConnectionFromEnvironment(Settings);

            // наконец, из командной строки
            CommandLineUtility.ConfigureConnectionFromCommandLine(Settings, Args);

            Logger.LogInformation($"Using connection string: {connectionString}");

            if (!Settings.Verify(false))
            {
                throw new IrbisException("Can't build connection settings");
            }

        } // method BuildConnectionSettings

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
            BuildConnectionSettings();
            Connection = Factory.CreateSyncConnection();
            Settings.ThrowIfNull(nameof(Settings)).Apply(Connection);

        } // method ConfigureConnection

        #endregion

        #region MagnaApplication members

        /// <inheritdoc cref="MagnaApplication.ConfigureServices"/>
        protected override void ConfigureServices
            (
                HostBuilderContext context,
                IServiceCollection services
            )
        {
            base.ConfigureServices(context, services);

            services.RegisterIrbisProviders();
            ConfigureConnection(context, services);

        } // method ConfigureServices

        /// <inheritdoc cref="MagnaApplication.Run"/>
        public override int Run ()
        {
            try
            {
                Logger = new NullLogger<IrbisApplication>();

                PreRun();

                using var host = Magna.Host;
                using var connection = Connection.ThrowIfNull(nameof(Connection));
                connection.Connect();
                if (!connection.Connected)
                {
                    Logger.LogError("Can't connect");
                    Logger.LogInformation(IrbisException.GetErrorDescription(connection.LastError));

                    return 1;
                }
                else
                {
                    Logger.LogInformation("Successfully connected");
                }

                Magna.Host.Start();

                var result = ActualRun();

                Logger.LogInformation("Disconnecting");

                return result;

            }
            catch (Exception exception)
            {
                Logger.LogError
                    (
                        exception,
                        nameof(IrbisApplication) + "::" + nameof(Run)
                    );
            }

            return 1;

        } // method Run

        #endregion

    } // class IrbisApplication

} // namespace ManagedIrbis.AppServices
