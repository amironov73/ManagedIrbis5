// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable CompareOfFloatsByEqualityOperator
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* HslColor.cs -- цветовая модель "тон, насыщенность, яркость"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;

#endregion

#nullable enable

namespace AM.Drawing
{
    //
    // https://ru.wikipedia.org/wiki/HSL
    //
    // HSL, HLS или HSI (от англ. hue, saturation,
    // lightness (intensity)) — цветовая модель, в которой цветовыми
    // координатами являются тон, насыщенность и светлота.
    // Следует отметить, что HSV и HSL — две разные цветовые модели
    // (lightness — светлота, что отличается от яркости).
    //

    /// <summary>
    /// Цветовая модель "тон, насыщенность, яркость".
    /// </summary>
    public struct HslColor
    {
        #region Constants

        /// <summary>
        /// Минимальное значение компонента.
        /// </summary>
        public const float MinComponentValue = 0f;

        /// <summary>
        /// Максимальное значение компонента.
        /// </summary>
        public const float MaxComponentValue = 1f;

        #endregion

        #region Properties

        private float _h;

        /// <summary>
        /// Цветовой тон.
        /// </summary>
        public float H
        {
            get => _h;
            set => _h = _CheckComponent(value, "H");
        }

        private float _l;

        /// <summary>
        /// Насыщенность.
        /// </summary>
        public float L
        {
            get => _l;
            set => _l = _CheckComponent(value, "L");
        }

        private float _s;

        /// <summary>
        /// Яркость (светлота).
        /// </summary>
        public float S
        {
            get => _s;
            set => _s = _CheckComponent(value, "S");
        }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public HslColor
            (
                float h,
                float s,
                float l
            )
        {
            _h = _CheckComponent(h, "H");
            _s = _CheckComponent(s, "S");
            _l = _CheckComponent(l, "L");
        }

        #endregion

        #region Private members

        private static float _CheckComponent
            (
                float value,
                string name
            )
        {
            if (value < MinComponentValue || value > MaxComponentValue)
            {
                throw new ArgumentOutOfRangeException(name);
            }

            return value;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.Equals(object?)"/>
        public override bool Equals
            (
                object? obj
            )
        {
            if (obj is HslColor other)
            {
                return H == other.H && L == other.L && S == other.S;
            }

            return false;
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            var result = H.GetHashCode();
            result = 29 * result + L.GetHashCode();
            result = 29 * result + S.GetHashCode();

            return result;
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"H: {H}; S: {S}; L: {L}";

        #endregion

    } // struct HslColor

} // namespace AM.Drawing
