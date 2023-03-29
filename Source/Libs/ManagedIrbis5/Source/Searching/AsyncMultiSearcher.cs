// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AsyncMultiSearcher.cs -- асинхронный поиск по нескольким каталогам сразу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Threading.Tasks;

using AM;

using JetBrains.Annotations;

using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Асинхронный поиск по нескольким каталогам сразу.
/// Наивная реализация.
/// </summary>
[PublicAPI]
public sealed class AsyncMultiSearcher
{
    #region Properties

    /// <summary>
    /// Провайдер.
    /// </summary>
    public IAsyncProvider Provider { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="provider">Провайдер.</param>
    public AsyncMultiSearcher
        (
            IAsyncProvider provider
        )
    {
        Sure.NotNull (provider);

        Provider = provider;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Поиск во всех перечисленных каталогах.
    /// </summary>
    public async Task<RecordBacket> SearchAllAsync
        (
            string expression,
            IEnumerable<string> databases
        )
    {
        Sure.NotNullNorEmpty (expression);
        Sure.NotNull (expression);

        var result = new RecordBacket();
        var savedDatabase = Provider.Database;

        try
        {
            foreach (var database in databases)
            {
                var parameters = new SearchParameters
                {
                    Database = Provider.EnsureDatabase (database),
                    Expression = expression
                };
                var found = await Provider.SearchAsync (parameters);
                if (found is not null)
                {
                    foreach (var item in found)
                    {
                        var reference = new RecordReference
                        {
                            Database = database,
                            Mfn = item.Mfn
                        };
                        result.Add (reference);
                    }
                }
            }
        }
        finally
        {
            Provider.Database = savedDatabase;
        }

        return result;
    }

    /// <summary>
    /// Поиск до первых найденных записей.
    /// </summary>
    public async Task<RecordBacket> SearchAnyAsync
        (
            string expression,
            IEnumerable<string> databases
        )
    {
        var result = new RecordBacket();
        var savedDatabase = Provider.Database;

        try
        {
            foreach (var database in databases)
            {
                var parameters = new SearchParameters
                {
                    Database = Provider.EnsureDatabase (database),
                    Expression = expression
                };
                var found = await Provider.SearchAsync (parameters);
                if (found is not null)
                {
                    foreach (var item in found)
                    {
                        var reference = new RecordReference
                        {
                            Database = database,
                            Mfn = item.Mfn
                        };
                        result.Add (reference);
                    }

                    if (found.Length != 0)
                    {
                        break;
                    }
                }
            }
        }
        finally
        {
            Provider.Database = savedDatabase;
        }

        return result;
    }

    #endregion
}
