// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* RefEnumerable.cs -- спан, элементы которого перечисляются по ссылке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Collections;

/// <summary>
/// Обертка для перечисляемого спана.
/// </summary>
public readonly ref struct RefEnumerable<T>
{
    #region Nested class

    /// <summary>
    /// Перечислитель.
    /// </summary>
    public ref struct RefEnumerator<TT>
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public RefEnumerator
            (
                Span<TT> data
            ) : this()
        {
            _data = data;
            _position = -1;
        }

        #endregion

        #region Private members

        private readonly Span<TT> _data;
        private int _position;

        #endregion

        #region IEnumerable imitation

        /// <summary>
        /// Ссылка на текущий элемент.
        /// </summary>
        public ref TT Current => ref _data[_position];

        /// <summary>
        /// Переход к следующему элементу.
        /// </summary>
        public bool MoveNext() => ++_position < _data.Length;

        #endregion
    }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RefEnumerable (Span<T> data) => _data = data;

    #endregion

    #region Private members

    private readonly Span<T> _data;

    #endregion

    #region IRefEnumerable<T> imitation

    /// <summary>
    /// Запрос перечислителя.
    /// </summary>
    public RefEnumerator<T> GetEnumerator() => new RefEnumerator<T> (this._data);

    #endregion
}
