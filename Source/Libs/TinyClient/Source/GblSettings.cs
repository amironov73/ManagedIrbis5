// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* GblSettings.cs -- настройки для глобальной корректировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;
using System.Text;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Настройки для глобальной корректировки.
    /// </summary>
    public sealed class GblSettings
    {
        #region Constants

        /// <summary>
        /// Разделитель элементов в строке.
        /// </summary>
        private const string Delimiter = "\x001F\x001E";

        #endregion

        #region Properties

        /// <summary>
        /// Actualize records after processing.
        /// </summary>
        public bool Actualize { get; set; } = true;

        /// <summary>
        /// Process 'autoin.gbl'.
        /// </summary>
        public bool Autoin { get; set; }

        /// <summary>
        /// Database name.
        /// </summary>
        public string? Database { get; set; }

        /// <summary>
        /// File name.
        /// </summary>
        public string? FileName { get; set; }

        /// <summary>
        /// First record MFN.
        /// </summary>
        public int FirstRecord { get; set; } = 1;

        /// <summary>
        /// Provide formal control.
        /// </summary>
        public bool FormalControl { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        public int MaxMfn { get; set; }

        /// <summary>
        /// List of MFN to process.
        /// </summary>
        public int[]? MfnList { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        /// <remarks>0 means 'all records in the database'.
        /// </remarks>
        public int MinMfn { get; set; }

        /// <summary>
        /// Number of records to process.
        /// </summary>
        public int NumberOfRecords { get; set; }

        /// <summary>
        /// Search expression.
        /// </summary>
        public string? SearchExpression { get; set; }

        /// <summary>
        /// Statements.
        /// </summary>
        public List<GblStatement> Statements { get; private set; } = new ();

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public GblSettings()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connection">Настроенное подключение.</param>
        public GblSettings (SyncConnection connection) =>
            Database = connection.Database;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="connection">Настроенное подключение.</param>
        /// <param name="statements">Операторы ГК.</param>
        public GblSettings
            (
                SyncConnection connection,
                IEnumerable<GblStatement> statements
            )
            : this (connection) => Statements.AddRange (statements);

        #endregion

        #region Public methods

        /// <summary>
        /// Кодирование пользовательского запроса.
        /// </summary>
        public void Encode
            (
                SyncQuery query
            )
        {
            query.Add (Actualize ? 1 : 0);
            if (!string.IsNullOrEmpty (FileName))
            {
                query.AddAnsi ("@" + FileName);
            }
            else
            {
                var builder = new StringBuilder();

                // "!" здесь означает, что передавать будем в UTF-8
                builder.Append ('!');

                // не знаю, что тут означает "0"
                builder.Append ('0');
                builder.Append (Delimiter);
                foreach (var statement in Statements)
                {
                    // TODO: сделать подстановку параметров
                    // $encoded .=  $settings->substituteParameters(strval($statement));
                    builder.Append (statement.EncodeForProtocol());
                    builder.Append (Delimiter);
                }

                builder.Append (Delimiter);
                query.AddUtf (builder.ToString());
            }

            // отбор записей на основе поиска
            query.AddUtf (SearchExpression); // поиск по словарю
            query.Add (MinMfn); // нижняя граница MFN
            query.Add (MaxMfn); // верхняя граница MFN

            //query.AddUtf(SequentialExpression); // последовательный поиск

            // TODO поддержка режима "кроме отмеченных"
            if (MfnList is { Length: not 0 })
            {
                query.Add (MfnList.Length);
                foreach (var mfn in MfnList)
                {
                    query.Add (mfn);
                }
            }
            else
            {
                var count = MaxMfn - MinMfn + 1;
                query.Add (count);
                for (var mfn = 0; mfn <= MaxMfn; mfn++)
                {
                    query.Add (mfn);
                }
            }

            if (!FormalControl)
            {
                query.AddAnsi ("*");
            }

            if (!Autoin)
            {
                query.AddAnsi ("&");
            }
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                SyncConnection connection,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given interval of MFN.
        /// </summary>
        public static GblSettings ForInterval
            (
                SyncConnection connection,
                string database,
                int minMfn,
                int maxMfn,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                Database = database,
                MinMfn = minMfn,
                MaxMfn = maxMfn
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                string database,
                IEnumerable<int> mfnList,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given list of MFN.
        /// </summary>
        public static GblSettings ForList
            (
                SyncConnection connection,
                string database,
                IEnumerable<int> mfnList
            )
        {
            var result = new GblSettings (connection)
            {
                Database = database,
                MfnList = mfnList.ToArray()
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                SyncConnection connection,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                SearchExpression = searchExpression
            };

            return result;
        }

        /// <summary>
        /// Create <see cref="GblSettings"/>
        /// for given searchExpression.
        /// </summary>
        public static GblSettings ForSearchExpression
            (
                SyncConnection connection,
                string database,
                string searchExpression,
                IEnumerable<GblStatement> statements
            )
        {
            var result = new GblSettings (connection, statements)
            {
                Database = database,
                SearchExpression = searchExpression
            };

            return result;
        }

        /// <summary>
        /// Set (server) file name.
        /// </summary>
        public GblSettings SetFileName
            (
                string fileName
            )
        {
            FileName = fileName;

            return this;
        }

        /// <summary>
        /// Set first record and number of records
        /// to process.
        /// </summary>
        public GblSettings SetRange
            (
                int firstRecord,
                int numberOfRecords
            )
        {
            FirstRecord = firstRecord;
            NumberOfRecords = numberOfRecords;

            return this;
        }

        /// <summary>
        /// Set search expression.
        /// </summary>
        public GblSettings SetSearchExpression
            (
                string searchExpression
            )
        {
            SearchExpression = searchExpression;

            return this;
        }

        #endregion
    }
}
