// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ChainMemory.cs -- цепочка фрагментов памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;

#endregion

#nullable enable

namespace AM;

/// <summary>
/// Цепочка фрагментов памяти.
/// </summary>
/// <typeparam name="T">Тип фрагментов.</typeparam>
public sealed class ChainMemory<T>
{
    /// <summary>
    /// Перечислитель для цепочки фрагментов памяти.
    /// </summary>
    public struct ChainEnumerator<TT>
    {
        private ChainMemory<TT>? _head;
        private int _offset;

#nullable disable
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="head">Начальное звено цепочки.</param>
        public ChainEnumerator
            (
                ChainMemory<TT> head
            )
            : this()
        {
            _head = head;
            _offset = -1;
        }
#nullable enable

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public bool MoveNext()
        {
            AGAIN:
            if (_head is null)
            {
                return false;
            }

            ++_offset;
            if (_offset >= _head!.Data.Length)
            {
                _head = _head.Next;
                _offset = -1;
                goto AGAIN;
            }

            Current = _head.Data.Span[_offset];

            return true;
        }

        /// <inheritdoc cref="IEnumerator.Current"/>
        public TT Current { get; private set; }
    }

    #region Properties

    /// <summary>
    /// Указатель на данные.
    /// </summary>
    public ReadOnlyMemory<T> Data { get; set; }

    /// <summary>
    /// Указатель на следующий фрагмент.
    /// </summary>
    public ChainMemory<T>? Next { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="data">Указатель на данные.</param>
    public ChainMemory (ReadOnlyMemory<T> data)
    {
        Data = data;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление следующего элемента.
    /// </summary>
    /// <param name="data">Указатель на данные.</param>
    /// <returns>Следующий элемент.</returns>
    public ChainMemory<T> Append (ReadOnlyMemory<T> data)
    {
        var next = new ChainMemory<T> (data);
        Next = next;

        return next;
    }

    /// <summary>
    /// Подсчитывает общую длину цепочки.
    /// </summary>
    /// <returns>Количество элементов.</returns>
    public int TotalLength()
    {
        var result = 0;

        for (var item = this; item is not null; item = item.Next)
        {
            result += item.Data.Length;
        }

        return result;
    }

    /// <summary>
    /// Превращение в массив.
    /// </summary>
    public T[] ToArray()
    {
        var total = TotalLength();
        var result = new T[total];
        var mem = new Memory<T> (result);

        for (var item = this; item is not null; item = item.Next)
        {
            item.Data.CopyTo (mem);
            mem = mem.Slice (item.Data.Length);
        }

        return result;
    }

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    public ChainEnumerator<T> GetEnumerator()
    {
        return new ChainEnumerator<T> (this);
    }

    #endregion
}
