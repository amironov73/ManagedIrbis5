// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* AuthorComparer.cs -- сравнивает авторов для сортировки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Fields;

/// <summary>
/// Сравнивает авторов для сортировки.
/// </summary>
public static class AuthorComparer
{
    #region Nested classes

    private sealed class FullNameComparer
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

    private sealed class FamilyNameComparer
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
    /// Сравнение авторов <see cref="AuthorInfo"/>
    /// по полю <see cref="AuthorInfo.FamilyName"/> (только фамилия).
    /// </summary>
    public static IComparer<AuthorInfo> FamilyName() => new FamilyNameComparer();

    /// <summary>
    /// Сравнение авторов <see cref="AuthorInfo"/>
    /// по полю <see cref="AuthorInfo.FullName"/> (ФИО полностью).
    /// </summary>
    public static IComparer<AuthorInfo> FullName() => new FullNameComparer();

    #endregion
}
