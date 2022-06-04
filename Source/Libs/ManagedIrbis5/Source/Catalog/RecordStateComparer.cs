// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RecordStateComparer.cs -- сравнитель для записей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Catalog;

/// <summary>
/// Сравнитель для записей <see cref="Record"/>.
/// </summary>
public sealed class RecordStateComparer
{
    #region Nested classes

    /// <summary>
    /// Сравнение двух состояний <see cref="RecordState"/>
    /// по <see cref="RecordState.Id"/>
    /// </summary>
    public sealed class ById
        : IEqualityComparer<RecordState>
    {
        #region IEqualityComparer<T> members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                RecordState x,
                RecordState y
            )
        {
            return x.Id == y.Id;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                RecordState obj
            )
        {
            return obj.Id;
        }

        #endregion
    }

    /// <summary>
    /// Сравнение двух состояний <see cref="RecordState"/>
    /// по <see cref="RecordState.Mfn"/>
    /// </summary>
    public sealed class ByMfn
        : IEqualityComparer<RecordState>
    {
        #region IEqualityComparer<T> members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                RecordState x,
                RecordState y
            )
        {
            return x.Mfn == y.Mfn;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                RecordState obj
            )
        {
            return obj.Mfn;
        }

        #endregion
    }

    /// <summary>
    /// Сравнение двух <see cref="RecordState"/>
    /// по <see cref="RecordState.Mfn"/> и
    /// <see cref="RecordState.Version"/>.
    /// </summary>
    public sealed class ByVersion
        : IEqualityComparer<RecordState>
    {
        #region IEqualityComparer<T> members

        /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
        public bool Equals
            (
                RecordState x,
                RecordState y
            )
        {
            return x.Mfn == y.Mfn
                   && x.Version == y.Version;
        }

        /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
        public int GetHashCode
            (
                RecordState obj
            )
        {
            return unchecked (obj.Mfn * 37 + obj.Version);
        }

        #endregion
    }

    #endregion
}
