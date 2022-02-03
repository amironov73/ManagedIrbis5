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

/* BitcoinCashAddress.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
public sealed class BitcoinCashAddress
    : BitcoinLikeCryptoCurrencyAddress
{
    #region Constructions

    /// <summary>
    /// Конструктор.
    /// </summary>
    public BitcoinCashAddress
        (
            string address,
            double? amount,
            string? label = null,
            string? message = null
        )
        : base (BitcoinLikeCryptoCurrencyType.BitcoinCash, address, amount, label, message)
    {
    }

    #endregion
}
