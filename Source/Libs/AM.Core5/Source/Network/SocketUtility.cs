// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SocketUtility.cs -- работа с сокетами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

#endregion

#nullable enable

namespace AM.Network
{
    /// <summary>
    /// Работа с сокетами.
    /// </summary>
    public static class SocketUtility
    {
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
            // Sure.NotNullNorEmpty(address, nameof(address));

            // if (address.OneOf("localhost", "local", "(local)"))
            // {
            //     return IPAddress.Loopback;
            // }

            IPAddress? result = default;
            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily != AddressFamily.InterNetwork)
                {
                    // Log.Error
                    //     (
                    //         nameof(SocketUtility)
                    //         + "::"
                    //         + nameof(ResolveAddressIPv4)
                    //         + Resources.AddressMustBeIPv4ButGiven
                    //         + result.AddressFamily
                    //     );
                    //
                    // throw new ArsMagnaException(Resources.AddressMustBeIPv4);
                }
            }
            catch
            {
                var entry = Dns.GetHostEntry(address);
                if (entry.AddressList != null && entry.AddressList.Length != 0)
                {
                    var addresses = entry.AddressList
                        .Where(item => item.AddressFamily == AddressFamily.InterNetwork)
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        // Log.Error
                        //     (
                        //         nameof(SocketUtility) + "::" + nameof(ResolveAddressIPv4)
                        //         + Resources.CantResolveIPv4Address2
                        //     );
                        //
                        // throw new ArsMagnaException(Resources.CantResolveIPv4Address);
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                // Log.Error
                //     (
                //         nameof(SocketUtility) + "::" + nameof(ResolveAddressIPv4)
                //         + Resources.CantResolveAddress2
                //     );
                //
                // throw new ArsMagnaException(Resources.CantResolveAddress);
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
            // Sure.NotNullNorEmpty(address, nameof(address));

            // if (address.OneOf("localhost", "local", "(local)"))
            // {
            //     return IPAddress.IPv6Loopback;
            // }

            IPAddress? result = default;
            try
            {
                result = IPAddress.Parse(address);
                if (result.AddressFamily != AddressFamily.InterNetworkV6)
                {
                    // Log.Error
                    //     (
                    //         nameof(SocketUtility)
                    //         + "::"
                    //         + nameof(ResolveAddressIPv6)
                    //         + Resources.AddressMustBeIPv6ButGiven
                    //         + result.AddressFamily
                    //     );
                    //
                    // throw new Exception(Resources.AddressMustBeIPv6);
                }
            }
            catch
            {
                var entry = Dns.GetHostEntry(address);
                if (entry.AddressList != null && entry.AddressList.Length != 0)
                {
                    var addresses = entry.AddressList
                        .Where(item => item.AddressFamily == AddressFamily.InterNetworkV6)
                        .ToArray();

                    if (addresses.Length == 0)
                    {
                        // Log.Error
                        //     (
                        //         nameof(SocketUtility)
                        //         + "::"
                        //         + nameof(ResolveAddressIPv6)
                        //         + Resources.CantResolveIPv6Address2
                        //     );
                        //
                        // throw new ArsMagnaException(Resources.CantResolveIPv6Address);
                    }

                    result = addresses.Length == 1
                        ? addresses[0]
                        : addresses[new Random().Next(addresses.Length)];
                }
            }

            if (ReferenceEquals(result, null))
            {
                // Log.Error
                //     (
                //         nameof(SocketUtility)
                //         + "::"
                //         + nameof(ResolveAddressIPv6)
                //         + Resources.CantResolveAddress2
                //     );
                //
                // throw new ArsMagnaException(Resources.CantResolveAddress);
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
            // Sure.NonNegative(dataLength, nameof(dataLength));

            using var result = new MemoryStream(dataLength);
            var buffer = new byte[32 * 1024];
            while (dataLength > 0)
            {
                var readed = socket.Receive(buffer);

                if (readed <= 0)
                {
                    // Log.Error
                    //     (
                    //         nameof(SocketUtility)
                    //         + "::"
                    //         + nameof(ReceiveExact)
                    //         + Resources.ErrorReadingSocket
                    //     );
                    //
                    // throw new ArsMagnaException(Resources.SocketReadingError);
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
            return socket.ReceiveToEnd(stream);
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
            var buffer = new byte[32 * 1024];
            while (true)
            {
                var readed = socket.Receive(buffer);

                if (readed < 0)
                {
                    // Log.Error
                    //     (
                    //         nameof(SocketUtility)
                    //         + "::"
                    //         + nameof(ReceiveToEnd)
                    //         + Resources.ErrorReadingSocket
                    //     );
                    //
                    // throw new ArsMagnaException(Resources.SocketReadingError);
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