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

/* Utf8Utility.cs -- работа с кодировкой UTF-8
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM.Core.Properties;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Работа с кодировкой UTF-8.
/// </summary>
public static class Utf8Utility
{
    #region Public methods

    /// <summary>
    /// Подсчитывает количество байт, необходимых для размещения
    /// текста в кодировке UTF-8.
    /// </summary>
    /// <param name="text">Указатель на текст в UCS-16.</param>
    /// <param name="length">Количество символов в тексте.</param>
    /// <returns>Необходимое количество байт.</returns>
    /// <remarks>Суррогатные пары никак не учитываются!</remarks>
    public static unsafe uint CountBytes
        (
            char* text,
            uint length
        )
    {
        var result = 0u;

        unchecked
        {
            while (length-- != 0)
            {
                uint c = *text++;
                ++result;
                if (c >= 1u << 7)
                {
                    ++result;
                }

                if (c >= 1u << 11)
                {
                    ++result;
                }

                if (c >= 1u << 16)
                {
                    ++result;
                }

                if (c >= 1u << 21)
                {
                    ++result;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Подсчитывает количество байт, необходимых для размещения
    /// текста в кодировке UTF-8.
    /// </summary>
    public static unsafe uint CountBytes
        (
            ReadOnlySpan<char> text
        )
    {
        fixed (char* ptr = text)
            unchecked
            {
                return CountBytes (ptr, (uint)text.Length);
            }
    }

    /// <summary>
    /// Подсчет количества code point в данных.
    /// </summary>
    /// <param name="data">Указатель на начало данных.</param>
    /// <param name="length">Длина блока в байтах.</param>
    /// <returns>Количество code point либо -1, если данные битые.
    /// </returns>
    public static unsafe int CountChars
        (
            byte* data,
            uint length
        )
    {
        var result = 0;

        while (length != 0)
            unchecked
            {
                var chr = *data++;
                uint delta;

                if ((chr & 0x80) == 0)
                {
                    delta = 1;
                }
                else if ((chr & 0xE0) == 0xC0)
                {
                    delta = 2;
                }
                else if ((chr & 0xF0) == 0xE0)
                {
                    delta = 3;
                }
                else if ((result & 0xF8u) == 0xF0u)
                {
                    delta = 4;
                }
                else
                {
                    return -1;
                }

                if (length < delta)
                {
                    return -1;
                }

                length -= delta;
                ++result;
            }

        return result;
    }

    /// <summary>
    /// Считывание из потока одного символа в кодировке UTF-8.
    /// </summary>
    /// <param name="stream">Поток байтов.</param>
    /// <returns>Прочитанный символ либо <c>'\0'</c>.</returns>
    public static char ReadChar
        (
            Stream stream
        )
    {
        unchecked
        {
            int next;
            var result = stream.ReadByte();
            if (result < 0)
            {
                return (char)0;
            }

            if ((result & 0x80) == 0)
            {
                // 1-Byte sequence: 000000000xxxxxxx = 0xxxxxxx
            }
            else if ((result & 0xE0) == 0xC0)
            {
                // 2-Byte sequence: 00000yyyyyxxxxxx = 110yyyyy 10xxxxxx

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result = (result & 0x1F) << 6;
                result |= next & 0x3F;
            }
            else if ((result & 0xF0) == 0xE0)
            {
                // 3-Byte sequence: zzzzyyyyyyxxxxxx = 1110zzzz 10yyyyyy 10xxxxxx

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result = (result & 0x0F) << 12;
                result |= (next & 0x3F) << 6;

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result |= next & 0x3F;
            }
            else if ((result & 0xF8u) == 0xF0u)
            {
                // 4-Byte sequence: 11101110wwwwzzzzyy + 110111yyyyxxxxxx = 11110uuu 10uuzzzz 10yyyyyy 10xxxxxx

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result = (result & 0x07) << 18;
                result |= (next & 0x3F) << 12;

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result |= (next & 0x3F) << 6;

                next = stream.ReadByte();
                if (next < 0)
                {
                    return (char)0;
                }

                result |= next & 0x3F;
            }
            else
            {
                throw new ArsMagnaException (Resources.BadSymbol);
            }

            return (char)result;
        }
    }

    /// <summary>
    /// Валидация текста в кодировке UTF-8.
    /// </summary>
    /// <param name="data">Указатель на начало текста.</param>
    /// <param name="length">Размер блока текста в байтах.</param>
    /// <returns>Результат проверки.</returns>
    public static unsafe bool Validate
        (
            byte* data,
            uint length
        )
    {
        /*

           Well-Formed UTF-8 Byte Sequences

           +--------------------+------------+-------------+------------+-------------+
           | Code Points        | First Byte | Second Byte | Third Byte | Fourth Byte |
           +--------------------+------------+-------------+------------+-------------+
           | U+0000..U+007F     | 00..7F     |             |            |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+0080..U+07FF     | C2..DF     | 80..BF      |            |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+0800..U+0FFF     | E0         | A0..BF      | 80..BF     |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+1000..U+CFFF     | E1..EC     | 80..BF      | 80..BF     |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+D000..U+D7FF     | ED         | 80..9F      | 80..BF     |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+E000..U+FFFF     | EE..EF     | 80..BF      | 80..BF     |             |
           +--------------------+------------+-------------+------------+-------------+
           | U+10000..U+3FFFF   | F0         | 90..BF      | 80..BF     | 80..BF      |
           +--------------------+------------+-------------+------------+-------------+
           | U+40000..U+FFFFF   | F1..F3     | 80..BF      | 80..BF     | 80..BF      |
           +--------------------+------------+-------------+------------+-------------+
           | U+100000..U+10FFFF | F4         | 80..8F      | 80..BF     | 80..BF      |
           +--------------------+------------+-------------+------------+-------------+

         */

        if (length == 0)
        {
            // к пустому блоку у нас нет претензий
            return true;
        }

        if (data == null)
        {
            throw new ArgumentNullException (nameof (data));
        }

        unchecked
        {
            while (length-- != 0)
            {
                var b1 = *data++;

                if (b1 <= 0x7F)
                {
                    continue;
                }

                if (b1 < 0xC2)
                {
                    return false;
                }

                if (length == 0)
                {
                    return false;
                }

                --length;
                var b2 = *data++;

                if (b1 <= 0xDF)
                {
                    if (!(b2 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                if (length == 0)
                {
                    return false;
                }

                --length;
                var b3 = *data++;

                if (b1 == 0xE0)
                {
                    if (!(b2 is >= 0xA0 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                if (b1 <= 0xEC)
                {
                    if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                if (b1 == 0xED)
                {
                    if (!(b2 is >= 0x80 and <= 0x9F && b3 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                if (b1 <= 0xEF)
                {
                    if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                --length;
                var b4 = *data++;

                if (b1 == 0xF0)
                {
                    if (!(b2 is >= 0x90 and <= 0xBF && b3 is >= 0x80 and <= 0xBF && b4 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                if (b1 <= 0xF4)
                {
                    if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF && b4 is >= 0x80 and <= 0xBF))
                    {
                        return false;
                    }

                    continue;
                }

                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Валидация текста в кодировке UTF-8.
    /// </summary>
    public static unsafe bool Validate
        (
            ReadOnlySpan<byte> text
        )
    {
        fixed (byte* ptr = text)
            unchecked
            {
                return Validate (ptr, (uint)text.Length);
            }
    }

    /// <summary>
    /// Валидация текста в кодировке UTF-8.
    /// </summary>
    public static bool Validate
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        while (true)
        {
            var b1 = stream.ReadByte();
            if (b1 < 0)
            {
                // к пустому потоку мы не имеем претензий

                return true;
            }

            if (b1 < 0x80)
            {
                continue;
            }

            if (b1 < 0xC2)
            {
                return false;
            }

            // должен быть следующий байт
            var b2 = stream.ReadByte();
            if (b2 < 0)
            {
                return false;
            }

            if (b1 <= 0xDF)
            {
                if (!(b2 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            // должен быть следующий байт
            var b3 = stream.ReadByte();
            if (b3 < 0)
            {
                return false;
            }

            if (b1 == 0xE0)
            {
                if (!(b2 is >= 0xA0 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            if (b1 <= 0xEC)
            {
                if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            if (b1 == 0xED)
            {
                if (!(b2 is >= 0x80 and <= 0x9F && b3 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            if (b1 <= 0xEF)
            {
                if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            // должен быть следующий байт
            var b4 = stream.ReadByte();
            if (b4 < 0)
            {
                return false;
            }

            if (b1 == 0xF0)
            {
                if (!(b2 is >= 0x90 and <= 0xBF && b3 is >= 0x80 and <= 0xBF && b4 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            if (b1 <= 0xF4)
            {
                if (!(b2 is >= 0x80 and <= 0xBF && b3 is >= 0x80 and <= 0xBF && b4 is >= 0x80 and <= 0xBF))
                {
                    return false;
                }

                continue;
            }

            return false;
        }
    }

    /// <summary>
    /// Запись в поток одного символа в кодировке UTF-8.
    /// </summary>
    /// <param name="stream">Поток.</param>
    /// <param name="chr">Записываемый символ.</param>
    public static void WriteChar
        (
            Stream stream,
            char chr
        )
    {
        unchecked
        {
            var uchr = (uint)chr;

            if (uchr < 1u << 7)
            {
                stream.WriteByte ((byte)chr);
            }

            if (uchr < 1u << 11)
            {
                stream.WriteByte ((byte)((uchr >> 6) | 0xC0u));
                stream.WriteByte ((byte)((uchr & 0x3Fu) | 0xC8u));
            }

            if (uchr < 1u << 16)
            {
                stream.WriteByte ((byte)((uchr >> 12) | 0xE0u));
                stream.WriteByte ((byte)(((uchr >> 6) & 0x3Fu) | 0x80u));
                stream.WriteByte ((byte)((uchr & 0x3Fu) | 0x80u));
            }

            if (uchr < 1u << 21)
            {
                stream.WriteByte ((byte)((uchr >> 18) | 0xF0u));
                stream.WriteByte ((byte)(((uchr >> 12) & 0x3Fu) | 0x80u));
                stream.WriteByte ((byte)(((uchr >> 6) & 0x3Fu) | 0x80u));
                stream.WriteByte ((byte)((uchr & 0x3Fu) | 0x80u));
            }
        }
    }

    #endregion
}
