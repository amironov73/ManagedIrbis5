// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SocketUtility.cs -- работа с сокетами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;

using AM.Core.Properties;

#endregion

#nullable enable

namespace AM.Net
{
    /// <summary>
    /// Работа с сокетами.
    /// </summary>
    public static class SocketUtility
    {
        #region Public methods

        /// <summary>
        /// Пытаемся разрешить IP-адрес.
        /// </summary>
        public static IPAddress ResolveAddress
            (
                string address,
                AddressFamily expectedFamily,
                IPAddress local
            )
        {
            if (address.IsOneOf ("localhost", "local", "(local)"))
            {
                return local;
            }

            IPAddress? result;

            try
            {
                result = IPAddress.Parse (address);
                if (result.AddressFamily != expectedFamily)
                {
                    Magna.Error
                        (
                            nameof (SocketUtility) + "::" + nameof (ResolveAddress)
                            + ": expected=" + expectedFamily
                            + ", got=" + result.AddressFamily
                        );

                    throw new ArsMagnaException (Resources.CantResolveAddress);

                } // if

            } // try

            catch
            {
                var entry  = Dns.GetHostEntry (address);
                result = entry.AddressList.FirstOrDefault
                    (
                        item => item.AddressFamily == expectedFamily
                    );

            } // catch

            if (ReferenceEquals (result, null))
            {
                Magna.Error
                    (
                        nameof (SocketUtility) + "::" + nameof (ResolveAddress)
                        + Resources.CantResolveAddress2
                    );

                throw new ArsMagnaException (Resources.CantResolveAddress);

            } // if

            return result;

        } // method ResolveAddress

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
        /// Чтение из сокета строго указанного количества байтов.
        /// </summary>
        public static byte[] ReceiveExact
            (
                this Socket socket,
                int dataLength
            )
        {
            using var result = new MemoryStream (dataLength);
            var buffer = new byte [32 * 1024];

            while (dataLength > 0)
            {
                var readed = socket.Receive (buffer);

                if (readed <= 0)
                {
                    Magna.Error
                        (
                            nameof(SocketUtility)
                            + "::"
                            + nameof(ReceiveExact)
                            + Resources.ErrorReadingSocket
                        );

                    throw new ArsMagnaException (Resources.SocketReadingError);

                } // if

                result.Write(buffer, 0, readed);

                dataLength -= readed;

            } // while

            return result.ToArray();

        } // method ReceiveExact

        /// <summary>
        /// Чтение данных из сокета вплоть до его закрытия.
        /// </summary>
        public static byte[] ReceiveToEnd
            (
                this Socket socket
            )
        {
            using var stream = new MemoryStream();

            return socket.ReceiveToEnd (stream);

        } // method ReceiveToEnd

        /// <summary>
        /// Чтение данных из сокета вплоть до его закрытия.
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
                    Magna.Error
                        (
                            nameof (SocketUtility)
                            + "::"
                            + nameof(ReceiveToEnd)
                            + Resources.ErrorReadingSocket
                        );

                    throw new ArsMagnaException (Resources.SocketReadingError);

                } // if

                if (readed == 0)
                {
                    break;
                }

                stream.Write(buffer, 0, readed);

            } // while

            return stream.ToArray();

        } // method ReceiveToEnd

        #endregion

    } // class SocketUtility

} // namespace AM.Net
