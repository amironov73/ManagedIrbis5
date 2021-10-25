// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* HttpServerSocket.cs -- серверный сокет для HTTP
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using AM;
using AM.Globalization;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server.Sockets
{
    /// <summary>
    /// Серверный сокет для HTTP.
    /// </summary>
    public sealed class HttpServerSocket
        : Tcp4ServerSocket
    {
        #region Properties

        /// <summary>
        /// Два перевода строки. Означают начало данных.
        /// </summary>
        public static byte[] TwoNewLines = { 0x0D, 0x0A, 0x0D, 0x0A };

        /// <summary>
        /// Строка "IRBIS_END_REQUEST". Означает конец данных.
        /// </summary>
        public static byte[] EndRequest = { 0x49, 0x52, 0x42, 0x49,
            0x53, 0x5F, 0x45, 0x4E, 0x44, 0x5F, 0x52, 0x45, 0x51,
            0x55, 0x45, 0x53, 0x54 };

        #endregion

        #region Construction

        /// <summary>
        /// Construction.
        /// </summary>
        public HttpServerSocket
            (
                TcpClient client,
                CancellationToken token
            )
            : base (client, token)
        {
        }

        #endregion

        #region IrbisServerSocket members

        /// <inheritdoc cref="IAsyncServerSocket.ReceiveAllAsync" />
        public override async Task<MemoryStream?> ReceiveAllAsync()
        {
            //
            // Типичный запрос
            //
            // POST /cgi HTTP/1.1
            // User-Agent: GPNTB/Irbis64
            // Host: 127.0.0.1:6666
            // Accept: *.*
            // Content-length: 66
            //
            // A
            // C
            // A
            // 678232
            // 1
            // password
            // login
            //
            //
            //
            // login
            // password
            // IRBIS_END_REQUEST
            //

            var received = await base.ReceiveAllAsync();
            if (received is null)
            {
                return received;
            }

            var bytes = received.ToArray();
            var startData = ArrayUtility.IndexOf (bytes, TwoNewLines);
            var endData = ArrayUtility.IndexOf (bytes, EndRequest);
            if (startData < 0 || endData < 0)
            {
                throw new IrbisException();
            }

            startData += TwoNewLines.Length;
            var dataLength = endData - startData;
            if (bytes[endData - 2] == 0x0D && bytes[endData - 1] == 0x0A)
            {
                // Убираем лишний перевод строки
                dataLength -= 2;
            }

            bytes = bytes.GetSlice (startData, dataLength);
            var prefixString = dataLength.ToInvariantString() + "\r\n";
            var result = new MemoryStream (dataLength + prefixString.Length);
            var prefix = IrbisEncoding.Ansi.GetBytes (prefixString);
            result.Write (prefix, 0, prefix.Length);
            result.Write (bytes, 0, bytes.Length);
            result.Position = 0;

            return result;

        } // method ReceiveAllAsync

        /// <inheritdoc cref="IAsyncServerSocket.SendAsync" />
        public override Task<bool> SendAsync
            (
                IEnumerable<ReadOnlyMemory<byte>> data
            )
        {
            var dataLength = 0;
            var chunks = data.ToList();
            foreach (var bytes in chunks)
            {
                dataLength += bytes.Length;
            }

            var culture = BuiltinCultures.AmericanEnglish;
            var httpHeaders = "HTTP/1.1 200 OK\r\n"
                + "Date: " + DateTime.Now.ToString ("F", culture) + "\r\n"
                + "Connection: close\r\n"
                + "Server: IRBIS64\r\n"
                + "Content-Type: application/octet-stream\r\n"
                + "Content-Length: " + dataLength.ToInvariantString()
                + "\r\n\r\n";

            chunks.Insert (0, IrbisEncoding.Ansi.GetBytes (httpHeaders));

            return base.SendAsync (chunks);

        } // method SendAsync

        #endregion

    } // class HttpServerSocket

} // namespace ManagedIrbis.Server.Sockets
