// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* AsyncTeapotSearcher.cs -- асинхронная реализация "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.AOT.Stemming;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Стандартная асинхронная реализация "поиска для чайников".
/// </summary>
public class AsyncTeapotSearcher
    : IAsyncTeapotSearcher
{
    #region Properties

    /// <summary>
    /// Список применяемых префиксов.
    /// </summary>
    public IList<string> Prefixes { get; }

    /// <summary>
    /// Суффикс, присоединяемый к терминам поискового запроса.
    /// </summary>
    public string? Suffix { get; set; }

    /// <summary>
    /// Стеммер (опционально).
    /// </summary>
    public IStemmer? Stemmer { get; set; }

    /// <summary>
    /// Провайдер сервисов.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    public AsyncTeapotSearcher
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        ServiceProvider = serviceProvider;
        _logger = LoggingUtility.GetLogger (serviceProvider, typeof (AsyncTeapotSearcher));

        Suffix = "$";
        Prefixes = new List<string>
        {
            CommonSearches.KeywordPrefix,
            CommonSearches.AuthorPrefix,
            CommonSearches.CollectivePrefix,
            CommonSearches.TitlePrefix
        };
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    private string StemTheWordIfHasTheMeaning
        (
            string text
        )
    {
        if (UnicodeUtility.TextContainsNonLatinNorCyrillicSymbols (text))
        {
            return text;
        }

        if (Stemmer is not null)
        {
            return Stemmer.Stem (text);
        }

        return text;
    }

    #endregion

    #region ITeapotSearcher members

    /// <inheritdoc cref="ITeapotSearcher.BuildSearchExpression"/>
    public Task<string> BuildSearchExpressionAsync
        (
            string query
        )
    {
        _logger.LogTrace ("{Query}", query);

        if (string.IsNullOrWhiteSpace (query))
        {
            return Task.FromResult (string.Empty);
        }

        query = query.Trim();

        var terms = new CaseInsensitiveDictionary<object?>
        {
            [query] = null
        };
        foreach (Match match in Regex.Matches (query, @"\w+"))
        {
            terms[match.Value] = null;
        }

        var builder = StringBuilderPool.Shared.Get();
        var first = true;
        foreach (var term in terms.Keys.Order())
        {
            if (StopWords.IsStandardStopWord (term))
            {
                continue;
            }

            var item = StemTheWordIfHasTheMeaning (term);
            foreach (var prefix in Prefixes)
            {
                if (!first)
                {
                    builder.Append (" + ");
                }

                builder.Append (Q.WrapIfNeeded (prefix + item + Suffix));

                first = false;
            }
        }

        var result = builder.ReturnShared();

        return Task.FromResult (result);
    }

    /// <inheritdoc cref="ITeapotSearcher.Search"/>
    public async Task<int[]> SearchAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        database = connection.EnsureDatabase (database);
        if (limit < 1)
        {
            limit = 500;
        }

        var expression = await BuildSearchExpressionAsync (query);
        if (string.IsNullOrWhiteSpace (expression))
        {
            return Array.Empty<int>();
        }

        evaluator ??= ServiceProvider.GetService <IRelevanceEvaluator>()
            ?? new StandardRelevanceEvaluator (ServiceProvider, expression);

        var parameters = new SearchParameters
        {
            Database = database,
            Expression = expression,
            NumberOfRecords = limit * 2
        };
        var found = await connection.SearchAsync (parameters);
        if (found is null)
        {
            return Array.Empty<int>();
        }

        var batch = FoundItem.ToMfn (found);
        var records = await connection.ReadRecordsAsync (database, batch);
        if (records.IsNullOrEmpty())
        {
            return Array.Empty<int>();
        }

        var pairs = new List<Pair<int, double>>();
        foreach (var record in records)
        {
            var relevance = evaluator.EvaluateRelevance (record);
            var pair = new Pair<int, double> (record.Mfn, relevance);
            pairs.Add (pair);
        }

        var result = pairs
            .OrderByDescending (pair => pair.Second)
            .Take (limit)
            .Select (pair => pair.First)
            .ToArray();

        return result;
    }

    /// <inheritdoc cref="ITeapotSearcher.Search"/>
    public async Task<string[]> SearchFormatAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            string? format = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        database = connection.EnsureDatabase (database);
        if (limit < 1)
        {
            limit = 500;
        }

        var expression = await BuildSearchExpressionAsync (query);
        if (string.IsNullOrWhiteSpace (expression))
        {
            return Array.Empty<string>();
        }

        evaluator ??= ServiceProvider.GetService <IRelevanceEvaluator>()
            ?? new StandardRelevanceEvaluator (ServiceProvider, expression);

        var searchParameters = new SearchParameters
        {
            Database = database,
            Expression = expression,
            NumberOfRecords = limit * 2
        };
        var found = await connection.SearchAsync (searchParameters);
        if (found is null)
        {
            return Array.Empty<string>();
        }

        var batch = FoundItem.ToMfn (found);
        var records = await connection.ReadRecordsAsync (database, batch);
        if (records.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        var pairs = new List<Pair<Record, double>>();
        foreach (var record in records)
        {
            var relevance = evaluator.EvaluateRelevance (record);
            var pair = new Pair<Record, double> (record, relevance);
            pairs.Add (pair);
        }

        var mfns = pairs
            .OrderByDescending (pair => pair.Second)
            .Take (limit)
            .Select (pair => pair.First!.Mfn)
            .ToArray();
        if (mfns.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        var formatParameters = new FormatRecordParameters
        {
            Database = connection.EnsureDatabase(),
            Format = format ?? "@",
            Mfns = mfns
        };
        if (!await connection.FormatRecordsAsync (formatParameters))
        {
            return Array.Empty<string>();
        }

        var result = formatParameters.Result.AsArray();

        return found.IsNullOrEmpty()
            ? Array.Empty<string>()
            : result;
    }

    /// <inheritdoc cref="ITeapotSearcher.Search"/>
    public async Task<Record[]> SearchReadAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        )
    {
        Sure.NotNull (connection);
        connection.EnsureConnected();

        database = connection.EnsureDatabase (database);
        if (limit < 1)
        {
            limit = 500;
        }

        var expression = await BuildSearchExpressionAsync (query);
        if (string.IsNullOrWhiteSpace (expression))
        {
            return Array.Empty<Record>();
        }

        evaluator ??= ServiceProvider.GetService <IRelevanceEvaluator>()
            ?? new StandardRelevanceEvaluator (ServiceProvider, expression);

        var parameters = new SearchParameters
        {
            Database = database,
            Expression = expression,
            NumberOfRecords = limit * 2
        };
        var found = await connection.SearchAsync (parameters);
        if (found is null)
        {
            return Array.Empty<Record>();
        }

        var batch = FoundItem.ToMfn (found);
        var records = await connection.ReadRecordsAsync (database, batch);
        if (records.IsNullOrEmpty())
        {
            return Array.Empty<Record>();
        }

        var pairs = new List<Pair<Record, double>>();
        foreach (var record in records)
        {
            var relevance = evaluator.EvaluateRelevance (record);
            var pair = new Pair<Record, double> (record, relevance);
            pairs.Add (pair);
        }

        var result = pairs
            .OrderByDescending (pair => pair.Second)
            .Take (limit)
            .Select (pair => pair.First!)
            .ToArray();

        return result;
    }

    #endregion
}
