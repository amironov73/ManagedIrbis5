// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchTerm.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM;
using AM.Text;

using ManagedIrbis.Client;
using ManagedIrbis.Direct;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

//
// В общем виде операнд поискового выражения можно
// представить следующим образом:
//
// “<префикс><термин>$”/(tag1,tag2,…tagN)
//
// где:
//
// <префикс> - префикс, определяющий вид
// термина(вид словаря);
// <термин> - собственно термин словаря;
// $ - признак правого усечения термина;
// определяет совокупность терминов, имеющих
// начальную последовательность символов,
// совпадающую с указанным термином;
//может отсутствовать – в этом случае поиск
// идет по точному значению указанного термина.
// “ – символ-ограничитель термина (двойные кавычки);
// должен использоваться обязательно, если термин
// включает в себя символы пробел, круглые скобки,
// решетка (#), а также символы, совпадающие
// с обозначениями логических операторов;
// / (tag1, tag2,…tagN) – конструкция квалификации
// термина; определяет метки поля, в которых должен
// находиться указанный термин, или точнее – вторую
// часть ссылки термина
// (Приложение  5. ТАБЛИЦЫ ВЫБОРА ПОЛЕЙ (ТВП));
// может отсутствовать – что означает отсутствие
// дополнительных требований в части меток полей.
//

/// <summary>
/// Leaf node of AST.
/// </summary>
public sealed class SearchTerm
    : ISearchTree
{
    #region Properties

    /// <inheritdoc cref="ISearchTree.Parent"/>
    public ISearchTree? Parent { get; set; }

    /// <summary>
    /// K=keyword
    /// </summary>
    public string? Term { get; set; }

    /// <summary>
    /// $ or @
    /// </summary>
    public string? Tail { get; set; }

    /// <summary>
    /// /(tag,tag,tag)
    /// </summary>
    public string[]? Context { get; set; }

    #endregion

    #region Private members

    private TermLink[] _ExactSearch
        (
            ISyncProvider provider,
            string database,
            string term,
            int limit
        )
    {
        if (provider is DirectProvider direct)
        {
            var accessor = direct.GetAccessor (database);
            var result = accessor.Accessor.ReadLinks (term);

            return result;
        }

        throw new NotImplementedException (nameof (_ExactSearch));
    }

    private TermLink[] _TrimSearch
        (
            ISyncProvider provider,
            string database,
            string term,
            int limit
        )
    {
        throw new NotImplementedException (nameof (_TrimSearch));
    }

    private TermLink[] _MorphoSearch
        (
            ISyncProvider provider,
            string database,
            string term,
            int limit
        )
    {
        throw new NotImplementedException (nameof (_MorphoSearch));
    }

    #endregion

    #region ISearchTree members

    /// <inheritdoc cref="ISearchTree.Children" />
    public ISearchTree[] Children => Array.Empty<ISearchTree>();

    /// <inheritdoc cref="ISearchTree.Value" />
    public string? Value => Term;

    /// <inheritdoc cref="ISearchTree.Find" />
    public TermLink[] Find
        (
            SearchContext context
        )
    {
        Sure.NotNull (context);

        var provider = context.Provider;
        var database = provider.EnsureDatabase();
        var term = Term.ThrowIfNull (nameof (Term));
        TermLink[] result;

        switch (Tail)
        {
            case null:
            case "":
                result = _ExactSearch (provider, database, term, 1000);
                break;

            case "$":
                result = _TrimSearch (provider, database, term, 1000);
                break;

            case "@":
                result = _MorphoSearch (provider, database, term, 1000);
                break;

            default:
                Magna.Logger.LogError
                    (
                        nameof (SearchTerm) + "::" + nameof (Find)
                        + ": unexpected tail: {Tail}",
                        Tail.ToVisibleString()
                    );

                throw new IrbisException ("Unexpected search term tail");
        }

        // TODO implement context filtering

        return result;
    }

    /// <inheritdoc cref="ISearchTree.ReplaceChild"/>
    public void ReplaceChild
        (
            ISearchTree fromChild,
            ISearchTree? toChild
        )
    {
        Magna.Logger.LogError
            (
                nameof (SearchTerm) + "::" + nameof (ReplaceChild)
                + ": not implemented"
            );

        throw new NotImplementedException();
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();

        builder.Append ('"');
        builder.Append (Term);
        if (!string.IsNullOrEmpty (Tail))
        {
            builder.Append (Tail);
        }

        builder.Append ('"');
        if (!ReferenceEquals (Context, null))
        {
            builder.Append ("/(");
            builder.Append
                (
                    string.Join
                        (
                            ",",
                            Context
                        )
                );
            builder.Append (')');
        }

        var result = builder.ToString();
        StringBuilderPool.Shared.Return (builder);

        return result;
    }

    #endregion
}
