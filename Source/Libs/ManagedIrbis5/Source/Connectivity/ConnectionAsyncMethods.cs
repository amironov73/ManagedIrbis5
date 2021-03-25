// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionAsyncMethods.cs -- асинхронные методы для подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Асинхронные методы для подключения к серверу ИРБИС64.
    /// </summary>
    public static class ConnectionAsyncMethods
    {
        #region Public methods

        /// <summary>
        /// Чтение библиографической записи с сервера.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mfn"></param>
        /// <returns></returns>
        public static async Task<Record?> ReadRecordAsync(this IAsyncConnection connection, int mfn) =>
            await connection.ReadRecordAsync(new ReadRecordParameters { Mfn = mfn });

        /// <summary>
        /// Чтение всех терминов с указанным префиксом.
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public static async Task<Term[]> ReadAllTermsAsync
            (
                this IAsyncConnection connection,
                string prefix
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<Term>();
            }

            prefix = prefix.ToUpperInvariant();
            var result = new List<Term>();
            var startTerm = prefix;
            var flag = true;
            while (flag)
            {
                var terms = await connection.ReadTermsAsync(startTerm, 1024);
                if (terms.Length == 0)
                {
                    break;
                }

                int startIndex = 0;
                if (result.Count != 0)
                {
                    var lastTerm = result[^1];
                    var firstTerm = terms[0];
                    if (firstTerm.Text == lastTerm.Text)
                    {
                        startIndex = 1;
                    }
                }

                for (var i = startIndex; i < terms.Length; i++)
                {
                    var term = terms[i];
                    var text = term.Text;
                    if (string.IsNullOrEmpty(text))
                    {
                        break;
                    }

                    if (!text.StartsWith(prefix))
                    {
                        flag = false;
                        break;
                    }

                    result.Add(term);
                }
            }

            return result.ToArray();
        } // method ReadAllTermsAsync


        /// <summary>
        /// Чтение постингов указанного термина.
        /// </summary>
        /// <param name="term">Термин.</param>
        /// <param name="numberOfPostings">Максимальное количество постингов</param>
        /// <returns>Массив прочитанных постингов.</returns>
        public static async Task<TermPosting[]> ReadPostingsAsync
            (
                this IAsyncConnection connection,
                string term,
                int numberOfPostings
            )
        {
            var parameters = new PostingParameters
            {
                Database = connection.Database,
                Terms = new[] { term },
                NumberOfPostings = numberOfPostings
            };

            return await connection.ReadPostingsAsync(parameters);
        }

        /// <summary>
        /// Чтение терминов словаря.
        /// </summary>
        /// <param name="startTerm">Начальный термин.</param>
        /// <param name="numberOfTerms">Максимальное число терминов.</param>
        /// <returns>Массив прочитанных терминов.</returns>
        public static async Task<Term[]> ReadTermsAsync
            (
                this IAsyncConnection connection,
                string startTerm,
                int numberOfTerms
            )
        {
            var parameters = new TermParameters
            {
                Database = connection.Database,
                StartTerm = startTerm,
                NumberOfTerms = numberOfTerms
            };

            return await connection.ReadTermsAsync(parameters);
        } // method ReadTermsAsync


        /// <summary>
        /// Расширенный поиск.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Массив MFN найденных записей.</returns>
        public static async Task<int[]> SearchAsync
            (
                this IAsyncConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return Array.Empty<int>();
            }

            var query = new Query(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression
            };
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return Array.Empty<int>();
            }

            return FoundItem.ParseMfn(response);
        } // method SearchAsync

        /// <summary>
        /// Определение количества записей, удовлетворяющих
        /// заданному запросу.
        /// </summary>
        /// <param name="expression">Выражение для поиска по словарю.</param>
        /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
        public static async Task<int> SearchCountAsync
            (
                this IAsyncConnection connection,
                string expression
            )
        {
            if (!connection.CheckConnection())
            {
                return -1;
            }

            var query = new Query(connection, CommandCode.Search);
            var parameters = new SearchParameters
            {
                Database = connection.Database,
                Expression = expression,
                FirstRecord = 0
            };
            parameters.Encode(connection, query);
            var response = await connection.ExecuteAsync(query);
            if (response is null
                || !response.CheckReturnCode())
            {
                return -1;
            }

            return response.ReadInteger();
        } // method SearchCountAsync

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Task<Record[]> SearchReadAsync
            (
                this IAsyncConnection connection,
                string expression,
                int limit = 0
            )
        {
            throw new NotImplementedException();
        } // method SearchReadAsync

        /// <summary>
        /// Поиск записей на сервере.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static Task<Record?> SearchSingleRecordAsync
            (
                this IAsyncConnection connection,
                string expression
            )
        {
            throw new NotImplementedException();
        } // method SearchSingleRecordAsync


        public static Task<Record[]> WriteRecordsAsync
            (
                this IAsyncConnection connection,
                Record[] records,
                bool lockFlag = false,
                bool actualize = true
            )
        {
            throw new NotImplementedException();
        } // method WriteRecordsAsync

        public static async Task<bool> WriteTextFileAsync
            (
                this IAsyncConnection connection,
                params FileSpecification[] specifications
            )
        {
            if (!connection.CheckConnection())
            {
                return false;
            }

            var query = new Query(connection, CommandCode.ReadDocument);
            foreach (var specification in specifications)
            {
                query.AddAnsi(specification.ToString());
            }
            var response = await connection.ExecuteAsync(query);
            if (response is null)
            {
                return false;
            }

            throw new NotImplementedException();
        } // method WriteTextFileAsync

        #endregion

    } // class ConenctionAsyncMethods

} // namespace ManagedIrbis
