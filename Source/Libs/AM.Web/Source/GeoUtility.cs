// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* GeoUtility.cs -- полезные методы для работы с геолокацией IP-адресов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

using MaxMind.GeoIP2;

#endregion

#nullable enable

namespace AM.Web;

/// <summary>
/// Полезные методы для работы с геолокацией IP-адресов
/// </summary>
public static class GeoUtility
{
    #region Private members

    private static DatabaseReader? _databaseReader;

    private static IPAddress[]? _localAddresses;

    private static DatabaseReader _GetDatabaseReader()
    {
        _databaseReader ??= new DatabaseReader ("GeoLite2-Country.mmdb");

        return _databaseReader;
    }

    private static IPAddress[] _GetLocalAddresses()
    {
        _localAddresses ??= Dns.GetHostAddresses
            (
                Dns.GetHostName(),
                AddressFamily.InterNetwork
            );

        return _localAddresses;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, не является ли адрес местным.
    /// </summary>
    /// <remarks>
    /// Местным мы считаем: 1) loopback, 2) любой из сетевых интерфейсов
    /// хоста, 3) любой из адресов в той же подсети.
    /// </remarks>
    public static bool IsLocal
        (
            IPAddress address
        )
    {
        Sure.NotNull (address);

        // это петля?
        if (IPAddress.IsLoopback (address))
        {
            return true;
        }

        // это один из наших сетевых интерфейсов?
        var addresses = _GetLocalAddresses();
        foreach (var one in addresses)
        {
            if (one.Equals (address))
            {
                return true;
            }
        }

        // это наша подсеть?
        // TODO implement

        return false;
    }

    /// <summary>
    /// Проверка, расположен ли хост с указанным адресом в России.
    /// </summary>
    /// <returns>Если не удается определить, возвращает <c>false</c>
    /// </returns>
    public static bool IsRussian
        (
            IPAddress address
        )
    {
        Sure.NotNull (address);

        var reader = _GetDatabaseReader();
        var response = reader.Country (address);
        var country = response.Country;
        var result = country.IsoCode == "RU";

        return result;
    }

    /// <summary>
    /// Проверка, не является ли указанный адрес частным.
    /// </summary>
    /// <remarks>
    /// <para>Частные адреса IPv4:</para>
    /// <list type="bullet">
    /// <item>
    /// <term>10.0.0.0 - 10.255.255.255</term>
    /// <description>10/8 prefix</description>
    /// </item>
    /// <item>
    /// <term>172.16.0.0 - 172.31.255.255</term>
    /// <description>172.16/12 prefix</description>
    /// </item>
    /// <item>
    /// <term>192.168.0.0 - 192.168.255.255</term>
    /// <description>192.168/16 prefix</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static bool IsPrivate
        (
            IPAddress address
        )
    {
        Sure.NotNull (address);

        if (address.AddressFamily != AddressFamily.InterNetwork)
        {
            throw new ArgumentOutOfRangeException
                (
                    nameof(address),
                    "Only IPv4 supported"
                );
        }

        var parts = address.GetAddressBytes();
        var result = parts[0] == 10
            || parts[0] == 192 && parts[1] == 168
            || parts[0] == 172 && parts[1] >= 16 && parts[1] <= 31;

        return result;
    }

    #endregion
}
