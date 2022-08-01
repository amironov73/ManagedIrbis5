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

/* Mail.cs -- электронная почта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;

using static AM.Drawing.QRCoding.PayloadGenerator;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Электронная почта.
/// </summary>
public sealed class Mail
    : Payload
{
    #region Construction

    /// <summary>
    /// Creates an email payload with subject and message/text
    /// </summary>
    /// <param name="mailReceiver">Receiver's email address</param>
    /// <param name="subject">Subject line of the email</param>
    /// <param name="message">Message content of the email</param>
    /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
    public Mail
        (
            string? mailReceiver = null,
            string? subject = null,
            string? message = null,
            MailEncoding encoding = MailEncoding.MAILTO
        )
    {
        _mailReceiver = mailReceiver;
        _subject = subject;
        _message = message;
        _encoding = encoding;
    }

    #endregion

    #region Private members

    private readonly string? _mailReceiver, _subject, _message;
    private readonly MailEncoding _encoding;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var returnVal = string.Empty;
        switch (_encoding)
        {
            case MailEncoding.MAILTO:
                var parts = new List<string>();
                if (!string.IsNullOrEmpty (_subject))
                {
                    parts.Add ("subject=" + Uri.EscapeDataString (_subject));
                }

                if (!string.IsNullOrEmpty (_message))
                {
                    parts.Add ("body=" + Uri.EscapeDataString (_message));
                }

                var queryString = parts.Any() ? $"?{string.Join ("&", parts.ToArray())}" : "";
                returnVal = $"mailto:{_mailReceiver}{queryString}";
                break;

            case MailEncoding.MATMSG:
                returnVal = $"MATMSG:TO:{_mailReceiver};SUB:{EscapeInput (_subject)};BODY:{EscapeInput (_message)};;";
                break;

            case MailEncoding.SMTP:
                returnVal = $"SMTP:{_mailReceiver}:{EscapeInput (_subject, true)}:{EscapeInput (_message, true)}";
                break;
        }

        return returnVal;
    }

    #endregion

    /// <summary>
    /// Схема кодирования.
    /// </summary>
    public enum MailEncoding
    {
        /// <summary>
        /// Mailto:
        /// </summary>
        MAILTO,

        /// <summary>
        /// MATMSG.
        /// </summary>
        MATMSG,

        /// <summary>
        /// SMTP.
        /// </summary>
        SMTP
    }
}
