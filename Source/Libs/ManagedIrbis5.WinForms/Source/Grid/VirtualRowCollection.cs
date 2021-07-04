// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* VirtualRowCollection.cs -- виртуальная коллекция строк грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using System.Collections;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    /// Виртуальная коллекция строк грида.
    /// </summary>
    public class VirtualRowCollection<T>
        : ISiberianRowCollection
    {
        #region Properties

        /// <summary>
        /// Грид, которому принадлежат строки.
        /// </summary>
        public SiberianGrid? Grid { get; set; }

        /// <summary>
        /// Адаптер, предоставляющий данные.
        /// </summary>
        public IVirtualAdapter<T> Adapter { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="adapter">Адаптер.</param>
        /// <param name="count">Количество виртуальных строк.</param>
        public VirtualRowCollection
            (
                IVirtualAdapter<T> adapter,
                int count = 0
            )
        {
            Adapter = adapter;
            Count = count != 0 ? count : adapter.TotalLength;
            _cache = null;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Получение строки с указанным индексом.
        /// </summary>
        public SiberianRow GetRow
            (
                int index
            )
        {
            var result = new SiberianRow
            {
                Grid = Grid,
                Index = index
            };

            if (Count == 0)
            {
                Count = Adapter.TotalLength;
            }

            if (_cache is null || index < _cacheIndex
                || index >= _cacheIndex + _cacheLength)
            {
                var data = Adapter.ReadData(index, Adapter.PreferredPortion);
                if (data is not null)
                {
                    _cache = data.Data;
                    _cacheIndex = data.FirstLine;
                    _cacheLength = data.Length;
                }
            }

            if (_cache is not null && index >= _cacheIndex
                && index < _cacheIndex + _cacheLength)
            {
                result.Data = _cache[index - _cacheIndex];
            }

            return result;

        } // method GetRow

        #endregion

        #region Private members

        // Кеш
        private T[]? _cache;

        // Индекс первого элемента
        private int _cacheIndex;

        // количество элементов в кеше
        private int _cacheLength;

        #endregion

        #region IEnumerable<T> members

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
        public IEnumerator<SiberianRow> GetEnumerator()
        {
            var dummyRow = new SiberianRow();

            for (var i = 0; i < Count; i++)
            {
                dummyRow.Grid = Grid;
                dummyRow.Index = i;
                yield return dummyRow;
            }

        } // method GetEnumerator

        #endregion

        #region ICollection<T> members

        /// <inheritdoc cref="ICollection{T}.Add"/>
        public void Add(SiberianRow item)
        {
            throw new NotImplementedException();

        } // method Add

        /// <inheritdoc cref="Clear"/>
        public void Clear()
        {
            _cache = null;
            _cacheIndex = 0;
            _cacheLength = 0;

        } // method Clear

        /// <inheritdoc cref="ICollection{T}.Contains"/>
        public bool Contains(SiberianRow item) => throw new NotImplementedException();

        /// <inheritdoc cref="ICollection{T}.CopyTo"/>
        public void CopyTo(SiberianRow[] array, int arrayIndex) =>
            throw new NotImplementedException();

        /// <inheritdoc cref="ICollection{T}.Remove"/>
        public bool Remove(SiberianRow item) => throw new NotImplementedException();

        /// <inheritdoc cref="ICollection{T}.Count"/>
        public int Count { get; internal set; }

        /// <inheritdoc cref="ICollection{T}.IsReadOnly"/>
        public bool IsReadOnly => true;

        #endregion

        #region IList<T> members

        /// <inheritdoc cref="IList{T}.IndexOf"/>
        public int IndexOf(SiberianRow item) => throw new NotImplementedException();

        /// <inheritdoc cref="IList{T}.Insert"/>
        public void Insert(int index, SiberianRow item) => throw new NotImplementedException();

        /// <inheritdoc cref="IList{T}.RemoveAt"/>
        public void RemoveAt(int index) => throw new NotImplementedException();

        /// <inheritdoc cref="IList{T}.this"/>
        public SiberianRow this[int index]
        {
            get => GetRow(index);
            set => throw new NotImplementedException();

        } // property this

        #endregion

    } // class VirtualRowCollection

} // namespace ManagedIrbis.WinForms.Grid
