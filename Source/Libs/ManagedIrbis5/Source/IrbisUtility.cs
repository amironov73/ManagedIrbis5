// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisUtility.cs -- различные вспомогательные методы для подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Text;

using AM;
using AM.Collections;
using AM.IO;
using AM.Security;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis;

/// <summary>
/// Различные вспомогательные методы, пригождающиеся
/// при работе к сервером ИРБИС64.
/// </summary>
public static class IrbisUtility
{
    #region Properties

    /// <summary>
    /// Общеупотребимые рабочие листы.
    /// </summary>
    public static string[] WellKnownWorksheets =
    {
        "ASP",   // описание статьи из сборника/журнала/газеты
        "AUNTD", // аналитическое описание юридического документа или НТД
        "IBIS",  // упрощенное библиографическое описание книги
        "MUSP",  // описание музейного предмета
        "OJ",    // сводное описание журнала
        "PAZK",  // описание книги под автором, заглавием или коллективом
        "PRF",   // проверка фонда
        "PVK",   // описание книги под временным коллективом (труды конференций и т. п.)
        "SPEC"   // описание спецификации тома
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Кодирование строки подключения.
    /// </summary>
    /// <param name="plainText">Строка подключения.</param>
    /// <param name="password">Пароль. Если <c>null</c>,
    /// то используется пароль по умолчанию.</param>
    /// <returns>Зашифрованная строка подключения.</returns>
    public static string Encrypt
        (
            string plainText,
            string? password = null
        )
    {
        // Пустая строка зашифрованной быть не может
        if (string.IsNullOrEmpty (plainText))
        {
            return plainText;
        }

        if (plainText[0] == '!')
        {
            // строка уже в Base64
            return plainText;
        }

        if (plainText[0] == '?')
        {
            // строка уже зашифрована
            return plainText;
        }

        if (string.IsNullOrEmpty (password))
        {
            password = "irbis";
        }

        return "?" + SecurityUtility.Encrypt (plainText, password);
    }

    /// <summary>
    /// Декодирование строки подключения, если она закодирована или зашифрована.
    /// </summary>
    /// <param name="possiblyEncrypted">Строка подключения, возможно, закодированная
    /// или зашифрованная.</param>
    /// <param name="password">Пароль. Если <c>null</c>,
    /// то используется пароль по умолчанию.</param>
    /// <returns>Расшифрованная строка подключения.</returns>
    public static string Decrypt
        (
            string possiblyEncrypted,
            string? password = null
        )
    {
        // Пустая строка зашифрованной быть не может
        if (string.IsNullOrEmpty (possiblyEncrypted))
        {
            return possiblyEncrypted;
        }

        // С восклицательного знака начинается строка,
        // закодированная в банальный Base64.
        if (possiblyEncrypted[0] == '!')
        {
            var enc = possiblyEncrypted.Substring (1);
            var res = SecurityUtility.DecryptFromBase64 (enc);

            return res;
        }

        // Зашифрованная строка должна начинаться со знака вопроса.
        if (possiblyEncrypted[0] != '?')
        {
            return possiblyEncrypted;
        }

        var encrypted = possiblyEncrypted.Substring (1);
        if (string.IsNullOrEmpty (password))
        {
            password = "irbis";
        }

        var result = SecurityUtility.Decrypt (encrypted, password);

        return result;
    }

    /// <summary>
    /// Кодирование произвольного массива байт
    /// в строку вида %01%23%45.
    /// Такое кодирование позволяет хранить в записи,
    /// например, небольшие JPEG-файлы.
    /// </summary>
    public static string? EncodePercentString
        (
            byte[]? array
        )
    {
        if (array.IsNullOrEmpty())
        {
            return null;
        }

        var estimatedLength = array.Length;
        foreach (var b in array)
        {
            if ((b < 'A' || b > 'Z') && (b < 'a' || b > 'z') && (b < '0' || b > '9'))
            {
                estimatedLength += 2;
            }
        }

        var builder = StringBuilderPool.Shared.Get();
        builder.EnsureCapacity (estimatedLength);
        foreach (var b in array)
        {
            if (b >= 'A' && b <= 'Z'
                || b >= 'a' && b <= 'z'
                || b >= '0' && b <= '9'
               )
            {
                builder.Append ((char)b);
            }
            else
            {
                builder.Append ($"%{b:X2}");
            }
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Декодирование строки вида %01%23%44.
    /// Такие строки используются ИРБИС64 для хранения в записях,
    /// например, обложек книг (по факту представляющих собой
    /// небольшие JPEG-файлы).
    /// </summary>
    public static byte[] DecodePercentString
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return Array.Empty<byte>();
        }

        var predictedLength = text.Length / 2;
        using var stream = MemoryCenter.GetMemoryStream (predictedLength);
        for (var i = 0; i < text.Length; i++)
        {
            var c = text[i];
            if (c != '%')
            {
                stream.WriteByte ((byte)c);
            }
            else
            {
                if (i >= text.Length - 2)
                {
                    Magna.Logger.LogError
                        (
                            nameof (IrbisUtility) + "::" + nameof (DecodePercentString)
                            + ": unexpected end of stream"
                        );

                    throw new FormatException (nameof (text));
                }

                var b = byte.Parse
                    (
                        text.Substring (i + 1, 2),
                        NumberStyles.HexNumber
                    );
                stream.WriteByte (b);
                i += 2;
            }
        }

        return stream.ToArray();
    }

    /// <summary>
    /// Декодирование строки.
    /// </summary>
    public static string? UrlDecode
        (
            string? text,
            Encoding encoding
        )
    {
        int _HexToInt (char h) => h is >= '0' and <= '9'
            ? h - '0'
            : h is >= 'a' and <= 'f'
                ? h - 'a' + 10
                : h is >= 'A' and <= 'F'
                    ? h - 'A' + 10
                    : -1;

        Sure.NotNull (encoding);

        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var bytes = new List<byte>();
        var count = text.Length;
        for (var pos = 0; pos < count; pos++)
        {
            var ch = text[pos];
            if (ch == '+')
            {
                ch = ' ';
            }
            else if (ch == '%' && pos < count - 2)
            {
                var h1 = _HexToInt (text[pos + 1]);
                var h2 = _HexToInt (text[pos + 2]);

                if (h1 >= 0 && h2 >= 0)
                {
                    var b = (byte)((h1 << 4) | h2);
                    pos += 2;

                    bytes.Add (b);
                    continue;
                }
            }

            bytes.Add ((byte)ch);
        }

        var result = encoding.GetString (bytes.ToArray());

        return result;
    }

    /// <summary>
    /// Текущий рабочий лист соответствует сводной записи журнала?
    /// </summary>
    [Pure]
    public static bool IsMagazineSummary
        (
            string? worksheet
        )
    {
        return worksheet.SameString ("J");
    }

    /// <summary>
    /// Текущий рабочий лист соответствует выпуску журнала?
    /// </summary>
    [Pure]
    public static bool IsMagazineIssue
        (
            string? worksheet
        )
    {
        return worksheet.SameString (Constants.Nj)
            || worksheet.SameString (Constants.Njp)
            || worksheet.SameString (Constants.Spec);
    }

    /// <summary>
    /// Текущий рабочий лист ASP?
    /// </summary>
    [Pure]
    public static bool IsAsp
        (
            string? worksheet
        )
    {
        return worksheet.SameString (Constants.Asp);
    }

    /// <summary>
    /// Текущий рабочий лист относится к книжным: PAZK, SPEC или PVK?
    /// </summary>
    [Pure]
    public static bool IsBook
        (
            string? worksheet
        )
    {
        return worksheet.SameString (Constants.Pazk)
               || worksheet.SameString (Constants.Spec)
               || worksheet.SameString (Constants.Pvk);
    }

    /// <summary>
    /// Текущий рабочий лист PAZK?
    /// </summary>
    [Pure]
    public static bool IsPazk
        (
            string? worksheet
        )
    {
        return worksheet.SameString (Constants.Pazk);
    }

    /// <summary>
    /// Текущий рабочий лист SPEC?
    /// </summary>
    [Pure]
    public static bool IsSpec
        (
            string? worksheet
        )
    {
        return worksheet.SafeContainsNoCase (Constants.Spec);
    }

    /// <summary>
    /// Проверка, безопасен ли символ для включения его в URL "как есть".
    /// </summary>
    public static bool IsUrlSafeChar
        (
            char ch
        )
    {
        if (ch is >= 'a' and <= 'z' or >= 'A' and <= 'Z' or >= '0' and <= '9')
        {
            return true;
        }

        switch (ch)
        {
            case '-':
            case '_':
            case '.':
            case '!':
            case '*':
            case '(':
            case ')':
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Кодирование строки в форму, совместимую с URL.
    /// </summary>
    public static string? UrlEncode
        (
            string? text,
            Encoding encoding
        )
    {
        char _IntToHex (int n) => n <= 9
            ? (char)(n + '0')
            : (char)(n - 10 + 'A');

        Sure.NotNull (encoding);

        if (string.IsNullOrEmpty (text))
        {
            return text;
        }

        var bytes = encoding.GetBytes (text);
        var estimatedLength = bytes.Length;
        foreach (var b in bytes)
        {
            var c = (char)b;
            if (!IsUrlSafeChar (c))
            {
                estimatedLength += 2;
            }
        }

        var result = new ValueStringBuilder (stackalloc char[estimatedLength]);
        foreach (var b in bytes)
        {
            var c = (char)b;

            if (IsUrlSafeChar (c))
            {
                result.Append (c);
            }
            else if (c == ' ')
            {
                result.Append ('+');
            }
            else
            {
                result.Append ('%');
                result.Append (_IntToHex ((b >> 4) & 0x0F));
                result.Append (_IntToHex (b & 0x0F));
            }
        }

        return result.ToString();
    }

    /// <summary>
    /// Перевод даты из Юлианского календаря
    /// в привычный нам григорианский.
    /// </summary>
    public static string FromJulianDate
        (
            DateTime date
        )
    {
        var calendar = new JulianCalendar();
        var dateInJulian = calendar.ToDateTime
            (
                date.Year,
                date.Month,
                date.Day,
                date.Hour,
                date.Minute,
                date.Second,
                date.Millisecond
            );

        return dateInJulian.ToString ("yyyyMMdd");
    }

    /// <summary>
    /// Применение значения поля.
    /// </summary>
    public static IList<Field> ApplyFieldValue
        (
            this IList<Field> fields,
            int tag,
            string? value
        )
    {
        Sure.NotNull (fields);

        Field? targetField = null;
        foreach (var field in fields)
        {
            if (field.Tag == tag)
            {
                targetField = field;
                break;
            }
        }

        if (string.IsNullOrEmpty (value))
        {
            if (!ReferenceEquals (targetField, null))
            {
                fields.Remove (targetField);
            }
        }
        else
        {
            if (ReferenceEquals (targetField, null))
            {
                targetField = new Field { Tag = tag };
                fields.Add (targetField);
            }

            targetField.DecodeBody (value);
        }

        return fields;
    }

    #endregion
}
