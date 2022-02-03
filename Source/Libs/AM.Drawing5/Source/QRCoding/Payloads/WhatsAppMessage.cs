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

/* WhatsAppMessage.cs -- сообщение WhatsApp
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text.RegularExpressions;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Сообщение WhatsApp.
/// </summary>
public class WhatsAppMessage
    : Payload
{
    #region Construction

    /// <summary>
    /// Let's you compose a WhatApp message and send it the receiver number.
    /// </summary>
    /// <param name="number">Receiver phone number where the <paramref name="number"/>
    /// is a full phone number in international format.
    /// Omit any zeroes, brackets, or dashes when adding the phone number in international format.
    /// Use: 1XXXXXXXXXX | Don't use: +001-(XXX)XXXXXXX
    /// </param>
    /// <param name="message">The message</param>
    public WhatsAppMessage
        (
            string number,
            string message
        )
    {
        Sure.NotNull (number);
        Sure.NotNullNorEmpty (message);

        _number = number;
        _message = message;
    }

    /// <summary>
    /// Let's you compose a WhatApp message. When scanned the user
    /// is asked to choose a contact who will receive the message.
    /// </summary>
    /// <param name="message">The message</param>
    public WhatsAppMessage
        (
            string message
        )
    {
        Sure.NotNullNorEmpty (message);

        _number = string.Empty;
        _message = message;
    }

    #endregion

    #region Private members

    private readonly string _number, _message;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var cleanedPhone = Regex.Replace (_number, @"^[0+]+|[ ()-]", string.Empty);
        return ($"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString (_message)}");
    }

    #endregion
}
