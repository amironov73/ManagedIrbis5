// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TeapotSearcher.cs -- реализация "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using AM;
using AM.Collections;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable
namespace ManagedIrbis.Searching;

/// <summary>
/// Стандартная реализация "поиска для чайников".
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
    /// Провайдер сервисов.
    /// </summary>
    public IServiceProvider ServiceProvider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="serviceProvider"></param>
    public TeapotSearcher
        (
            IServiceProvider serviceProvider
        )
    {
        Sure.NotNull (serviceProvider);

        ServiceProvider = serviceProvider;
        _logger = LoggingUtility.GetLogger (serviceProvider, typeof (TeapotSearcher));

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
        foreach (var term in terms)
        {
            foreach (var prefix in Prefixes)
            {
                if (!first)
                {
                    builder.Append (" + ");
                }

                builder.Append (Q.WrapIfNeeded (prefix + term));

                first = false;
            }
        }

        return builder.ReturnShared();
    }

    #endregion
}
