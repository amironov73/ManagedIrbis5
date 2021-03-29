// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* BookComparer.cs -- сравнение описаний книг BookInfo
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сравнение описаний книг <see cref="BookInfo"/>.
    /// </summary>
    public static class BookComparer
    {
        #region Nested classes

        /// <summary>
        /// Сравнение по автору, затем по заглавию.
        /// </summary>
        class AuthorAndTitleComparer
            : IComparer<BookInfo>
        {
            #region IComparer<T> members

                public int Compare
                    (
                        BookInfo? x,
                        BookInfo? y
                    )
            {
                var authorX = x?.FirstAuthor;
                var authorY = y?.FirstAuthor;

                if (authorX is null)
                {
                    if (authorY is not null)
                    {
                        return -1;
                    }
                }
                else
                {
                    if (authorY is null)
                    {
                        return 1;
                    }

                    var result = Author.Compare(authorX, authorY);
                    if (result != 0)
                    {
                        return result;
                    }
                }

                return Title.Compare(x.Title, y.Title);
            } // method Compare

            #endregion
        } // class AuthorAndTitleComparer

        #endregion

        #region Properties

        /// <summary>
        /// Сравнивает <see cref="AuthorInfo"/> по ФИО.
        /// </summary>
        public static IComparer<AuthorInfo> Author { get; } = AuthorComparer.FamilyName();

        /// <summary>
        /// Сравнивает <see cref="TitleInfo"/> по полному заголовку.
        /// </summary>
        public static IComparer<TitleInfo> Title { get; } = TitleComparer.FullTitle();

        #endregion

        #region Public methods

        /// <summary>
        /// Сравнение <see cref="BookInfo"/>
        /// по авторам, затем по заглавиям.
        /// </summary>
        public static IComparer<BookInfo> AuthorAndTitle() =>
            new AuthorAndTitleComparer();

        #endregion

    } // class BookComparer

} // namespace ManagedIrbis.Fields
