// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InMemoryProvider.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using AM;
using AM.PlatformAbstraction;
using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.InMemory
{
    /// <summary>
    /// InMemoryProvider
    /// </summary>
    public class InMemoryProvider
        : ISyncIrbisProvider
    {
        #region Properties

        /// <summary>
        /// Базы данных.
        /// </summary>
        public List<InMemoryDatabase> Databases { get; }

        /// <summary>
        /// Ресурсы.
        /// </summary>
        public IResourceProvider Resources { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public InMemoryProvider
            (
                IResourceProvider resources
            )
        {
            Resources = resources;
            Databases = new();
            PlatformAbstraction = PlatformAbstractionLayer.Current;
        }

        #endregion

        #region ISyncIrbisProvider

        public void Dispose()
        {
            // Ничего не нужно делать
        }

        public ValueTask DisposeAsync() => ValueTask.CompletedTask;

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public event EventHandler? BusyChanged;
        public event EventHandler? Disposing;
        public string? Database { get; set; }
        public bool Connected { get; }
        public bool Busy { get; }
        public int LastError { get; }
        public PlatformAbstractionLayer PlatformAbstraction { get; }
        public void CancelOperation()
        {
            throw new NotImplementedException();
        }

        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        public void Configure(string configurationString)
        {
            throw new NotImplementedException();
        }

        public string GetGeneration()
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

        public bool Connect() => true;

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

        public bool Disconnect() => true;

        public bool FileExist(FileSpecification specification)
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

        #endregion

    } // class InMemoryProvider

} // namespace ManagedIrbis.InMemory
