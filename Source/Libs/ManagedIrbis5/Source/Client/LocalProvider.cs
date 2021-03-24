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
        : IrbisProvider
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

        public override bool Connected { get; }

        public override IPftFormatter AcquireFormatter() =>
            throw new NotImplementedException();

        public override MenuFile? ReadMenuFile(FileSpecification specification) =>
            throw new NotImplementedException();

        public override Record? ReadRecordVersion(int mfn, int version) =>
            throw new NotImplementedException();

        public override Term[] ReadTerms(TermParameters parameters) =>
            throw new NotImplementedException();

        public override bool FileExist(FileSpecification specification) =>
            throw new NotImplementedException();

        public override string GetGeneration() =>
            throw new NotImplementedException();

        public override IniFile GetUserIniFile()
        {
            throw new NotImplementedException();
        }

        public override void WriteRecord(Record record)
        {
            throw new NotImplementedException();
        }

        public override void Configure(string configurationString) =>
            throw new NotImplementedException();

        public override PlatformAbstractionLayer PlatformAbstraction { get; set; }

        public override string ReadFile(FileSpecification file) =>
            throw new NotImplementedException();

        public override int[] Search(string expression) =>
            throw new NotImplementedException();

        public override TermLink[] ExactSearchLinks(string term) =>
            throw new NotImplementedException();

        public override TermLink[] ExactSearchTrimLinks(string term, int i) =>
            throw new NotImplementedException();

        public override string FormatRecord(Record record, string format) =>
            throw new NotImplementedException();

        public override int GetMaxMfn() =>
            throw new NotImplementedException();

        public override ServerVersion GetServerVersion() =>
            throw new NotImplementedException();

        public override string[] FormatRecords(int[] mfns, string format) =>
            throw new NotImplementedException();

        public override string[] ListFiles(FileSpecification specification) =>
            throw new NotImplementedException();

        public override bool NoOp() =>
            throw new NotImplementedException();

        public override void ReleaseFormatter(IPftFormatter formatter) =>
            throw new NotImplementedException();

        public override Record ReadRecord(int mfn) =>
            throw new NotImplementedException();

        public override void Dispose() =>
            throw new NotImplementedException();

        #endregion

    } // class LocalProvider

} // namespace ManagedIrbis.Client
