// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedMember.Global

/* MagazineManager.cs -- работа с периодикой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;
using AM.Linq;

using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines;

/// <summary>
/// Работа с периодикой.
/// </summary>
public sealed class MagazineManager
{
    #region Constants

    /// <summary>
    /// Вид документа – сводное описание газеты.
    /// </summary>
    public const string Newspaper = "V=01";

    /// <summary>
    /// Вид документа – сводное описание журнала.
    /// </summary>
    public const string Magazine = "V=02";

    #endregion

    #region Properties

    /// <summary>
    /// Клиент для связи с сервером.
    /// </summary>
    public ISyncProvider Connection { get; }

    /// <summary>
    /// Конфигурация библиографических записей.
    /// </summary>
    public RecordConfiguration RecordConfiguration { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MagazineManager
        (
            IHost host,
            ISyncProvider connection,
            RecordConfiguration? recordConfiguration = null
        )
    {
        Sure.NotNull (host);
        Sure.NotNull (connection);

        _logger = LoggingUtility.GetLogger (host, typeof (BindingManager));
        var options = new MemoryCacheOptions();
        _cache = new MemoryCache (options);
        Connection = connection;
        RecordConfiguration = recordConfiguration ?? RecordConfiguration.GetDefault();
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Получение записи по ее шифру в базе.
    /// </summary>
    private Record? _GetRecordByIndex
        (
            string index
        )
    {
        if (!_cache.TryGetValue (index, out Record? result))
        {
            result = Connection.ByIndex (index);
            if (result is not null)
            {
                _cache.Set (index, result);
            }
        }

        if (result is null)
        {
            _logger.LogDebug ("No record with index\"{Index}\"", index);
        }

        return result;
    }

    /// <summary>
    /// Получение рабочего листа по шифру в базе. Логирование проблем.
    /// </summary>
    private string? _GetWorksheetByIndex
        (
            string index
        )
    {
        var record = _GetRecordByIndex (index);
        if (record is null)
        {
            return null;
        }

        if (!RecordConfiguration.WorksheetOK (record))
        {
            _logger.LogDebug ("Problem with worksheet: Index=\"{Index}\", MFN={Mfn}", index, record.Mfn);
            return null;
        }

        var result = RecordConfiguration.GetWorksheet (record);

        return result;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение перечня всех журналов из базы.
    /// </summary>
    public MagazineInfo[] GetAllMagazines
        (
            bool alternative = false
        )
    {
        var searchExpression = alternative ? "VRL=J" : "TJ=$";

        return GetAllMagazines (searchExpression);
    }

    /// <summary>
    /// Получение перечня всех журналов из базы.
    /// </summary>
    public MagazineInfo[] GetAllMagazines
        (
            string searchExpression
        )
    {
        Sure.NotNullNorEmpty (searchExpression);

        var batch = BatchRecordReader.Search
            (
                Connection,
                searchExpression,
                Connection.Database.ThrowIfNull(),
                1000
            );
        var result = batch.AsParallel()
            .Select (record => MagazineInfo.Parse (record))
            .NonNullItems()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Получение журнала по шифру его записи.
    /// </summary>
    public MagazineInfo? GetMagazine
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineInfo.Parse (record);
    }

    /// <summary>
    /// Получение журнала по MFN его записи.
    /// </summary>
    public MagazineInfo? GetMagazine
        (
            int mfn
        )
    {
        Sure.Positive (mfn);

        var record = Connection.ReadRecord (mfn);

        return record is null ? null : MagazineInfo.Parse (record);
    }

    /// <summary>
    /// Получение журнала по его выпуску.
    /// </summary>
    public MagazineInfo? GetMagazine
        (
            MagazineIssueInfo issue
        )
    {
        Sure.VerifyNotNull (issue);

        var index = issue.BuildIssueIndex();
        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineInfo.Parse (record);
    }

    /// <summary>
    /// Получение выпуска журнала по статье из этого выпуска.
    /// </summary>
    public MagazineIssueInfo? GetIssue
        (
            MagazineArticleInfo article
        )
    {
        Sure.VerifyNotNull (article);

        var source = article.Sources.ThrowIfNullOrEmpty().First();
        source.Verify (true);
        var index = source.Index.ThrowIfNullOrEmpty (); // шифр выпуска
        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineIssueInfo.Parse (record);
    }

    /// <summary>
    /// Получение выпуска журнала с указанным номером.
    /// </summary>
    public MagazineIssueInfo? GetIssue
        (
            MagazineInfo magazine,
            string year,
            string number
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        var index = magazine.BuildIssueIndex (year, number);
        var record = _GetRecordByIndex (index);
        if (record is null)
        {
            return null;
        }

        return MagazineIssueInfo.Parse (record);
    }

    /// <summary>
    /// Получение выпуска журнала с указанным номером.
    /// </summary>
    public MagazineIssueInfo? GetIssue
        (
            MagazineInfo magazine,
            string year,
            string? volume,
            string number
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        var index = string.IsNullOrEmpty (volume)
            ? magazine.BuildIssueIndex (year, number)
            : magazine.BuildIssueIndex (year, volume, number);
        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineIssueInfo.Parse (record);
    }

    /// <summary>
    /// Получение выпуска журнала с указанным номером.
    /// </summary>
    public MagazineIssueInfo? GetIssue
        (
            MagazineInfo magazine,
            YearVolumeNumber yearVolumeNumber
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.VerifyNotNull (yearVolumeNumber);

        var index = magazine.BuildIssueIndex (yearVolumeNumber);
        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineIssueInfo.Parse (record);
    }

    /// <summary>
    /// Получение выпуска журнала с указанным номером.
    /// </summary>
    public MagazineIssueInfo? GetIssue
        (
            string magazineIndex,
            YearVolumeNumber yearVolumeNumber
        )
    {
        Sure.NotNullNorEmpty (magazineIndex);
        Sure.VerifyNotNull (yearVolumeNumber);

        var index = magazineIndex + "/" + yearVolumeNumber;
        var record = _GetRecordByIndex (index);

        return record is null ? null : MagazineIssueInfo.Parse (record);
    }

    /// <summary>
    /// Получение списка выпусков данного журнала.
    /// </summary>
    public MagazineIssueInfo[] GetIssues
        (
            MagazineInfo magazine
        )
    {
        Sure.VerifyNotNull (magazine);

        var searchExpression = $"\"I933={magazine.Index}/$\"";
        var records = BatchRecordReader.Search
            (
                Connection,
                searchExpression,
                Connection.Database.ThrowIfNull(),
                1000
            );

        var result = records
            .Select (record => MagazineIssueInfo.Parse (record))
            .NonNullItems()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Получение списка выпусков данного журнала за определенный год.
    /// </summary>
    public MagazineIssueInfo[] GetIssues
        (
            MagazineInfo magazine,
            string year
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.NotNullNorEmpty (year);

        var searchExpression = $"\"I={magazine.Index}/{year}/$\"";
        var records = BatchRecordReader.Search
            (
                Connection,
                searchExpression,
                Connection.EnsureDatabase(),
                1000
            );

        var result = records
            .Select (record => MagazineIssueInfo.Parse (record))
            .NonNullItems()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Получение списка статей из выпуска.
    /// </summary>
    public MagazineArticleInfo[] GetArticles
        (
            MagazineIssueInfo issue
        )
    {
        Sure.VerifyNotNull (issue);

        var searchExpression = $"\"II={issue.Index}\"";
        var records = BatchRecordReader.Search
            (
                Connection,
                Connection.Database.ThrowIfNull(),
                searchExpression,
                1000
            );

        var result = records
            .Select (record => MagazineArticleInfo.ParseAsp (record))
            .NonNullItems()
            .ToArray();

        return result;
    }

    /// <summary>
    /// Подсчёт числа статей, расписанных в рабочем листе ASP.
    /// </summary>
    public int CountExternalArticles
        (
            MagazineIssueInfo issue
        )
    {
        Sure.VerifyNotNull (issue);

        var searchExpression = $"\"II={issue.Index}\"";
        var result = Connection.SearchCount (searchExpression);

        return result;
    }

    /// <summary>
    /// Создание выпуска журнала в базе по описанию.
    /// </summary>
    public MagazineIssueInfo CreateIssue
        (
            MagazineInfo magazine,
            YearVolumeNumber yearVolumeNumber,
            ExemplarInfo[]? exemplars,
            string? supplement = null
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.VerifyNotNull (yearVolumeNumber);

        _logger.LogTrace
            (
                "Creating issue {Issue} for magazine {Magazine}",
                yearVolumeNumber,
                magazine
            );

        var magazineIndex = magazine.Index.ThrowIfNullOrEmpty();
        var issueIndex = magazineIndex + "/" + yearVolumeNumber;
        var record = _GetRecordByIndex (issueIndex);

        MagazineIssueInfo result;
        if (record is not null)
        {
            result = MagazineIssueInfo.Parse (record);

            if (!exemplars.IsNullOrEmpty())
            {
                var available = ExemplarInfo.ParseRecord (record);
                var merged = ExemplarInfo.MergeExemplars (available, exemplars);
                record.RemoveField (ExemplarInfo.ExemplarTag);
                foreach (var one in merged!)
                {
                    var field = one.ToField();
                    record.Add (field);
                }
            }
        }
        else
        {
            result = new MagazineIssueInfo
            {
                Index = issueIndex,
                DocumentCode = issueIndex,
                MagazineCode = magazineIndex,
                Year = yearVolumeNumber.Year,
                Volume = yearVolumeNumber.Volume,
                Number = yearVolumeNumber.Number,
                Supplement = supplement,
                Worksheet = "NJ",
                Exemplars = exemplars
            };
            record = result.ToRecord();
            record.Database = Connection.EnsureDatabase();
            Connection.WriteRecord (record);
        }

        return result;
    }

    /// <summary>
    /// Регистрация выпуска журнала в базе по описанию.
    /// </summary>
    public MagazineIssueInfo RegisterIssue
        (
            MagazineInfo magazine,
            YearVolumeNumber yearVolumeNumber,
            ExemplarInfo[]? exemplars,
            string? supplement = null
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.VerifyNotNull (yearVolumeNumber);

        _logger.LogTrace
            (
                "Registering issue {Issue} for magazine {Magazine}",
                yearVolumeNumber,
                magazine
            );

        // var magazineIndex = magazine.Index.ThrowIfNullOrEmpty();
        // var issueIndex = magazineIndex + "/" + yearVolumeNumber;

        // TODO: зарегистрировать выпуск

        throw new NotImplementedException();
    }

    /// <summary>
    /// Существует ли сводная запись журнала с указанным шифром?
    /// </summary>
    public bool MagazineExist
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        var worksheet = _GetWorksheetByIndex (index);

        return IrbisUtility.IsMagazineSummary (worksheet);
    }

    /// <summary>
    /// Существует ли запись выпуска с указанными шифром, годом и номером?
    /// </summary>
    public bool IssueExist
        (
            string magazineIndex,
            string year,
            string number
        )
    {
        Sure.NotNullNorEmpty (magazineIndex);
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (number);

        var index = magazineIndex + "/" + year + "/" + number;
        var worksheet = _GetWorksheetByIndex (index);

        return IrbisUtility.IsMagazineIssue (worksheet);
    }

    /// <summary>
    /// Существует ли запись выпуска с указанными шифром, годом, томом и номером?
    /// </summary>
    public bool IssueExist
        (
            string magazineIndex,
            string year,
            string volume,
            string number
        )
    {
        Sure.NotNullNorEmpty (magazineIndex);
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (volume);
        Sure.NotNullNorEmpty (number);

        var index = magazineIndex + "/" + year + "/" + volume + "/" + number;
        var worksheet = _GetWorksheetByIndex (index);

        return IrbisUtility.IsMagazineIssue (worksheet);
    }

    /// <summary>
    /// Существует ли запись выпуска с указанными шифром, годом и номером?
    /// </summary>
    public bool IssueExist
        (
            string magazineIndex,
            YearVolumeNumber yearVolumeNumber
        )
    {
        Sure.NotNullNorEmpty (magazineIndex);
        Sure.VerifyNotNull (yearVolumeNumber);

        var index = magazineIndex + "/" + yearVolumeNumber;
        var worksheet = _GetWorksheetByIndex (index);

        return IrbisUtility.IsMagazineIssue (worksheet);
    }

    #endregion
}
