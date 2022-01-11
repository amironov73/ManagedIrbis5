// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Logging;

#endregion

#nullable enable

namespace TeleIrbis;

/*

  Допустим, мы хотим опубликовать .NET-приложение. Раньше, в .NET 5 оно
  спокойно публиковалось, а теперь, в эпоху .NET 6 внезапно выскакивает ошибка

    Microsoft.NET.ConflictResolution.targets(112, 5): [NETSDK1152] обнаружено
    несколько выходных файлов публикации с одним и тем же относительным путем:
    C:\Users\amiro\.nuget\packages\nlog.config\4.7.12\contentFiles\any\any\NLog.config,
    D:\Projects\ManagedIrbis5\Source\Apps\TeleIrbis\NLog.config.

  Понятно, что самый лучший вариант здесь - разобраться, откуда у нас два файла
  NLog.config, но если нужно решить вопрос по-быстрому, то выход таков: добавить в проект атрибут ErrorOnDuplicatePublishOutputFiles

  <PropertyGroup>
   <ErrorOnDuplicatePublishOutputFiles>false</ErrorOnDuplicatePublishOutputFiles>
  </PropertyGroup>

  Костыль, конечно, но выручает.

 */

 /*
  * Куда у нас складывается лог -- в "C:\ProgramData\TeleIrbis\LogFile.txt"
  * На *nix: "/usr/share/TeleIrbis/LogFile.txt"
  */

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
        service.SetDescription ("Telegram bot service for IRBIS64");
        service.SetDisplayName ("Telegram Bot");
        service.SetServiceName ("TelegramBot");

        service.StartAutomaticallyDelayed();
        service.RunAsNetworkService();
        service.EnableShutdown();

        service.UseNLog();

        // Необязательная настройка восстановления после сбоев
        service.EnableServiceRecovery (recovery => { recovery.RestartService (1); });

        // Реакция на исключение
        service.OnException (exception =>
        {
            var log = HostLogger.Get<BotService>();
            log.ErrorFormat ($"Exception {exception}");
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
        var result = HostFactory.Run (ConfigureService);

        return (int)result;
    }

    /// <summary>
    /// Заглушка на случай сбоев сертификатов.
    /// </summary>
    private static bool _ServerCertificateValidationCallback
        (
            object sender,
            X509Certificate? certificate,
            X509Chain? chain,
            SslPolicyErrors sslpolicyerrors
        )
    {
        return true;
    }

    private static void ConfigureSecurityProtocol()
    {
        ServicePointManager.SecurityProtocol =

            // SecurityProtocolType.Ssl3 |
            SecurityProtocolType.Tls |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls12;
        ServicePointManager.CheckCertificateRevocationList = false;
        ServicePointManager.ServerCertificateValidationCallback
            = _ServerCertificateValidationCallback;
    }

    // private static void SetupLogging()
    // {
    //     HostLogger.UseLogger(new NLogLogWriterFactory.NLogHostLoggerConfigurator());
    // }

    #endregion

    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main (string[] args)
    {
        //SetupLogging();
        ConfigureSecurityProtocol();

        return ConfigureAndRunService (args);
    }

    #endregion
}
