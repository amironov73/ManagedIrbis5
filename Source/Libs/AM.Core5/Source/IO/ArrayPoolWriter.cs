// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable ReplaceSliceWithRangeIndexer

/* ArrayPoolWriter.cs -- аналог MemoryStream на массивах из пула
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

using AM.Collections;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Аналог <see cref="System.IO.MemoryStream"/>,
/// использующий массивы из системного пула.
/// Работает только на запись.
/// </summary>
public sealed class ArrayPoolWriter
    : IEnumerable<ReadOnlyMemory<byte>>,
        IDisposable
{
    #region Constants

    /// <summary>
    /// Размер массива по умолчанию.
    /// </summary>
    public const int DefaultChunkSize = 2048;

    #endregion

    #region Properties

    /// <summary>
    /// Размер чанка.
    /// </summary>
    public int ChunkSize => _chunkSize;

    /// <summary>
    /// Используемый пул.
    /// </summary>
    public ArrayPool<byte> Pool => _pool;

    /// <summary>
    /// Всего байт записано.
    /// </summary>
    public int TotalWritten => _total;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ArrayPoolWriter()
        : this (ArrayPool<byte>.Shared, DefaultChunkSize)
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="arrayPool">Пул массивов.</param>
    /// <param name="chunkSize">Размер чанка.</param>
    public ArrayPoolWriter
        (
            ArrayPool<byte> arrayPool,
            int chunkSize
        )
    {
        if (chunkSize < 16)
        {
            throw new ArgumentException ("Value too small", nameof (chunkSize));
        }

        _pool = arrayPool;
        _chunkSize = chunkSize;
        _chunks = new SinglyLinkedList<byte[]>();
    }

    #endregion

    #region Private members

    private readonly int _chunkSize;
    private readonly ArrayPool<byte> _pool;
    private readonly SinglyLinkedList<byte[]> _chunks;
    private int _position;
    private int _total;

    #endregion

    #region Public methods

    /// <summary>
    /// Запись в буфер.
    /// </summary>
    /// <param name="data">Данные для записи.</param>
    public void Write
        (
            ReadOnlySpan<byte> data
        )
    {
        if (data.IsEmpty)
        {
            return;
        }

        if (_chunks.First is null)
        {
            _chunks.Add (_pool.Rent (_chunkSize));
        }

        var remaining = data.Length;
        while (remaining > 0)
        {
            if (_position >= _chunkSize)
            {
                _chunks.Add (_pool.Rent (_chunkSize));
                _position = 0;
            }

            var piece = Math.Min (remaining, _chunkSize - _position);
            var target = _chunks.Last!.Value.AsSpan (_position, piece);

            data.Slice (0, piece).CopyTo (target);
            data = data.Slice (piece);

            remaining -= piece;
            _total += piece;
            _position += piece;
        }
    }

    /// <summary>
    /// Запись одного байта.
    /// </summary>
    /// <param name="value">Записываемый байт.</param>
    public void Write
        (
            byte value
        )
    {
        var data = MemoryMarshal.CreateSpan (ref value, 1);
        Write (data);
    }

    #endregion

    #region IEnumerable<T> members

    /// <inheritdoc cref="IEnumerable.GetEnumerator"/>
    [ExcludeFromCodeCoverage]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public IEnumerator<ReadOnlyMemory<byte>> GetEnumerator()
    {
        return new Enumerator (this);
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        foreach (var array in _chunks)
        {
            _pool.Return (array);
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return _total.ToInvariantString();
    }

    #endregion

    #region Enumerator

    /// <summary>
    /// Перечисляет записанные фрагменты.
    /// </summary>
    struct Enumerator
        : IEnumerator<ReadOnlyMemory<byte>>
    {
        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Enumerator
            (
                ArrayPoolWriter writer
            )
            : this()
        {
            _writer = writer;
        }

        #endregion

        #region Private members

        private readonly ArrayPoolWriter _writer;
        private SinglyLinkedListNode<byte[]>? _current;

        #endregion

        #region IEnumerator members

        /// <inheritdoc cref="IEnumerator.MoveNext"/>
        public bool MoveNext()
        {
            return (_current = _current is null ? _writer._chunks.First : _current.Next) is not null;
        }

        /// <inheritdoc cref="IEnumerator.Reset" />
        [ExcludeFromCodeCoverage]
        public void Reset()
        {
            _current = null;
        }

        /// <inheritdoc cref="IEnumerator.Current"/>
        [ExcludeFromCodeCoverage]
        object IEnumerator.Current => Current;

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
        }

        /// <inheritdoc cref="IEnumerator{T}.Current"/>
        public ReadOnlyMemory<byte> Current
        {
            get
            {
                if (_current is null)
                {
                    return default;
                }

                var array = _current.Value;

                return ReferenceEquals (_current, _writer._chunks.Last)
                    ? array.AsMemory (0, _writer._position)
                    : array.AsMemory();
            }
        }

        #endregion
    }

    #endregion
}
