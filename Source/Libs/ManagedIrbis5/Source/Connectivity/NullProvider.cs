// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* NullProvider.cs -- пустой клиент для нужд тестирования
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Пустой клиент для нужд тестирования.
    /// Не выполняет никаких осмысленных действий.
    /// </summary>
    public sealed class NullProvider
        : ISyncIrbisProvider
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public event EventHandler? BusyChanged;
        public string Database { get; set; }
        public bool Connected { get; }
        public bool Busy { get; }
        public int LastError { get; }
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

        public PlatformAbstractionLayer PlatformAbstraction { get; set; }
        public IPftFormatter AcquireFormatter()
        {
            throw new NotImplementedException();
        }

        public void Configure(string configurationString)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public string ReadFile(FileSpecification file)
        {
            throw new NotImplementedException();
        }

        public TermLink[] ExactSearchLinks(string term)
        {
            throw new NotImplementedException();
        }

        public TermLink[] ExactSearchTrimLinks(string term, int i)
        {
            throw new NotImplementedException();
        }

        public string FormatRecord(string format, Record record)
        {
            throw new NotImplementedException();
        }

        public string FormatRecord(string format, int mfn)
        {
            throw new NotImplementedException();
        }

        public string[] FormatRecords(int[] mfns, string format)
        {
            throw new NotImplementedException();
        }

        public int GetMaxMfn(string? databaseName = default)
        {
            throw new NotImplementedException();
        }

        public ServerVersion GetServerVersion()
        {
            throw new NotImplementedException();
        }

        public string[] ListFiles(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public bool NoOperation()
        {
            throw new NotImplementedException();
        }

        public MenuFile? ReadMenuFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public Record ReadRecord(int mfn)
        {
            throw new NotImplementedException();
        }

        public void ReleaseFormatter(IPftFormatter formatter)
        {
            throw new NotImplementedException();
        }

        public int[] Search(string expression)
        {
            throw new NotImplementedException();
        }

        public Record? ReadRecordVersion(int mfn, int version)
        {
            throw new NotImplementedException();
        }

        public Term[] ReadTerms(TermParameters parameters)
        {
            throw new NotImplementedException();
        }

        public bool FileExist(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public string GetGeneration()
        {
            throw new NotImplementedException();
        }

        public IniFile GetUserIniFile()
        {
            throw new NotImplementedException();
        }

        public void WriteRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public void ParseConnectionString(string connectionString)
        {
            throw new NotImplementedException();
        }

        public string? ReadTextFile(FileSpecification specification)
        {
            throw new NotImplementedException();
        }

        public GblResult GlobalCorrection(GblSettings settings)
        {
            throw new NotImplementedException();
        }
    } // class NullProvider

} // namespace ManagedIrbis
