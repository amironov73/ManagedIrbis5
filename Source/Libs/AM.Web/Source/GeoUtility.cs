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
using System.Net;
using System.Net.Sockets;

using AM.Net;

using MaxMind.GeoIP2;

#endregion

#nullable enable

namespace AM.Web;

/*
    Марсианский пакет — это IP-пакет, видимый в общедоступном Интернете,
    который содержит адрес источника или пункта назначения,
    зарезервированный для специального использования Администрацией
    адресного пространства Интернета (IANA). В общедоступном Интернете
    такой пакет либо имеет поддельный адрес источника, и он фактически
    не может быть создан как заявлено, либо пакет не может быть доставлен.
    Требование сделать это содержится в RFC 1812, раздел 5.2.3.

    Марсианские пакеты обычно возникают из-за подмены IP-адресов
    при атаках типа «отказ в обслуживании», но также могут возникать
    из-за сбоя сетевого оборудования или неправильной конфигурации хоста.

    В терминологии Linux марсианский пакет — это IP-пакет, полученный
    ядром через определённый интерфейс, а таблицы маршрутизации указывают,
    что исходный IP-адрес ожидается на другом интерфейсе.

    Как в IPv4, так и в IPv6 марсианские пакеты имеют адреса отправителя
    или получателя в специальных диапазонах, определённых в RFC 6890.
 */

/// <summary>
/// Полезные методы для работы с геолокацией IP-адресов
/// </summary>
public static class GeoUtility
{
    #region Private members

    private static DatabaseReader? _databaseReader;

    private static IPRange[]? _localAddresses;

    private static DatabaseReader _GetDatabaseReader()
    {
        _databaseReader ??= new DatabaseReader ("GeoLite2-Country.mmdb");

        return _databaseReader;
    }

    private static IPRange[] _GetLocalAddresses()
    {
        _localAddresses ??= NetUtility.GetLocalNetwork();

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

        // это наша подсеть?
        var addresses = _GetLocalAddresses();
        foreach (var one in addresses)
        {
            if (one.Contains (address))
            {
                return true;
            }
        }

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
    /// Немаршрутизируемый адрес?
    /// </summary>
    public static bool IsBogonNetwork
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

        var bytes = address.GetAddressBytes();

        if (bytes[0] == 0)
        {
            // 0.0.0.0/8
            // Диапазон описан в RFC1122 , RFC3330 и RFC1700 как
            // "Этот хост в этой сети" (this host on this network),
            // хотя, учитывая варианты применения, правильнее было бы
            // назвать его как "любой адрес".
            //
            // В частности, IP-адрес 0.0.0.0 используется для:
            //
            // - обозначения в конфигурационных файлах серверов
            //   и выводе netstat информации о том, что определенный
            //   сервис "слушает" запросы на всех IP-адресах данного сервера;
            //
            // - конфигурации маршрута по умолчанию на активном
            //   сетевом оборудовании;
            //
            // - использования в качестве src address в запросах
            //   на получение IP-адреса (DHCPDISCOVER);
            //
            // - обозначения IP-адреса в суммаризованных событиях
            //   безопасности IDS/IPS/WAF/etc (например,
            //   TCP Host Sweep - обозначение dst host в случае
            //   инициации коннектов к большому количеству IP-адресов).
            //
            // Подробнее: https://www.securitylab.ru/blog/personal/aodugin/305208.php

            return true;
        }

        if (bytes[0] >= 240)
        {
            // 240.0.0.0/4
            // В соответствии с RFC1122 , данный диапазон IP-адресов,
            // исторически также известный как Class E, зарезервирован
            // под использование в будущем. Юмор ситуации в том,
            // что RFC1122 издавался еще в августе 1989 года,
            // сейчас IPv4-адреса закончились, но для IETF будущее еще
            // не наступило, потому что из всей большой подсети /4
            // до сих пор используется только один адрес. Но, наверное,
            // если посчитать статистику по всем подсетям всех организаций
            // мира, этот адрес окажется в лидерах, потому что сервисы,
            // использующие broadcast, обращаются к адресу 255.255.255.255,
            // который и принадлежит описанному диапазону.

            return true;
        }

        if (bytes[0] >= 224)
        {
            // 224.0.0.0/4
            // Этот диапазон в исторической классификации еще называется
            // "Class D". Выделен под Multicast, уточнение специфики
            // работы которого тоже вроде как отдельная заметка.
            // В RFC5771 подробно расписано использование подсетей
            // внутри блока, но суть остается той же: эти адреса
            // не закреплены ни за каким провайдером, и, соответственно,
            // через Интернет не должны светиться.

            return true;
        }

        if (bytes[0] == 169 && bytes[1] == 254)
        {
            // 169.254.0.0/16
            // В соответствии с RFC3927 , определен как Link-Local
            // для автоматической конфигурации. Думаю, каждый человек
            // хоть раз в жизни, но успел столкнуться с ситуацией,
            // когда ПК, не получив IP-адрес от DHCP-сервера, присваивает
            // сам себе непонятный и нигде не прописанный ранее IP,
            // начинающийся на 169.254... Это и есть реализация рекомендаций
            // из RFC3927.

            return true;
        }

        if (bytes[0] == 192 && bytes[1] == 0 && bytes[2] == 2)
        {
            // 192.0.2.0/8
            // адреса TEST-NET

            return true;
        }

        return false;
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
