// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UtfReader.cs -- чтение блока памяти в кодировке UTF-8.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM.Text;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Чтение блока памяти в кодировке UTF-8.
/// </summary>
public ref struct UtfReader
{
    #region Properties

    /// <summary>
    /// Блок памяти.
    /// </summary>
    public ReadOnlySpan<byte> Bytes { get; }

    /// <summary>
    /// Достигнут конец блока?
    /// </summary>
    public bool IsEOF => Position >= Bytes.Length;

    /// <summary>
    /// Текущая позиция в блоке.
    /// </summary>
    public int Position { get; private set; }

    /// <summary>
    /// Оставшийся текст.
    /// </summary>
    public ReadOnlySpan<byte> Remaining => Bytes.Slice (Position);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор для упрощения тестов.
    /// </summary>
    /// <param name="text"></param>
    public UtfReader
        (
            string text
        )
        : this (Encoding.UTF8.GetBytes (text))
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UtfReader
        (
            ReadOnlySpan<byte> bytes
        )
    {
        Bytes = bytes;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Подсчет оставшегося количества символов.
    /// </summary>
    public unsafe int CountChars()
    {
        var result = 0;
        var length = Bytes.Length - Position;

        fixed (byte* ptr = Bytes)
        {
            var src = ptr + Position;
            var stop = src + length;

            while (src < stop)
            {
                uint c = *src++;
                if ((c & 0x80u) == 0u)
                {
                    // 1 байт: 000000000xxxxxxx = 0xxxxxxx
                    // пустой блок
                }
                else if ((c & 0xE0u) == 0xC0u)
                {
                    // 2 байта: 00000yyyyyxxxxxx = 110yyyyy 10xxxxxx
                    src++;
                }
                else if ((c & 0xF0u) == 0xE0u)
                {
                    // 3 байта: zzzzyyyyyyxxxxxx = 1110zzzz 10yyyyyy 10xxxxxx
                    src++;
                    src++;
                }
                else if ((c & 0xF8u) == 0xF0u)
                {
                    // 4 байта: 11101110wwwwzzzzyy + 110111yyyyxxxxxx = 11110uuu 10uuzzzz 10yyyyyy 10xxxxxx
                    src += 3;
                }
                else
                {
                    throw new ArsMagnaException();
                }

                result++;
            }
        }

        return result;
    }

    /// <summary>
    /// Управляющий символ?
    /// </summary>
    public bool IsControl() => char.IsControl (PeekChar());

    /// <summary>
    /// Цифра?
    /// </summary>
    public bool IsDigit() => char.IsDigit (PeekChar());

    /// <summary>
    /// Буква?
    /// </summary>
    public bool IsLetter() => char.IsLetter (PeekChar());

    /// <summary>
    /// Буква или цифра?
    /// </summary>
    public bool IsLetterOrDigit() => char.IsLetterOrDigit (PeekChar());

    /// <summary>
    /// Часть числа?
    /// </summary>
    public bool IsNumber() => char.IsNumber (PeekChar());

    /// <summary>
    /// Знак пунктуации?
    /// </summary>
    public bool IsPunctuation() => char.IsPunctuation (PeekChar());

    /// <summary>
    /// Разделитель?
    /// </summary>
    public bool IsSeparator() => char.IsSeparator (PeekChar());

    /// <summary>
    /// Символ?
    /// </summary>
    public bool IsSymbol() => char.IsSymbol (PeekChar());

    /// <summary>
    /// Пробельный символ?
    /// </summary>
    public bool IsWhiteSpace() => char.IsWhiteSpace (PeekChar());

    /// <summary>
    /// Подглядывание одного символа.
    /// </summary>
    public char PeekChar()
    {
        if (Position >= Bytes.Length)
        {
            return '\0';
        }

        uint result = Bytes[Position];
        if ((result & 0x80u) == 0u)
        {
            // 1 байт: 000000000xxxxxxx = 0xxxxxxx
            // пустой блок
        }
        else if ((result & 0xE0u) == 0xC0u)
        {
            // 2 байта: 00000yyyyyxxxxxx = 110yyyyy 10xxxxxx
            result = (result & 0x1Fu) << 6;
            result |= Bytes[Position + 1] & 0x3Fu;
        }
        else if ((result & 0xF0u) == 0xE0u)
        {
            // 3 байта: zzzzyyyyyyxxxxxx = 1110zzzz 10yyyyyy 10xxxxxx
            result = (result & 0x0Fu) << 12;
            result |= (Bytes[Position + 1] & 0x3Fu) << 6;
            result |= Bytes[Position + 2] & 0x3Fu;
        }
        else if ((result & 0xF8u) == 0xF0u)
        {
            // 4 байта: 11101110wwwwzzzzyy + 110111yyyyxxxxxx = 11110uuu 10uuzzzz 10yyyyyy 10xxxxxx
            result = (result & 0x07u) << 18;
            result |= (Bytes[Position + 1] & 0x3Fu) << 12;
            result |= (Bytes[Position + 2] & 0x3Fu) << 6;
            result |= Bytes[Position + 3] & 0x3Fu;
        }
        else
        {
            throw new ArsMagnaException();
        }

        return (char) result;
    }

    /// <summary>
    /// Чтение одного символа.
    /// </summary>
    public char ReadChar()
    {
        if (Position >= Bytes.Length)
        {
            return '\0';
        }

        uint result = Bytes[Position++];
        if ((result & 0x80u) == 0u)
        {
            // 1 байт: 000000000xxxxxxx = 0xxxxxxx
            // пустой блок
        }
        else if ((result & 0xE0u) == 0xC0u)
        {
            // 2 байта: 00000yyyyyxxxxxx = 110yyyyy 10xxxxxx
            result = (result & 0x1Fu) << 6;
            result |= Bytes[Position++] & 0x3Fu;
        }
        else if ((result & 0xF0u) == 0xE0u)
        {
            // 3 байта: zzzzyyyyyyxxxxxx = 1110zzzz 10yyyyyy 10xxxxxx
            result = (result & 0x0Fu) << 12;
            result |= (Bytes[Position++] & 0x3Fu) << 6;
            result |= Bytes[Position++] & 0x3Fu;
        }
        else if ((result & 0xF8u) == 0xF0u)
        {
            // 4 байта: 11101110wwwwzzzzyy + 110111yyyyxxxxxx = 11110uuu 10uuzzzz 10yyyyyy 10xxxxxx
            result = (result & 0x07u) << 18;
            result |= (Bytes[Position++] & 0x3Fu) << 12;
            result |= (Bytes[Position++] & 0x3Fu) << 6;
            result |= Bytes[Position++] & 0x3Fu;
        }
        else
        {
            throw new ArsMagnaException();
        }

        return (char) result;
    }

    /// <summary>
    /// Чтение до конца строки в сыром формате.
    /// </summary>
    public ReadOnlySpan<byte> ReadByteLine()
    {
        if (IsEOF)
        {
            return default;
        }

        var start = Position;
        while (!IsEOF)
        {
            var c = PeekChar();
            if (c == '\r' || c == '\n')
            {
                break;
            }

            ReadChar();
        }

        var length = Position - start;
        var result = Bytes.Slice (start, length);

        if (!IsEOF)
        {
            var c = PeekChar();
            if (c == '\r')
            {
                ReadChar();
                c = PeekChar();
            }

            if (c == '\n')
            {
                ReadChar();
            }
        }

        return result;
    }

    /// <summary>
    /// Чтение до конца строки.
    /// </summary>
    public string? ReadLine()
    {
        if (IsEOF)
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (!IsEOF)
        {
            var c = PeekChar();
            if (c == '\r' || c == '\n')
            {
                break;
            }

            c = ReadChar();
            builder.Append (c);
        }

        if (!IsEOF)
        {
            var c = PeekChar();

            if (c == '\r')
            {
                ReadChar();
                c = PeekChar();
            }

            if (c == '\n')
            {
                ReadChar();
            }
        }

        return builder.ReturnShared();
    }

    /// <summary>
    /// Пропуск строки.
    /// </summary>
    public void SkipLine()
    {
        if (IsEOF)
        {
            return;
        }

        while (!IsEOF)
        {
            var c = PeekChar();
            if (c == '\r' || c == '\n')
            {
                break;
            }

            ReadChar();
        }

        if (!IsEOF)
        {
            var c = PeekChar();

            if (c == '\r')
            {
                ReadChar();
                c = PeekChar();
            }

            if (c == '\n')
            {
                ReadChar();
            }
        }
    }

    /// <summary>
    /// Считывание вплоть до указанного символа.
    /// </summary>
    /// <returns>Сам символ считывается, но в результат не помещается.
    /// </returns>
    public ReadOnlySpan<byte> ReadTo
        (
            byte stop
        )
    {
        if (IsEOF)
        {
            return default;
        }

        var start = Position;
        var end = start;
        while (Position < Bytes.Length)
        {
            var c = Bytes[Position];
            Position++;
            if (c == stop)
            {
                break;
            }

            end++;
        }

        return Bytes.Slice (start, end - start);
    }

    /// <summary>
    /// Считывание вплоть до указанного символа.
    /// </summary>
    /// <returns>Сам символ считывается, но в результат не помещается.
    /// </returns>
    public ReadOnlySpan<byte> ReadTo (char stop) => ReadTo ((byte) stop);

    #endregion
}
