﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
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

using ManagedIrbis.Client;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
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
            Sure.NotNull(context, nameof(context));

            IrbisProvider provider = context.Provider;
            TermLink[] result;
            string term = Term.ThrowIfNull(nameof(Term));

            switch (Tail)
            {
                case null:
                case "":
                    result = provider.ExactSearchLinks(term);
                    break;

                case "$":
                    result = provider.ExactSearchTrimLinks(term, 1000);
                    break;

                case "@":
                    Magna.Error
                        (
                            "SearchTerm::Find: "
                            + "@ not implemented"
                        );

                    throw new NotImplementedException();

                default:
                    Magna.Error
                        (
                            "SearchTerm::Find: "
                            + "unexpected tail: "
                            + Tail.ToVisibleString()
                        );

                    throw new IrbisException("Unexpected tail");
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
            Magna.Error
                (
                    "SearchTerm::ReplaceChild: "
                    + "not implemented"
                );

            throw new NotImplementedException();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.Append('"');
            result.Append(Term);
            if (!string.IsNullOrEmpty(Tail))
            {
                result.Append(Tail);
            }
            result.Append('"');
            if (!ReferenceEquals(Context, null))
            {
                result.Append("/(");
                result.Append
                    (
                        string.Join
                        (
                            ",",
                            Context
                        )
                    );
                result.Append(')');
            }

            return result.ToString();
        }

        #endregion

    } // class SearchTerm

} // namespace ManagedIrbis.Infrastructure
