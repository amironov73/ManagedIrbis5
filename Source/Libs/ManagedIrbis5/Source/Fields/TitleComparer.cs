// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TitleComparer.cs -- сравнение заголовков TitleInfo.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сравнение заголовков <see cref="TitleInfo"/>.
    /// </summary>
    public static class TitleComparer
    {
        #region Nested classes

        /// <summary>
        /// Сравнение по полному заголовку.
        /// </summary>
        class FullTitleComparer
            : IComparer<TitleInfo>
        {
            #region IComparer<T> members

            public int Compare (TitleInfo? x, TitleInfo? y) =>
                string.CompareOrdinal (x?.FullTitle, y?.FullTitle);

            #endregion
        } // class FullTitleComparer

        /// <summary>
        /// Сравнение согласно числовому значению.
        /// </summary>
        class NumericComparer
            : IComparer<TitleInfo>
        {
            #region IComparer<T> members

            public int Compare (TitleInfo? x, TitleInfo? y) =>
                NumberText.Compare (x?.FullTitle, y?.FullTitle);

            #endregion
        } // class NumericComparer

        /// <summary>
        /// Сравнение только по заголовку.
        /// </summary>
        class TitleOnlyComparer
            : IComparer<TitleInfo>
        {
            #region IComparer<T> members

            public int Compare ( TitleInfo? x, TitleInfo? y ) =>
                string.CompareOrdinal (x?.Title, y?.Title);

            #endregion
        } // method TitleOnlyComparer

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение <see cref="TitleInfo"/>
        /// по полю <see cref="TitleInfo.FullTitle"/>.
        /// </summary>
        public static IComparer<TitleInfo> FullTitle() => new FullTitleComparer();

        /// <summary>
        /// Сравнение <see cref="TitleInfo"/>
        /// по полю <see cref="TitleInfo.FullTitle"/>
        /// с учетом значений чисел.
        /// </summary>
        public static IComparer<TitleInfo> Numeric() => new NumericComparer();

        /// <summary>
        /// Сравнение <see cref="TitleInfo"/>
        /// по полю <see cref="TitleInfo.Title"/>.
        /// </summary>
        public static IComparer<TitleInfo> TitleOnly() => new TitleOnlyComparer();

        #endregion

    } // class TitleComparer

} // namespace ManagedIrbis.Fields
