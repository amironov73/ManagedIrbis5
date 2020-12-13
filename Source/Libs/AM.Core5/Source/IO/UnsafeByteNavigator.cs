// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* UnsafeByteNavigator.cs -- навигатор по массиву байт, небезопасный вариант
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.Contracts;
using System.Text;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Навигатор по массиву байт, небезопасный вариант.
    /// </summary>
    public unsafe ref struct UnsafeByteNavigator
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
        public bool IsEOF => _position >= _length;

        /// <summary>
        /// Длина массива.
        /// </summary>
        [Pure]
        public int Length => _length;

        /// <summary>
        /// Текущая позиция.
        /// </summary>
        [Pure]
        public int Position => _position;

        /// <summary>
        /// Данные в виде спана.
        /// </summary>
        [Pure]
        public ReadOnlySpan<byte> AsSpan => new ReadOnlySpan<byte>(_data, _length);

        /// <summary>
        /// Данные, хранящиеся в навигаторе.
        /// </summary>
        [Pure]
        public byte[] Data => AsSpan.ToArray();

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public UnsafeByteNavigator
            (
                byte *data,
                int length
            )
        {
            _data = data;
            _length = length;
            _position = 0;
            Encoding = Encoding.Default;
        }

        #endregion

        #region Private members

        private readonly byte *_data;
        private readonly int _length;
        private int _position;

        #endregion

        #region Public methods

        /// <summary>
        /// Клонирование навигатора.
        /// </summary>
        public UnsafeByteNavigator Clone()
        {
            var result = new UnsafeByteNavigator (_data, _length)
            {
                Encoding = Encoding,
                _position = _position
            };

            return result;
        } // method Clone

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

            var result = AsSpan.Slice(_position);

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
        public int PeekByte() => _position >= _length
                ? EOF
                : _data[_position];

        /// <summary>
        /// Peek one char.
        /// </summary>
        [Pure]
        public char PeekChar()
        {
            if (_position >= _length)
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
            if (_position >= _length)
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
            if (_position >= _length)
            {
                return '\0';
            }

            var result = (char)_data[_position++];

            return result;
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

            return AsSpan.Slice(start, stop - start);
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
    } // struct UnsafeByteNavigator
} // namespace AM.IO
