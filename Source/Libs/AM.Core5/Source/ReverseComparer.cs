// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ReverseComparer.cs -- превращает сравнение элементов в противоположное
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Превращает сравнение элементов в противоположное.
/// </summary>
public sealed class ReverseComparer<T>
    : IComparer<T>
{
    #region Properties

    /// <summary>
    /// Внутреннее сравнение.
    /// </summary>
    public IComparer<T> Inner { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReverseComparer
        (
            IComparer<T> inner
        )
    {
        Sure.NotNull (inner);

        Inner = inner;
    }

    #endregion

    #region IComparer<T> members

    /// <inheritdoc cref="IComparer{T}.Compare"/>
    public int Compare
        (
            T? x,
            T? y
        )
    {
        return -Inner.Compare (x, y);
    }

    #endregion
}
