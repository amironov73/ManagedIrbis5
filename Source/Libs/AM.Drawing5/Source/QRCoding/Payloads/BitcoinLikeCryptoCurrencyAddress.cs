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

/* BitcoinLikeCryptoCurrencyAddress.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Адрес криптовалюты.
/// </summary>
public class BitcoinLikeCryptoCurrencyAddress
    : Payload
{
    #region Construction

    /// <summary>
    /// Generates a Bitcoin like cryptocurrency payment payload. QR Codes with this payload can open a payment app.
    /// </summary>
    /// <param name="currencyType">Bitcoin like cryptocurrency address of the payment receiver</param>
    /// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
    /// <param name="amount">Amount of coins to transfer</param>
    /// <param name="label">Reference label</param>
    /// <param name="message">Referece text aka message</param>
    public BitcoinLikeCryptoCurrencyAddress
        (
            BitcoinLikeCryptoCurrencyType currencyType,
            string address,
            double? amount,
            string? label = null,
            string? message = null
        )
    {
        _currencyType = currencyType;
        _address = address;

        if (!string.IsNullOrEmpty (label))
        {
            _label = Uri.EscapeDataString (label);
        }

        if (!string.IsNullOrEmpty (message))
        {
            _message = Uri.EscapeDataString (message);
        }

        _amount = amount;
    }

    #endregion

    #region Private members

    private readonly BitcoinLikeCryptoCurrencyType _currencyType;
    private readonly string? _address, _label, _message;
    private readonly double? _amount;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        string? query = null;

        var queryValues = new KeyValuePair<string, string?>[]
        {
            new (nameof (_label), _label),
            new (nameof (_message), _message),
            new (nameof (_amount),
                _amount.HasValue ? _amount.Value.ToString ("#.########", CultureInfo.InvariantCulture) : null)
        };

        if (queryValues.Any (keyPair => !string.IsNullOrEmpty (keyPair.Value)))
        {
            query = "?" + string.Join ("&", queryValues
                .Where (keyPair => !string.IsNullOrEmpty (keyPair.Value))
                .Select (keyPair => $"{keyPair.Key}={keyPair.Value}")
                .ToArray());
        }

        return $"{Enum.GetName (typeof (BitcoinLikeCryptoCurrencyType), _currencyType).ToLower()}:{_address}{query}";
    }

    #endregion

    /// <summary>
    /// Криптовалюты.
    /// </summary>
    public enum BitcoinLikeCryptoCurrencyType
    {
        /// <summary>
        /// Bitcoin.
        /// </summary>
        Bitcoin,

        /// <summary>
        /// BitcoinCash.
        /// </summary>
        BitcoinCash,

        /// <summary>
        /// Litecoin.
        /// </summary>
        Litecoin
    }
}
