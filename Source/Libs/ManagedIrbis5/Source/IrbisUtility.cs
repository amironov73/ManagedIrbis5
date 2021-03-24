// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* IrbisUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using AM;
using AM.Security;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    public static class IrbisUtility
    {
        #region Public methods

        /// <summary>
        /// Декодирование строки подключения, если она закодирована или зашифрована.
        /// </summary>
        /// <param name="possiblyEncrypted">Строка подключения.</param>
        /// <param name="password">Пароль. Если null, то используется пароль по умолчанию.</param>
        /// <returns>Расшифрованная строка подключения.</returns>
        public static string DecryptConnectionString
            (
                string possiblyEncrypted,
                string? password
            )
        {
            // Пустая строка зашифрованной быть не может.
            if (string.IsNullOrEmpty(possiblyEncrypted))
            {
                return possiblyEncrypted;
            }

            // С восклицательного знака начинается строка,
            // закодированная в банальный Base64.
            if (possiblyEncrypted[0] == '!')
            {
                var enc = possiblyEncrypted.Substring(1);
                var res = SecurityUtility.DecryptFromBase64(enc);
                return res;
            }

            // Зашифрованная строка должна начинаться со знака вопроса.
            if (possiblyEncrypted[0] != '?')
            {
                return possiblyEncrypted;
            }

            var encrypted = possiblyEncrypted.Substring(1);
            if (string.IsNullOrEmpty(password))
            {
                password = "irbis";
            }

            var result = SecurityUtility.Decrypt(encrypted, password);

            return result;
        } // method DecryptConnectionString

        /// <summary>
        ///
        /// </summary>
        public static string EncodePercentString
            (
                byte[]? array
            )
        {
            if (ReferenceEquals(array, null)
                || array.Length == 0)
            {
                return string.Empty;
            }

            var result = new StringBuilder();

            foreach (var b in array)
            {
                if (b >= 'A' && b <= 'Z'
                    || b >= 'a' && b <= 'z'
                    || b >= '0' && b <= '9'
                    )
                {
                    result.Append((char)b);
                }
                else
                {
                    result.AppendFormat
                        (
                            "%{0:X2}",
                            b
                        );
                }
            }

            return result.ToString();
        } // method EncodePercentString

        /// <summary>
        ///
        /// </summary>
        public static byte[] DecodePercentString
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return new byte[0];
            }

            var predictedLength = text.Length / 2;
            using var stream = new MemoryStream(predictedLength);
            for (var i = 0; i < text.Length; i++)
            {
                var c = text[i];
                if (c != '%')
                {
                    stream.WriteByte((byte) c);
                }
                else
                {
                    if (i >= text.Length - 2)
                    {
                        Magna.Error
                        (
                            "IrbisUtility::DecodePercentString: "
                            + "unexpected end of stream"
                        );

                        throw new FormatException("text");
                    }

                    var b = byte.Parse
                    (
                        text.Substring(i + 1, 2),
                        NumberStyles.HexNumber
                    );
                    stream.WriteByte(b);
                    i += 2;
                }
            }

            return stream.ToArray();
        } // method DecodePercentString

        /// <summary>
        /// Decode string.
        /// </summary>
        public static string? UrlDecode
            (
                string? text,
                Encoding encoding
            )
        {
            int _HexToInt(char h) =>
                h >= '0' && h <= '9'
                    ? h - '0'
                    : h >= 'a' && h <= 'f'
                        ? h - 'a' + 10
                        : h >= 'A' && h <= 'F'
                            ? h - 'A' + 10
                            : -1;

            if (string.IsNullOrEmpty(text))
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
                    var h1 = _HexToInt(text[pos + 1]);
                    var h2 = _HexToInt(text[pos + 2]);

                    if (h1 >= 0 && h2 >= 0)
                    {
                        var b = (byte)((h1 << 4) | h2);
                        pos += 2;

                        bytes.Add(b);
                        continue;
                    }
                }

                bytes.Add((byte)ch);
            }

            var result = encoding.GetString(bytes.ToArray());

            return result;
        } // method UrlDecode

        public static bool IsUrlSafeChar
            (
                char ch
            )
        {
            if (ch >= 'a' && ch <= 'z'
                || ch >= 'A' && ch <= 'Z'
                || ch >= '0' && ch <= '9')
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
        /// Encode string.
        /// </summary>
        public static string? UrlEncode
            (
                string? text,
                Encoding encoding
            )
        {
            char _IntToHex ( int n ) => n <= 9
                    ? (char) (n + '0')
                    : (char) (n - 10 + 'A');

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            var bytes = encoding.GetBytes(text);
            var result = new StringBuilder();

            foreach (var b in bytes)
            {
                var c = (char)b;

                if (IsUrlSafeChar(c))
                {
                    result.Append(c);
                }
                else if (c == ' ')
                {
                    result.Append('+');
                }
                else
                {
                    result.Append('%');
                    result.Append(_IntToHex((b >> 4) & 0x0F));
                    result.Append(_IntToHex(b & 0x0F));
                }
            }

            return result.ToString();
        } // method UrlEncode

        /// <summary>
        ///
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

            return dateInJulian.ToString("yyyyMMdd");
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
            Field? targetField = null;
            foreach (var field in fields)
            {
                if (field.Tag == tag)
                {
                    targetField = field;
                    break;
                }
            }

            if (string.IsNullOrEmpty(value))
            {
                if (!ReferenceEquals(targetField, null))
                {
                    fields.Remove(targetField);
                }
            }
            else
            {
                if (ReferenceEquals(targetField, null))
                {
                    targetField = new Field { Tag = tag };
                    fields.Add(targetField);
                }
                targetField.Decode(value);
            }

            return fields;
        } // method ApplyFieldValue

        #endregion

    } // class IrbisUtility

} // namespace ManagedIrbis
