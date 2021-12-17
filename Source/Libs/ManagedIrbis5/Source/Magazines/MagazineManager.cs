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
using System.Collections.Generic;
using System.Linq;

using AM;
using AM.Linq;

using ManagedIrbis.Batch;
using ManagedIrbis.Fields;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
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
            Connection = connection;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение перечня всех журналов из базы.
        /// </summary>
        public MagazineInfo[] GetAllMagazines()
        {
            var result = new List<MagazineInfo>();

            var batch = BatchRecordReader.Search
                (
                    Connection,
                    Connection.Database.ThrowIfNull ("Connection.Database"),
                    "VRL=J",
                    1000
                );
            foreach (var record in batch)
            {
                var magazine = MagazineInfo.Parse (record);
                if (!ReferenceEquals (magazine, null))
                {
                    result.Add (magazine);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Получение журнала по MFN его записи.
        /// </summary>
        public MagazineInfo? GetMagazine
            (
                int mfn
            )
        {
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
        public MagazineIssueInfo GetIssue
            (
                MagazineArticleInfo article
            )
        {
            Magna.Error
                (
                    nameof (MagazineManager) + "::" + nameof (GetIssue)
                    + ": not implemented"
                );

            throw new NotImplementedException();
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

            return MagazineIssueInfo.Parse(record);
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
            if (record is null)
            {
                return null;
            }

            return MagazineIssueInfo.Parse(record);
        }

        /// <summary>
        /// Получение списка выпусков данного журнала.
        /// </summary>
        public MagazineIssueInfo[] GetIssues
            (
                MagazineInfo magazine
            )
        {
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
            /*
            string searchExpression = string.Format
                (
                    "\"II={0}\"",
                    issue.Index
                );
            int result = Connection.SearchCount(searchExpression);

            return result;

            */

            throw new NotImplementedException();
        }

        /// <summary>
        /// Создание журнала в базе по описанию.
        /// </summary>
        public MagazineIssueInfo CreateMagazine
            (
                MagazineInfo magazine,
                string year,
                string issue,
                ExemplarInfo[]? exemplars
            )
        {
            var fullIndex = magazine.Index + "/" + year + "/" + issue;
            var result = new MagazineIssueInfo
            {
                Index = fullIndex,
                DocumentCode = fullIndex,
                MagazineCode = magazine.Index,
                Year = year,
                Number = issue,
                Worksheet = "NJ",
                Exemplars = exemplars
            };

            return result;
        }

        #endregion
    }
}
