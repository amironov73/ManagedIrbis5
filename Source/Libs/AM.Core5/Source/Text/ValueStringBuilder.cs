// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ValueStringBuilder.cs -- StringBuilder, оформленный как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.IO;
using System.Text;

#endregion

namespace AM.Text;

//
// Вдохновлено кодом из BCL:
// https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/Text/ValueStringBuilder.cs
//

/// <summary>
/// Аналог системного <see cref="StringBuilder"/>, оформленный
/// как структура.
/// </summary>
public ref struct ValueStringBuilder
{
    #region Properties

    /// <summary>
    /// Емкость в символах.
    /// </summary>
    public int Capacity => _characters.Length;

    /// <summary>
    /// Текущая длина в символах.
    /// </summary>
    public int Length
    {
        get => _position;
        set
        {
            Sure.NonNegative (value, nameof (value));
            Sure.AssertState (value <= _characters.Length);
            _position = value;
        }
    }

    /// <summary>
    /// Сырой буфер.
    /// </summary>
    public ReadOnlySpan<char> RawCharacters => _characters;

    /// <summary>
    /// Доступ по индексу.
    /// </summary>
    public ref char this [int index] => ref _characters[index];

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="characters">Начальный буфер.</param>
    public ValueStringBuilder
        (
            Span<char> characters
        )
        : this()
    {
        _characters = characters;
    }

    #endregion

    #region Private members

    private char[]? _array;
    private Span<char> _characters;
    private int _position;

    #endregion

    #region Public methods

    /// <summary>
    /// Выдача построенного на данный момент значения как спана.
    /// </summary>
    public ReadOnlySpan<char> AsSpan()
    {
        return _characters.Slice (0, _position);
    }

    /// <summary>
    /// Выдача построенного на данный момент значения как спана.
    /// </summary>
    public ReadOnlySpan<char> AsSpan (int start)
    {
        return _characters.Slice (start, _position - start);
    }

    /// <summary>
    /// Выдача построенного на данный момент значения как спана.
    /// </summary>
    public ReadOnlySpan<char> AsSpan (int start, int length)
    {
        return _characters.Slice (start, length);
    }

    /// <summary>
    /// Добавление одного символа.
    /// </summary>
    public void Append
        (
            char c
        )
    {
        if (_position == _characters.Length)
        {
            Grow (1);
        }

        _characters[_position] = c;
        ++_position;
    }

    /// <summary>
    /// Добавление спана символов.
    /// </summary>
    public void Append
        (
            ReadOnlySpan<char> text
        )
    {
        var newPosition = _position + text.Length;
        if (newPosition > _characters.Length)
        {
            Grow (text.Length);
        }

        text.CopyTo (_characters.Slice (_position));
        _position = newPosition;
    }

    /// <summary>
    /// Добавление пары спанов.
    /// </summary>
    public void Append
        (
            ReadOnlySpan<char> text1,
            ReadOnlySpan<char> text2
        )
    {
        var delta = text1.Length + text2.Length;
        var newPosition = _position + delta;
        if (newPosition > _characters.Length)
        {
            Grow (delta);
        }

        text1.CopyTo (_characters.Slice (_position));
        text2.CopyTo (_characters.Slice (_position + text1.Length));
        _position = newPosition;
    }

    /// <summary>
    /// Добавление трех спанов.
    /// </summary>
    public void Append
        (
            ReadOnlySpan<char> text1,
            ReadOnlySpan<char> text2,
            ReadOnlySpan<char> text3
        )
    {
        var delta = text1.Length + text2.Length + text3.Length;
        var newPosition = _position + delta;
        if (newPosition > _characters.Length)
        {
            Grow (delta);
        }

        text1.CopyTo (_characters.Slice (_position));
        text2.CopyTo (_characters.Slice (_position + text1.Length));
        text3.CopyTo (_characters.Slice (_position + text1.Length + text2.Length));
        _position = newPosition;
    }

    /// <summary>
    /// Добавление целого числа со знаком.
    /// </summary>
    public unsafe void Append
        (
            int value
        )
    {
        // int.MinValue занимает 11 символов с учетом знака

        var remaining = _characters.Length - _position;
        if (remaining >= 11)
        {
            var buffer = _characters.Slice (_position);
            var written = FastNumber.Int32ToChars (value, buffer);
            _position += written;
        }
        else
        {
            Span<char> buffer = stackalloc char[11];
            var written = FastNumber.Int32ToChars (value, buffer);
            var newPosition = _position + written;
            if (newPosition > _characters.Length)
            {
                Grow (written);
            }

            buffer.Slice (0, written).CopyTo (_characters.Slice (_position));
            _position = newPosition;
        }
    }

    /// <summary>
    /// Добавление целого числа без знака.
    /// </summary>
    public unsafe void Append
        (
            uint value
        )
    {
        // uint.MaxValue занимает 10 символов с учетом знака

        var remaining = _characters.Length - _position;
        if (remaining >= 10)
        {
            var buffer = _characters.Slice (_position);
            var written = FastNumber.UInt32ToChars (value, buffer);
            _position += written;
        }
        else
        {
            Span<char> buffer = stackalloc char[10];
            var written = FastNumber.UInt32ToChars (value, buffer);
            var newPosition = _position + written;
            if (newPosition > _characters.Length)
            {
                Grow (written);
            }

            buffer.Slice (0, written).CopyTo (_characters.Slice (_position));
            _position = newPosition;
        }
    }

    /// <summary>
    /// Добавление длинного целого числа со знаком.
    /// </summary>
    public unsafe void Append
        (
            long value
        )
    {
        // long.MinValue занимает 20 символов с учетом знака

        var remaining = _characters.Length - _position;
        if (remaining >= 20)
        {
            var buffer = _characters.Slice (_position);
            var written = FastNumber.Int64ToChars (value, buffer);
            _position += written;
        }
        else
        {
            Span<char> buffer = stackalloc char[20];
            var written = FastNumber.Int64ToChars (value, buffer);
            var newPosition = _position + written;
            if (newPosition > _characters.Length)
            {
                Grow (written);
            }

            buffer.Slice (0, written).CopyTo (_characters.Slice (_position));
            _position = newPosition;
        }
    }

    /// <summary>
    /// Добавление длинного целого числа без знака.
    /// </summary>
    public unsafe void Append
        (
            ulong value
        )
    {
        // ulong.MaxValue занимает 20 символов с учетом знака

        var remaining = _characters.Length - _position;
        if (remaining >= 20)
        {
            var buffer = _characters.Slice (_position);
            var written = FastNumber.UInt64ToChars (value, buffer);
            _position += written;
        }
        else
        {
            Span<char> buffer = stackalloc char[20];
            var written = FastNumber.UInt64ToChars (value, buffer);
            var newPosition = _position + written;
            if (newPosition > _characters.Length)
            {
                Grow (written);
            }

            buffer.Slice (0, written).CopyTo (_characters.Slice (_position));
            _position = newPosition;
        }
    }

    /// <summary>
    /// Добавление перевода строки.
    /// </summary>
    public void AppendLine()
    {
        Append (Environment.NewLine);
    }

    /// <summary>
    /// Очистка.
    /// </summary>
    public void Clear()
    {
        _position = 0;
    }

    /// <summary>
    /// Освобождаем ресурсы, если были заняты.
    /// </summary>
    public void Dispose()
    {
        var borrowed = _array;
        this = default; // для спокойствия
        if (borrowed is not null)
        {
            ArrayPool<char>.Shared.Return (borrowed);
        }
    }

    /// <summary>
    /// Увеличение емкости, если необходимо.
    /// </summary>
    public void EnsureCapacity
        (
            int capacity
        )
    {
        if (capacity > _characters.Length)
        {
            Grow (capacity - _position);
        }
    }

    /// <summary>
    /// Увеличение емкости на указанное количество символов.
    /// </summary>
    public void Grow
        (
            int additional
        )
    {
        var newCapacity = (int)Math.Max
            (
                (uint)(_position + additional),
                (uint)(Capacity * 2)
            );
        var borrowed = ArrayPool<char>.Shared.Rent (newCapacity);
        _characters.Slice (0, _position).CopyTo (borrowed);
        if (_array is not null)
        {
            ArrayPool<char>.Shared.Return (_array);
        }

        _characters = _array = borrowed;
    }

    /// <summary>
    /// Получение перечислителя.
    /// </summary>
    public ReadOnlySpan<char>.Enumerator GetEnumerator()
    {
        return AsSpan().GetEnumerator();
    }

    /// <summary>
    /// Вставка символов в указанную позицию.
    /// </summary>
    public void Insert
        (
            int index,
            char value,
            int count
        )
    {
        if (_position > _characters.Length - count)
        {
            Grow (count);
        }

        var remaining = _position - index;
        _characters.Slice (index, remaining).CopyTo (_characters.Slice (index + count));
        _characters.Slice (index, count).Fill (value);
        _position += count;
    }

    /// <summary>
    /// Вставка текста в указанную позицию.
    /// </summary>
    public void Insert
        (
            int index,
            ReadOnlySpan<char> text
        )
    {
        if (text.IsEmpty)
        {
            return;
        }

        var count = text.Length;
        if (_position > _characters.Length - count)
        {
            Grow (count);
        }

        var remaining = _position - index;
        _characters.Slice (index, remaining).CopyTo (_characters.Slice (index + count));
        text.CopyTo (_characters.Slice (index));
        _position += count;
    }

    /// <summary>
    /// Чтение строки непосредственно в <see cref="StringBuilder"/>.
    /// </summary>
    /// <param name="reader">Поток, из которого считывается строка.</param>
    /// <param name="appendNewLine">Добавлять перевод строки в конец?</param>
    /// <returns><c>false</c>, если достигнут конец потока.</returns>
    public bool ReadLine
        (
            TextReader reader,
            bool appendNewLine = false
        )
    {
        Sure.NotNull (reader);

        var first = true;
        while (true)
        {
            var chr = reader.Read();
            if (chr < 0)
            {
                return !first;
            }

            if (chr == '\n')
            {
                if (appendNewLine)
                {
                    Append ((char) chr);
                }

                return true;
            }

            if (chr != '\r')
            {
                Append ((char) chr);
            }

            first = false;
        }
    }

    /// <summary>
    /// Удаление указанного количества символов.
    /// </summary>
    public void Remove
        (
            int startIndex,
            int length
        )
    {
        Sure.NonNegative (startIndex);
        Sure.NonNegative (length);

        if (length == 0)
        {
            return;
        }

        if (length > _position - startIndex)
        {
            throw new ArgumentOutOfRangeException (nameof (length));
        }

        if (_position == length && startIndex == 0)
        {
            _position = 0;
            return;
        }

        var endIndex = startIndex + length;
        var remaining = _position - endIndex;
        _characters.Slice (endIndex, remaining).CopyTo (_characters.Slice (startIndex));
        _position -= length;
    }

    /// <summary>
    /// Замена одной подстроки на другую.
    /// </summary>
    public void Replace
        (
            ReadOnlySpan<char> from,
            ReadOnlySpan<char> to
        )
    {
        var index = 0;

        var delta = to.Length - from.Length;
        while (true)
        {
            var actual = _characters[index..];
            var found = actual.IndexOf (from);
            if (found < 0)
            {
                return;
            }

            var head = actual.Slice (found);
            if (delta < 0)
            {
                // двигаем хвост влево
                head.Slice (from.Length).CopyTo (head.Slice (to.Length));
            }
            else if (delta > 0)
            {
                // двигаем хвост вправо, при необходимости увеличиваем буфер
                var tail = _position - found - from.Length;
                EnsureCapacity(_position + delta);
                head = _characters.Slice (found); // возможно, буфер увеличился!
                head.Slice (from.Length, tail)
                    .CopyTo (head.Slice (to.Length));
            }

            to.CopyTo (head);

            index += to.Length;
            _position += delta;
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var result = AsSpan().ToString();
        Dispose();

        return result;
    }

    #endregion
}
