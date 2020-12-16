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
        public Decoder Decoder { get; }

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
                ReadOnlySpan<byte> data,
                Encoding? encoding = default
            )
        {
            _data = data;
            _position = 0;
            encoding ??= Encoding.Default;
            Decoder = encoding.GetDecoder();
        } // constructor

        #endregion

        #region Private members

        private readonly ReadOnlySpan<byte> _data;
        private int _position;

        #endregion

        #region Public methods

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
        /// Заглядывание вперед на указанную дистанцию.
        /// </summary>
        /// <param name="distantion">Дистанция, байты.</param>
        public int LookAhead
            (
                int distantion
            )
        {
            var position = _position + distantion;

            return position >= _data.Length
                ? EOF
                : _data[position];
        }

        /// <summary>
        /// Peek one char.
        /// </summary>
        [Pure]
        public unsafe char PeekChar()
        {
            // Максимальная длина для UTF-8
            const int MaxBytes = 6;

            var current = PeekByte();
            if (current < 0)
            {
                return '\0';
            }

            var bytes = stackalloc byte[MaxBytes];
            bytes[0] = (byte)current;
            var count = 1;

            var result = '?';
            var ptr = &result;
            Decoder.Reset();
            Decoder.Convert(bytes, count, ptr, 1,
                false, out var _, out var charsUsed,
                out var _);
            while (charsUsed != 1)
            {
                if (count == MaxBytes)
                {
                    return '\0';
                }

                current = LookAhead(count);
                if (current < 0)
                {
                    return '\0';
                }

                ++bytes;
                *bytes = (byte)current;
                count++;

                Decoder.Convert(bytes, count, ptr, 1,
                    false, out _, out charsUsed,
                    out _);
            }

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
        public unsafe char ReadChar()
        {
            // Максимальная длина для UTF-8
            const int MaxBytes = 6;

            var current = ReadByte();
            if (current < 0)
            {
                return '\0';
            }

            var bytes = stackalloc byte[MaxBytes];
            bytes[0] = (byte)current;
            var count = 1;

            var result = '?';
            var ptr = &result;
            Decoder.Reset();
            Decoder.Convert(bytes, count, ptr, 1,
                false, out var _, out var charsUsed,
                out var _);
            while (charsUsed != 1)
            {
                if (count == MaxBytes)
                {
                    return '\0';
                }

                current = ReadByte();
                if (current < 0)
                {
                    return '\0';
                }

                ++bytes;
                *bytes = (byte)current;
                count++;

                Decoder.Convert(bytes, count, ptr, 1,
                    false, out _, out charsUsed,
                    out _);
            }

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
                var c = PeekByte();
                if (c == '\r' || c == '\n')
                {
                    break;
                }

                ReadByte();
            }

            var stop = _position;
            if (!IsEOF)
            {
                var c = PeekByte();
                if (c == '\r')
                {
                    ReadByte();
                    c = PeekByte();
                }

                if (c == '\n')
                {
                    ReadByte();
                }
            }

            return _data.Slice(start, stop - start);
        } // method ReadLine

        /// <summary>
        /// Чтение до конца строки.
        /// </summary>
        public ReadOnlySpan<byte> ReadIrbisLine()
        {
            if (IsEOF)
            {
                return ReadOnlySpan<byte>.Empty;
            }

            var start = _position;
            while (!IsEOF)
            {
                var c = PeekByte();
                if (c == 0x1F || c == 0x1E)
                {
                    break;
                }

                ReadByte();
            }

            var stop = _position;
            if (!IsEOF)
            {
                var c = PeekByte();
                if (c == 0x1F)
                {
                    ReadByte();
                    c = PeekByte();
                }

                if (c == 0x1E)
                {
                    ReadByte();
                }
            }

            return _data.Slice(start, stop - start);
        } // method ReadIrbisLine

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
                var c = PeekByte();
                if (c == '\r' || c == '\n')
                {
                    break;
                }
                ReadByte();
            }

            if (!IsEOF)
            {
                var c = PeekByte();

                if (c == '\r')
                {
                    ReadByte();
                    c = PeekByte();
                }

                if (c == '\n')
                {
                    ReadByte();
                }
            }
        } // method SkipLine

        #endregion

    } // struct ValueByteNavigator

} // namespace AM.IO
