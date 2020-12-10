// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ValueQuery.cs -- клиентский запрос к серверу ИРБИС64 (для синхронного сценария)
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
    /// Клиентский запрос к серверу ИРБИС64
    /// (для синхронного сценария).
    /// </summary>
    public readonly ref struct ValueQuery
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueQuery
            (
                Connection connection,
                string commandCode
            )
            : this()
        {
            // Sure.NotNullNorEmpty(commandCode, nameof(commandCode));

            _stream = new MemoryStream(1024);

            var header = commandCode + "\n"
                 + connection.Workstation + "\n"
                 + commandCode + "\n"
                 + connection.ClientId.ToInvariantString() + "\n"
                 + connection.QueryId.ToInvariantString() + "\n"
                 + connection.Password + "\n"
                 + connection.Username + "\n"
                 + "\n\n";
            AddAnsi(header);
        } // constructor

        #endregion

        #region Private members

        private readonly MemoryStream _stream;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add(int value) => AddAnsi(value.ToInvariantString());
        // method Add

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
            _stream.Write(converted);
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
            value ??= String.Empty;
            var converted = IrbisEncoding.Utf8.GetBytes(value);
            _stream.Write(converted);
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
            foreach (var b in _stream.ToArray())
            {
                writer.Write($" {b:X2}");
            }
        } // method AddDebug

        /// <summary>
        /// Получение массива фрагментов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public byte[] GetBody() => _stream.ToArray();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => (int)_stream.Length;

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine()
        {
            _stream.WriteByte(10);
        }

        #endregion
    }
}
