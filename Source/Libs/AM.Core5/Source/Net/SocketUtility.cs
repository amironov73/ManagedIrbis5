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

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Net;

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
        Sure.NotNullNorEmpty (address);
        Sure.NotNull (local);

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
                Magna.Logger.LogError
                    (
                        nameof (SocketUtility) + "::" + nameof (ResolveAddress)
                        + ": expected={Expected}, got={Got}",
                        expectedFamily,
                        result.AddressFamily
                    );

                throw new ArsMagnaException (Resources.CantResolveAddress);
            }
        }
        catch
        {
            var entry = Dns.GetHostEntry (address);
            result = entry.AddressList.FirstOrDefault
                (
                    item => item.AddressFamily == expectedFamily
                );
        }

        if (result is null)
        {
            Magna.Logger.LogError
                (
                    nameof (SocketUtility) + "::" + nameof (ResolveAddress)
                    + ": can't resolve address"
                );

            throw new ArsMagnaException (Resources.CantResolveAddress);
        }

        return result;
    }

    /// <summary>
    /// Пытаемся разрешить IPv4-адрес.
    /// </summary>
    public static IPAddress ResolveAddressIPv4
        (
            string address
        )
    {
        Sure.NotNullNorEmpty (address);

        return ResolveAddress (address, AddressFamily.InterNetwork, IPAddress.Loopback);
    }

    /// <summary>
    /// Пытаемся разрешить IPv6-адрес.
    /// </summary>
    public static IPAddress ResolveAddressIPv6
        (
            string address
        )
    {
        Sure.NotNullNorEmpty (address);

        return ResolveAddress (address, AddressFamily.InterNetworkV6, IPAddress.IPv6Loopback);
    }

    /// <summary>
    /// Чтение из сокета строго указанного количества байтов.
    /// </summary>
    public static byte[] ReceiveExact
        (
            this Socket socket,
            int dataLength
        )
    {
        Sure.NotNull (socket);
        Sure.Positive (dataLength);

        using var result = new MemoryStream (dataLength);
        var buffer = new byte [32 * 1024];

        while (dataLength > 0)
        {
            var readed = socket.Receive (buffer);

            if (readed <= 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (SocketUtility) + "::" + nameof (ReceiveExact)
                        + ": error reading socket"
                    );

                throw new ArsMagnaException (Resources.SocketReadingError);
            }

            result.Write (buffer, 0, readed);

            dataLength -= readed;
        }

        return result.ToArray();
    }

    /// <summary>
    /// Чтение данных из сокета вплоть до его закрытия.
    /// </summary>
    public static byte[] ReceiveToEnd
        (
            this Socket socket
        )
    {
        Sure.NotNull (socket);

        using var stream = new MemoryStream();

        return socket.ReceiveToEnd (stream);
    }

    /// <summary>
    /// Чтение данных из сокета вплоть до его закрытия.
    /// </summary>
    public static byte[] ReceiveToEnd
        (
            this Socket socket,
            MemoryStream stream
        )
    {
        Sure.NotNull (socket);
        Sure.NotNull (stream);

        var buffer = new byte[32 * 1024];

        while (true)
        {
            var readed = socket.Receive (buffer);
            if (readed < 0)
            {
                Magna.Logger.LogError
                    (
                        nameof (SocketUtility) + "::" + nameof (ReceiveToEnd)
                        + ": error reading socket"
                    );

                throw new ArsMagnaException (Resources.SocketReadingError);
            }

            if (readed == 0)
            {
                break;
            }

            stream.Write (buffer, 0, readed);
        }

        return stream.ToArray();
    }

    #endregion
}
