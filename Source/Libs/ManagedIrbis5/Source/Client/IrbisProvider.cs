// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedParameter.Local

/* IrbisProvider.cs -- абстрактный ИРБИС-провайдер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

using AM.PlatformAbstraction;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft;

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Абстрактный ИРБИС-провайдер.
    /// </summary>
    public abstract class IrbisProvider
    {
        /// <summary>
        /// Текущая база данных.
        /// </summary>
        public string? Database { get; set; }

        public abstract bool Connected { get; }

        /// <summary>
        public abstract void Configure(string configurationString);

        /// Слой платформенной абстракции.
        /// </summary>
        public abstract PlatformAbstractionLayer PlatformAbstraction { get; set; }

        /// <summary>
        /// Чтение файла с сервера.
        /// </summary>
        public abstract string ReadFile(FileSpecification file);

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        public abstract int[] Search(string expression);

        public abstract TermLink[] ExactSearchLinks(string term);

        public abstract TermLink[] ExactSearchTrimLinks(string term, int i);

        public abstract string FormatRecord(Record record, string format);

        public abstract int GetMaxMfn();

        public abstract ServerVersion GetServerVersion();

        public abstract string[] FormatRecords(int[] mfns, string format);

        public abstract string[] ListFiles(FileSpecification specification);

        public abstract bool NoOp();

        public abstract void ReleaseFormatter(IPftFormatter formatter);

        public abstract Record ReadRecord(int mfn);

        public abstract void Dispose();
    } // class IrbisProvider

} // namespace ManagedIrbis.Client
