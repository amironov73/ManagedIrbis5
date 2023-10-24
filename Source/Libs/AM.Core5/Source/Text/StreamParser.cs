// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* StreamParser.cs -- считывание из потока чисел, идентификаторов и прочего
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using AM.IO;

using JetBrains.Annotations;

#endregion

namespace AM.Text;

/// <summary>
/// Считывание из потока чисел, идентификаторов
/// и прочего.
/// </summary>
[PublicAPI]
public sealed class StreamParser
    : IDisposable
{
    #region Constants

    /// <summary>
    /// Символ, означающий, что достигнут конец потока.
    /// </summary>
    public const char Eof = unchecked ((char) -1);

    #endregion

    #region Properties

    /// <summary>
    /// Достигнут ли конец потока?
    /// </summary>
    public bool EndOfStream => PeekChar() == Eof;

    /// <summary>
    /// Используемый <see cref="TextReader"/>.
    /// </summary>
    public TextReader Reader { get; }

    /// <summary>
    /// Парсер для системной консоли.
    /// </summary>
    public static StreamParser Console => _consoleParser ??= new (System.Console.In);

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StreamParser
        (
            TextReader reader,
            bool ownReader = false
        )
    {
        Sure.NotNull (reader);

        Reader = reader;
        _ownReader = ownReader;
    }

    #endregion

    #region Private members

    private readonly bool _ownReader;

    private static StreamParser? _consoleParser;

    private string _ReadNumber()
    {
        var builder = StringBuilderPool.Shared.Get();
        var c = PeekChar();
        if (c is '-' or '+')
        {
            builder.Append (ReadChar());
        }

        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        c = PeekChar();
        if (c == '.')
        {
            builder.Append (ReadChar());
            while (IsDigit())
            {
                builder.Append (ReadChar());
            }

            c = PeekChar();
        }

        if (c is 'e' or 'E')
        {
            builder.Append (ReadChar());
            c = PeekChar();
            if (c is '-' or '+')
            {
                builder.Append (ReadChar());
            }

            while (IsDigit())
            {
                builder.Append (ReadChar());
            }

            //c = PeekChar();
        }

        return builder.ReturnShared();
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Конструирование <see cref="StreamParser"/> из указанного файла.
    /// </summary>
    public static StreamParser FromFile
        (
            string fileName,
            Encoding encoding
        )
    {
        Sure.FileExists (fileName);
        Sure.NotNull (encoding);

        var reader = TextReaderUtility.OpenRead
            (
                fileName,
                encoding
            );
        var result = new StreamParser
            (
                reader,
                true
            );

        return result;
    }

    /// <summary>
    /// Конструирование <see cref="StreamParser"/>
    /// из заданного текста.
    /// </summary>
    public static StreamParser FromString
        (
            string? text
        )
    {
        text ??= string.Empty;

        var reader = new StringReader (text);
        var result = new StreamParser
            (
                reader,
                true
            );

        return result;
    }

    /// <summary>
    /// Управляющий символ?
    /// </summary>
    public bool IsControl()
    {
        var c = PeekChar();
        return char.IsControl (c);
    }

    /// <summary>
    /// Цифра?
    /// </summary>
    public bool IsDigit()
    {
        var c = PeekChar();
        return char.IsDigit (c);
    }

    /// <summary>
    /// Буква?
    /// </summary>
    public bool IsLetter()
    {
        var c = PeekChar();
        return char.IsLetter (c);
    }

    /// <summary>
    /// Буква или цифра?
    /// </summary>
    public bool IsLetterOrDigit()
    {
        var c = PeekChar();
        return char.IsLetterOrDigit (c);
    }

    /// <summary>
    /// Часть числа?
    /// </summary>
    public bool IsNumber()
    {
        var c = PeekChar();
        return char.IsNumber (c);
    }

    /// <summary>
    /// Знак пунктуации?
    /// </summary>
    public bool IsPunctuation()
    {
        var c = PeekChar();
        return char.IsPunctuation (c);
    }

    /// <summary>
    /// Символ-разделитель?
    /// </summary>
    public bool IsSeparator()
    {
        var c = PeekChar();
        return char.IsSeparator (c);
    }

    /// <summary>
    /// Суррогат?
    /// </summary>
    public bool IsSurrogate()
    {
        var c = PeekChar();
        return char.IsSurrogate (c);
    }

    /// <summary>
    /// Символ?
    /// </summary>
    public bool IsSymbol()
    {
        var c = PeekChar();
        return char.IsSymbol (c);
    }

    /// <summary>
    /// Пробельный символ?
    /// </summary>
    public bool IsWhiteSpace()
    {
        var c = PeekChar();
        return char.IsWhiteSpace (c);
    }

    /// <summary>
    /// Подглядывание одного символа из потока.
    /// Продвижения вперед по потоку не происходит.
    /// </summary>
    /// <returns>
    /// Если достигнут конец потока, возвращает
    /// <see cref="Eof"/>.
    /// </returns>
    public char PeekChar()
    {
        return unchecked ((char)Reader.Peek());
    }

    /// <summary>
    /// Считывание одного символа из потока.
    /// </summary>
    /// <returns>
    /// Если достигнут конец потока, возвращает
    /// <see cref="Eof"/>.
    /// </returns>
    public char ReadChar()
    {
        return unchecked ((char)Reader.Read());
    }

    /// <summary>
    /// Считывание из потока числа с фиксированной точкой.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public decimal? ReadDecimal
        (
            IFormatProvider? provider = null
        )
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var result = _ReadNumber();

        return decimal.Parse
            (
                result,
                provider ?? CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Считывание из потока числа с плавающей точкой двойной точности.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public double? ReadDouble
        (
            IFormatProvider? provider = null
        )
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var number = _ReadNumber();

        try
        {
            return double.Parse
                (
                    number,
                    provider ?? CultureInfo.InvariantCulture
                );
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока челого 16-битного числа со знаком.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public short? ReadInt16()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        if (PeekChar() == '-')
        {
            builder.Append (ReadChar());
        }

        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseInt16();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 32-битного числа со знаком.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public int? ReadInt32()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        if (PeekChar() == '-')
        {
            builder.Append (ReadChar());
        }

        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseInt32();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 64-битного числа со знаком.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public long? ReadInt64()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        if (PeekChar() == '-')
        {
            builder.Append (ReadChar());
        }

        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseInt64();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 128-битного числа со знаком.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public Int128? ReadInt128()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        if (PeekChar() == '-')
        {
            builder.Append (ReadChar());
        }

        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return  Int128.Parse
                (
                    builder.ReturnShared(),
                    CultureInfo.InvariantCulture
                );
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока числа с плавающей точкой одинарной точности.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public float? ReadSingle
        (
            IFormatProvider? provider = null
        )
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var number = _ReadNumber();

        try
        {
            return float.Parse
                (
                    number,
                    provider ?? CultureInfo.InvariantCulture
                );
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока числа с плавающей точкой половинной точности.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public Half? ReadHalf
        (
            IFormatProvider? provider = null
        )
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var number = _ReadNumber();

        try
        {
            return Half.Parse
                (
                    number,
                    provider ?? CultureInfo.InvariantCulture
                );
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 16-битного числа без знака.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public ushort? ReadUInt16()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseUInt16();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 32-битного числа без знака.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public uint? ReadUInt32()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseUInt32();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 64-битного числа без знака.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public ulong? ReadUInt64()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return builder.ReturnShared().ParseUInt64();
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Считывание из потока целого 128-битного числа без знака.
    /// </summary>
    /// <returns>
    /// При неудаче возвращает <c>null</c>.
    /// </returns>
    public UInt128? ReadUInt128()
    {
        if (!SkipWhitespace())
        {
            return null;
        }

        var builder = StringBuilderPool.Shared.Get();
        while (IsDigit())
        {
            builder.Append (ReadChar());
        }

        try
        {
            return UInt128.Parse
                (
                    builder.ReturnShared(),
                    CultureInfo.InvariantCulture
                );
        }
        catch
        {
            // пустой блок
        }

        return null;
    }

    /// <summary>
    /// Пропускаем управляющие символы.
    /// </summary>
    public bool SkipControl()
    {
        while (true)
        {
            if (EndOfStream)
            {
                return false;
            }

            if (IsControl())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем пунктуацию.
    /// </summary>
    public bool SkipPunctuation()
    {
        while (true)
        {
            if (EndOfStream)
            {
                return false;
            }

            if (IsPunctuation())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Пропускаем пробельные символы.
    /// </summary>
    public bool SkipWhitespace()
    {
        while (true)
        {
            if (EndOfStream)
            {
                return false;
            }

            if (IsWhiteSpace())
            {
                ReadChar();
            }
            else
            {
                return true;
            }
        }
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        if (_ownReader)
        {
            Reader.Dispose();
        }
    }

    #endregion
}
