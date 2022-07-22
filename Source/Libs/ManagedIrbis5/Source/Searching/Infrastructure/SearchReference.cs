// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchReference.cs -- ссылка на результат прошлого поиска по его номеру
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM;

using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// "#N" -- ссылка на результат прошлого поиска по его номеру.
/// </summary>
internal sealed class SearchReference
    : ISearchTree
{
    #region Properties

    /// <inheritdoc cref="ISearchTree.Parent"/>
    public ISearchTree? Parent { get; set; }

    /// <summary>
    /// Номер поиска (нумерация с 1).
    /// </summary>
    public string? Number { get; set; }

    #endregion

    #region ISearchTree members

    /// <inheritdoc cref="ISearchTree.Children"/>
    public ISearchTree[] Children => Array.Empty<ISearchTree>();

    /// <inheritdoc cref="ISearchTree.Value"/>
    public string? Value => Number;

    /// <inheritdoc cref="ISearchTree.Find"/>
    public TermLink[] Find
        (
            SearchContext context
        )
    {
        Sure.NotNull (context);

        var result = Array.Empty<TermLink>();

        var number = Number.SafeToInt32 (-1);
        if (number > 0)
        {
            var history = context.Manager.SearchHistory;
            if (number <= history.Count)
            {
                var previous = history[number - 1];
                var query = previous.Query;

                if (!string.IsNullOrEmpty (query))
                {
                    var found = context.Provider.Search (query);
                    result = TermLink.FromMfn (found);
                }
            }
        }

        return result;
    }

    /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
    public void ReplaceChild
        (
            ISearchTree fromChild,
            ISearchTree? toChild
        )
    {
        Sure.NotNull (fromChild);

        Magna.Logger.LogError
            (
                nameof (SearchReference) + "::" + nameof (ReplaceChild)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return "#" + Number;
    }

    #endregion
}
