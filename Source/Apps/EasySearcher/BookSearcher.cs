// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable PartialTypeWithSinglePart
// ReSharper disable StringLiteralTypo

/* Searcher.cs -- искатель по каталогу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Linq;

using ManagedIrbis;

#endregion

#nullable enable

namespace EasySearcher;

/// <summary>
/// Искатель по каталогу
/// </summary>
public sealed class BookSearcher
{
    #region Public methods

    /// <summary>
    /// Поиск книг.
    /// </summary>
    public async Task<string[]> SearchForBooksAsync
        (
            string query
        )
    {
        var connectionString = Magna.Configuration["connection"];
        if (string.IsNullOrEmpty (connectionString))
        {
            return Array.Empty<string>();
        }

        await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        connection.ParseConnectionString (connectionString);
        await connection.ConnectAsync();
        if (!connection.IsConnected)
        {
            return Array.Empty<string>();
        }

        var parameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = query,
            Format = "@brief"
        };
        var found = await connection.SearchAsync (parameters);
        if (found.IsNullOrEmpty())
        {
            return Array.Empty<string>();
        }

        var result = FoundItem.ToText (found)
            .NonEmptyLines()
            .Order (StringComparer.InvariantCultureIgnoreCase)
            .ToArray();

        return result;
    }

    #endregion
}
