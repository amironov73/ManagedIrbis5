// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MagnaApplication.cs -- класс-приложение
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.AppServices
{
    /// <summary>
    /// Класс-приложение.
    /// </summary>
    public class MagnaApplication
    {
        #region Properties

        /// <summary>
        /// Аргументы командной строки.
        /// </summary>
        public string[] Args { get; }

        /// <summary>
        /// Результат разбора командной строки.
        /// </summary>
        public ParseResult? ParseResult { get; protected set; }

        /// <summary>
        /// Конфигурация.
        /// </summary>
        [AllowNull]
        public IConfiguration Configuration { get; protected set; }

        /// <summary>
        /// Логгер.
        /// </summary>
        [AllowNull]
        public ILogger Logger { get; protected set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        public MagnaApplication
            (
                string[] args
            )
        {
            Args = args;

        } // constructor

        #endregion

        #region Private members

        private bool _prerun;

        #endregion

        #region Public methods

        /// <summary>
        /// Построение конфигурации.
        /// </summary>
        public virtual ConfigurationBuilder BuildConfiguration ()
        {
            var result = new ConfigurationBuilder();

            return result;

        } // method BuildConfiguration

        /// <summary>
        /// Построение хоста.
        /// </summary>
        public virtual IHostBuilder BuildHost()
        {
            return Host.CreateDefaultBuilder(Args);
        }

        /// <summary>
        /// Корневая команда для разбора командной строки.
        /// </summary>
        public virtual RootCommand? BuildRootCommand() => null;

        /// <summary>
        /// Конфигурирование сервисов.
        /// </summary>
        /// <param name="context">Контекст.</param>
        /// <param name="services">Коллекция сервисов.</param>
        public virtual void ConfigureServices
            (
                HostBuilderContext context,
                IServiceCollection services
            )
        {
            services.AddOptions();

        } // method ConfigureServices

        /// <summary>
        /// Конфигурирование логирования.
        /// </summary>
        /// <param name="logging">Билдер.</param>
        public virtual void ConfigureLogging
            (
                ILoggingBuilder logging
            )
        {
            logging.AddConsole();
            logging.AddConfiguration(Configuration);

        } // method ConfigureLogging

        /// <summary>
        /// Разбор командной строки.
        /// </summary>
        public virtual ParseResult? ParseCommandLine()
        {
            var rootCommand = BuildRootCommand();
            if (rootCommand is null)
            {
                return null;
            }

            var result = new CommandLineBuilder(rootCommand)
                .UseDefaults()
                .Build()
                .Parse(Args);

            return result;

        } // method ParseCommandLine

        /// <summary>
        /// Конфигурирование перед запуском.
        /// </summary>
        public virtual MagnaApplication PreRun()
        {
            if (_prerun)
            {
                return this;
            }

            Configuration = BuildConfiguration().Build();
            ParseResult = ParseCommandLine();

            var hostBuilder = BuildHost();
            hostBuilder.ConfigureServices(ConfigureServices);
            hostBuilder.ConfigureServices
                (
                    services => services.AddLogging(ConfigureLogging)
                );

            var host = hostBuilder.Build();
            Magna.Host = host;

            Logger = host.Services
                .GetService<ILoggerFactory>()
                .CreateLogger<MagnaApplication>();

            _prerun = true;

            return this;

        } // method PreRun

        /// <summary>
        /// Собственно работа приложения.
        /// </summary>
        /// <param name="action">Выполняемые действия</param>
        /// <returns>Код, возвращаемый операционной системе.
        /// </returns>
        public virtual int Run
            (
                Action<MagnaApplication> action
            )
        {
            try
            {
                PreRun();
                Magna.Application = this;

                using var host = Magna.Host;
                host.Start();

                action(this);

            }
            catch (Exception exception)
            {
                Logger.LogError
                    (
                        exception,
                        nameof(MagnaApplication) + "::" + nameof(Run)
                    );

                return 1;
            }

            return 0;

        } // method Run

        #endregion

    } // class MagnaApplication

} // namespace AM.AppServices
