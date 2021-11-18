// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Ean8.cs -- работа со штрих-кодом EAN8
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // Information from Wikipedia
    // https://ru.wikipedia.org/wiki/European_Article_Number
    //
    // European Article Number, EAN (европейский номер
    // товара) — европейский стандарт штрихкода,
    // предназначенный для кодирования идентификатора
    // товара и производителя. Является надмножеством
    // американского стандарта UPC.
    //
    // Пример проверки контрольной суммы
    // 46009333 (папиросы «Беломорканал») — код EAN-8.
    // 4x3 + 6x1 + 0x3 + 0x1 + 9x3 + 3x1 + 3x3 + 3x1=
    // 12 + 6 + 0 + 0 + 27 + 3 + 9 + 3= 60.
    // Контрольная сумма = 0 — номер правильный.
    //

    /// <summary>
    /// Работа со штрих-кодом EAN8.
    /// </summary>
    public sealed class Ean8
    {
        #region Private data

        /// <summary>
        /// Коэффициенты для вычисления контрольной цифры.
        /// </summary>
        private static readonly int[] _coefficients
            = { 3, 1, 3, 1, 3, 1, 3, 1 };

        #endregion

        #region Public methods

        /// <summary>
        /// Вычисление контрольной цифры.
        /// </summary>
        public static char ComputeCheckDigit
            (
                ReadOnlySpan<char> digits
            )
        {
            var sum = 0;
            for (var i = 0; i < 7; i++)
            {
                sum = sum + (digits[i] - '0') * _coefficients[i];
            }

            var result = (char)(10 - sum % 10 + '0');

            return result;
        }

        /// <summary>
        /// Проверка контрольной цифры.
        /// </summary>
        public static bool CheckControlDigit
            (
                ReadOnlySpan<char> digits
            )
        {
            var sum = 0;
            for (var i = 0; i < 8; i++)
            {
                sum = sum + (digits[i] - '0') * _coefficients[i];
            }

            var result = sum % 10 == 0;

            return result;
        }

        #endregion

    }
}
