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

/* BookSearcher.cs -- искатель по каталогу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis;
using ManagedIrbis.Searching;

#endregion

#nullable enable

namespace EasySearcher;

/// <summary>
/// Искатель по каталогу.
/// </summary>
public sealed class BookSearcher
{
    #region Public methods

    /// <summary>
    /// Поиск книг.
    /// </summary>
    public async Task<FoundItem[]> SearchForBooksAsync
        (
            string query
        )
    {
        var connectionString = Magna.Configuration["connection"];
        if (string.IsNullOrEmpty (connectionString))
        {
            return Array.Empty<FoundItem>();
        }

        await using var connection = ConnectionFactory.Shared.CreateAsyncConnection();
        connection.ParseConnectionString (connectionString);
        await connection.ConnectAsync();
        if (!connection.IsConnected)
        {
            return Array.Empty<FoundItem>();
        }

        var strategy = GetSearchStrategy (query);

        var found = await strategy (connection, query);

        return found;
    }

    #endregion

    #region Private members

    private delegate Task<FoundItem[]> SearchImplementation
        (AsyncConnection connection, string query);

    private static SearchImplementation GetSearchStrategy
        (
            string query
        )
    {
        if (query.Contains ('='))
        {
            return SimpleSearch;
        }

        return SearchWithTeapot;
    }

    private static async Task<FoundItem[]> SimpleSearch
        (
            AsyncConnection connection,
            string query
        )
    {
        var parameters = new SearchParameters
        {
            Database = connection.EnsureDatabase(),
            Expression = query,
            Format = "@brief"
        };
        var result = await connection.SearchAsync (parameters);
        if (result.IsNullOrEmpty())
        {
            return Array.Empty<FoundItem>();
        }

        return result;
    }

    private static string CleanText
        (
            string text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        string result;
        var textKind = TextUtility.DetermineTextKind (text);
        if (textKind == TextKind.Html)
        {
            result = text.Replace ('\x1F', ' ');
            result = Regex.Replace (result, @"<br\s*/>", "\n");
            result = result.Replace ("<br>", "\n", StringComparison.OrdinalIgnoreCase);
            result = Regex.Replace (result, @"<p[^>]*?>", "\n", RegexOptions.IgnoreCase);
            result = result.Replace ("</tr>", "\n", StringComparison.OrdinalIgnoreCase);
            result = result.Replace ("</div>", "\n", StringComparison.OrdinalIgnoreCase);
            result = Regex.Replace (result, "<[Aa][^>]*?>([^<]*?)</[Aa]>", "$1");

            result = HtmlText.ToPlainText (result)
                     ?? string.Empty;

            result = result.Trim();
        }
        else if (textKind == TextKind.RichText)
        {
            result = RichTextStripper.StripRichTextFormat (text)
                     ?? string.Empty;
        }
        else
        {
            result = text;
        }

        return result;
    }

    private static async Task<FoundItem[]> SearchWithTeapot
        (
            AsyncConnection connection,
            string query
        )
    {
        var serviceProvider = Magna.Host.Services;
        var teapot = new AsyncTeapotSearcher (serviceProvider);
        var result = await teapot.SearchItemsAsync
            (
                connection,
                query
            );
        if (result.IsNullOrEmpty())
        {
            return Array.Empty<FoundItem>();
        }

        foreach (var item in result)
        {
            item.Text = CleanText (item.Text!);
        }

        return result;
    }

    #endregion
}
