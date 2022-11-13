// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMemberInSuper.Global
// ReSharper disable UnusedType.Global

/* IAsyncTeapotSearcher.cs -- асинхронный интерфейс "поиска для чайников"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Threading.Tasks;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Асинхронный интерфейс "поиска для чайников".
/// </summary>
public interface IAsyncTeapotSearcher
{
    /// <summary>
    /// Построение поискового выражения по запросу на естественном языке.
    /// </summary>
    Task<string> BuildSearchExpressionAsync (string query);

    /// <summary>
    /// Поиск записей, удовлетворяющих запросу на естественном языке.
    /// </summary>
    Task<int[]> SearchAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        );

    /// <summary>
    /// Поиск записей, удовлетворяющих запросу на естественном языке
    /// с последующим расформатированием.
    /// </summary>
    Task<string[]> SearchFormatAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            string? format = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        );

    /// <summary>
    /// Поиск записей, удовлетворяющих запросу на естественном языке.
    /// </summary>
    Task<Record[]> SearchReadAsync
        (
            IAsyncProvider connection,
            string query,
            string? database = null,
            IRelevanceEvaluator? evaluator = null,
            int limit = 500
        );
}
