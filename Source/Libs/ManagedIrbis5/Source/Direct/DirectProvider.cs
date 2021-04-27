// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/* DirectProvider.cs -- провайдер, работающий напрямую с файлами баз данных ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AM;
using AM.IO;
using AM.PlatformAbstraction;
using AM.Threading;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Direct
{
    /// <summary>
    /// Провайдер, работающий напрямую с файлами баз данных ИРБИС64.
    /// </summary>
    public class DirectProvider
        : ISyncProvider
    {
        public event EventHandler? Disposing;

        /// <summary>
        /// Конструктор.
        /// </summary>
        public DirectProvider()
        {
            Busy = new BusyState();
            PlatformAbstraction = PlatformAbstractionLayer.Current;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="rootPath">Корневой путь.</param>
        public DirectProvider
            (
                string rootPath
            )
        {
            Busy = new BusyState();
            PlatformAbstraction = PlatformAbstractionLayer.Current;
        }

        /// <inheritdoc cref="ISyncProvider.FileExist"/>
        public bool FileExist
            (
                FileSpecification specification
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IIrbisProvider.GetGeneration"/>
        public string GetGeneration()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IIrbisProvider.PlatformAbstraction"/>
        public PlatformAbstractionLayer PlatformAbstraction
        {
            get;
            set;
        }

        /// <inheritdoc cref="IIrbisProvider.Configure"/>
        public void Configure
            (
                string configurationString
            )
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Disposing.Raise(this);
        }

        /// <inheritdoc cref="IAsyncDisposable.DisposeAsync"/>
        public ValueTask DisposeAsync()
        {
            Dispose();
            return ValueTask.CompletedTask;
        }

        /// <inheritdoc cref="IServiceProvider.GetService"/>
        public object? GetService
            (
                Type serviceType
            )
        {
            throw new NotImplementedException();
        }

        private void SetBusy(bool busy)
        {
            Busy = busy;
            BusyChanged?.Invoke(this, EventArgs.Empty);
        }

        private void SetConnected(bool state) => Connected = state;

        private void SetLastError(int code) => LastError = code;

        public event EventHandler? BusyChanged;
        public string? Database { get; set; } = "IBIS";
        public bool Connected { get; private set; }
        public BusyState Busy { get; private set; }
        public int LastError { get; private set; }
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        public WaitHandle GetWaitHandle()
        {
            throw new NotImplementedException();
        }

        public bool ActualizeRecord(ActualizeRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public bool CreateDatabase(CreateDatabaseParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool CreateDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool DeleteDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool Disconnect()
        {
            throw new NotImplementedException();
        }

        public bool FormatRecords(FormatRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public FullTextResult? FullTextSearch(SearchParameters searchParameters, TextParameters textParameters)
        {
            throw new NotImplementedException();
        }

        public DatabaseInfo? GetDatabaseInfo(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public int GetMaxMfn(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public ServerStat? GetServerStat()
        {
            throw new NotImplementedException();
        }

        public ServerVersion? GetServerVersion()
        {
            throw new NotImplementedException();
        }

        public GblResult? GlobalCorrection(GblSettings settings)
        {
            throw new NotImplementedException();
        }

        public string[]? ListFiles(params FileSpecification[] specifications)
        {
            throw new NotImplementedException();
        }

        public ProcessInfo[]? ListProcesses()
        {
            throw new NotImplementedException();
        }

        public UserInfo[]? ListUsers()
        {
            throw new NotImplementedException();
        }

        public bool NoOperation()
        {
            throw new NotImplementedException();
        }

        public string? PrintTable(TableDefinition definition)
        {
            throw new NotImplementedException();
        }

        public byte[]? ReadBinaryFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public TermPosting[]? ReadPostings(PostingParameters parameters)
        {
            throw new NotImplementedException();
        }

        public Record? ReadRecord(ReadRecordParameters parameters)
        {
            throw new NotImplementedException();
        }

        public TermPosting[]? ReadRecordPostings(ReadRecordParameters parameters, string prefix)
        {
            throw new NotImplementedException();
        }

        public Term[]? ReadTerms(TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        public string? ReadTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool ReloadDictionary(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool ReloadMasterFile(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool RestartServer()
        {
            throw new NotImplementedException();
        }

        public FoundItem[]? Search(SearchParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool TruncateDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UnlockDatabase(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UnlockRecords(IEnumerable<int> mfnList, string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public bool UpdateIniFile(IEnumerable<string> lines)
        {
            throw new NotImplementedException();
        }

        public bool UpdateUserList(IEnumerable<UserInfo> users)
        {
            throw new NotImplementedException();
        }

        public bool WriteTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool WriteRecord(WriteRecordParameters parameters)
        {
            throw new NotImplementedException();
        }
    } // class DirectProvider

} // namespace ManagedIrbis.Direct
