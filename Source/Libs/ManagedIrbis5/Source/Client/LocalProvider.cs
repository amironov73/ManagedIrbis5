// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* LocalProvider.cs -- провайдер, работающий с локальными файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Threading.Tasks;
using AM.IO;
using AM.PlatformAbstraction;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Провайдер, работающий с локальными файлами.
    /// </summary>
    public class LocalProvider
        : ISyncIrbisProvider
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LocalProvider()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public LocalProvider(string rootPath)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IrbisProvider members

        public string Host { get; set; }
        public ushort Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string Workstation { get; set; }
        public int ClientId { get; }
        public int QueryId { get; }
        public bool Connected { get; }

        public bool Busy { get; }

        public int LastError { get; }

        public bool CheckProviderState()
        {
            throw new NotImplementedException();
        }

        public IPftFormatter AcquireFormatter() =>
            throw new NotImplementedException();

        public MenuFile? ReadMenuFile(FileSpecification specification) =>
            throw new NotImplementedException();

        public Record? ReadRecordVersion(int mfn, int version) =>
            throw new NotImplementedException();

        public Term[] ReadTerms(TermParameters parameters) =>
            throw new NotImplementedException();

        public bool FileExist(FileSpecification specification) =>
            throw new NotImplementedException();

        public string GetGeneration() =>
            throw new NotImplementedException();

        public IniFile GetUserIniFile()
        {
            throw new NotImplementedException();
        }

        public void WriteRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public void Configure(string configurationString) =>
            throw new NotImplementedException();

        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        public string ReadFile(FileSpecification file) =>
            throw new NotImplementedException();

        public int[] Search(string expression) =>
            throw new NotImplementedException();

        public TermLink[] ExactSearchLinks(string term) =>
            throw new NotImplementedException();

        public TermLink[] ExactSearchTrimLinks(string term, int i) =>
            throw new NotImplementedException();

        public string FormatRecord(Record record, string format) =>
            throw new NotImplementedException();

        public int GetMaxMfn() =>
            throw new NotImplementedException();

        public ServerVersion GetServerVersion() =>
            throw new NotImplementedException();

        public string[] FormatRecords(int[] mfns, string format) =>
            throw new NotImplementedException();

        public string[] ListFiles(FileSpecification specification) =>
            throw new NotImplementedException();

        public bool NoOperation() =>
            throw new NotImplementedException();

        public void ReleaseFormatter(IPftFormatter formatter) =>
            throw new NotImplementedException();

        public Record ReadRecord(int mfn) =>
            throw new NotImplementedException();

        public void Dispose() =>
            throw new NotImplementedException();

        #endregion

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    } // class LocalProvider

} // namespace ManagedIrbis.Client
