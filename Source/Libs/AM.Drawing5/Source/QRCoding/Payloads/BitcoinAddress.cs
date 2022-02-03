// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* BitcoinAddress.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Адрес Bitcoin.
/// </summary>
public sealed class BitcoinAddress
    : BitcoinLikeCryptoCurrencyAddress
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BitcoinAddress
        (
            string address,
            double? amount,
            string? label = null,
            string? message = null
        )
        : base (BitcoinLikeCryptoCurrencyType.Bitcoin, address, amount, label, message)
    {
    }

    #endregion
}
