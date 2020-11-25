// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Utf8String.cs -- строка в кодировке UTF-8
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Строка в кодировке UTF-8.
    /// </summary>
    public sealed class Utf8String
    {
        #region Properties

        public static Utf8String Empty { get; }
            = new Utf8String(Array.Empty<byte>());

        /// <summary>
        /// Длина строки в байтах.
        /// Строка неизменяемая.
        /// Размещается в куче.
        /// Не обязана завершаться нулём.
        /// Может содержать нуль внутри себя.
        /// </summary>
        public int Length => _bytes.Length;

        /// <summary>
        /// Индексер. Выдает отдельные байты строки.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte this[int index] => _bytes[index];

        /// <summary>
        /// Индексер. Выдает подстроку.
        /// </summary>
        /// <param name="range"></param>
        /// <returns></returns>
        public Utf8String this[Range range] => new Utf8String(_bytes[range]);

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public Utf8String
            (
                byte[] bytes
            )
        {
            _bytes = bytes;
        }

        #endregion

        #region Private members

        private readonly byte[] _bytes;

        #endregion

        #region Public methods

        public ReadOnlySpan<byte> AsBytes() => new ReadOnlySpan<byte>(_bytes);

        /// <summary>
        /// Сравнение с другой строкой.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo
            (
                Utf8String? other
            )
        {
            if (other is null)
            {
                return 1;
            }

            for (var i = 0; ; i++)
            {
                if (i == Length)
                {
                    return i == other.Length
                        ? 0
                        : -1;
                }

                var result = this[i] - other[i];
                if (result != 0)
                {
                    return result;
                }
            }
        }

        public bool Contains(byte value)
        {
            foreach (var b in _bytes)
            {
                if (b == value)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Contains(char value) => throw new NotImplementedException();

        public bool Contains(Utf8String value) => throw new NotImplementedException();

        public bool EndsWith(byte value)
        {
            if (Length == 0)
            {
                return false;
            }

            return _bytes[^1] == value;
        }

        public bool EndsWith(char value) => throw new NotImplementedException();

        public bool EndsWith(Utf8String value) => throw new NotImplementedException();

        public Utf8String Replace(byte oldChar, byte newChar) => throw new NotImplementedException();

        public Utf8String Replace(char oldChar, char newChar) => throw new NotImplementedException();

        public Utf8String Replace(Utf8String oldText, Utf8String newText) => throw new NotImplementedException();

        public bool StartsWith(byte value)
        {
            if (Length == 0)
            {
                return false;
            }

            return _bytes[0] == value;
        }

        public bool StartsWith(char value) => throw new NotImplementedException();

        public bool StartsWith(Utf8String value) => throw new NotImplementedException();

        public Utf8String Trim() => throw new NotImplementedException();

        public Utf8String TrimStart() => throw new NotImplementedException();

        public Utf8String TrimEnd() => throw new NotImplementedException();

        #endregion

        #region Operators

        public static bool operator !=(Utf8String? left, Utf8String? right)
            => throw new NotImplementedException();

        public static bool operator ==(Utf8String? left, Utf8String? right)
            => throw new NotImplementedException();

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.GetHashCode"/>
        public override int GetHashCode() => _bytes.GetHashCode();

        /// <inheritdoc cref="Object.Equals(Object)"/>
        public override bool Equals(object? obj) => throw new NotImplementedException();

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => Encoding.UTF8.GetString(_bytes);

        #endregion
    }
}
