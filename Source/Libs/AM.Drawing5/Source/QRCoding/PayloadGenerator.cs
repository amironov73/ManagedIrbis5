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

/* PayloadGenerator.cs -- генератор полезной нагрузки
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Drawing.QRCoding;

/// <summary>
/// Генератор полезной нагрузки.
/// </summary>
public static class PayloadGenerator
{
    #region Internal members

    internal static bool IsValidIban
        (
            string iban
        )
    {
        //Clean IBAN
        var ibanCleared = iban.ToUpper().Replace (" ", "").Replace ("-", "");

        //Check for general structure
        var structurallyValid = Regex.IsMatch (ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

        //Check IBAN checksum
        var sum = $"{ibanCleared.Substring (4)}{ibanCleared.Substring (0, 4)}".ToCharArray().Aggregate ("",
            (current, c) => current + (char.IsLetter (c) ? (c - 55).ToString() : c.ToString()));
        var m = 0;
        for (var i = 0; i < (int)Math.Ceiling ((sum.Length - 2) / 7d); i++)
        {
            var offset = (i == 0 ? 0 : 2);
            var start = i * 7 + offset;
            var n = (i == 0 ? "" : m.ToString()) + sum.Substring (start, Math.Min (9 - offset, sum.Length - start));
            if (!int.TryParse (n, NumberStyles.Any, CultureInfo.InvariantCulture, out m))
                break;
            m = m % 97;
        }

        var checksumValid = m == 1;
        return structurallyValid && checksumValid;
    }

    internal static bool IsValidQRIban
        (
            string iban
        )
    {
        var foundQrIid = false;
        try
        {
            var ibanCleared = iban.ToUpper().Replace (" ", "").Replace ("-", "");
            var possibleQrIid = Convert.ToInt32 (ibanCleared.Substring (4, 5));
            foundQrIid = possibleQrIid is >= 30000 and <= 31999;
        }
        catch
        {
            Magna.Logger.LogDebug (nameof (IsValidQRIban));
        }

        return IsValidIban (iban) && foundQrIid;
    }

    internal static bool IsValidBic
        (
            string bic
        )
    {
        return Regex.IsMatch (bic.Replace (" ", ""), @"^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
    }


    internal static string ConvertStringToEncoding
        (
            string message,
            string encoding
        )
    {
        var iso = Encoding.GetEncoding (encoding);
        var utf8 = Encoding.UTF8;
        var utfBytes = utf8.GetBytes (message);
        var isoBytes = Encoding.Convert (utf8, iso, utfBytes);
        return iso.GetString (isoBytes, 0, isoBytes.Length);
    }

    internal static string EscapeInput
        (
            string? input,
            bool simple = false
        )
    {
        if (string.IsNullOrEmpty (input))
        {
            return string.Empty;
        }

        char[] forbiddenChars = { '\\', ';', ',', ':' };
        if (simple)
        {
            forbiddenChars = new[] { ':' };
        }

        foreach (var c in forbiddenChars)
        {
            input = input.Replace (c.ToString(), "\\" + c);
        }

        return input;
    }

    internal static bool IsHexStyle
        (
            string input
        )
    {
        return (Regex.IsMatch (input, @"\A\b[0-9a-fA-F]+\b\Z") || Regex.IsMatch (input, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z"));
    }

    internal static bool ChecksumMod10
        (
            string? digits
        )
    {
        if (string.IsNullOrEmpty (digits) || digits.Length < 2)
        {
            return false;
        }

        var mods = new[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

        var remainder = 0;
        for (var i = 0; i < digits.Length - 1; i++)
        {
            var num = Convert.ToInt32 (digits[i]) - 48;
            remainder = mods[(num + remainder) % 10];
        }

        var checksum = (10 - remainder) % 10;
        return checksum == Convert.ToInt32 (digits[^1]) - 48;
    }

    #endregion
}
