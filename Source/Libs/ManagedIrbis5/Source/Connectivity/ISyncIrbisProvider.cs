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

/* ISyncIrbisProvider.cs -- интерфейс синхронного ИРБИС-провайдера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

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
    /// Интерфейс синхронного ИРБИС-провайдера.
    /// </summary>
    public interface ISyncIrbisProvider
        : IBasicIrbisProvider
    {
        ///<summary>
        /// Слой платформенной абстракции.
        /// </summary>
        PlatformAbstractionLayer PlatformAbstraction { get; set; }

        IPftFormatter AcquireFormatter();

        /// <summary>
        /// Конфигурирование провайдера.
        /// </summary>
        void Configure(string configurationString);

        bool Connect();

        /// <summary>
        /// Чтение файла с сервера.
        /// </summary>
        string ReadFile(FileSpecification file);

        TermLink[] ExactSearchLinks(string term);

        TermLink[] ExactSearchTrimLinks(string term, int i);

        string FormatRecord(string format, Record record);

        string FormatRecord(string format, int mfn);

        string[] FormatRecords(int[] mfns, string format);

        int GetMaxMfn(string? databaseName = default);

        ServerVersion GetServerVersion();

        string[] ListFiles(FileSpecification specification);

        bool NoOperation();

        MenuFile? ReadMenuFile(FileSpecification specification);
        Record ReadRecord(int mfn);
        void ReleaseFormatter(IPftFormatter formatter);


        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        int[] Search(string expression);

        Record? ReadRecordVersion(int mfn, int version);

        Term[] ReadTerms(TermParameters parameters);

        bool FileExist(FileSpecification specification);

        string GetGeneration();

        IniFile GetUserIniFile();

        void WriteRecord(Record record);

        void ParseConnectionString(string connectionString);

        string? ReadTextFile(FileSpecification specification);

        GblResult GlobalCorrection(GblSettings settings);
    } // interface ISyncIrbisProvider

} // namespace ManagedIrbis
