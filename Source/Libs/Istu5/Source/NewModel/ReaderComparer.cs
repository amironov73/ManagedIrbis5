// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ReaderComparer.cs -- сравнивает читателей по их билетам
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace Istu.NewModel;

/// <summary>
/// Сравнивает читателей по их билетам.
/// </summary>
public sealed class ReaderComparer
    : IEqualityComparer<Reader>
{
    #region IEqualityComparer<T> members

    /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
    public bool Equals (Reader? x, Reader? y) =>
        string.CompareOrdinal (x?.Ticket, y?.Ticket) == 0;

    /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
    public int GetHashCode (Reader obj)
    {
        return obj.Ticket?.GetHashCode() ?? 0;
    }

    #endregion
}
