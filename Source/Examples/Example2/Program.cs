// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global

//
// Пример организации "кровавого энтерпрайза" на основе ИРБИС64
// с логированием, инверсией зависимостей, фоновыми сервисами,
// асинхронщиной, настраиваемым конфигурированием и т. д.
//

using System;
using System.Threading;
using System.Threading.Tasks;

using ManagedIrbis;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#nullable enable

/// <summary>
/// Настройки для нашего сервиса
/// (см. секцию SampleOptions в appsettings.json).
/// </summary>
internal class SampleOptions
{
    public string? ConnectionString { get; set; }
}

/// <summary>
/// Наш сервис.
/// Он не делает ничего особенного,
/// просто подключается к серверу ИРБИС64
/// и запрашивает максимальный MFN в базе,
/// после этого он завершает работу.
/// </summary>
internal class SampleService
    : BackgroundService
{
    private readonly ILogger _logger;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IAsyncConnection _connection;
    private readonly IOptions<SampleOptions> _options;

    public SampleService
        (
            ILogger<SampleService> logger,
            IHostApplicationLifetime lifetime,
            IAsyncConnection connection,
            IOptions<SampleOptions> options
        )
    {
        _logger = logger;
        _lifetime = lifetime;
        _connection = connection;
        _options = options;
    }

    /// <summary>
    /// Полезная деятельность нашего сервиса.
    /// </summary>
    protected override async Task ExecuteAsync
        (
            CancellationToken stoppingToken
        )
    {
        using var scope = _logger.BeginScope("FreshMeat");

        _logger.LogInformation("Starting");

        var connectionString = _options.Value.ConnectionString;
        _logger.LogInformation($"Credentials: {connectionString}");
        _connection.ParseConnectionString(connectionString);
        var success = await _connection.ConnectAsync();
        if (success)
        {
            var maxMfn = await _connection.GetMaxMfnAsync();
            _logger.LogInformation($"Max MFN={maxMfn}");
            await _connection.DisconnectAsync();
        }
        else
        {
            _logger.LogError($"Can't connect!");
        }

        _logger.LogInformation("Stopping");
        _lifetime.StopApplication();
    }
}

class Program
{
    static async Task<int> Main(string[] args)
    {
        // Включаем конфигурирование в appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Настраиваем хост
        var builder = Host.CreateDefaultBuilder(args).ConfigureServices
                (
                    services =>
                    {
                        // включаем логирование
                        services.AddLogging(logging => logging.AddConsole());

                        // регистрируем провайдеры ИРБИС64
                        services.RegisterIrbisProviders();

                        // регистрируем наш сервис
                        services.AddHostedService<SampleService>();

                        // откуда брать настройки
                        var section = config.GetSection("SampleOptions");
                        services.Configure<SampleOptions>(section);
                    }
                );

        using var host = builder.Build();
        await host.StartAsync(); // запускаем наш сервис
        await host.WaitForShutdownAsync(); // ожидаем его окончания

        // К этому моменту всё успешно выполнено либо произошла ошибка
        Console.WriteLine("THAT'S ALL, FOLKS!");

        return 0;
    }
}
