// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* AsyncQuery.cs -- клиентский запрос к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    //
    // async-методы не могут принимать ref-параметры,
    // поэтому пришлось сделать отдельный класс
    //

    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64.
    /// </summary>
    public sealed class AsyncQuery
        : IQuery
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public AsyncQuery
            (
                IConnectionSettings connection,
                string commandCode
            )
        {
           _stream = new QueryStream(1024);
           _stream.AddHeader(connection, commandCode);
        } // constructor

        #endregion

        #region Private members

        private readonly QueryStream _stream;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add (int value) => _stream.Add(value);

        /// <summary>
        /// Добавление строки с флагом "да-нет".
        /// </summary>
        public void Add(bool value) => Add(value ? 1 : 0);

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public void AddAnsi (string? value) => _stream.AddAnsi(value);

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        public void AddUtf (string? value) => _stream.AddUtf(value);

        /// <summary>
        /// Добавление формата.
        /// </summary>
        public void AddFormat (string? format) => _stream.AddFormat(format);

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug (TextWriter writer) => _stream.Debug(writer);

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf (TextWriter writer) => _stream.DebugUtf(writer);

        /// <summary>
        /// Получение массива байтов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public ReadOnlyMemory<byte> GetBody() => _stream.GetBody();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => _stream.GetLength();

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => _stream.NewLine();

        #endregion

    } // class AsyncQuery

} // namespace ManagedIrbis.Infrastructure
