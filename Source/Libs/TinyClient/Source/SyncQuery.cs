// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SyncQuery.cs -- клиентский запрос
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64
    /// (для синхронного сценария).
    /// </summary>
    public readonly struct SyncQuery
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SyncQuery
            (
                SyncConnection connection,
                string commandCode
            )
            : this()
        {
            _writer = new MemoryStream (2048);

            // Заголовок запроса
            AddAnsi (commandCode);
            AddAnsi (connection.Workstation);
            AddAnsi (commandCode);
            Add (connection.ClientId);
            Add (connection.QueryId);
            AddAnsi (connection.Password);
            AddAnsi (connection.Username);
            NewLine();
            NewLine();
            NewLine();
        }

        #endregion

        #region Private members

        private readonly MemoryStream _writer;

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строки с целым числом (плюс перевод строки).
        /// </summary>
        public void Add
            (
                int value
            )
        {
            var buffer = new byte[12];
            var length = Private.Int32ToBytes (value, buffer);
            _writer.Write (buffer, 0, length);
            NewLine();
        }

        /// <summary>
        /// Добавление строки с флагом "да-нет".
        /// </summary>
        public void Add (bool value) => Add (value ? 1 : 0);

        /// <summary>
        /// Добавление строки в кодировке ANSI (плюс перевод строки).
        /// </summary>
        public void AddAnsi
            (
                string? value
            )
        {
            if (value is not null)
            {
                var bytes = Utility.Ansi.GetBytes (value);
                _writer.Write (bytes, 0, bytes.Length);
            }

            NewLine();
        }

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
                var bytes = Encoding.UTF8.GetBytes (value);
                _writer.Write (bytes, 0, bytes.Length);
            }

            NewLine();
        }

        /// <summary>
        /// Добавление формата.
        /// </summary>
        public void AddFormat
            (
                string? format
            )
        {
            if (ReferenceEquals (format, null) || format.Length == 0)
            {
                NewLine();
            }
            else
            {
                format = format.Trim();
                if (string.IsNullOrEmpty (format))
                {
                    NewLine();
                }
                else
                {
                    if (format.StartsWith ("@"))
                    {
                        AddAnsi (format);
                    }
                    else
                    {
                        var prepared = Private.PrepareFormat (format);
                        AddUtf ("!" + prepared);
                    }
                }
            }
        }

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void Debug
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            var span = GetBody();
            foreach (var b in span)
            {
                writer.Write ($" {b:X2}");
            }
        }

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugAnsi
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            writer.WriteLine (Utility.Ansi.GetString (_writer.ToArray()));
        }

        /// <summary>
        /// Отладочная печать.
        /// </summary>
        public void DebugUtf
            (
                TextWriter? writer = null
            )
        {
            writer ??= Console.Out;

            writer.WriteLine (Encoding.UTF8.GetString (_writer.ToArray()));
        }

        /// <summary>
        /// Получение массива байтов, из которых состоит
        /// клиентский запрос.
        /// </summary>
        public byte[] GetBody() => _writer.ToArray();

        /// <summary>
        /// Подсчет общей длины запроса (в байтах).
        /// </summary>
        public int GetLength() => unchecked ((int)_writer.Length);

        /// <summary>
        /// Добавление одного перевода строки.
        /// </summary>
        public void NewLine() => _writer.WriteByte (10);

        #endregion
    }
}
