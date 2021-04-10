// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MagazineComparer.cs -- сравнение описаний журналов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Сравнение описаний журналов.
    /// </summary>
    public static class MagazineComparer
    {
        /// <summary>
        /// Сравнение журналов по их заглавиям.
        /// </summary>
        public class ByTitle : IComparer<MagazineInfo>
        {
            /// <inheritdoc cref="IComparer{T}.Compare"/>
            public int Compare(MagazineInfo? x, MagazineInfo? y)
                => string.Compare(x?.Title, y?.Title, StringComparison.CurrentCulture);
        }

    } // class MagazineComparer

} // namespace ManagedIrbis.Magazines
