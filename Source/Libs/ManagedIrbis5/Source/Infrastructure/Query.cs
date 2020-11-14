// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Query.cs -- клиентский запрос к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Клиентский запрос к серверу ИРБИС64.
    /// </summary>
    public sealed class Query
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public Query
            (
                Connection connection,
                string commandCode
            )
        {
           // Sure.NotNullNorEmpty(commandCode, nameof(commandCode));

           _chunks = new List<byte[]>
           {
               _newLine // будет заменена на длину пакета
           };

           var header = commandCode + "\n"
                + connection.Workstation + "\n"
                + commandCode + "\n"
                + connection.ClientId.ToInvariantString() + "\n"
                + connection.QueryId.ToInvariantString() + "\n"
                + connection.Password + "\n"
                + connection.Username + "\n"
                + "\n\n";
            AddAnsi(header);
        }

        #endregion

        #region Private members

        private static readonly byte[] _newLine = { 10 };

        private readonly List<byte[]> _chunks;

        #endregion

        #region Public methods

        /// <summary>
        /// Add integer number.
        /// </summary>
        public Query Add
            (
                int value
            )
        {
            return AddAnsi(value.ToInvariantString());
        }

        /// <summary>
        /// Add the text in ANSI encoding.
        /// </summary>
        public Query AddAnsi
            (
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                var converted = IrbisEncoding.Ansi.GetBytes(value);
                _chunks.Add(converted);
                NewLine();
            }

            return this;
        }

        /// <summary>
        /// Add the text in UTF-8 encoding.
        /// </summary>
        public Query AddUtf
            (
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                byte[] converted = IrbisEncoding.Utf8.GetBytes(value);
                _chunks.Add(converted);
                NewLine();
            }

            return this;
        }

        /// <summary>
        /// Debug print.
        /// </summary>
        public void Debug
            (
                TextWriter writer
            )
        {
            foreach (var memory in _chunks)
            {
                foreach (var b in memory)
                {
                    writer.Write($" {b:X2}");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        public byte[][] GetChunks()
        {
            return _chunks.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        public int GetLength()
        {
            int result = 0;

            foreach (var chunk in _chunks)
            {
                result += chunk.Length;
            }

            return result;
        }

        /// <summary>
        /// Перевод строки.
        /// </summary>
        public Query NewLine()
        {
            _chunks.Add(_newLine);

            return this;
        }

        #endregion
    }
}
