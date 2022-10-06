// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ITeapotSearcher.cs -- общий интерфейс "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Общий интерфейс "поиска для чайников".
/// </summary>
public interface ITeapotSearcher
{
    /// <summary>
    /// Построение поискового выражения по запросу на естественном языке.
    /// </summary>
    string BuildSearchExpression (string query);

    /// <summary>
    /// Поиск записей, удовлетворяющих запросу на естественном языке.
    /// </summary>
    int[] Search
        (
            ISyncProvider connection,
            string query,
            string? database = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        );
}
