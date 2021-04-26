// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* QueryStream.cs -- общий код для SyncQuery и AsyncQuery
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Общий код для <see cref="SyncQuery"/> и <see cref="AsyncQuery"/>.
    /// </summary>
    sealed class QueryStream
        : MemoryStream
    {
        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public QueryStream()
        {
        } // constructor

        /// <summary>
        /// Конструктор с начальной емкостью.
        /// </summary>
        /// <param name="capacity">Начальная емкость буфера.</param>
        public QueryStream
            (
                int capacity
            )
            : base(capacity)
        {
        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add (int value) => AddAnsi (value.ToInvariantString());

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public void AddAnsi
            (
                string? value
            )
        {
            value ??= string.Empty;
            var converted = IrbisEncoding.Ansi.GetBytes(value);
            Write(converted);
            NewLine();
        } // method AddAnsi

        /// <summary>
        /// Добавление стандартного заголовка.
        /// </summary>
        /// <param name="connection">Данные о подключении.</param>
        /// <param name="commandCode">Код команды.</param>
        public void AddHeader
            (
                IConnectionSettings connection,
                string commandCode
            )
        {
            var header = commandCode + "\n"
                + connection.Workstation + "\n"
                + commandCode + "\n"
                + connection.ClientId.ToInvariantString() + "\n"
                + connection.QueryId.ToInvariantString() + "\n"
                + connection.Password + "\n"
                + connection.Username + "\n"
                + "\n\n";

            AddAnsi(header);
        } // method AddHeader

        /// <summary>
        /// Добавление строки в кодировке UTF-8 (плюс перевод строки).
        /// </summary>
        public void AddUtf
            (
                string? value
            )
        {
            value ??= String.Empty;
            var converted = IrbisEncoding.Utf8.GetBytes(value);
            Write(converted);
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
            foreach (var b in ToArray())
            {
                writer.Write($" {b:X2}");
            }
        } // method Debug

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf (TextWriter writer) =>
            writer.WriteLine(IrbisEncoding.Utf8.GetString(ToArray()));

        /// <summary>
        /// Получение массива фрагментов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public byte[] GetBody () => ToArray();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => unchecked((int)Length);

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => WriteByte(10);

        #endregion

    } // class QueryUtility

} // namespace ManagedIrbis.Infrastructure
