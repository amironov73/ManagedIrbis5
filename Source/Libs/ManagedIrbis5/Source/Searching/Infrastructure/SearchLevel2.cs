﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel2.cs --
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
    // второй оператор контекстного И; соединение двух
    // терминов таким оператором контекстного И
    // обозначает требование поиска записей, в которых
    // оба термина присутствуют в одном и том же
    // повторении поля (или точнее – когда у терминов
    // совпадают вторые и третьи части ссылок).
    //

    /// <summary>
    /// level1 (F) level1
    /// </summary>
    sealed class SearchLevel2
        : ComplexLevel<SearchLevel1>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel2()
            : base(" (F) ")
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
                = new TermLinkComparer.ByOccurrence();
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
    }
}
