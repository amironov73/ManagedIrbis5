// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SearchLevel1.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Linq;

using AM;

#endregion

namespace ManagedIrbis.Infrastructure
{
    //
    // (точка обрамленная пробелами) третий оператор
    // контекстного И; соединение двух терминов таким
    // оператором контекстного И обозначает требование
    // поиска записей, в которых оба термина присутствуют
    // в одном и том же повторении поля друг за другом
    // (или точнее – когда у терминов совпадают вторые
    // и третьи части ссылок, а третьи части ссылок
    // отличаются на единицу).
    //

    /// <summary>
    /// level0 . level0
    /// </summary>
    sealed class SearchLevel1
        : ComplexLevel<SearchLevel0>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel1()
            : base(" . ")
        {
        }

        #endregion

        #region ISearchTree members

        /// <inheritdoc cref="ComplexLevel{T}.Find"/>
        public override TermLink[] Find
            (
                SearchContext context
            )
        {
            Sure.NotNull(context, nameof(context));

            TermLink[] result = Items[0].Find(context);
            IEqualityComparer<TermLink> comparer
                = new TermLinkComparer.ByIndex();
            for (int i = 1; i < Items.Count; i++)
            {
                if (result.Length == 0)
                {
                    return result;
                }
                result = result.Intersect
                    (
                        Items[i].Find(context),
                        comparer
                    )
                    .ToArray();
            }
            result = result.Distinct(comparer).ToArray();

            return result;
        }

        #endregion

    } // class SearchLevel1

} // namespace ManagedIrbis.Infrastructure
