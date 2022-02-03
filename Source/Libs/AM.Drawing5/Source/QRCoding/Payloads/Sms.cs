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

/* Sms.cs -- сервис коротких сообщений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Сервис коротких сообщений.
/// </summary>
public sealed class Sms
    : Payload
{
    #region Construction

    /// <summary>
    /// Creates a SMS payload without text
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="encoding">Encoding type</param>
    public Sms
        (
            string number,
            SMSEncoding encoding = SMSEncoding.SMS
        )
    {
        Sure.NotNullNorEmpty (number);

        _number = number;
        _subject = string.Empty;
        _encoding = encoding;
    }

    /// <summary>
    /// Creates a SMS payload with text (subject)
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="subject">Text of the SMS</param>
    /// <param name="encoding">Encoding type</param>
    public Sms
        (
            string number,
            string subject,
            SMSEncoding encoding = SMSEncoding.SMS
        )
    {
        Sure.NotNullNorEmpty (number);
        Sure.NotNullNorEmpty (subject);

        _number = number;
        _subject = subject;
        _encoding = encoding;
    }

    #endregion

    #region Private members

    private readonly string _number, _subject;
    private readonly SMSEncoding _encoding;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var returnVal = string.Empty;
        switch (_encoding)
        {
            case SMSEncoding.SMS:
                var queryString = string.Empty;
                if (!string.IsNullOrEmpty (_subject))
                    queryString = $"?body={Uri.EscapeDataString (_subject)}";
                returnVal = $"sms:{_number}{queryString}";
                break;

            case SMSEncoding.SMS_iOS:
                var queryStringiOS = string.Empty;
                if (!string.IsNullOrEmpty (_subject))
                    queryStringiOS = $";body={Uri.EscapeDataString (_subject)}";
                returnVal = $"sms:{_number}{queryStringiOS}";
                break;

            case SMSEncoding.SMSTO:
                returnVal = $"SMSTO:{_number}:{_subject}";
                break;
        }

        return returnVal;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public enum SMSEncoding
    {
        /// <summary>
        ///
        /// </summary>
        SMS,

        /// <summary>
        ///
        /// </summary>
        SMSTO,

        /// <summary>
        ///
        /// </summary>
        SMS_iOS
    }
}
