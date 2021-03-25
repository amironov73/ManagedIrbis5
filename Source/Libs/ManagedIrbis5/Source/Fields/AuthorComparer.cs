// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* AuthorComparer.cs -- сравнивает авторов для сортировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сравнивает авторов для сортировки.
    /// </summary>
    public static class AuthorComparer
    {
        #region Nested classes

        class FullNameComparer
            : IComparer<AuthorInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    AuthorInfo? x,
                    AuthorInfo? y
                )
            {
                return string.CompareOrdinal
                    (
                        x?.FullName,
                        y?.FullName
                    );
            }

            #endregion
        }

        class FamilyNameComparer
            : IComparer<AuthorInfo>
        {
            #region IComparer<T> members

            public int Compare
                (
                    AuthorInfo? x,
                    AuthorInfo? y
                )
            {
                return string.CompareOrdinal
                    (
                        x?.FamilyName,
                        y?.FamilyName
                    );
            }

            #endregion
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="AuthorInfo"/>
        /// by <see cref="AuthorInfo.FamilyName"/> field.
        /// </summary>
        public static IComparer<AuthorInfo> ByFamilyName() => new FamilyNameComparer();

        /// <summary>
        /// Compare <see cref="AuthorInfo"/>
        /// by <see cref="AuthorInfo.FullName"/> field.
        /// </summary>
        public static IComparer<AuthorInfo> ByFullName() => new FullNameComparer();

        #endregion

    } // class AuthorComparer

} // namespace ManagedIrbis.Fields
