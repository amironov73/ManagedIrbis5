// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedParameter.Global
// ReSharper disable UnusedType.Global

/* InMemoryProvider.cs -- провайдер ИРБИС64, хранящий свои данные в оперативной памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Records;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// Провайдер ИРБИС64, хранящий свои данные в оперативной памяти.
    /// </summary>
    public sealed class InMemoryProvider
        : ISyncProvider
    {
        #region Constants

        /// <summary>
        /// Я не знаю, какой код возврата надо использовать.
        /// </summary>
        private const int DontKnowWhatCodeToUse = -12345678;

        /// <summary>
        /// Всё хорошо, прекрасная маркиза!
        /// </summary>
        private const int SoFarSoGood = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Провайдер только для чтения?
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Базы данных.
        /// </summary>
        public CaseInsensitiveDictionary<InMemoryDatabase> Databases { get; }

        /// <summary>
        /// Ресурсы.
        /// </summary>
        public IResourceProvider Resources { get; }

        /// <inheritdoc cref="ISupportLogging.Logger"/>
        // TODO implement
        public ILogger? Logger => null;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryProvider
            (
                IResourceProvider resources,
                IServiceProvider serviceProvider
            )
        {
            _serviceProvider = serviceProvider;
            _cancellation = new CancellationTokenSource();
            Busy = new BusyState();
            Resources = resources;
            Databases = new();
            PlatformAbstraction = PlatformAbstractionLayer.Current;
        }

        #endregion

        #region Private members

        private readonly CancellationTokenSource _cancellation;
        private readonly IServiceProvider _serviceProvider;

        private void SetBusy(bool busy) => Busy.SetState(busy);

        private string TranslateSpecification
            (
                FileSpecification specification
            )
        {
            var fileName = specification.FileName.ThrowIfNull("specification.FileName");

            // TODO: implement

            return fileName;
        }

        /// <summary>
        /// Текущая база данных.
        /// </summary>
        private InMemoryDatabase CurrentDatabase => GetDatabase(Database)
            .ThrowIfNull("CurrentDatabase");

        private InMemoryDatabase? GetDatabase
            (
                string? databaseName
            )
        {
            databaseName ??= Database ?? throw new IrbisException();
            if (!Databases.TryGetValue(databaseName, out var result))
            {
                return default;
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Дамп провайдера.
        /// </summary>
        public void DumpAll
            (
                TextWriter output
            )
        {
            foreach (var database in Databases.Values)
            {
                database.Dump(output);
            }

            Resources.Dump(output);
        }

        /// <summary>
        /// Загрузка данных (в нативном для провайдера формате) из указанной папки.
        /// </summary>
        public void LoadData(string path) => throw new NotImplementedException();

        /// <summary>
        /// Сохранение данных (в нативном для провайдера формате) в указанную папку.
        /// </summary>
        public void SaveData(string path) => throw new NotImplementedException();

        #endregion

        #region ICancellable members

        /// <inheritdoc cref="ICancellable.Busy"/>
        public BusyState Busy { get; private set; }

        /// <inheritdoc cref="ICancellable.CancelOperation"/>
        public void CancelOperation() => _cancellation.Cancel();

        /// <inheritdoc cref="ICancellable.ThrowIfCancelled"/>
        public void ThrowIfCancelled()
        {
            if (_cancellation.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }
        }

        #endregion

        #region IIrbisProvider members

        /// <inheritdoc cref="IIrbisProvider.Disposing"/>
        public event EventHandler? Disposing;

        /// <inheritdoc cref="IIrbisProvider.Database"/>
        public string? Database { get; set; }

        /// <inheritdoc cref="IIrbisProvider.Connected"/>
        public bool Connected { get; private set; }

        /// <inheritdoc cref="IGetLastError.LastError"/>
        public int LastError { get; private set; }

        /// <inheritdoc cref="IIrbisProvider.PlatformAbstraction"/>
        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        /// <inheritdoc cref="IIrbisProvider.CheckProviderState"/>
        public bool CheckProviderState() => true;

        /// <inheritdoc cref="IIrbisProvider.Configure"/>
        public void Configure
            (
                string configurationString
            )
        {
            // TODO: что делать?
        }

        /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
        public string GetGeneration() => "64";

        /// <inheritdoc cref="IIrbisProvider.GetWaitHandle"/>
        public WaitHandle GetWaitHandle() => Busy.WaitHandle;

        #endregion

        #region ISyncProvider

        /// <inheritdoc cref="ISyncProvider.ActualizeRecord"/>
        public bool ActualizeRecord
            (
                ActualizeRecordParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.Connect"/>
        public bool Connect()
        {
            Connected = true;
            LastError = SoFarSoGood;

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.CreateDatabase"/>
        public bool CreateDatabase
            (
                CreateDatabaseParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            var name = parameters.Database.ThrowIfNullOrWhiteSpace();

            if (Databases.ContainsKey(name))
            {
                LastError = DontKnowWhatCodeToUse;
                return false;
            }

            var database = new InMemoryDatabase(name, ReadOnly);
            Databases.Add(database.Name, database);
            LastError = SoFarSoGood;

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.CreateDictionary"/>
        public bool CreateDictionary
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.DeleteDatabase"/>
        public bool DeleteDatabase
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            var name = databaseName ?? Database ?? throw new IrbisException();
            LastError = SoFarSoGood;

            return Databases.Remove(name);
        }

        /// <inheritdoc cref="ISyncProvider.Disconnect"/>
        public bool Disconnect()
        {
            Connected = false;
            LastError = SoFarSoGood;

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.FileExist"/>
        public bool FileExist
            (
                FileSpecification specification
            )
        {
            using var guard = new BusyGuard(Busy);

            var fileName = TranslateSpecification(specification);
            LastError = SoFarSoGood;

            return Resources.ResourceExists(fileName);
        }

        /// <inheritdoc cref="ISyncProvider.FormatRecords"/>
        public bool FormatRecords
            (
                FormatRecordParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.FullTextSearch"/>
        public FullTextResult? FullTextSearch
            (
                SearchParameters searchParameters,
                TextParameters textParameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.GetDatabaseInfo"/>
        public DatabaseInfo? GetDatabaseInfo
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.GetMaxMfn"/>
        public int GetMaxMfn
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            var name = databaseName ?? Database ?? throw new IrbisException();
            var database = Databases[name];
            var result = database.Master.Count;
            LastError = SoFarSoGood;

            return result;
        }

        /// <inheritdoc cref="ISyncProvider.GetServerStat"/>
        public ServerStat? GetServerStat()
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.GetServerVersion"/>
        public ServerVersion? GetServerVersion()
        {
            LastError = SoFarSoGood;

            return new()
            {
                Organization = "Open source версия",
                ConnectedClients = 1,
                MaxClients = int.MaxValue,
                Version = "64.2020.1"
            };
        }

        /// <inheritdoc cref="ISyncProvider.GlobalCorrection"/>
        public GblResult? GlobalCorrection
            (
                GblSettings settings
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ListFiles"/>
        public string[]? ListFiles
            (
                params FileSpecification[] specifications
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ListProcesses"/>
        public ProcessInfo[]? ListProcesses()
        {
            LastError = SoFarSoGood;

            return Array.Empty<ProcessInfo>();
        }

        /// <inheritdoc cref="ISyncProvider.ListUsers"/>
        public UserInfo[]? ListUsers()
        {
            using var guard = new BusyGuard(Busy);

            var specification = new FileSpecification
            {
                Path = IrbisPath.Data,
                FileName = "client_m.mnu"
            };

            var text = ReadTextFile(specification);
            if (string.IsNullOrEmpty(text))
            {
                LastError = DontKnowWhatCodeToUse;
                return default;
            }

            var result = UserInfo.Parse(text);
            LastError = SoFarSoGood;

            return result;
        }

        /// <inheritdoc cref="ISyncProvider.NoOperation"/>
        public bool NoOperation() => true;

        /// <inheritdoc cref="ISyncConnection"/>
        public string? PrintTable
            (
                TableDefinition definition
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadBinaryFile"/>
        public byte[]? ReadBinaryFile
            (
                FileSpecification specification
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadPostings"/>
        public TermPosting[]? ReadPostings
            (
                PostingParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadRecord{T}"/>
        public T? ReadRecord<T>
            (
                ReadRecordParameters parameters
            )
            where T: class, IRecord, new()
        {
            if (typeof(T) != typeof(Record))
            {
                throw new NotSupportedException();
            }

            using var guard = new BusyGuard(Busy);

            var db = GetDatabase(parameters.Database);
            if (db is null)
            {
                LastError = DontKnowWhatCodeToUse;
                return default;
            }

            LastError = SoFarSoGood;

            return (T?) (object?) db.ReadRecord(parameters.Mfn);

        } // method ReadRecord

        /// <inheritdoc cref="ISyncProvider.ReadRecordPostings"/>
        public TermPosting[]? ReadRecordPostings
            (
                ReadRecordParameters parameters,
                string prefix
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadTerms"/>
        public Term[]? ReadTerms
            (
                TermParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReadTextFile"/>
        public string? ReadTextFile
            (
                FileSpecification specification
            )
        {
            using var guard = new BusyGuard(Busy);

            var fileName = TranslateSpecification(specification);

            LastError = SoFarSoGood;

            return Resources.ReadResource(fileName);
        }

        /// <inheritdoc cref="ISyncProvider.ReloadDictionary"/>
        public bool ReloadDictionary
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.ReloadMasterFile"/>
        public bool ReloadMasterFile
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.RestartServer"/>
        public bool RestartServer()
        {
            LastError = SoFarSoGood;

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.Search"/>
        public FoundItem[]? Search
            (
                SearchParameters parameters
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.TruncateDatabase"/>
        public bool TruncateDatabase
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            var name = databaseName ?? Database ?? throw new IrbisException();
            var database = Databases[name];
            if (!database.ReadOnly)
            {
                database.Master.Clear();
                database.Inverted.Clear();
            }

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.UnlockDatabase"/>
        public bool UnlockDatabase
            (
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            var database = GetDatabase(databaseName);
            if (database is null)
            {
                LastError = DontKnowWhatCodeToUse;
                return false;
            }

            database.Locked = false;
            LastError = SoFarSoGood;

            return true;
        }

        /// <inheritdoc cref="ISyncProvider.UnlockRecords"/>
        public bool UnlockRecords
            (
                IEnumerable<int> mfnList,
                string? databaseName = default
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.UpdateIniFile"/>
        public bool UpdateIniFile
            (
                IEnumerable<string> lines
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.UpdateUserList"/>
        public bool UpdateUserList
            (
                IEnumerable<UserInfo> users
            )
        {
            using var guard = new BusyGuard(Busy);

            throw new NotImplementedException();
        }

        /// <inheritdoc cref="ISyncProvider.WriteTextFile"/>
        public bool WriteTextFile
            (
                FileSpecification specification
            )
        {
            using var guard = new BusyGuard(Busy);

            if (ReadOnly)
            {
                LastError = DontKnowWhatCodeToUse;
                return true;
            }

            var fileName = TranslateSpecification(specification);
            LastError = SoFarSoGood;

            return Resources.WriteResource(fileName, specification.Content);
        }

        /// <inheritdoc cref="ISyncProvider.WriteRecord"/>
        public bool WriteRecord
            (
                WriteRecordParameters parameters
            )
        {
            if (ReadOnly)
            {
                LastError = DontKnowWhatCodeToUse;
                return true;
            }

            if (parameters.Record is not Record record)
            {
                // TODO: обработка других типов записей?

                LastError = DontKnowWhatCodeToUse;
                return false;
            }

            var db = GetDatabase(record.Database);
            if (db is null)
            {
                LastError = DontKnowWhatCodeToUse;
                return false;
            }

            if (db.ReadOnly)
            {
                LastError = DontKnowWhatCodeToUse;
                return true;
            }

            if (!db.WriteRecord(record))
            {
                LastError = DontKnowWhatCodeToUse;
                return false;
            }

            parameters.MaxMfn = db.Master.Count + 1;
            LastError = SoFarSoGood;

            return true;
        }

        #endregion

        #region IServiceProvider members

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService(Type serviceType) => _serviceProvider.GetService(serviceType);

        #endregion

        #region IAsyncDisposable members

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Busy.Dispose();
            Disposing?.Invoke(this, EventArgs.Empty);
        }

        #endregion

    } // class InMemoryProvider

} // namespace ManagedIrbis.InMemory
