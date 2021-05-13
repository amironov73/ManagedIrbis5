// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* Iterator.cs -- абстрактный итератор
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Collections
{
    /// <summary>
    /// Абстрактный перечислитель, заимствованный из <c>System.IO</c>.
    /// </summary>
    public abstract class Iterator<T>
        : IEnumerable<T>,
          IEnumerator<T>
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Iterator()
        {
            State = -1;
        }

        #endregion

        #region Private members

        /// <summary>
        /// Состояние перечислителя.
        /// В начале равно -1.
        /// </summary>
        protected int State;

        /// <summary>
        /// Текущий перечисляемый элемент.
        /// </summary>
        protected T? _current;

        #endregion

        #region IEnumerable<T> members

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => this;

        /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion

        #region IEnumerator<T> members

        /// <inheritdoc cref="IEnumerator{T}.Current"/>
        public T Current => _current!;

        /// <inheritdoc cref="IEnumerator.Current"/>
        object? IEnumerator.Current => Current;

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public abstract bool MoveNext();

        /// <inheritdoc cref="IEnumerator.Reset"/>
        [ExcludeFromCodeCoverage]
        public void Reset() => throw new NotImplementedException();

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public virtual void Dispose() {}

        #endregion

    } // class Iterator<T>

} // namespace AM.Collections
