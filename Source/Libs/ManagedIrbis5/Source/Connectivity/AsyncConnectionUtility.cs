// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AsyncConnectionUtility.cs -- асинхронные методы для подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AM;
using AM.Collections;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Асинхронные методы для подключения к серверу ИРБИС64.
/// </summary>
public static class AsyncConnectionUtility
{
    #region Public methods

    /// <summary>
    /// Форматирование указанной записи по ее MFN.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="format">Спецификация формата.</param>
    /// <param name="mfn">MFN записи.</param>
    /// <returns>Результат расформатирования, возможно,
    /// пустая строка или <c>null</c></returns>
    public static async Task<string?> FormatRecordAsync
        (
            this AsyncConnection connection,
            string format,
            int mfn
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (format);
        Sure.Positive (mfn);

        if (!connection.CheckProviderState())
        {
            return null;
        }

        var query = new AsyncQuery (connection, CommandCode.FormatRecord);
        query.AddAnsi (connection.Database);
        var prepared = IrbisFormat.PrepareFormat (format);
        query.AddAnsi (prepared);
        query.Add (1);
        query.Add (mfn);
        var response = await connection.ExecuteAsync (query);
        if (!response.IsGood())
        {
            return null;
        }

        var result = response.ReadRemainingUtfText().TrimEnd();

        return result;
    }

    /// <summary>
    /// Форматирование записи в клиентском представлении.
    /// </summary>
    /// <returns>Результат расформатирования, возможно,
    /// пустая строка или <c>null</c>.</returns>
    public static async Task<string?> FormatRecordAsync
        (
            this AsyncConnection connection,
            string format,
            Record record
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (format);
        Sure.NotNull (record);

        if (!connection.CheckProviderState())
        {
            return null;
        }

        if (string.IsNullOrEmpty (format))
        {
            return string.Empty;
        }

        var query = new AsyncQuery (connection, CommandCode.FormatRecord);
        query.AddAnsi (connection.EnsureDatabase (string.Empty));
        query.AddFormat (format);
        query.Add (-2);
        query.AddUtf (record.Encode());
        var response = await connection.ExecuteAsync (query);
        if (!response.IsGood())
        {
            return null;
        }

        var result = response.ReadRemainingUtfText().TrimEnd();

        return result;
    }

    /// <summary>
    /// Получение списка файлов, соответствующих спецификации.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="specification">Спецификация необходимого имени файла.</param>
    /// <returns>Массив с именами найденных на сервере файлов,
    /// возможно, пустой.</returns>
    public static async Task<string[]?> ListFiles
        (
            this AsyncConnection connection,
            string specification
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (specification);

        if (!connection.CheckProviderState())
        {
            return null;
        }

        if (string.IsNullOrEmpty (specification))
        {
            return Array.Empty<string>();
        }

        var response = await connection.ExecuteAsync (CommandCode.ListFiles, specification);

        return SyncConnectionUtility.ListFiles (response);
    }

    /// <summary>
    /// Получение списка файлов, соответствующих спецификации.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="specification">Спецификация необходимого имени файла.</param>
    /// <returns>Массив с именами найденных на сервере файлов,
    /// возможно, пустой.</returns>
    public static async Task<string[]?> ListFilesAsync
        (
            this AsyncConnection connection,
            FileSpecification specification
        )
    {
        Sure.NotNull (connection);
        Sure.VerifyNotNull (specification);

        if (!connection.CheckProviderState())
        {
            return null;
        }

        var response = await connection.ExecuteAsync (CommandCode.ListFiles, specification);

        return SyncConnectionUtility.ListFiles (response);
    }

    /// <summary>
    /// Чтение библиографической записи с сервера по ее MFN.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="mfn">MFN записи.</param>
    /// <returns>Заруженная запись либо <c>null</c>.</returns>
    public static async Task<Record?> ReadRecordAsync
        (
            this IAsyncProvider connection,
            int mfn
        )
    {
        Sure.NotNull (connection);
        Sure.Positive (mfn);

        return await connection.ReadRecordAsync<Record>
            (
                new ()
                {
                    Database = connection.Database,
                    Mfn = mfn
                }
            );
    }

    /// <summary>
    /// Чтение библиографической записи с сервера по ее MFN.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="mfn">MFN записи.</param>
    /// <returns>Заруженная запись либо <c>null</c>.</returns>
    public static async Task<T?> ReadRecordAsync<T>
        (
            this IAsyncProvider connection,
            int mfn
        )
        where T : class, IRecord, new()
    {
        Sure.NotNull (connection);
        Sure.Positive (mfn);

        return await connection.ReadRecordAsync<T>
            (
                new ()
                {
                    Database = connection.Database,
                    Mfn = mfn
                }
            );
    }

    /// <summary>
    /// Чтение всех терминов с указанным префиксом.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="prefix">Префикс в поисковом словаре,
    /// может быть пустой строкой (означает вывод вообще всех
    /// терминов из словаря).</param>
    /// <remarks>Внимание! Терминов может оказаться очень много!</remarks>
    public static async Task<Term[]> ReadAllTermsAsync
        (
            this IAsyncProvider connection,
            string prefix
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (prefix);

        if (!connection.CheckProviderState())
        {
            return Array.Empty<Term>();
        }

        prefix = prefix.ToUpperInvariant();
        var result = new List<Term>();
        var startTerm = prefix;
        var flag = true;
        while (flag)
        {
            var terms = await connection.ReadTermsAsync (startTerm, 1024);
            if (terms.IsNullOrEmpty())
            {
                break;
            }

            var startIndex = 0;
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
                if (string.IsNullOrEmpty (text))
                {
                    break;
                }

                if (!text.StartsWith (prefix))
                {
                    flag = false;
                    break;
                }

                result.Add (term);
            }
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение постингов указанного термина.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="term">Термин.</param>
    /// <param name="numberOfPostings">Максимальное количество постингов</param>
    /// <returns>Массив прочитанных постингов.</returns>
    public static async Task<TermPosting[]?> ReadPostingsAsync
        (
            this IAsyncProvider connection,
            string term,
            int numberOfPostings
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (term);
        Sure.NonNegative (numberOfPostings);

        var parameters = new PostingParameters
        {
            Database = connection.Database,
            Terms = new[] { term },
            NumberOfPostings = numberOfPostings
        };

        return await connection.ReadPostingsAsync (parameters);
    }

    /// <summary>
    /// Чтение терминов словаря.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="startTerm">Начальный термин.</param>
    /// <param name="numberOfTerms">Максимальное число терминов.</param>
    /// <returns>Массив прочитанных терминов.</returns>
    public static async Task<Term[]?> ReadTermsAsync
        (
            this IAsyncProvider connection,
            string startTerm,
            int numberOfTerms
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (startTerm);
        Sure.NonNegative (numberOfTerms);

        var parameters = new TermParameters
        {
            Database = connection.Database,
            StartTerm = startTerm,
            NumberOfTerms = numberOfTerms
        };

        return await connection.ReadTermsAsync (parameters);
    }

    /// <summary>
    /// Расширенный поиск.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Массив MFN найденных записей.</returns>
    public static async Task<int[]> SearchAsync
        (
            this AsyncConnection connection,
            string expression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (expression);

        if (!connection.CheckProviderState())
        {
            return Array.Empty<int>();
        }

        var query = new AsyncQuery (connection, CommandCode.Search);
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression
        };
        parameters.Encode (connection, query);
        var response = await connection.ExecuteAsync (query);
        if (response is null
            || !response.CheckReturnCode())
        {
            return Array.Empty<int>();
        }

        return FoundItem.ParseMfn (response);
    }

    /// <summary>
    /// Определение количества записей, удовлетворяющих
    /// заданному запросу.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Количество найденных записей либо -1, если произошла ошибка.</returns>
    public static async Task<int> SearchCountAsync
        (
            this AsyncConnection connection,
            string expression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (expression);

        if (!connection.CheckProviderState())
        {
            return -1;
        }

        var query = new AsyncQuery (connection, CommandCode.Search);
        var parameters = new SearchParameters
        {
            Database = connection.Database,
            Expression = expression,
            FirstRecord = 0
        };
        parameters.Encode (connection, query);
        var response = await connection.ExecuteAsync (query);
        if (response is null
            || !response.CheckReturnCode())
        {
            return -1;
        }

        return response.ReadInteger();
    }

    /// <summary>
    /// Поиск записей на сервере.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю</param>
    /// <param name="limit">Ограничение сверху для количества возвращаемых записей.</param>
    /// <returns>Массив с найденными записями, возможно, пустой.</returns>
    public static Task<Record[]> SearchReadAsync
        (
            this AsyncConnection connection,
            string expression,
            int limit = 0
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (expression);
        Sure.NonNegative (limit);

        if (!connection.CheckProviderState())
        {
            return Task.FromResult (Array.Empty<Record>());
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Поиск одной записи на сервере.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="expression">Выражение для поиска по словарю.</param>
    /// <returns>Найденная запись либо <c>null</c>.</returns>
    public static Task<Record?> SearchSingleRecordAsync
        (
            this AsyncConnection connection,
            string expression
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull (expression);

        if (!connection.CheckProviderState())
        {
            return Task.FromResult<Record?> (null);
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Асинхронное сохранение записей на сервере.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="records">Список записей.</param>
    /// <param name="lockFlag">Оставлять записи в заблокированном состоянии.</param>
    /// <param name="actualize">Актуализировать словарь.</param>
    /// <param name="dontParse">Не парсить полученные от сервера записи.</param>
    /// <returns>Признак успешного завершения операции.</returns>
    public static async Task<bool> WriteRecordsAsync
        (
            this AsyncConnection connection,
            IEnumerable<Record> records,
            bool lockFlag = false,
            bool actualize = true,
            bool dontParse = true
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) records);

        // TODO: сделать IRecord

        if (!connection.CheckProviderState())
        {
            return false;
        }

        var query = new AsyncQuery (connection, CommandCode.SaveRecordGroup);
        query.Add (lockFlag);
        query.Add (actualize);
        var recordList = new List<Record>();
        foreach (var record in records)
        {
            var line = connection.EnsureDatabase (record.Database)
                       + IrbisText.IrbisDelimiter
                       + ProtocolText.EncodeRecord (record);
            query.AddUtf (line);
            recordList.Add (record);
        }

        if (recordList.Count == 0)
        {
            return true;
        }

        var response = await connection.ExecuteAsync (query);
        if (!response.IsGood())
        {
            return false;
        }

        if (!dontParse)
        {
            foreach (var record in recordList)
            {
                ProtocolText.ParseResponseForWriteRecords (response, record);
            }
        }

        return true;
    }

    /// <summary>
    /// Запись/обновление файлов на сервере.
    /// </summary>
    /// <param name="connection">Подключение</param>
    /// <param name="specifications">Список спецификаций файлов с содержимым.</param>
    /// <returns>Признау кспешного завершения операции.</returns>
    public static async Task<bool> WriteTextFilesAsync
        (
            this AsyncConnection connection,
            IEnumerable<FileSpecification> specifications
        )
    {
        Sure.NotNull (connection);
        Sure.NotNull ((object?) specifications);

        if (!connection.CheckProviderState())
        {
            return false;
        }

        var query = new AsyncQuery (connection, CommandCode.ReadDocument);
        var count = 0;
        foreach (var specification in specifications)
        {
            query.AddAnsi (specification.ToString());
            ++count;
        }

        if (count is 0)
        {
            return true;
        }

        var response = await connection.ExecuteAsync (query);

        return response is not null;
    }

    #endregion
}
