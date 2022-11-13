// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TeapotSearcher.cs -- синхронная реализация "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Стандартная синхронная реализация "поиска для чайников".
/// </summary>
public class TeapotSearcher
    : ITeapotSearcher
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
    /// Провайдер сервисов.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider">Провайдер сервисов</param>
    public TeapotSearcher
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        ServiceProvider = serviceProvider;
        _logger = LoggingUtility.GetLogger (serviceProvider, typeof (TeapotSearcher));

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

    #endregion

    #region ITeapotSearcher members

    /// <inheritdoc cref="ITeapotSearcher.BuildSearchExpression"/>
    public string BuildSearchExpression
        (
            string query
        )
    {
        _logger.LogTrace (nameof (BuildSearchExpression) + ": {Query}", query);

        if (string.IsNullOrWhiteSpace (query))
        {
            return string.Empty;
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

            foreach (var prefix in Prefixes)
            {
                if (!first)
                {
                    builder.Append (" + ");
                }

                builder.Append (Q.WrapIfNeeded (prefix + term + Suffix));

                first = false;
            }
        }

        return builder.ReturnShared();
    }

    /// <inheritdoc cref="ITeapotSearcher.Search"/>
    public int[] Search
        (
            ISyncProvider connection,
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

        var expression = BuildSearchExpression (query);
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
        var found = connection.Search (parameters);
        if (found is null)
        {
            return Array.Empty<int>();
        }

        var batch = FoundItem.ToMfn (found);
        var records = connection.ReadRecords (database, batch);
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
    public Record[] SearchRead
        (
            ISyncProvider connection,
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

        var expression = BuildSearchExpression (query);
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
        var found = connection.Search (parameters);
        if (found is null)
        {
            return Array.Empty<Record>();
        }

        var batch = FoundItem.ToMfn (found);
        var records = connection.ReadRecords (database, batch);
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
