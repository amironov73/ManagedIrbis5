// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SocketUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

#endregion

#nullable enable

namespace AM.Net
{
    /// <summary>
    ///
    /// </summary>
    public static class SocketUtility
    {
        #region Public methods

        /// <summary>
        /// Пытаемся разрешить IP-адрес.
        /// </summary>
        /// <returns>Resolved IP address of the host.</returns>
        public static IPAddress ResolveAddress
            (
                string address,
                AddressFamily expectedFamily,
                IPAddress local
            )
        {
            if (address.IsOneOf("localhost", "local", "(local)"))
            {
                return local;
            }

            IPAddress? result;

            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily != expectedFamily)
                {
                    Magna.Error
                        (
                            nameof(SocketUtility) + "::" + nameof(ResolveAddress)
                            + ": expected=" + expectedFamily
                            + ", got=" + result.AddressFamily
                        );

                    throw new Exception("Can't resolve IP address");
                }
            }
            catch
            {
                var entry  = Dns.GetHostEntry(address);
                result = entry.AddressList.FirstOrDefault
                    (
                        item => item.AddressFamily == expectedFamily
                    );
            }

            if (ReferenceEquals(result, null))
            {
                Magna.Error
                    (
                        nameof(SocketUtility) + "::" + nameof(ResolveAddress)
                        + ": can't resolve address"
                    );

                throw new ArsMagnaException("Can't resolve address");
            }

            return result;
        }


        /// <summary>
        /// Пытаемся разрешить IPv4-адрес.
        /// </summary>
        public static IPAddress ResolveAddressIPv4 (string address) =>
            ResolveAddress (address, AddressFamily.InterNetwork, IPAddress.Loopback);

        /// <summary>
        /// Пытаемся разрешить IPv6-адрес.
        /// </summary>
        public static IPAddress ResolveAddressIPv6(string address) =>
            ResolveAddress (address, AddressFamily.InterNetworkV6, IPAddress.IPv6Loopback);

        /// <summary>
        /// Receive specified amount of data from the socket.
        /// </summary>
        public static byte[] ReceiveExact
            (
                this Socket socket,
                int dataLength
            )
        {
            using var result = new MemoryStream(dataLength);
            byte[] buffer = new byte[32 * 1024];

            while (dataLength > 0)
            {
                int readed = socket.Receive(buffer);

                if (readed <= 0)
                {
                    Magna.Error
                    (
                        "SocketUtility::ReceiveExact: "
                        + "error reading socket"
                    );

                    throw new ArsMagnaException
                    (
                        "Socket reading error"
                    );
                }

                result.Write(buffer, 0, readed);

                dataLength -= readed;
            }

            return result.ToArray();
        }

        /// <summary>
        /// Read from the socket as many data as possible.
        /// </summary>
        public static byte[] ReceiveToEnd
            (
                this Socket socket
            )
        {
            using var stream = new MemoryStream();
            return socket.ReceiveToEnd (stream);
        }

        /// <summary>
        /// Read from the socket as many data as possible.
        /// </summary>
        public static byte[] ReceiveToEnd
            (
                this Socket socket,
                MemoryStream stream
            )
        {
            byte[] buffer = new byte[32 * 1024];

            while (true)
            {
                int readed = socket.Receive(buffer);

                if (readed < 0)
                {
                    Magna.Error
                        (
                            "SocketUtility::ReceiveToEnd: "
                            + "error reading socket"
                        );

                    throw new ArsMagnaException
                        (
                            "Socket reading error"
                        );
                }

                if (readed == 0)
                {
                    break;
                }

                stream.Write(buffer, 0, readed);
            }

            return stream.ToArray();
        }


        #endregion
    }

}
