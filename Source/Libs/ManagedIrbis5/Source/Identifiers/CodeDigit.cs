// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CodeDigit.cs -- элемент ISBN/ISSN и аналогичных идентификаторов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Identifiers
{
    /// <summary>
    /// Элемент ISBN/ISSN и аналогичных идентификаторов.
    /// </summary>
    public struct CodeDigit
    {
        #region Properties

        /// <summary>
        /// Цифра как текст.
        /// </summary>
        public char Digit;

        /// <summary>
        /// Числовое значение.
        /// </summary>
        public int Value;

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CodeDigit
            (
                char digit,
                int value
            )
        {
            Digit = digit;
            Value = value;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Поиск цифры в массиве.
        /// </summary>
        public static CodeDigit? FindDigit
            (
                char digit,
                ReadOnlySpan<CodeDigit> allowedDigits
            )
        {
            foreach (var current in allowedDigits)
            {
                if (current.Digit == digit)
                {
                    return current;
                }
            }

            return null;
        }

        /// <summary>
        /// Извлечение цифр из идентификатора.
        /// </summary>
        public static CodeDigit[] ExtractDigits
            (
                ReadOnlySpan<char> identifier,
                ReadOnlySpan<CodeDigit> allowedDigits
            )
        {
            var result = new List<CodeDigit> (identifier.Length);

            foreach (var c in identifier)
            {
                var found = FindDigit (c, allowedDigits);
                if (found is not null)
                {
                    result.Add (found.Value);
                }
            }

            return result.ToArray();
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="ValueType.ToString" />
        public override string ToString()
        {
            return Digit.ToString();
        }

        #endregion

    }
}
