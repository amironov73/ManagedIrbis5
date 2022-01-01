// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TextBuffer.cs -- буфер для вывода текста с отслеживанием позиции
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Буфер для вывода текста с отслеживанием позиции.
/// </summary>
public sealed class TextBuffer
{
    #region Properties

    /// <summary>
    /// Номер колонки (столбца) (нумерация с 1).
    /// </summary>
    public int Column { get; private set; }

    /// <summary>
    /// Общая длина буфера (количество записанных символов).
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Номер строки (нумерация с 1).
    /// </summary>
    public int Line { get; private set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextBuffer()
    {
        _array = new char[1024];
        Column = 1;
        Line = 1;
    }

    #endregion

    #region Private members

    private char[] _array;

    private void _CalculateColumn()
    {
        for (var i = Length - 1; i >= 0; i--)
        {
            if (_array[i] == '\n')
            {
                break;
            }

            Column++;
        }
    }

    private void _EnsureCapacity
        (
            int required
        )
    {
        var length = _array.Length;
        var needResize = false;

        while (length < required)
        {
            length = length * 2;
            needResize = true;
        }

        if (needResize)
        {
            Array.Resize (ref _array, length);
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Удаление последнего символа (при наличии).
    /// </summary>
    public bool Backspace()
    {
        if (Length == 0)
        {
            return false;
        }

        Length--;
        Column--;
        if (Column == 0)
        {
            Line--;
            Column = 1;
            _CalculateColumn();
        }

        return true;
    }

    /// <summary>
    /// Удаление всего текста из буфера.
    /// </summary>
    public TextBuffer Clear()
    {
        Length = 0;
        Line = 1;
        Column = 1;

        return this;
    }

    /// <summary>
    /// Получение последнего символа в буфере.
    /// Если буфер пуст, возвращается <c>'\0'</c>.
    /// </summary>
    [Pure]
    public char GetLastChar()
    {
        if (Length == 0)
        {
            return '\0';
        }

        var result = _array[Length - 1];

        return result;
    }

    /// <summary>
    /// Предваряется явным переводом строки?
    /// </summary>
    public bool PrecededByNewLine()
    {
        var newLine = Environment.NewLine.ToCharArray();
        var len = newLine.Length;

        if (Length < len)
        {
            return false;
        }

        var result = ArrayUtility.Coincide
            (
                _array,
                Length - len,
                newLine,
                0,
                len
            );

        return result;
    }

    /// <summary>
    /// Удаление последних пустых строк.
    /// </summary>
    public TextBuffer RemoveEmptyLines()
    {
        var newLine = Environment.NewLine.ToCharArray();
        var len = newLine.Length;

        while (Length > len)
        {
            if (!ArrayUtility.Coincide
                    (
                        _array,
                        Length - len,
                        newLine,
                        0,
                        len
                    ))
            {
                break;
            }

            Length -= len;
            Line--;
            Column = 1;
            _CalculateColumn();
        }

        return this;
    }

    /// <summary>
    /// Добавление символа в конец буфера.
    /// </summary>
    public TextBuffer Write
        (
            char c
        )
    {
        _EnsureCapacity (Length + 1);
        _array[Length] = c;
        Length++;

        if (c == '\n')
        {
            Line++;
            Column = 1;
        }
        else
        {
            Column++;
        }

        return this;
    }

    /// <summary>
    /// Добавление строки в конец буфера.
    /// </summary>
    public TextBuffer Write
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return this;
        }

        var characters = text.ToCharArray();
        _EnsureCapacity (Length + characters.Length);

        foreach (var c in characters)
        {
            if (c == '\n')
            {
                Line++;
                Column = 1;
            }
            else
            {
                Column++;
            }

            _array[Length] = c;
            Length++;
        }

        return this;
    }

    /// <summary>
    /// Добавление в буфер форматированного текста.
    /// </summary>
    public TextBuffer Write
        (
            string format,
            params object[] arguments
        )
    {
        var text = string.Format (format, arguments);

        return Write (text);
    }

    /// <summary>
    /// Добавление в буфер перевода строки
    /// (в зависимости от платформы может
    /// быть одним или двумя символами).
    /// </summary>
    public TextBuffer WriteLine()
    {
        return Write (Environment.NewLine);
    }

    /// <summary>
    /// Добавление в буфер текста и перевода строки.
    /// </summary>
    public TextBuffer WriteLine
        (
            string? text
        )
    {
        Write (text);

        return WriteLine();
    }

    /// <summary>
    /// Добавление в буфер форматированного текста
    /// и перевода строки.
    /// </summary>
    public TextBuffer WriteLine
        (
            string format,
            params object[] arguments
        )
    {
        Sure.NotNull (format, nameof (format));

        var text = string.Format (format, arguments);

        return WriteLine (text);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return new string (_array, 0, Length);
    }

    #endregion
}
