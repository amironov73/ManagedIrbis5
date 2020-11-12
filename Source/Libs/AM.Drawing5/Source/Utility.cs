// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Utility.cs -- сборник простых вспомогательных методов.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace AM.Drawing
{
    /// <summary>
    /// Сборник простых вспомогательных методов.
    /// </summary>
    public static class Utility
    {
        #region Private members

        /// <summary>
        /// Нормализация компонента цвета.
        /// </summary>
        private static int _Normalize
            (
                float component
            )
        {
            return Math.Max(0, Math.Min(255, (int)component));
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Смешение двух цветов в заданной пропорции.
        /// </summary>
        /// <param name="color1">Первый цвет.</param>
        /// <param name="color2">Второй цвет.</param>
        /// <param name="amount">Доля второго цвета (число от 0 до 1,
        /// 0 - остается только первый цвет, 1 - остается только второй
        /// цвет).</param>
        /// <returns>Смешанный цвет.</returns>
        public static Color Blend
            (
                Color color1,
                Color color2,
                float amount
            )
        {
            var amount1 = 1f - amount;
            var red = _Normalize(color1.R * amount1 + color2.R * amount);
            var green = _Normalize(color1.G * amount1 + color2.G * amount);
            var blue = _Normalize(color1.B * amount1 + color2.B * amount);

            return Color.FromArgb(red, green, blue);
        }

        #endregion
    }
}
