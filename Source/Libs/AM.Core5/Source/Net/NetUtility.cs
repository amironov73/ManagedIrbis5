// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* NetUtility.cs -- полезные методы для работы с локальной сетью
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

#endregion

#nullable enable

namespace AM.Net;

/// <summary>
/// Полезные методы для работы с локальной сетью.
/// </summary>
public static class NetUtility
{
    #region Public methods

    /// <summary>
    /// Возвращает диапазоны адресов локальной сети.
    /// </summary>
    public static IPRange[] GetLocalNetwork()
    {
        var result = new List<IPRange>();
        foreach (var adapter in NetworkInterface.GetAllNetworkInterfaces())
        {
            foreach (var unicast in adapter.GetIPProperties().UnicastAddresses)
            {
                if (unicast.Address.AddressFamily == AddressFamily.InterNetwork)
                {
                    var addressBytes = unicast.Address.GetAddressBytes();
                    var bitMask = unicast.IPv4Mask.GetAddressBytes();
                    addressBytes = Bits.And (addressBytes, bitMask);
                    var begin = new IPAddress (addressBytes);
                    var end = new IPAddress (Bits.Or (addressBytes, Bits.Not (bitMask)));
                    var range = new IPRange (begin, end);

                    var flag = true;
                    foreach (var already in result)
                    {
                        if (already.Contains (range))
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (flag)
                    {
                        result.Add (range);
                    }
                }
            }
        }

        return result.ToArray();
    }

    #endregion
}
