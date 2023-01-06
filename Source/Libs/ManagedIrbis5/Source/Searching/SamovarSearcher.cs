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
using System.Text.RegularExpressions;

using AM;
using AM.AOT.Stemming;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Identifiers;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

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
        Prefixes = new List<string>
        {
            CommonSearches.KeywordPrefix,
            CommonSearches.AuthorPrefix,
            CommonSearches.CollectivePrefix,
            CommonSearches.TitlePrefix
        };
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Построение бесхитростного выражения.
    /// </summary>
    /// <param name="query">Выражение на естественном языке</param>
    /// <param name="withTrimming">С усечением?</param>
    public string BuildStraightExpression
        (
            string query,
            bool withTrimming
        )
    {
        if (string.IsNullOrWhiteSpace (query))
        {
            return string.Empty;
        }

        query = query.Trim();
        if (string.IsNullOrEmpty (query))
        {
            return string.Empty;
        }

        return string.Empty;
    }

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


        var database = connection.EnsureDatabase (options.Database);
        var outputLimit = options.OutputLimit;
        if (outputLimit < 1)
        {
            outputLimit = 500;
        }

        var expression = BuildStraightExpression (query, false);
        if (string.IsNullOrWhiteSpace (expression))
        {
            return Array.Empty<Bagel>();
        }

        return Array.Empty<Bagel>();
    }

    #endregion
}
