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

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Транзакция Monero.
/// </summary>
public class MoneroTransaction
    : Payload
{
    #region Construction

    /// <summary>
    /// Creates a monero transaction payload
    /// </summary>
    /// <param name="address">Receiver's monero address</param>
    /// <param name="txAmount">Amount to transfer</param>
    /// <param name="txPaymentId">Payment id</param>
    /// <param name="recipientName">Receipient's name</param>
    /// <param name="txDescription">Reference text / payment description</param>
    public MoneroTransaction
        (
            string address,
            float? txAmount = null,
            string? txPaymentId = null,
            string? recipientName = null,
            string? txDescription = null
        )
    {
        if (string.IsNullOrEmpty (address))
        {
            throw new MoneroTransactionException ("The address is mandatory and has to be set.");
        }

        _address = address;
        if (txAmount != null && txAmount <= 0)
        {
            throw new MoneroTransactionException ("Value of 'txAmount' must be greater than 0.");
        }

        _txAmount = txAmount;
        _txPaymentId = txPaymentId;
        _recipientName = recipientName;
        _txDescription = txDescription;
    }

    #endregion

    #region Private members

    private readonly string? _address, _txPaymentId, _recipientName, _txDescription;
    private readonly float? _txAmount;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var moneroUri =
            $"monero://{_address}{(!string.IsNullOrEmpty (_txPaymentId) || !string.IsNullOrEmpty (_recipientName) || !string.IsNullOrEmpty (_txDescription) || _txAmount != null ? "?" : string.Empty)}";
        moneroUri += (!string.IsNullOrEmpty (_txPaymentId)
            ? $"tx_payment_id={Uri.EscapeDataString (_txPaymentId)}&"
            : string.Empty);
        moneroUri += (!string.IsNullOrEmpty (_recipientName)
            ? $"recipient_name={Uri.EscapeDataString (_recipientName)}&"
            : string.Empty);
        moneroUri += (_txAmount != null ? $"tx_amount={_txAmount.ToString().Replace (",", ".")}&" : string.Empty);
        moneroUri += (!string.IsNullOrEmpty (_txDescription)
            ? $"tx_description={Uri.EscapeDataString (_txDescription)}"
            : string.Empty);
        return moneroUri.TrimEnd ('&');
    }

    #endregion


    public class MoneroTransactionException : Exception
    {
        public MoneroTransactionException()
        {
        }

        public MoneroTransactionException (string message)
            : base (message)
        {
        }

        public MoneroTransactionException (string message, Exception inner)
            : base (message, inner)
        {
        }
    }
}
