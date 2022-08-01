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

/* Mms.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
///
/// </summary>
public sealed class Mms
    : Payload
{
    #region Construction

    /// <summary>
    /// Creates a MMS payload without text
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="encoding">Encoding type</param>
    public Mms
        (
            string number,
            MMSEncoding encoding = MMSEncoding.MMS
        )
    {
        Sure.NotNullNorEmpty (number);

        _number = number;
        _subject = string.Empty;
        _encoding = encoding;
    }

    /// <summary>
    /// Creates a MMS payload with text (subject)
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="subject">Text of the MMS</param>
    /// <param name="encoding">Encoding type</param>
    public Mms
        (
            string number,
            string subject,
            MMSEncoding encoding = MMSEncoding.MMS
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
    private readonly MMSEncoding _encoding;

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var returnVal = string.Empty;
        switch (_encoding)
        {
            case MMSEncoding.MMSTO:
                var queryStringMmsTo = string.Empty;
                if (!string.IsNullOrEmpty (_subject))
                {
                    queryStringMmsTo = $"?subject={Uri.EscapeDataString (_subject)}";
                }

                returnVal = $"mmsto:{_number}{queryStringMmsTo}";
                break;

            case MMSEncoding.MMS:
                var queryStringMms = string.Empty;
                if (!string.IsNullOrEmpty (_subject))
                {
                    queryStringMms = $"?body={Uri.EscapeDataString (_subject)}";
                }

                returnVal = $"mms:{_number}{queryStringMms}";
                break;
        }

        return returnVal;
    }

    #endregion

    /// <summary>
    ///
    /// </summary>
    public enum MMSEncoding
    {
        /// <summary>
        ///
        /// </summary>
        MMS,

        /// <summary>
        ///
        /// </summary>
        MMSTO
    }
}
