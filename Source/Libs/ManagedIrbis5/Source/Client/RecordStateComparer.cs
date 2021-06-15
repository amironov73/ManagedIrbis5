// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RecordStateComparer.cs -- сравнивает два состояния записи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Сравнивает два состояния записи <see cref="Record"/>.
    /// </summary>
    public sealed class RecordStateComparer
    {
        #region Nested classes

        /// <summary>
        /// Compares <see cref="RecordState"/>
        /// by <see cref="RecordState.Mfn"/>
        /// </summary>
        public sealed class ByMfn
            : IEqualityComparer<RecordState>
        {
            #region IEqualityComparer<T> members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals (RecordState x, RecordState y) => x.Mfn == y.Mfn;

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode (RecordState obj) => obj.Mfn;

            #endregion

        } // class ByMfn

        /// <summary>
        /// Compares <see cref="RecordState"/>
        /// by <see cref="RecordState.Mfn"/> and
        /// <see cref="RecordState.Version"/>.
        /// </summary>
        public sealed class ByVersion
            : IEqualityComparer<RecordState>
        {
            #region IEqualityComparer<T> members

            /// <inheritdoc cref="IEqualityComparer{T}.Equals(T,T)"/>
            public bool Equals (RecordState x, RecordState y) =>
                x.Mfn == y.Mfn && x.Version == y.Version;

            /// <inheritdoc cref="IEqualityComparer{T}.GetHashCode(T)"/>
            public int GetHashCode (RecordState obj) => unchecked (obj.Mfn * 37 + obj.Version);

            #endregion

        } // method ByVersion

        #endregion

    } // class RecordStateComparer

} // namespace ManagedIrbis.Client
