// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SinglyLinkedList.cs -- простой односвязный список
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
    /// Простой односвязный список. Сделан исключительно для экономии памяти,
    /// т. к. стандартный <see cref="LinkedList{T}"/> является
    /// двусвязным списком и для каждой ноды расходует на 16 байт больше
    /// (в 64-битном режиме): один указатель на предыдущую ноду
    /// и ещё один указатель на список. Здесь всего этого нет.
    /// </summary>
    public sealed class SinglyLinkedList<T>
        : ICollection<T>
    {
        #region Properties

        /// <summary>
        /// Первое звено списка.
        /// </summary>
        public SinglyLinkedListNode<T>? First { get; private set; }

        /// <summary>
        /// Последнее звено списка.
        /// </summary>
        public SinglyLinkedListNode<T>? Last { get; private set; }

        #endregion

        #region Private members

        private int _count;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление в начало списка.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void AddFirst
            (
                T? value
            )
        {
            var node = new SinglyLinkedListNode<T>(value);

            if (First is null)
            {
                First = node;
                Last = node;
            }
            else
            {
                node.Next = First;
                First = node;
            }

            ++_count;
        }

        /// <summary>
        /// Поиск заданного значения в списке.
        /// </summary>
        /// <param name="value">Искомое значение.</param>
        /// <returns>Указатель на первое звено, содержащее указанный элемент.
        /// </returns>
        public SinglyLinkedListNode<T>? Find
            (
                T? value
            )
        {
            var node = First;
            var comparer = EqualityComparer<T>.Default;
            while (node is not null)
            {
                if (value is null)
                {
                    if (node.Value is null)
                    {
                        return node;
                    }
                }
                else
                {
                    var nodeValue = node.Value;
                    if (nodeValue is not null)
                    {
                        if (comparer.Equals(value, nodeValue))
                        {
                            return node;
                        }
                    }
                }

                node = node.Next;
            }

            return default;
        }

        /// <summary>
        /// Удаление первого элемента из списка.
        /// </summary>
        public void RemoveFirst()
        {
            if (First is not null)
            {
                if (First == Last)
                {
                    First = null;
                    Last = null;
                }
                else
                {
                    First = First.Next;
                }

                --_count;
            }
        }

        #endregion

        #region ICollection<T> members

        [ExcludeFromCodeCoverage]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<T> GetEnumerator() => new Enumerator (this);

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void Add
            (
                T? item
            )
        {
            // добавляем в конец списка

            var node = new SinglyLinkedListNode<T>(item);

            First ??= node;
            if (Last is not null)
            {
                Last.Next = node;
            }

            Last = node;
            ++_count;
        }

        /// <inheritdoc cref="ICollection{T}.Clear"/>
        public void Clear()
        {
            First = default;
            Last = default;
            _count = 0;
        }

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains ( T? item ) => Find(item) is not null;

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo
            (
                T?[] array,
                int arrayIndex
            )
        {
            var node = First;

            while (node is not null)
            {
                array[arrayIndex++] = node.Value;
                node = node.Next;
            }
        }

        /// <inheritdoc cref="ICollection{T}.Remove"/>
        public bool Remove
            (
                T? item
            )
        {
            if (First is null)
            {
                return false;
            }

            var node = First;
            SinglyLinkedListNode<T>? previous = null;
            var comparer = EqualityComparer<T>.Default;
            while (node is not null)
            {
                if (item is null)
                {
                    if (node.Value is null)
                    {
                        break;
                    }
                }
                else
                {
                    var nodeValue = node.Value;
                    if (nodeValue is not null)
                    {
                        if (comparer.Equals(item, nodeValue))
                        {
                            break;
                        }
                    }
                }

                previous = node;
                node = node.Next;
            }

            if (node is null)
            {
                // не нашли
                return false;
            }

            if (previous is null)
            {
                // это первый элемент
                RemoveFirst();

                return true;
            }

            // это не первый элемент
            previous.Next = node.Next;
            --_count;

            if (ReferenceEquals(node, Last))
            {
                // это последний элемент
                Last = previous;
            }

            return true;
        }

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count => _count;

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => false;

        #endregion

        #region Enumerator

        /// <summary>
        /// Перечислитель для односвязного списка.
        /// </summary>
        struct Enumerator
            : IEnumerator<T>
        {
            #region Construction

            /// <summary>
            /// Конструктор.
            /// </summary>
            public Enumerator
                (
                    SinglyLinkedList<T> list
                )
                : this()
            {
                _list = list;
            }

            #endregion

            #region Private members

            private readonly SinglyLinkedList<T> _list;
            private SinglyLinkedListNode<T>? _current;

            #endregion

            #region IEnumerator members

            /// <inheritdoc cref="IEnumerator.MoveNext"/>
            public bool MoveNext() =>
                (_current is null ? _current = _list.First : _current = _current!.Next) is not null;

            /// <inheritdoc cref="IEnumerator.Reset" />
            public void Reset() => _current = null;

            /// <inheritdoc cref="IEnumerator.Current"/>
            [ExcludeFromCodeCoverage]
            object? IEnumerator.Current => Current;

            /// <inheritdoc cref="IDisposable.Dispose"/>
            public void Dispose() {}

            /// <inheritdoc cref="IEnumerator{T}.Current"/>
            public T Current => _current!.Value!;

            #endregion

        } // struct Enumerator

        #endregion

    } // class SinglyLinkedList

} // namespace AM.Collections
