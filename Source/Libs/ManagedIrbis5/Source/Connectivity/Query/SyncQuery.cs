// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable ReplaceSliceWithRangeIndexer
// ReSharper disable UnusedMember.Global

/* SyncQuery.cs -- клиентский запрос к серверу ИРБИС64 (для синхронного сценария)
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

using Microsoft.Toolkit.HighPerformance;
using Microsoft.Toolkit.HighPerformance.Buffers;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    //
    // В синхронном сценарии у нас есть возможность
    // использовать структуру, чтобы избежать трафика памяти
    //
    // Может показаться, что мы допускаем ошибку, т. к.
    // в принимающем методе будет использоваться копия структуры,
    // но на самом деле все ОК, т. к. единственное поле
    // нашей структуры - указатель на поток, в который
    // и происходит добавление данных. Так что все добавленные
    // данные разделяются между всеми копиями структуры.
    //
    // Оверхеда на копирование нет, т. к. копирование этой
    // структуры равноценно передаче простого указателя
    // на поток.
    //
    // Единственная беда -- вынужденное дублирование кода.
    // Его, насколько смог, победил с помощью класса
    // QueryStream.
    //

    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64
    /// (для синхронного сценария).
    /// </summary>
    public readonly struct SyncQuery
        : IQuery,
        IDisposable
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncQuery
            (
                IConnectionSettings connection,
                string commandCode
            )
            : this()
        {
            _writer = new ArrayPoolBufferWriter<byte>(1024);

            // Заголовок запроса
            AddAnsi(commandCode);
            AddAnsi(connection.Workstation);
            AddAnsi(commandCode);
            Add(connection.ClientId);
            Add(connection.QueryId);
            AddAnsi(connection.Password);
            AddAnsi(connection.Username);
            NewLine();
            NewLine();
            NewLine();
        } // constructor

        #endregion

        #region Private members

        private readonly ArrayPoolBufferWriter<byte> _writer;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public unsafe void Add
            (
                int value
            )
        {
            Span<byte> span = stackalloc byte[12];
            var length = FastNumber.Int32ToBytes(value, span);
            _writer.Write((ReadOnlySpan<byte>) span.Slice(0, length));
            NewLine();
        } // method Add

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public unsafe void AddAnsi
            (
                string? value
            )
        {
            if (value is not null)
            {
                var length = value.Length;
                Span<byte> span = length < 2048
                    ? stackalloc byte[length]
                    : new byte[length];
                length = IrbisEncoding.Ansi.GetBytes(value, span);
                _writer.Write((ReadOnlySpan<byte>) span.Slice(0, length));
            }

            NewLine();
        } // method AddAnsi

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        public void AddUtf
            (
                string? value
            )
        {
            if (value is not null)
            {
                var utf = IrbisEncoding.Utf8;
                var length = utf.GetByteCount(value);
                Span<byte> span = length < 2048
                    ? stackalloc byte[length]
                    : new byte[length];
                length = IrbisEncoding.Ansi.GetBytes(value, span);
                _writer.Write((ReadOnlySpan<byte>) span.Slice(0, length));
            }
            NewLine();
        } // method AddUtf

        /// <summary>
        /// Добавление формата.
        /// </summary>
        public void AddFormat
            (
                string? format
            )
        {
            if (string.IsNullOrEmpty(format))
            {
                NewLine();
            }
            else
            {
                AddAnsi(format);
            }

        } // method AddFormat

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug
            (
                TextWriter writer
            )
        {
            var span = GetBody().Span;
            foreach (var b in span)
            {
                writer.Write($" {b:X2}");
            }
        } // method Debug

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf
            (
                TextWriter writer
            )
        {
            writer.WriteLine (IrbisEncoding.Utf8.GetString (_writer.WrittenSpan));
        } // method DebugUtf

        /// <summary>
        /// Получение массива байтов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public ReadOnlyMemory<byte> GetBody() => _writer.WrittenMemory;

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => _writer.WrittenCount;

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => _writer.Write((byte) 10);

        #endregion

        #region IDisposable members

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() => _writer.Dispose();

        #endregion

    } // struct SyncQuery

} // namespace ManagedIrbis.Infrastructure
