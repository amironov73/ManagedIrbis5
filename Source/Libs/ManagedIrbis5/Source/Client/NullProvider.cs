// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NullProvider.cs -- null provider used for testing
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
    /// Null provider used for testing.
    /// </summary>
    public sealed class NullProvider
        : ISyncIrbisProvider
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public NullProvider()
        {
            Database = "IBIS";
        }

        #endregion

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

        public void Configure(string configurationString)
        {
        }

        public PlatformAbstractionLayer PlatformAbstraction { get; set; }

        public string ReadFile(FileSpecification file)
        {
            return string.Empty;
        }

        public int[] Search(string expression)
        {
            return Array.Empty<int>();
        }

        public TermLink[] ExactSearchLinks(string term)
        {
            return Array.Empty<TermLink>();
        }

        public TermLink[] ExactSearchTrimLinks(string term, int i)
        {
            return Array.Empty<TermLink>();
        }

        public string FormatRecord(Record record, string format)
        {
            return string.Empty;
        }

        public int GetMaxMfn()
        {
            return 0;
        }

        public ServerVersion GetServerVersion()
        {
            return null;
        }

        public string[] FormatRecords(int[] mfns, string format)
        {
            return Array.Empty<string>();
        }

        public string[] ListFiles(FileSpecification specification)
        {
            return Array.Empty<string>();
        }

        public bool NoOperation()
        {
            return true;
        }

        public void ReleaseFormatter(IPftFormatter formatter)
        {
        }

        public Record ReadRecord(int mfn)
        {
            return null;
        }

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }
    } // class NullProvider

} // namespace ManagedIrbis.Client
