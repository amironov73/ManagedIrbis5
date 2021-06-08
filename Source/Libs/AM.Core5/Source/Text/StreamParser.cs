// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* StreamParser.cs -- считывание из потока чисел, идентификаторов и прочего
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Text;

using AM.IO;

#endregion

#nullable enable

namespace AM.Text
{
    /// <summary>
    /// Считывание из потока чисел, идентификаторов
    /// и прочего.
    /// </summary>
    public sealed class StreamParser
        : IDisposable
    {
        #region Constants

        /// <summary>
        /// End of stream reached.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const char EOF = unchecked ((char)-1);

        #endregion

        #region Properties

        /// <summary>
        /// Is end of stream reached.
        /// </summary>
        public bool EndOfStream => PeekChar() == EOF;

        /// <summary>
        /// Underlying <see cref="TextReader"/>
        /// </summary>
        public TextReader Reader { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public StreamParser
            (
                TextReader reader,
                bool ownReader = false
            )
        {
            Reader = reader;
            _ownReader = ownReader;
        }

        #endregion

        #region Private members

        private readonly bool _ownReader;

        private StringBuilder _ReadNumber()
        {
            var result = new StringBuilder();
            var c = PeekChar();
            if (c == '-' || c == '+')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }
            c = PeekChar();
            if (c == '.')
            {
                result.Append(ReadChar());
                while (IsDigit())
                {
                    result.Append(ReadChar());
                }
                c = PeekChar();
            }
            if (c == 'e' || c == 'E')
            {
                result.Append(ReadChar());
                c = PeekChar();
                if (c == '-' || c == '+')
                {
                    result.Append(ReadChar());
                }
                while (IsDigit())
                {
                    result.Append(ReadChar());
                }
                //c = PeekChar();
            }
            //if ((c == 'F') || (c == 'f') || (c == 'D') || (c == 'd')
            //    || (c == 'M') || (c == 'm'))
            //{
            //    result.Append(ReadChar());
            //}

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Конструирование <see cref="StreamParser"/> из указанного файла
        /// </summary>
        public static StreamParser FromFile
            (
                string fileName,
                Encoding encoding
            )
        {
            Sure.FileExists(fileName, nameof(fileName));

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

        } // method FromFile

        /// <summary>
        /// Конструирование <see cref="StreamParser"/>
        /// из заданного текста.
        /// </summary>
        public static StreamParser FromString
            (
                string text
            )
        {
            var reader = new StringReader(text);
            var result = new StreamParser
                (
                    reader,
                    true
                );

            return result;

        } // method FromString

        /// <summary>
        /// Управляющий символ?
        /// </summary>
        public bool IsControl()
        {
            var c = PeekChar();
            return char.IsControl(c);
        }

        /// <summary>
        /// Цифра?
        /// </summary>
        public bool IsDigit()
        {
            var c = PeekChar();
            return char.IsDigit(c);
        }

        /// <summary>
        /// Буква?
        /// </summary>
        public bool IsLetter()
        {
            var c = PeekChar();
            return char.IsLetter(c);
        }

        /// <summary>
        /// Буква или цифра?
        /// </summary>
        public bool IsLetterOrDigit()
        {
            var c = PeekChar();
            return char.IsLetterOrDigit(c);
        }

        /// <summary>
        /// Часть числа?
        /// </summary>
        public bool IsNumber()
        {
            var c = PeekChar();
            return char.IsNumber(c);
        }

        /// <summary>
        /// Знак пунктуации?
        /// </summary>
        public bool IsPunctuation()
        {
            var c = PeekChar();
            return char.IsPunctuation(c);
        }

        /// <summary>
        /// Разделитель?
        /// </summary>
        public bool IsSeparator()
        {
            var c = PeekChar();
            return char.IsSeparator(c);
        }

        /// <summary>
        /// Суррогат?
        /// </summary>
        public bool IsSurrogate()
        {
            var c = PeekChar();
            return char.IsSurrogate(c);
        }

        /// <summary>
        /// Символ?
        /// </summary>
        public bool IsSymbol()
        {
            var c = PeekChar();
            return char.IsSymbol(c);
        }

        /// <summary>
        /// Пробельный символ?
        /// </summary>
        public bool IsWhiteSpace()
        {
            var c = PeekChar();
            return char.IsWhiteSpace(c);
        }

        /// <summary>
        /// Peek one character from stream.
        /// </summary>
        public char PeekChar()
        {
            return unchecked ((char)Reader.Peek());
        }

        /// <summary>
        /// Read one character from stream.
        /// </summary>
        /// <returns></returns>
        public char ReadChar()
        {
            return unchecked ((char) Reader.Read());
        }

        /// <summary>
        /// Read fixed point number from stream.
        /// </summary>
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
                    result.ToString(),
                    provider ?? CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Read floating point number from stream.
        /// </summary>
        public double? ReadDouble
            (
                IFormatProvider? provider = null
            )
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = _ReadNumber();

            return double.Parse
                (
                    result.ToString(),
                    provider ?? CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Read 16-bit signed integer from stream.
        /// </summary>
        public short? ReadInt16()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return short.Parse(result.ToString());
        }

        /// <summary>
        /// Read 32-bit signed integer from stream.
        /// </summary>
        public int? ReadInt32 ()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return int.Parse(result.ToString());
        }

        /// <summary>
        /// Read 64-bit signed integer from stream.
        /// </summary>
        public long? ReadInt64()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            if (PeekChar() == '-')
            {
                result.Append(ReadChar());
            }
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return long.Parse(result.ToString());
        }

        /// <summary>
        /// Read floating point number from stream.
        /// </summary>
        public float? ReadSingle
            (
                IFormatProvider? provider = null
            )
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = _ReadNumber();

            return float.Parse
                (
                    result.ToString(),
                    provider ?? CultureInfo.InvariantCulture
                );
        }

        /// <summary>
        /// Read 16-bit unsigned integer from stream.
        /// </summary>
        public ushort? ReadUInt16()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return ushort.Parse(result.ToString());
        }

        /// <summary>
        /// Read 16-bit unsigned integer from stream.
        /// </summary>
        public uint? ReadUInt32()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return uint.Parse(result.ToString());
        }

        /// <summary>
        /// Read 64-bit unsigned integer from stream.
        /// </summary>
        public ulong? ReadUInt64()
        {
            if (!SkipWhitespace())
            {
                return null;
            }

            var result = new StringBuilder();
            while (IsDigit())
            {
                result.Append(ReadChar());
            }

            return ulong.Parse(result.ToString());
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

    } // class StreamParser

} // namespace AM.Text
