// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Logging;

#endregion

#nullable enable

namespace TeleIrbis
{
    class Program
    {
        #region Private members

        /// <summary>
        /// Конфигурация сервиса средствами Topshelf.
        /// </summary>
        private static void ConfigureService
            (
                HostConfigurator configurator
            )
        {
            configurator.ApplyCommandLine();

            var service = configurator.Service<BotService>();
            service.SetDescription("Telegram bot service for IRBIS64");
            service.SetDisplayName("Telegram Bot");
            service.SetServiceName("TelegramBot");

            service.StartAutomaticallyDelayed();
            service.RunAsLocalSystem();
            service.EnableShutdown();

            service.UseNLog();

            // Необязательная настройка восстановления после сбоев
            service.EnableServiceRecovery(recovery =>
            {
                recovery.RestartService(1);

            });

            // Реакция на исключение
            service.OnException(exception =>
            {
                var log = HostLogger.Get<BotService>();
                log.ErrorFormat($"Exception {exception}");
            });
        }

        /// <summary>
        /// Конфигурирование и запуск сервиса.
        /// </summary>
        private static int ConfigureAndRunService
            (
                string[] args
            )
        {
            var result = HostFactory.Run(ConfigureService);

            return (int)result;
        }

        #endregion

        #region Program entry point

        /// <summary>
        /// Точка входа в программу.
        /// </summary>
        static int Main(string[] args)
        {
            return ConfigureAndRunService(args);
        }

        #endregion
    }
}
