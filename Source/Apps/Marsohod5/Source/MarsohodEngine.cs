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

/* MarsohodEngine.cs -- движок, выполняющий конверсию и импорт записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Configuration;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#endregion

#nullable enable

namespace Marsohod5
{
    /// <summary>
    /// Движок, выполняющий конверсию и импорт записей.
    /// </summary>
    public sealed class MarsohodEngine
        : BackgroundService
    {
        #region Construction

        public MarsohodEngine
            (
                IHost host,
                IOptions<MarsOptions> options,
                ILogger<MarsohodEngine> logger,
                IConfiguration configuration
            )
        {
            _host = host;
            _options = options.Value;
            _logger = logger;
            _configuration = configuration;
            _magazines = new ();

        }

        #endregion

        #region Private members

        private readonly IHost _host;
        private string? _folder;
        private readonly MarsOptions _options;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private SyncConnection? _connection;
        private readonly CaseInsensitiveDictionary<MarsMagazineInfo> _magazines;

        static readonly Regex _nameRegex = new
            (
                @"^(?<code>\w{4})(?<year>\d\d)(?<volume>_to\d+)?(?<number>_(?:no|vy)\d+)(?<chapter>_ch\d+)?$"
            );

        private void Error(string message)
        {
            _logger.LogError(message);
            throw new ApplicationException(message);
        }

        private void Configure()
        {
            _folder = _configuration["folder"];
            if (string.IsNullOrEmpty(_folder))
            {
                Error("No folder specified");
            }

            if (!Directory.Exists(_folder))
            {
                Error($"Directory {_folder} doesn't exist");
            }

            _connection = ConnectionFactory.Shared.CreateSyncConnection();
            _connection.ParseConnectionString(_options.ConnectionString);
            _connection.Connect();
        }

        private void LoadMagazines()
        {
            var records = _connection!.SearchRead("MARS=$");
            _logger.LogInformation($"{records.Length} records found");
            foreach (var record in records)
            {
                var magazine = MarsMagazineInfo.FromRecord(record, _options);
                if (magazine is { MarsCode: not null })
                {
                    _magazines.Add(magazine.MarsCode, magazine);
                }
            }

            _logger.LogInformation($"{_magazines.Count} magazines loaded");

        } // method LoadMagazines

        static string? GetCode
            (
                Record record,
                string name
            )
        {
            return record.Fields
                .GetField(903)
                .GetField('a', name)
                .GetSubField('b')
                .GetSubFieldValue()
                .FirstOrDefault();
        }

        private async Task ProcessInputFile
            (
                string inputFile,
                ChannelWriter<MarsohodRecord> channel,
                CancellationToken cancellationToken
            )
        {
            var fileName = Path.GetFileNameWithoutExtension(inputFile);
            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }

            _logger.LogInformation($"Найден файл: {inputFile}");
            var match = _nameRegex.Match(fileName);
            if (!match.Success)
            {
                _logger.LogInformation($"Неподдерживаемое имя файла: {fileName}");
                return;
            }

            var counter = 0;
            using (var stream = File.OpenRead(inputFile))
            {
                while (true)
                {
                    Record? sourceRecord;
                    try
                    {
                        sourceRecord = Iso2709.ReadRecord(stream, IrbisEncoding.Ansi);
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception, "Ошибка, пропускаем файл");
                        return;
                    }

                    if (sourceRecord is null)
                    {
                        break;
                    }

                    var payload = new MarsohodRecord
                    {
                        SourceRecord = sourceRecord
                    };
                    await channel.WriteAsync(payload, cancellationToken);
                    ++counter;
                }
            }

            _logger.LogInformation($"В файле {fileName} записей: {counter}");

        } // method ProcessInputFile

        private async Task DiscoverInputFiles
            (
                ChannelWriter<MarsohodRecord> channel,
                CancellationToken cancellationToken
            )
        {

            var inputFiles = Directory.EnumerateFiles(_folder!, _options.Pattern!);

            foreach (var inputFile in inputFiles)
            {
                await ProcessInputFile(inputFile, channel, cancellationToken);
            }

            channel.Complete();

        } // method DiscoverInputFiles

        private async Task ImportFiles
            (
                ChannelReader<MarsohodRecord> channel,
                CancellationToken cancellationToken
            )
        {
            while (!channel.Completion.IsCompleted)
            {
                var value = await channel.ReadAsync(cancellationToken);

                _logger.LogInformation($"Imported: {value.CurrentIssue}");
            }

        } // method ImportFiles

        #endregion

        #region Public methods

        protected override async Task ExecuteAsync
            (
                CancellationToken cancellationToken
            )
        {
            Configure();
            LoadMagazines();

            var options = new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = true
            };
            var channel = Channel.CreateUnbounded<MarsohodRecord>(options);
            var writing = DiscoverInputFiles(channel.Writer, cancellationToken);
            var reading = ImportFiles(channel.Reader, cancellationToken);

            await Task.WhenAll(writing, reading);

            _connection!.Disconnect();

            var lifetime = _host.Services.GetRequiredService<IHostApplicationLifetime>();
            lifetime.StopApplication();

        } // method ExecuteAsync

        #endregion

    } // class MarsohodEngine

} // namespace Marsohod5
