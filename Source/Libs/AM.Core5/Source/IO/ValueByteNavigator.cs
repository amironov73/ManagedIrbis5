// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ValueByteNavigator.cs -- навигатор по массиву байт, оформленный как структура
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Навигатор по массиву байт, оформленный как структура.
    /// </summary>
    public ref struct ValueByteNavigator
    {
        #region Constants

        /// <summary>
        /// Признак конца данных.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public const int EOF = -1;

        #endregion

        #region Properties

        /// <summary>
        /// Используемая кодировка.
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>
        /// Достигнут конец данных?
        /// </summary>
        // ReSharper disable once InconsistentNaming
        [Pure]
        public bool IsEOF => _position >= _data.Length;

        /// <summary>
        /// Длина массива.
        /// </summary>
        [Pure]
        public int Length => _data.Length;

        /// <summary>
        /// Текущая позиция.
        /// </summary>
        [Pure]
        public int Position => _position;

        /// <summary>
        /// Данные, хранящиеся в навигаторе.
        /// </summary>
        [Pure]
        public byte[] Data => _data.ToArray();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ValueByteNavigator
            (
                ReadOnlySpan<byte> data
            )
        {
            _data = data;
            _position = 0;
            Encoding = Encoding.Default;
        }

        #endregion

        #region Private members

        private readonly ReadOnlySpan<byte> _data;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование навигатора.
        /// </summary>
        public ValueByteNavigator Clone()
        {
            var result = new ValueByteNavigator (_data)
            {
                Encoding = Encoding,
                _position = _position
            };

            return result;
        } // method Clone

        /// <summary>
        /// Навигатор по двоичному файлу.
        /// </summary>
        public static ValueByteNavigator FromFile
            (
                string fileName
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));

            var data = File.ReadAllBytes(fileName);
            var result = new ValueByteNavigator(data);

            return result;
        } // method FromFile

        /// <summary>
        /// Выдать остаток данных.
        /// </summary>
        [Pure]
        public ReadOnlySpan<byte> GetRemainingData()
        {
            if (IsEOF)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            var result = _data.Slice(_position);

            return result;
        } // method GetRemainingData

        /// <summary>
        /// Текущий байт - управляющий?
        /// </summary>
        [Pure]
        public bool IsControl() => char.IsControl(PeekChar());

        /// <summary>
        /// Текущий байт - цифра?
        /// </summary>
        [Pure]
        public bool IsDigit() => char.IsDigit(PeekChar());

        /// <summary>
        /// Текущий байт - буква?
        /// </summary>
        [Pure]
        public bool IsLetter() => char.IsLetter(PeekChar());

        /// <summary>
        /// Текущий байт - буква или цифра?
        /// </summary>
        [Pure]
        public bool IsLetterOrDigit() => char.IsLetterOrDigit(PeekChar());

        /// <summary>
        /// Текущий байт - часть числа?
        /// </summary>
        [Pure]
        public bool IsNumber() => char.IsNumber(PeekChar());

        /// <summary>
        /// Текущий байт - знак пунктуации?
        /// </summary>
        [Pure]
        public bool IsPunctuation() => char.IsPunctuation(PeekChar());

        /// <summary>
        /// Текущий байт - разделитель?
        /// </summary>
        [Pure]
        public bool IsSeparator() => char.IsSeparator(PeekChar());

        /// <summary>
        /// Текущий байт - суррогат?
        /// </summary>
        [Pure]
        public bool IsSurrogate() => char.IsSurrogate(PeekChar());

        /// <summary>
        /// Текущий байт - символ?
        /// </summary>
        [Pure]
        public bool IsSymbol() => char.IsSymbol(PeekChar());

        /// <summary>
        /// Текущий байт - пробельный символ?
        /// </summary>
        [Pure]
        public bool IsWhiteSpace() => char.IsWhiteSpace(PeekChar());

        /// <summary>
        /// Абсолютное перемещение.
        /// </summary>
        public void MoveAbsolute
            (
                int position
            )
        {
            if (position > Length)
            {
                position = Length;
            }

            if (position < 0)
            {
                position = 0;
            }

            _position = position;
        } // method MoveAbsolute

        /// <summary>
        /// Относительное перемещение.
        /// </summary>
        public void MoveRelative
            (
                int delta
            )
        {
            var position = _position + delta;
            if (position > Length)
            {
                position = Length;
            }

            if (position < 0)
            {
                position = 0;
            }

            _position = position;
        } // method MoveRelative

        /// <summary>
        /// Подсмотреть один байт.
        /// </summary>
        [Pure]
        public int PeekByte() => _position >= _data.Length
                ? EOF
                : _data[_position];

        /// <summary>
        /// Peek one char.
        /// </summary>
        [Pure]
        public char PeekChar()
        {
            if (_position >= _data.Length)
            {
                return '\0';
            }

            // TODO implement properly

            var result = (char)_data[_position];

            return result;
        } // method PeekChar

        /// <summary>
        /// Чтение одного байта
        /// (текущая позиция продвигается).
        /// </summary>
        public int ReadByte()
        {
            if (_position >= _data.Length)
            {
                return -1;
            }

            var result = _data[_position++];

            return result;
        } // method ReadByte

        /// <summary>
        /// Чтение одного символа
        /// (текущая позиция продвигается).
        /// </summary>
        public char ReadChar()
        {
            if (_position >= _data.Length)
            {
                return '\0';
            }

            var result = (char)_data[_position++];

            return result;

            // TODO implement properly

            //byte[] bytes = new byte[Encoding.GetMaxByteCount(1)];
            //bytes[0] = _data[Position];
            //Position++;
            //int count = 1;

            //Decoder decoder = Encoding.GetDecoder();
            //decoder.Reset();

            //char[] chars = new char[1];
            //int bytesUsed, charsUsed;
            //bool completed;
            //decoder.Convert(bytes, 0, count, chars, 0, 1,
            //    false, out bytesUsed, out charsUsed,
            //    out completed);
            //while (charsUsed != 1)
            //{
            //    if (count == bytes.Length)
            //    {
            //        return '\0';
            //    }

            //    if (Position >= Length)
            //    {
            //        return '\0';
            //    }

            //    bytes[count] = _data[Position];
            //    count++;
            //    Position++;

            //    decoder.Convert(bytes, 0, count, chars, 0, 1,
            //        false, out bytesUsed, out charsUsed,
            //        out completed);
            //}

            //return chars[0];
        } // method ReadChar

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        public ReadOnlySpan<byte> ReadLine()
        {
            if (IsEOF)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            var start = _position;
            while (!IsEOF)
            {
                var c = PeekChar();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
                ReadChar();
            }

            var stop = _position;
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

            return _data.Slice(start, stop - start);
        } // method ReadLine

        /// <summary>
        /// Пропускаем строку
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
        } // method SkipLine

        #endregion
    } // struct ValueByteNavigator
} // namespace AM.IO
