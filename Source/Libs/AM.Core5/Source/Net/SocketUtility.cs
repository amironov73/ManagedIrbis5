// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
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
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#endregion

#nullable enable

namespace AM.Net
{
    /// <summary>
    ///
    /// </summary>
    public static class SocketUtility
    {
        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Resolve IPv4 address
        /// </summary>
        /// <returns>Resolved IP address of the host.</returns>
        public static IPAddress ResolveAddressIPv4
            (
                string address
            )
        {
            if (address.IsOneOf("localhost", "local", "(local)"))
            {
                return IPAddress.Loopback;
            }

            IPAddress? result = null;

            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily
                    != AddressFamily.InterNetwork)
                {
                    Magna.Error
                        (
                            "SocketUtility::ResolveAddressIPv4: "
                            + "address must be IPv4 but "
                            + result.AddressFamily
                        );

                    throw new Exception("Address must be IPv4");
                }
            }
            catch
            {
                IPHostEntry entry;

#if NETCORE || UAP

                entry = Dns.GetHostEntryAsync(address).Result;

#else

                entry = Dns.GetHostEntry(address);

#endif

                if (!ReferenceEquals(entry, null)
                    && !ReferenceEquals(entry.AddressList, null)
                    && entry.AddressList.Length != 0)
                {
                    IPAddress[] addresses = entry.AddressList
                        .Where
                        (
                            item => item.AddressFamily
                                    == AddressFamily.InterNetwork
                        )
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        Magna.Error
                            (
                                "SocketUtility::ResolveAddressIPv4: "
                                + "can't resolve IPv4 address"
                            );

                        throw new Exception("Can't resolve IPv4 address");
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                Magna.Error
                    (
                        "SocketUtility::ResolveAddressIPv4: "
                        + "can't resolve address"
                    );

                throw new ArsMagnaException("Can't resolve address");
            }

            return result;
        }

        /// <summary>
        /// Resolve IPv6 address
        /// </summary>
        /// <returns>Resolved IP address of the host.</returns>
        public static IPAddress ResolveAddressIPv6
            (
                string address
            )
        {
            if (address.IsOneOf("localhost", "local", "(local)"))
            {
                return IPAddress.IPv6Loopback;
            }

            IPAddress result = null;

            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily
                    != AddressFamily.InterNetworkV6)
                {
                    Magna.Error
                        (
                            "SocketUtility::ResolveAddressIPv6: "
                            + "address must be IPv6 but="
                            + result.AddressFamily
                        );

                    throw new Exception("Address must be IPv6");
                }
            }
            catch
            {
                IPHostEntry? entry = Dns.GetHostEntry(address);

                if (!ReferenceEquals(entry, null)
                    && !ReferenceEquals(entry.AddressList, null)
                    && entry.AddressList.Length != 0)
                {
                    IPAddress[] addresses = entry.AddressList
                        .Where
                        (
                            item => item.AddressFamily
                                    == AddressFamily.InterNetworkV6
                        )
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        Magna.Error
                            (
                                "SocketUtility::ResolveAddressIPv6: "
                                + "can't resolve IPv6 address"
                            );

                        throw new Exception("Can't resolve IPv6 address");
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                Magna.Error
                    (
                        "SocketUtility::ResolveAddressIPv6: "
                        + "can't resolve address"
                    );

                throw new ArsMagnaException("Can't resolve address");
            }

            return result;
        }

        /// <summary>
        /// Receive specified amount of data from the socket.
        /// </summary>
        public static byte[] ReceiveExact
            (
                this Socket socket,
                int dataLength
            )
        {
            using (MemoryStream result = new MemoryStream(dataLength))
            {
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
        }

        /// <summary>
        /// Read from the socket as many data as possible.
        /// </summary>
        public static byte[] ReceiveToEnd
            (
                this Socket socket
            )
        {
            using (MemoryStream stream = new MemoryStream())
            {
                return socket.ReceiveToEnd(stream);
            }
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
