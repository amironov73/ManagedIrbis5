// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace Marsohod5
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // включаем конфигурирование в appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .AddCommandLine(args)
                .Build();

            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    // включаем логирование
                    services.AddLogging(logging => logging.AddConsole());

                    // запоминаем конфигурацию на всякий случай
                    services.AddSingleton<IConfiguration>(configuration);

                    // регистрируем наш сервис
                    services.AddHostedService<MarsohodEngine>();

                    // откуда брать настройки
                    var section = configuration.GetSection("MarsOptions");
                    services.Configure<MarsOptions>(section);

                });

            using var host = hostBuilder.Build();

            await host.StartAsync(); // запускаем наш сервис
            await host.WaitForShutdownAsync(); // ожидаем его окончания

            // К этому моменту всё успешно выполнено либо произошла ошибка
            Console.WriteLine("THAT'S ALL, FOLKS!");

            return 0;

        } // method Main

    } // class Program

} // namespace Marsohod5
