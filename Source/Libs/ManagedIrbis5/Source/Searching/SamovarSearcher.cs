// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SamovarSearcher.cs -- следующая итерация "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM;
using AM.AOT.Stemming;
using AM.Collections;
using AM.Text;

using ManagedIrbis.ImportExport;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Следующая реализация "поиска для чайников".
/// </summary>
public class SamovarSearcher
{
    #region Properties

    /// <summary>
    /// Префикс, применяемый для поиска по автору.
    /// </summary>
    public string? AuthorPrefix { get; set; }

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
    public SamovarSearcher
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        ServiceProvider = serviceProvider;
        Suffix = "$";
        AuthorPrefix = CommonSearches.AuthorPrefix;
        Prefixes = new List<string>
        {
            CommonSearches.KeywordPrefix,
            CommonSearches.CollectivePrefix,
            CommonSearches.TitlePrefix
        };
    }

    #endregion

    #region Private members

    private bool _Search
        (
            IList<Bagel> result,
            ISyncProvider connection,
            string expression,
            SamovarOptions options,
            double rating
        )
    {
        if (string.IsNullOrEmpty (expression))
        {
            return false;
        }

        var parameters = new SearchParameters
        {
            Expression = expression,
            Format = IrbisFormat.All,
            Database = connection.Database.ThrowIfNull()
        };
        var found = connection.Search (parameters);
        if (found is null)
        {
            return false;
        }

        foreach (var item in found)
        {
            var record = new Record();
            record = ProtocolText.ParseResponseForAllFormat (item.Text, record);
            if (record is not null)
            {
                var bagel = new Bagel
                {
                    Record = record,
                    Rating = rating
                };
                result.Add (bagel);
            }
        }

        return true;
    }

    /// <summary>
    /// Построение бесхитростного выражения.
    /// </summary>
    /// <param name="query">Выражение на естественном языке</param>
    /// <param name="withTrimming">С усечением?</param>
    private string _BuildStraightExpression
        (
            string query,
            bool withTrimming
        )
    {
        if (string.IsNullOrWhiteSpace (query))
        {
            return string.Empty;
        }

        query = new Sparcer().SparceText (query);
        if (string.IsNullOrEmpty (query))
        {
            return string.Empty;
        }

        var suffix = withTrimming ? Suffix : string.Empty;
        var first = true;
        var result = new StringBuilder();
        foreach (var prefix in Prefixes)
        {
            if (prefix.SameString (AuthorPrefix))
            {
                // поиск по автору будет отработан отдельно
                continue;
            }

            if (!first)
            {
                result.Append (" + ");
            }

            result.Append (Q.WrapIfNeeded (prefix + query + suffix));
            first = false;
        }

        if (!string.IsNullOrEmpty (AuthorPrefix))
        {
            var authorNames = AuthorUtility.WithAndWithoutComma (query);
            if (!authorNames.IsNullOrEmpty())
            {
                foreach (var author in authorNames)
                {
                    if (!first)
                    {
                        result.Append (" + ");
                    }

                    result.Append (Q.WrapIfNeeded (AuthorPrefix + author + suffix));
                    first = false;
                }

            }
        }

        return result.ToString();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск записей.
    /// </summary>
    /// <param name="connection">Подключение.</param>
    /// <param name="query">Запрос на естественном языке.</param>
    /// <param name="options">Опции поиска (необязательно).</param>
    /// <returns></returns>
    public Bagel[] Search
        (
            ISyncProvider connection,
            string query,
            SamovarOptions? options = null
        )
    {
        Sure.NotNull (connection);
        Sure.NotNullNorEmpty (query);
        connection.EnsureConnected();

        options ??= new ();

        // var database = connection.EnsureDatabase (options.Database);
        var outputLimit = options.OutputLimit;
        if (outputLimit < 1)
        {
            outputLimit = 500;
        }

        var result = new List<Bagel>();
        var expression = _BuildStraightExpression (query, false);
        if (!_Search (result, connection, expression, options, 1_000))
        {
            return Array.Empty<Bagel>();
        }

        if (result.Count < outputLimit)
        {
            expression = _BuildStraightExpression (query, true);
            if (!_Search (result, connection, expression, options, 500))
            {
                return Array.Empty<Bagel>();
            }
        }

        var evaluator = ServiceProvider.GetService <IRelevanceEvaluator>()
                        ?? new StandardRelevanceEvaluator (ServiceProvider, expression);

        foreach (var bagel in result)
        {
            if (bagel.Rating == 0)
            {
                bagel.Rating = evaluator.EvaluateRelevance (bagel.Record!);
            }
        }

        result.Sort (Bagel.ReverseComparer);

        return result.Take (outputLimit).ToArray();
    }

    #endregion
}
