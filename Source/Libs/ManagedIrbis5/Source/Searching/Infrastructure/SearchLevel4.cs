﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* SearchLevel4.cs --
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
    // оператор логического И; соединение двух терминов
    // логическим оператором И обозначает требование
    // поиска записей, в которых присутствуют оба термина.
    //

    /// <summary>
    /// level3 * level3
    /// </summary>
    sealed class SearchLevel4
        : ComplexLevel<SearchLevel3>
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchLevel4()
            : base(" * ")
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
                = new TermLinkComparer.ByMfn();
            for (int i = 1; i < Items.Count; i++)
            {
                if (result.Length == 0)
                {
                    return result;
                }
                TermLink[] second = Items[i].Find(context);

                result = result.Intersect
                    (
                        second,
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
