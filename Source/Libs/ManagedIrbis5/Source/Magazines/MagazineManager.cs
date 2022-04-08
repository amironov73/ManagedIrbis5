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

using System.Linq;

using AM;
using AM.Collections;
using AM.Linq;

using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

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

    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Local
    public ISyncProvider Connection { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public MagazineManager
        (
            ISyncProvider connection
        )
    {
        Sure.NotNull (connection);

        Connection = connection;
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
        var record = Connection.ByIndex (index);

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
        var record = Connection.ByIndex (index);

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
        var record = Connection.ByIndex (index);
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
            string volume,
            string number
        )
    {
        Sure.VerifyNotNull (magazine);
        Sure.NotNullNorEmpty (year);
        Sure.NotNullNorEmpty (volume);
        Sure.NotNullNorEmpty (number);

        var index = magazine.BuildIssueIndex (year, volume, number);
        var record = Connection.ByIndex (index);

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
        var record = Connection.ByIndex (index);

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
        var record = Connection.ByIndex (index);

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
    /// Получение списка выпусков данного журнала.
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
                Connection.Database.ThrowIfNull(),
                searchExpression,
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

        var magazineIndex = magazine.Index.ThrowIfNullOrEmpty();
        var issueIndex = magazineIndex + "/" + yearVolumeNumber;
        var record = Connection.ByIndex (issueIndex);
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
    /// Существует ли сводная запись журнала с указанным шифром?
    /// </summary>
    public bool MagazineByIndex
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        var record = Connection.ByIndex (index);

        return record is not null;
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
        var record = Connection.ByIndex (index);

        return record is not null;
    }

    /// <summary>
    /// Существует ли запись выпуска с указанными шифром, годом и номером?
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
        var record = Connection.ByIndex (index);

        return record is not null;
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
        var record = Connection.ByIndex (index);

        return record is not null;
    }

    #endregion
}
